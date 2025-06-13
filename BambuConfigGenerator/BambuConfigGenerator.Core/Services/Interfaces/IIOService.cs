using BambuConfigGenerator.Contracts;
using BambuConfigGenerator.Core.Models;
using BambuConfigGenerator.Core.Models.UIModels;
using MvvmCross.ViewModels;

namespace BambuConfigGenerator.Core.Services.Interfaces;

public interface IIOService
{
    public UserSettingsModel LoadUserSettingsModel();

    public void SaveUserSettings(UserSettingsModel userSettingsModel);

    public void SaveCorrections(CorrectionParametersModel correction);
    public CorrectionParametersModel? LoadCorrections();

    public List<PresetParametersModel> LoadPresets();

    public Dictionary<string, string> GetFilamentConfigValuePairs(string path);

    public void SaveFilamentConfigValuePairs(string path, MvxObservableCollection<FilamentFileParamUIModel> filamentConfig);

    public FilamentModel GetFilamentTemplateContent(string path);

    public void SaveOutputConfiguration(string outputFinalPath, FilamentModel template);
}