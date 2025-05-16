using System.IO;
using System.Text.Json;
using BambuConfigGenerator.Models;

namespace BambuConfigGenerator.Core.Services
{
    internal interface IOService
    {
        public FilamentModel GetDefaultFilamentModel(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file was not found.", filePath);
            }

            try
            {
                string fileContent = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<FilamentModel>(fileContent) ?? throw new InvalidOperationException("Deserialization returned null.");
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Failed to deserialize the file content into a FilamentModel.", ex);
            }
        }
    }
}
