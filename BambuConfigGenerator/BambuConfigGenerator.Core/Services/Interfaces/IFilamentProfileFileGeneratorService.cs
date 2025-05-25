using BambuConfigGenerator.Contracts;
using BambuConfigGenerator.Contracts.Enums;

namespace BambuConfigGenerator.Core.Services.Interfaces;

internal interface IFilamentProfileFileGeneratorService
{
    CorrectionParametersModel Corrections { get; set; }
    string GetFilamentName(Printers printer, Nozzles nozzle);
    void SetParametersForCorrections (CorrectionParametersModel corrections);
    bool GenerateOutputFiles();
    string GetOutputFileName(Printers printer, Nozzles nozzle);
}