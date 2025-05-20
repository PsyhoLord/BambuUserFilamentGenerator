using BambuConfigGenerator.Core.Enums;

namespace BambuConfigGenerator.Core.Models;

public class CorrectionParametersModel
{
    public string Brand { get; set; }

    public FilamentTypes Type  { get; set; }

    public string Serial { get; set; }

    public List<Printers> SelectedPrinters { get; set; }

    public List<Nozzles> SelectedNozzles { get; set; }

    public int RecommendedTemperatureMin { get; set; }

    public int RecommendedTemperatureMax { get; set; }

    public double FilamentFlowRatio { get; set; }

    public string FolderWithTemplatesPath { get; set; } 

    public string SelectedOutputFolderPath { get; set; }
}