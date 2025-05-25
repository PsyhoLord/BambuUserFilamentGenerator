using BambuConfigGenerator.Contracts.Enums;

namespace BambuConfigGenerator.Core.Models.UIModels;

public class FilamentTypeUIModel
{
    public string Name { get; set; }

    public FilamentTypes FilamentType { get; set; }
}