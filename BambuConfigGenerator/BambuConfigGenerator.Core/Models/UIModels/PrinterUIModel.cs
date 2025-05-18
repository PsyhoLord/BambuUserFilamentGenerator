using BambuConfigGenerator.Core.Enums;
using System.ComponentModel;

namespace BambuConfigGenerator.Core.Models.UIModels;

public class PrinterUIModel
{
    public string Name { get; set; }

    public Printers Printer { get; set; }

    public bool IsSelected { get; set; }
}