using BambuConfigGenerator.Core.Models.UIModels;
using BambuConfigGenerator.Core.Services.Interfaces;
using BambuConfigGenerator.Core.Services.PlatformSpecific;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Reflection;

namespace BambuConfigGenerator.Core.ViewModels;

public class HomeViewModel : MvxViewModel
{
    public IMvxNavigationService NavigationService { get; }
    private bool _menuVisible;
    private ObservableCollection<MenuItem> _menuItems;
    private FileGeneratorViewModel _fileGeneratorVm;
    private int _selectedMenuItemIndex;
    private SettingsViewModel _settingsVm;
    private FilamentEditorViewModel _filamentEditorVm;
    private string _appVersion;

    public HomeViewModel(IFileFolderPickerService fileFolderPickerService,
        IIOService ioService, ITemplateFolderAnalyserService templateFolderAnalyserService,
        IMvxNavigationService navigationService, IUserSettingsService userSettingsService)
    {
        NavigationService = navigationService;
        
        OpenMenuCommand = new MvxCommand(OpenMenu);

        var fileGeneratorVm = new FileGeneratorViewModel(fileFolderPickerService, ioService, templateFolderAnalyserService,
            navigationService, userSettingsService);

        var filamentEditorVm = new FilamentEditorViewModel(fileFolderPickerService, ioService, templateFolderAnalyserService,
            navigationService, userSettingsService);

        var settingsVm = new SettingsViewModel(fileFolderPickerService, ioService, templateFolderAnalyserService,
            navigationService, userSettingsService);

        MenuItems =
        [
            new MenuItem { Name = "Filament Generator", ViewModel = fileGeneratorVm, IsVisible = true, IconSource = "/Resources/border_color_24dp.svg"},
            new MenuItem { Name = "Filament Editor", ViewModel = filamentEditorVm, IconSource = "/Resources/app_registration_24dp.svg" },
            new MenuItem { Name = "Settings", ViewModel = settingsVm, IconSource = "/Resources/settings_24dp.svg" }
        ];

        var appVersion = Assembly.GetExecutingAssembly().GetName().Version;
        AppVersion = $"v{appVersion}";
    }

    public string AppVersion
    {
        get => _appVersion;
        set => SetProperty(ref _appVersion, value);
    }

    public bool MenuVisible
    {
        get => _menuVisible;
        set => SetProperty(ref _menuVisible, value);
    }

    public IMvxCommand OpenMenuCommand { get; set; }

    private void OpenMenu()
    {
        MenuVisible = !MenuVisible;
    }

    public ObservableCollection<MenuItem> MenuItems
    {
        get => _menuItems;
        set => SetProperty(ref _menuItems, value);
    }

    public int SelectedMenuItemIndex
    {
        get => _selectedMenuItemIndex;
        set
        {
            if (SetProperty(ref _selectedMenuItemIndex, value))
            {
                ChangeMenuPage();
            }
        }
    }

    public void ChangeMenuPage()
    {
        MenuVisible = false;

        for (var i = 0; i < MenuItems.Count; i++)
        {
            MenuItems[i].IsVisible = SelectedMenuItemIndex == i;
        }
    }
}