using BambuConfigGenerator.Core.Enums;
using BambuConfigGenerator.Core.Models;

namespace BambuConfigGenerator.Core.Services;

internal interface IFilamentProfileFileGeneratorService
{
    CorrectionParametersModel Corrections { get; set; }
    string GetFilamentName(Printers printer, Nozzles nozzle);
    void SetParametersForCorrections (CorrectionParametersModel corrections);
    bool GenerateOutputFiles();
    string GetOutputFileName(Printers printer, Nozzles nozzle);
}