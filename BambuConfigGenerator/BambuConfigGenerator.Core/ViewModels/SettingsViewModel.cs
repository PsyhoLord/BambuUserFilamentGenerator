using BambuConfigGenerator.Core.Services;
using BambuConfigGenerator.Core.Services.PlatformSpecific;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace BambuConfigGenerator.Core.ViewModels;

public class SettingsViewModel : MvxViewModel
{
    public SettingsViewModel(IFileFolderPickerService fileFolderPickerService,
        IIOService ioService, ITemplateFolderAnalyserService templateFolderAnalyserService,
        IMvxNavigationService navigationService)
    {
        
    }
}