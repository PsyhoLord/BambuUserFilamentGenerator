using System.Globalization;
using System.IO;
using BambuConfigGenerator.Core.Enums;
using BambuConfigGenerator.Core.Models;
using BambuConfigGenerator.Models;
using Newtonsoft.Json;

namespace BambuConfigGenerator.Core.Services
{
    internal class FilamentProfileFileGeneratorService
    {
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

        public Dictionary<FilamentTypes, string> FilamentTypeNames = new Dictionary<FilamentTypes, string>()
        {
            {FilamentTypes.PETG, "PETG"},
            {FilamentTypes.PLA, "PLA"},
            {FilamentTypes.ABS, "ABS"},
            {FilamentTypes.PA, "PA"},
            {FilamentTypes.PVA, "PVA"},
            {FilamentTypes.PC, "PC"},
            {FilamentTypes.TPU, "TPU"},
            {FilamentTypes.HIPS, "HIPS"},
            //{FilamentTypes.PETG_CarbonFiber, "PETG Carbon Fiber"},
            //{FilamentTypes.PLA_CarbonFiber, "PLA Carbon Fiber"}
        };

        // public FilamentModel FilamentModel { get; set; }

        public CorrectionParametersModel Corrections { get; set; }

        public string FolderWithTemplatesPath { get; set; } 

        public string GetFilamentName(Printers printer, Nozzles nozzle)
        {
            return $"{Corrections.Brand} {Corrections.Type} @Bambu Lab {PrinterNames[printer]} {NozzleNames[nozzle]}";
        }

        public void SetFilamentTemplatePath(string path)
        {
            FolderWithTemplatesPath = path;
        }

        public void SetParametersForCorrections (CorrectionParametersModel corrections)
        {
            Corrections = corrections;
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

            var fileContent = File.ReadAllText(path).Replace("\r\n", "\n"); ;
            return JsonConvert.DeserializeObject<FilamentModel>(fileContent) ?? throw new InvalidOperationException("Failed to deserialize the file content.");
        }

        public bool GenerateOutputFiles(string outputPath)
        {
            var filamentId = Guid.NewGuid().ToString();
            foreach (var printer in Corrections.SelectedPrinters)
            {
                foreach (var nozzle in Corrections.SelectedNozzles)
                {
                    var template = GetFileContent($"{FolderWithTemplatesPath}\\" + GetTemplateFileName(printer, nozzle, Corrections.Type));

                    if (template == null)
                    {
                        throw new FileNotFoundException($"Template file not found for {printer} and {nozzle}.");
                    }

                    var outputFileName = GetOutputFileName(printer, nozzle);

                    template.CompatiblePrinters = [$"{PrinterNames[printer]} {NozzleNames[nozzle]}"];
                    template.FilamentId = filamentId;
                    template.FilamentSettingsId =
                    [
                        $"{PrinterNames[printer]} {FilamentTypeNames[Corrections.Type]} {Corrections.Serial} @{PrinterNames[printer]} {NozzleNames[nozzle]}"
                    ];

                    template.FilamentVendor = [Corrections.Brand];
                    template.Name =
                        $"{PrinterNames[printer]} {FilamentTypeNames[Corrections.Type]} {Corrections.Serial} @{PrinterNames[printer]} {NozzleNames[nozzle]}";

                    // Apply corrections:
                    template.NozzleTemperatureRangeLow = [$"{Corrections.RecommendedTemperatureMin:D}"];
                    template.NozzleTemperatureRangeHigh = [$"{Corrections.RecommendedTemperatureMax:D}"];
                    template.FilamentFlowRatio = [Corrections.FilamentFlowRatio.ToString("F2", CultureInfo.InvariantCulture)];

                    // Serialize the template with custom options

                    var outputFinalPath = $"{outputPath}\\{outputFileName}";
                    File.WriteAllText(outputFinalPath, JsonConvert.SerializeObject(template, Formatting.Indented));
                }
            }

            return true;
        }

        private string GetTemplateFileName(Printers printer, Nozzles nozzle, FilamentTypes filamentType)
        {
            //TEMPLATE PETG Basic @Bambu Lab A1 0.2 nozzle
            switch (printer)
            {
                case Printers.A1:
                case Printers.A1Mini:
                    return $"TEMPLATE {FilamentTypeNames[filamentType]} Basic @{PrinterNames[Printers.A1]} {NozzleNames[nozzle]}.json";
                case Printers.X1:
                case Printers.X1C:
                case Printers.X1E:
                case Printers.P1P:
                case Printers.P1S:
                    return $"TEMPLATE {FilamentTypeNames[filamentType]} Basic @{PrinterNames[Printers.X1]} {NozzleNames[nozzle]}.json";
                default:
                    throw new ArgumentOutOfRangeException(nameof(printer), printer, null);
            }
        }

        public string GetOutputFileName(Printers printer, Nozzles nozzle)
        {
            return $"{Corrections.Brand} {FilamentTypeNames[Corrections.Type]} {Corrections.Serial} @{PrinterNames[printer]} {NozzleNames[nozzle]}.json";
        }
    }
}
