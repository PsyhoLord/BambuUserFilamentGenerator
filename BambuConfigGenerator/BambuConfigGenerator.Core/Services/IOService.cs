using System.IO;
using System.Text.RegularExpressions;
using BambuConfigGenerator.Core.Models;
using BambuConfigGenerator.Core.Models.UIModels;
using MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace BambuConfigGenerator.Core.Services
{
    internal class IOService : IIOService
    {
        private const string UserConfigFileName = "UserConfig.json";

        public void SaveConfiguration(CorrectionParametersModel correction)
        {
            if (correction == null)
            {
                throw new ArgumentNullException(nameof(correction), "Correction parameters cannot be null.");
            }

            var jsonString = JsonConvert.SerializeObject(correction, Formatting.Indented);
            File.WriteAllText(UserConfigFileName, jsonString);
        }

        public CorrectionParametersModel? LoadConfiguration()
        {
            if (!File.Exists(UserConfigFileName))
            {
                return null;
            }

            var jsonString = File.ReadAllText(UserConfigFileName);
            var configuration = JsonConvert.DeserializeObject<CorrectionParametersModel>(jsonString);

            if (configuration == null)
            {
                File.Delete(UserConfigFileName);
            }

            return configuration;
        }

        public Dictionary<string, string> GetFilamentConfigValuePairs(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path and template name cannot be null or empty.");
            }
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The specified file does not exist.", path);
            }
            var fileContent = File.ReadAllText(path).Replace("\r\n", "\n");
            
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContent) ??
                   throw new InvalidOperationException("Failed to deserialize the file content.");

            var result = new Dictionary<string, string>();
            foreach (var kvp in dict)
            {
                var value = kvp.Value?.ToString().Replace("\r\n", "\n").Replace("\n", "");
                result.Add(kvp.Key, value??string.Empty);
            }

            return result;
        }

        public void SaveFilamentConfigValuePairs(string path, MvxObservableCollection<FilamentFileParamUIModel> filamentConfig)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            var newFileNameWithoutExtension = $"{fileNameWithoutExtension}_1";
            var extension = Path.GetExtension(path);
            var directory = Path.GetDirectoryName(path);
            var newPath = Path.Combine($"{directory}", $"{newFileNameWithoutExtension}{extension}");
            
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }
            if (filamentConfig == null || filamentConfig.Count == 0)
            {
                throw new ArgumentException("Filament configuration cannot be null or empty.", nameof(filamentConfig));
            }
            var dict = filamentConfig.ToDictionary(x => x.Key, x => x.Value);
            var jsonString = JsonConvert.SerializeObject(dict, Formatting.Indented);
            jsonString = jsonString
                .Replace("\\\"", "\"")
                .Replace("\\\\", "\\")
                .Replace("\"[", "[")
                .Replace("]\"", "]");
            var validatedModel = JsonConvert.DeserializeObject<FilamentModel>(jsonString);

            File.WriteAllText(newPath, JsonConvert.SerializeObject(validatedModel, Formatting.Indented));
        }

        public FilamentModel GetFilamentTemplateContent(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The specified file does not exist.", path);
            }

            var fileContent = File.ReadAllText(path).Replace("\r\n", "\n");
            
            return JsonConvert.DeserializeObject<FilamentModel>(fileContent) ??
                   throw new InvalidOperationException("Failed to deserialize the file content.");
        }

        public void SaveOutputConfiguration(string outputFinalPath, FilamentModel template)
        {
            File.WriteAllText(outputFinalPath, JsonConvert.SerializeObject(template, Formatting.Indented));
        }
    }
}
