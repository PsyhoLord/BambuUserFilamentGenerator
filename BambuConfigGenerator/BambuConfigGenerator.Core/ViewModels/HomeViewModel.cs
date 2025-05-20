using BambuConfigGenerator.Core.Models.UIModels;
using BambuConfigGenerator.Core.Services;
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
        IMvxNavigationService navigationService)
    {
        NavigationService = navigationService;
        FileGeneratorVm = new FileGeneratorViewModel(fileFolderPickerService, ioService, templateFolderAnalyserService,
            navigationService);// Mvx.IoCProvider.Resolve<FileGeneratorViewModel>();

        OpenMenuCommand = new MvxCommand(OpenMenu);

        MenuItems =
        [
            new MenuItem { Name = "Filament Generator", ViewModel = FileGeneratorVm, IsVisible = true },
            new MenuItem { Name = "Filament Editor", ViewModel = FilamentEditorVm },
            new MenuItem { Name = "Settings", ViewModel = SettingsVm }
        ];

        AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
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

    public FileGeneratorViewModel FileGeneratorVm
    {
        get => _fileGeneratorVm;
        set => SetProperty(ref _fileGeneratorVm, value);
    }

    public SettingsViewModel SettingsVm
    {
        get => _settingsVm;
        set => SetProperty(ref _settingsVm, value);
    }

    public FilamentEditorViewModel FilamentEditorVm
    {
        get => _filamentEditorVm;
        set => SetProperty(ref _filamentEditorVm, value);
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