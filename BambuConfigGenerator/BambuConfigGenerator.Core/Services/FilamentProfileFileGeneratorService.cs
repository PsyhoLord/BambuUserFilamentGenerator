using System.IO;
using System.Text.Json;
using BambuConfigGenerator.Models;

namespace BambuConfigGenerator.Core.Services
{
    internal class FilamentProfileFileGeneratorService
    {
        public enum Nozzles { Zero2, Zero4, Zero6, Zero8 };
        public enum Printers { A1, A1Mini, X1, X1C, X1E, P1P, P1S };

        public Dictionary<Nozzles, string> NozzleNames = new Dictionary<Nozzles, string>()
           {
               {Nozzles.Zero2, "0.2 nozzle" },
               {Nozzles.Zero4, "0.4 nozzle" },
               {Nozzles.Zero6, "0.6 nozzle" },
               {Nozzles.Zero8, "0.8 nozzle" }
           };

        public Dictionary<Printers, string> PrinterNames = new Dictionary<Printers, string>()
           {
               {Printers.A1Mini, "Bambu Lab A1M"},
               {Printers.A1, "Bambu Lab A1"},
               {Printers.X1, "Bambu Lab X1"},
               {Printers.X1C, "Bambu Lab X1 Carbon"},
               {Printers.X1E, "Bambu Lab X1E"},
               {Printers.P1P, "Bambu Lab P1P"},
               {Printers.P1S, "Bambu Lab P1S"}
           };

        public FilamentProfileFileGeneratorService(string brand, string filamentName, string color)
        {
            Brand = brand;
            FilamentName = filamentName;
            Color = color;
        }

        public string Brand { get; set; }
        public string FilamentName { get; set; }
        public string Color { get; set; }

        public string GetFilamentName(Printers printer, Nozzles nozzle)
        {
            return $"{Brand} {FilamentName} {Color} @Bambu Lab {PrinterNames[printer]} {NozzleNames[nozzle]}";
        }

        public FilamentModel GetFileContent(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The specified file does not exist.", path);
            }

            string fileContent = File.ReadAllText(path);
            return JsonSerializer.Deserialize<FilamentModel>(fileContent) ?? throw new InvalidOperationException("Failed to deserialize the file content.");
        }
    }
}
