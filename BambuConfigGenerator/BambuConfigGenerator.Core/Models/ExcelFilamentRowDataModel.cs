namespace BambuConfigGenerator.Core.Models;

public class ExcelFilamentRowDataModel
{
    // Brand	Type	Serial	Recommended Temp min	Recommended Temp max	Nozzle initial layer temp	Nozzle layers temp	FFR	K
    public string Brand { get; set; }

    public string Type { get; set; }

    public string Serial { get; set; }
    public int RecommendedTempMin { get; set; }
    public int RecommendedTempMax { get; set; }
    public int NozzleInitialLayerTemp { get; set; }
    public int NozzleLayersTemp { get; set; }
    public double FFR { get; set; }
    public double K { get; set; }
}