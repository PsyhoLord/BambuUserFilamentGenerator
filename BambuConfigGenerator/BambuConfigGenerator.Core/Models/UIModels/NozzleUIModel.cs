using BambuConfigGenerator.Core.Enums;

namespace BambuConfigGenerator.Core.Models.UIModels;

public class NozzleUIModel
{
    public string Name { get; set; }

    public Nozzles Nozzle { get; set; }

    public bool IsSelected { get; set; }
}