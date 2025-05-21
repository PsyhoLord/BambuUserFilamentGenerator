using BambuConfigGenerator.Core.Models;
using BambuConfigGenerator.Core.Models.UIModels;
using MvvmCross.ViewModels;

namespace BambuConfigGenerator.Core.Services;

public interface IIOService
{
    public void SaveConfiguration(CorrectionParametersModel correction);
    public CorrectionParametersModel? LoadConfiguration();

    public Dictionary<string, string> GetFilamentConfigValuePairs(string path);

    public void SaveFilamentConfigValuePairs(string path, MvxObservableCollection<FilamentFileParamUIModel> filamentConfig);

    public FilamentModel GetFilamentTemplateContent(string path);

    public void SaveOutputConfiguration(string outputFinalPath, FilamentModel template);
}