using System.IO;
using System.Text.Json;
using BambuConfigGenerator.Models;

namespace BambuConfigGenerator.Core.Services
{
    internal class IOService : IIOService
    {
        public FilamentModel GetDefaultFilamentModel(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Path cannot be null or empty.", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.", filePath);
            }

            var fileContent = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<FilamentModel>(fileContent) ?? throw new InvalidOperationException("Failed to deserialize the file content.");
        }
    }
}
