namespace BambuConfigGenerator.Contracts;

public class ExcelFilamentRowDataModel
{
    // Brand	Type	Serial	Recommended Temp min	Recommended Temp max	Nozzle initial layer temp	Nozzle layers temp	FFR	K
    public string Brand { get; set; }

    public string Type { get; set; }

    public string Serial { get; set; }
    public double RecommendedTempMin { get; set; }
    public double RecommendedTempMax { get; set; }
    public double NozzleInitialLayerTemp { get; set; }
    public double NozzleLayersTemp { get; set; }
    public double FFR { get; set; }
    public double K { get; set; }
}