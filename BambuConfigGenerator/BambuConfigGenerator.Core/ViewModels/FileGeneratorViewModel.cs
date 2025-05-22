using BambuConfigGenerator.Core.Models;
using BambuConfigGenerator.Core.Models.UIModels;
using BambuConfigGenerator.Core.Services.Interfaces;
using BambuConfigGenerator.Core.Services.PlatformSpecific;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace BambuConfigGenerator.Core.ViewModels;

public class FileGeneratorViewModel : MvxViewModel
{
    private readonly IFileFolderPickerService _fileFolderPickerService;
    private readonly IIOService _ioService;
    private readonly ITemplateFolderAnalyserService _templateFolderAnalyserService;
    private readonly IMvxNavigationService _navigationService;
    private readonly IUserSettingsService _userSettingsService;
    private string _filamentBrand = "3DPlast";
    private FilamentTypeUIModel _selectedFilamentType;
    private double _filamentFlowRatio = 0.96;
    private int _recommendedTemperatureMax = 220;
    private int _recommendedTemperatureMin = 190;
    private string _selectedFolder;
    private string _folderWithTemplatesPath;
    private string _serial;
    private ObservableCollection<PrinterUIModel> _printers;
    private ObservableCollection<NozzleUIModel> _nozzles;
    private ObservableCollection<FilamentTypeUIModel> _filamentTypes = new();
    private int _nozzleTemperatureInitialLayer;
    private int _nozzleTemperatureOtherLayers;

    public FileGeneratorViewModel(IFileFolderPickerService fileFolderPickerService,
        IIOService ioService, ITemplateFolderAnalyserService templateFolderAnalyserService,
        IMvxNavigationService  navigationService, IUserSettingsService userSettingsService)
    {
        _fileFolderPickerService = fileFolderPickerService;
        _ioService = ioService;
        _templateFolderAnalyserService = templateFolderAnalyserService;
        _navigationService = navigationService;
        _userSettingsService = userSettingsService;

        SelectFolderWithTemplates = new MvxAsyncCommand(SelectFolderWithTemplatesPath);
        SelectFolderCommand = new MvxAsyncCommand(SelectOutputFolder);
        GenerateCommand = new MvxCommand(Generate);
        OpenFolderWithTemplatesCommand = new MvxCommand(() => Process.Start("explorer.exe", FolderWithTemplatesPath));
        OpenFolderOutputCommand = new MvxCommand(() => Process.Start("explorer.exe", SelectedFolder));

        Init();
    }

    private void Init()
    {
        Printers =
        [
            new PrinterUIModel { Name = "A1 Mini", Printer = Enums.Printers.A1Mini },
            new PrinterUIModel { Name = "A1", Printer = Enums.Printers.A1 },
            //new PrinterUIModel { Name = "P1P", Printer = Enums.Printers.P1P },
            //new PrinterUIModel { Name = "P1S", Printer = Enums.Printers.P1S },
            new PrinterUIModel { Name = "X1", Printer = Enums.Printers.X1 },
            new PrinterUIModel { Name = "X1C", Printer = Enums.Printers.X1C },
            new PrinterUIModel { Name = "X1E", Printer = Enums.Printers.X1E }
        ];

        Nozzles =
        [
            new NozzleUIModel { Name = "0.2", Nozzle = Enums.Nozzles.Zero2 },
            new NozzleUIModel { Name = "0.4", Nozzle = Enums.Nozzles.Zero4 },
            new NozzleUIModel { Name = "0.6", Nozzle = Enums.Nozzles.Zero6 },
            new NozzleUIModel { Name = "0.8", Nozzle = Enums.Nozzles.Zero8 }
        ];
        
#if DEBUG
        // For testing purposes
        FolderWithTemplatesPath = @"C:\Bambu\Template";

        _templateFolderAnalyserService.SetFolderPath(FolderWithTemplatesPath);
        FilamentTypes = _templateFolderAnalyserService.FilamentTypes;
        SelectedFilamentType = FilamentTypes.FirstOrDefault();

        SelectedFolder = @"C:\Bambu\Output";
        Serial = "TEST";
        // SelectedFilamentType = FilamentTypes[0];
        Printers.FirstOrDefault(p => p.Printer == Enums.Printers.A1).IsSelected = true;
        Nozzles.FirstOrDefault(n=>n.Nozzle == Enums.Nozzles.Zero4).IsSelected = true;
#endif
        var configuration = _ioService.LoadCorrections();

        if (configuration != null)
            ApplyConfiguration(configuration);
        else
        {
            var userConfig = _userSettingsService.UserSettings;
            SelectedFolder = userConfig.FolderToBambuLabUserFilaments;
        }
    }

    private void ApplyConfiguration(CorrectionParametersModel configuration)
    {
        FilamentBrand = configuration.Brand;
        Serial = configuration.Serial;
        RecommendedTemperatureMin = configuration.RecommendedTemperatureMin;
        RecommendedTemperatureMax = configuration.RecommendedTemperatureMax;
        FilamentFlowRatio = configuration.FilamentFlowRatio;
        FolderWithTemplatesPath = configuration.FolderWithTemplatesPath;
        SelectedFolder = configuration.SelectedOutputFolderPath;
        NozzleTemperatureInitialLayer = configuration.NozzleTemperatureInitialLayer;
        NozzleTemperatureOtherLayers = configuration.NozzleTemperatureOtherLayers;

        _templateFolderAnalyserService.SetFolderPath(FolderWithTemplatesPath);
        if (!_templateFolderAnalyserService.IsValid)
        {
            MessageBox.Show("Invalid Folder", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else
        {
            FilamentTypes = _templateFolderAnalyserService.FilamentTypes;
            var filamentTypeFromCfg = FilamentTypes?.FirstOrDefault(f => f.FilamentType == configuration.Type);
            if (filamentTypeFromCfg != null)
            {
                SelectedFilamentType = filamentTypeFromCfg;
            }
        }

        var printer = Printers.FirstOrDefault(p => p.Printer == configuration.SelectedPrinters.FirstOrDefault());
        if (printer != null) printer.IsSelected = true;

        var nozzle = Nozzles.FirstOrDefault(n => n.Nozzle == configuration.SelectedNozzles.FirstOrDefault());
        if (nozzle != null) nozzle.IsSelected = true;
    }

    private async Task SelectFolderWithTemplatesPath()
    {
        var path = await _fileFolderPickerService.SelectFolder(string.Empty);
        if (string.IsNullOrEmpty(path))
            return;

        FolderWithTemplatesPath = path;

        _templateFolderAnalyserService.SetFolderPath(FolderWithTemplatesPath);
        if (!_templateFolderAnalyserService.IsValid)
        {
            MessageBox.Show("Invalid Folder", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else
        {
            FilamentTypes = _templateFolderAnalyserService.FilamentTypes;
        }
    }

    private async Task SelectOutputFolder()
    {
        var path = await _fileFolderPickerService.SelectFolder(string.Empty);
        if(string.IsNullOrEmpty(path))
            return;
        SelectedFolder = path;
    }

    private void Generate()
    {
        var filament = Mvx.IoCProvider.Resolve<IFilamentProfileFileGeneratorService>();

        var corrections = new CorrectionParametersModel
        {
            Brand = FilamentBrand,
            Type = SelectedFilamentType.FilamentType,
            Serial = Serial,
            RecommendedTemperatureMin = RecommendedTemperatureMin,
            RecommendedTemperatureMax = RecommendedTemperatureMax,
            FilamentFlowRatio = FilamentFlowRatio,
            SelectedNozzles = Nozzles.Where(n => n.IsSelected).Select(n => n.Nozzle).ToList(),
            SelectedPrinters = Printers.Where(p => p.IsSelected).Select(p => p.Printer).ToList(),
            FolderWithTemplatesPath = FolderWithTemplatesPath,
            SelectedOutputFolderPath = SelectedFolder,
            NozzleTemperatureInitialLayer = NozzleTemperatureInitialLayer,
            NozzleTemperatureOtherLayers = NozzleTemperatureOtherLayers
        };

        filament.SetParametersForCorrections(corrections);

        filament.GenerateOutputFiles();

        _ioService.SaveCorrections(corrections);

        MessageBox.Show("Success!!!", "WooHoo!!!", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    #region Properties

    public string FilamentBrand
    {
        get => _filamentBrand;
        set => SetProperty(ref _filamentBrand, value);
    }

    public FilamentTypeUIModel SelectedFilamentType
    {
        get => _selectedFilamentType;
        set => SetProperty(ref _selectedFilamentType, value);
    }

    public string Serial
    {
        get => _serial;
        set => SetProperty(ref _serial, value);
    }

    public int RecommendedTemperatureMin
    {
        get => _recommendedTemperatureMin;
        set => SetProperty(ref _recommendedTemperatureMin, value);
    }

    public int RecommendedTemperatureMax
    {
        get => _recommendedTemperatureMax;
        set => SetProperty(ref _recommendedTemperatureMax, value);
    }

    public int NozzleTemperatureInitialLayer
    {
        get => _nozzleTemperatureInitialLayer;
        set => SetProperty(ref _nozzleTemperatureInitialLayer, value);
    }

    public int NozzleTemperatureOtherLayers
    {
        get => _nozzleTemperatureOtherLayers;
        set => SetProperty(ref _nozzleTemperatureOtherLayers, value);
    }

    public double FilamentFlowRatio
    {
        get => _filamentFlowRatio;
        set => SetProperty(ref _filamentFlowRatio, value);
    }

    public string FolderWithTemplatesPath
    {
        get => _folderWithTemplatesPath;
        set => SetProperty(ref _folderWithTemplatesPath, value);
    }

    public string SelectedFolder
    {
        get => _selectedFolder;
        set => SetProperty(ref _selectedFolder, value);
    }

    public ObservableCollection<PrinterUIModel> Printers
    {
        get => _printers;
        set => SetProperty(ref _printers, value);
    }

    public ObservableCollection<NozzleUIModel> Nozzles
    {
        get => _nozzles;
        set => SetProperty(ref _nozzles, value);
    }

    public ObservableCollection<FilamentTypeUIModel> FilamentTypes
    {
        get => _filamentTypes;
        set => SetProperty(ref _filamentTypes, value);
    }

    public IMvxAsyncCommand SelectFolderWithTemplates { get; }

    public IMvxAsyncCommand SelectFolderCommand { get; }

    public IMvxCommand GenerateCommand { get; }
    public IMvxCommand OpenFolderWithTemplatesCommand { get; }
    public IMvxCommand OpenFolderOutputCommand { get; }

    

    #endregion
}