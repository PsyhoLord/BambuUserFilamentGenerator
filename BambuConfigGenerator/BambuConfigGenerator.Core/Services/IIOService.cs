using BambuConfigGenerator.Core.Models;
using BambuConfigGenerator.Models;

namespace BambuConfigGenerator.Core.Services;

public interface IIOService
{
    public void SaveConfiguration(CorrectionParametersModel correction);
    public CorrectionParametersModel? LoadConfiguration();

    public FilamentModel GetFilamentTemplateContent(string path);

    public void SaveOutputConfiguration(string outputFinalPath, FilamentModel template);
}