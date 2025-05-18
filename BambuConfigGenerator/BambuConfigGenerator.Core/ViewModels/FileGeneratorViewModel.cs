using System.Collections.ObjectModel;
using System.Windows;
using BambuConfigGenerator.Core.Models;
using BambuConfigGenerator.Core.Models.UIModels;
using BambuConfigGenerator.Core.Services;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace BambuConfigGenerator.Core.ViewModels;

public class FileGeneratorViewModel : MvxViewModel
{
    private readonly IFileFolderPickerService _fileFolderPickerService;
    private readonly ITemplateFolderAnalyserService _templateFolderAnalyserService;
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

    public FileGeneratorViewModel(IFileFolderPickerService fileFolderPickerService, ITemplateFolderAnalyserService templateFolderAnalyserService)
    {
        _fileFolderPickerService = fileFolderPickerService;
        _templateFolderAnalyserService = templateFolderAnalyserService;

        SelectFolderWithTemplates = new MvxAsyncCommand(SelectFolderWithTemplatesPath);
        SelectFolderCommand = new MvxAsyncCommand(SelectFolder);
        GenerateCommand = new MvxCommand(Generate);

        Init();
    }

    private void Init()
    {
        /*FilamentTypes =
        [
            new FilamentTypeUIModel { Name = "PLA", FilamentType = Enums.FilamentTypes.PLA },
            new FilamentTypeUIModel { Name = "PETG", FilamentType = Enums.FilamentTypes.PETG },
        ];*/

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
        SelectedFolder = @"C:\Bambu\Output";
        Serial = "TEST";
        // SelectedFilamentType = FilamentTypes[0];
        Printers.FirstOrDefault(p => p.Printer == Enums.Printers.A1).IsSelected = true;
        Nozzles.FirstOrDefault(n=>n.Nozzle == Enums.Nozzles.Zero4).IsSelected = true;
#endif
    }

    private async Task SelectFolderWithTemplatesPath()
    {
        var path = await _fileFolderPickerService.SelectFolder(string.Empty);
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

    private async Task SelectFolder()
    {
        var path = await _fileFolderPickerService.SelectFolder(string.Empty);
        SelectedFolder = path;
        
    }
    

    private void Generate()
    {
        var filament = new FilamentProfileFileGeneratorService();

        filament.SetFilamentTemplatePath(FolderWithTemplatesPath);

        filament.SetParametersForCorrections(new CorrectionParametersModel
        {
            Brand = FilamentBrand,
            Type = SelectedFilamentType.FilamentType,
            RecommendedTemperatureMin = RecommendedTemperatureMin,
            RecommendedTemperatureMax = RecommendedTemperatureMax,
            FilamentFlowRatio = FilamentFlowRatio,
            SelectedNozzles = Nozzles.Where(n => n.IsSelected).Select(n => n.Nozzle).ToList(),
            SelectedPrinters = Printers.Where(p => p.IsSelected).Select(p => p.Printer).ToList(),
            Serial = Serial
        });

        filament.GenerateOutputFiles(SelectedFolder);

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

    #endregion
}