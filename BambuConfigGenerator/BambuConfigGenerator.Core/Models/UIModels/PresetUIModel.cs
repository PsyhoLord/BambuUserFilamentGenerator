using BambuConfigGenerator.Contracts.Enums;

namespace BambuConfigGenerator.Core.Models.UIModels;

public class PresetUIModel
{
    public string Brand { get; set; }

    public FilamentTypes Type { get; set; }

    public string Serial { get; set; }

    public long RecommendedTempMin { get; set; }

    public long RecommendedTempMax { get; set; }

    public long NozzleInitialLayerTemp { get; set; }

    public long NozzleLayersTemp { get; set; }

    public double Ffr { get; set; }

    public double K { get; set; }
}