using System.IO;
using BambuConfigGenerator.Core.Models;
using BambuConfigGenerator.Models;
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
            ;
            return JsonConvert.DeserializeObject<FilamentModel>(fileContent) ??
                   throw new InvalidOperationException("Failed to deserialize the file content.");
        }

        public void SaveOutputConfiguration(string outputFinalPath, FilamentModel template)
        {
            File.WriteAllText(outputFinalPath, JsonConvert.SerializeObject(template, Formatting.Indented));
        }
    }
}
