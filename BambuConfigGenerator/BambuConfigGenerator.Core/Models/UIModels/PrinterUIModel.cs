using BambuConfigGenerator.Contracts.Enums;

namespace BambuConfigGenerator.Core.Models.UIModels;

public class PrinterUIModel
{
    public string Name { get; set; }

    public Printers Printer { get; set; }

    public bool IsSelected { get; set; }
}