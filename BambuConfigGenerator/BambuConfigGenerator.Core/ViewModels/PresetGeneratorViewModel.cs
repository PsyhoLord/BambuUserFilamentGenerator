using BambuConfigGenerator.Contracts;
using BambuConfigGenerator.Core.Models.UIModels;
using BambuConfigGenerator.Core.Services.Interfaces;
using BambuConfigGenerator.Core.Services.PlatformSpecific;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using Enums = BambuConfigGenerator.Contracts.Enums; // Assuming Enums is in the Contracts namespace

namespace BambuConfigGenerator.Core.ViewModels;

public class PresetGeneratorViewModel : MvxViewModel
{
    private readonly IFileFolderPickerService _fileFolderPickerService;
    private readonly IIOService _ioService;
    private readonly ITemplateFolderAnalyserService _templateFolderAnalyserService;
    private readonly IUserSettingsService _userSettingsService;
    private ObservableCollection<PrinterUIModel> _printers;
    private ObservableCollection<NozzleUIModel> _nozzles;
    private ObservableCollection<string> _brandList;
    private ObservableCollection<FilamentTypeUIModel> _filamentList;
    private ObservableCollection<PresetUIModel> _presetsList;
    private int _brandListSelectedIndex;
    private int _serialSelectedIndex;
    private int _listSelectedIndex;
    private string _folderWithTemplatesPath;
    private string _outputFolder;
    private ObservableCollection<FilamentFileParamUIModel> _presetParamsList;
    private List<PresetUIModel> _allPresetsList;

    public PresetGeneratorViewModel(IFileFolderPickerService fileFolderPickerService,
        IIOService ioService, ITemplateFolderAnalyserService templateFolderAnalyserService,
        IMvxNavigationService navigationService, IUserSettingsService userSettingsService)
    {
        _fileFolderPickerService = fileFolderPickerService;
        _ioService = ioService;
        _templateFolderAnalyserService = templateFolderAnalyserService;
        _userSettingsService = userSettingsService;
        ReadAvailablePresetsCommand = new MvxCommand(ReadAvailablePresets);
        GeneratePresetCommand = new MvxCommand(GenerateSinglePreset);
        GenerateAllPresetsCommand = new MvxCommand(GenerateAllPresets);
        SelectFolderWithTemplates = new MvxAsyncCommand(SelectFolderWithTemplatesPath);
        SelectFolderCommand = new MvxAsyncCommand(SelectOutputFolder);
        OpenFolderWithTemplatesCommand = new MvxCommand(() => Process.Start("explorer.exe", FolderWithTemplatesPath));
        OpenFolderOutputCommand = new MvxCommand(() => Process.Start("explorer.exe", OutputFolder));

        var configuration = _ioService.LoadCorrections();

        if (configuration != null)
            ApplyConfiguration(configuration);
        else
        {
            var userConfig = _userSettingsService.UserSettings;
            // SelectedFolder = userConfig.FolderToBambuLabUserFilaments;
        }

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
            // FilamentTypes = _templateFolderAnalyserService.FilamentTypes;
        }
    }

    private async Task SelectOutputFolder()
    {
        var path = await _fileFolderPickerService.SelectFolder(string.Empty);
        if (string.IsNullOrEmpty(path))
            return;
        OutputFolder = path;
    }

    private void ApplyConfiguration(CorrectionParametersModel configuration)
    {
        FolderWithTemplatesPath = configuration.FolderWithTemplatesPath;
        OutputFolder = configuration.SelectedOutputFolderPath;
    }

    private void GenerateAllPresets()
    {
        if (GenerateSinglePreset(_allPresetsList))
        {
            MessageBox.Show("Success!!!", "WooHoo!!!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("FAIL!!! Check nozzles and printers", "WooHoo!!!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void GenerateSinglePreset()
    {
        if (GenerateSinglePreset([PresetsList[SerialSelectedIndex]]))
        {
            MessageBox.Show("Success!!!", "WooHoo!!!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("FAIL!!! Check nozzles and printers", "WooHoo!!!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private bool GenerateSinglePreset(IEnumerable<PresetUIModel> presets)
    {
        var filamentGeneratorService = Mvx.IoCProvider.Resolve<IFilamentProfileFileGeneratorService>();

        var nozzles = Nozzles.Where(n => n.IsSelected).Select(n => n.Nozzle).ToList();
        var printers = Printers.Where(p => p.IsSelected).Select(p => p.Printer).ToList();

        if (nozzles == null || nozzles.Count == 0)
            return false;

        if (printers == null || printers.Count == 0)
            return false;

        foreach (var presetUiModel in presets)
        {
            var corrections = new CorrectionParametersModel
            {
                Brand = presetUiModel.Brand,
                Type = presetUiModel.Type,
                Serial = presetUiModel.Serial,
                RecommendedTemperatureMin = (int)presetUiModel.RecommendedTempMin,
                RecommendedTemperatureMax = (int)presetUiModel.RecommendedTempMax,
                NozzleTemperatureInitialLayer = (int)presetUiModel.NozzleInitialLayerTemp,
                NozzleTemperatureOtherLayers = (int)presetUiModel.NozzleLayersTemp,
                FilamentFlowRatio = presetUiModel.Ffr,
                SelectedNozzles = nozzles,
                SelectedPrinters = printers,
                FolderWithTemplatesPath = FolderWithTemplatesPath,
                SelectedOutputFolderPath = OutputFolder

            };

            filamentGeneratorService.SetParametersForCorrections(corrections);

            filamentGeneratorService.GenerateOutputFiles();

            _ioService.SaveCorrections(corrections);
        }

        return true;
    }

    private void ReadAvailablePresets()
    {
        var allPresetsModelsList = _ioService.LoadPresets();

        _allPresetsList = allPresetsModelsList.Select(p => new PresetUIModel
        {
            Type = p.Type,
            Serial = p.Serial,
            Brand = p.Brand,
            RecommendedTempMin = p.RecommendedTempMin,
            RecommendedTempMax = p.RecommendedTempMax,
            NozzleInitialLayerTemp = p.NozzleInitialLayerTemp,
            NozzleLayersTemp = p.NozzleLayersTemp,
            Ffr = p.Ffr,
            K = p.K
        }).ToList();

        BrandList = new ObservableCollection<string>(_allPresetsList.Select(p => p.Brand).Distinct());
        var filaments = _allPresetsList.Select(p => p.Type).Distinct()
            .Select(x => new FilamentTypeUIModel
            {
                FilamentType = x,
                Name = x.ToString()
            });
        FilamentList = new ObservableCollection<FilamentTypeUIModel>(filaments);

        var presetsUiModels = _allPresetsList
            .Where(p => p.Brand == BrandList[0] && p.Type == FilamentList[0].FilamentType);

        PresetsList = new ObservableCollection<PresetUIModel>(presetsUiModels);
        BrandListSelectedIndex = 0;
    }

    public IMvxCommand ReadAvailablePresetsCommand { get; }

    public IMvxCommand GeneratePresetCommand { get; set; }

    public IMvxCommand GenerateAllPresetsCommand { get; set; }

    public IMvxAsyncCommand SelectFolderWithTemplates { get; }

    public IMvxAsyncCommand SelectFolderCommand { get; }

    public IMvxCommand OpenFolderWithTemplatesCommand { get; }
    public IMvxCommand OpenFolderOutputCommand { get; }

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

    public ObservableCollection<string> BrandList
    {
        get => _brandList;
        set => SetProperty(ref _brandList, value);
    }

    public ObservableCollection<FilamentTypeUIModel> FilamentList
    {
        get => _filamentList;
        set => SetProperty(ref _filamentList, value);
    }

    public ObservableCollection<PresetUIModel> PresetsList
    {
        get => _presetsList;
        set => SetProperty(ref _presetsList, value);
    }

    public ObservableCollection<FilamentFileParamUIModel> PresetParamsList
    {
        get => _presetParamsList;
        set => SetProperty(ref _presetParamsList, value);
    }

    public int BrandListSelectedIndex
    {
        get => _brandListSelectedIndex;
        set
        {
            SetProperty(ref _brandListSelectedIndex, value);
            ApplyBrand();
        }
    }

    private void ApplyBrand()
    {
        if(FilamentList == null || FilamentList.Count == 0)
            return;

        var filamentList = _allPresetsList.Select(p => p.Type).Distinct()
            .Select(x => new FilamentTypeUIModel
            {
                FilamentType = x,
                Name = x.ToString()
            });
        FilamentList = new ObservableCollection<FilamentTypeUIModel>(filamentList);
        FilamentListSelectedIndex = 0;
    }

    public int FilamentListSelectedIndex
    {
        get => _listSelectedIndex;
        set
        {
            SetProperty(ref _listSelectedIndex, value);
            ApplyFilamentType();
        }
    }

    private void ApplyFilamentType()
    {
        if(PresetsList == null || PresetsList.Count == 0)
            return;

        if(_listSelectedIndex == -1)
            return;
        var presetsUiModels = _allPresetsList
            .Where(p => p.Brand == BrandList[_brandListSelectedIndex]
                        && p.Type == FilamentList[_listSelectedIndex].FilamentType);

        PresetsList = new ObservableCollection<PresetUIModel>(presetsUiModels);
        SerialSelectedIndex = _listSelectedIndex;
    }

    public int SerialSelectedIndex
    {
        get => _serialSelectedIndex;
        set
        {
            if(PresetsList == null || PresetsList.Count == 0)
                return;
            SetProperty(ref _serialSelectedIndex, value);
            if(_serialSelectedIndex == -1)
                return;
            ApplyPreset(PresetsList[_serialSelectedIndex]);
        }
    }

    private void ApplyPreset(PresetUIModel preset)
    {
        var presetPairs = ToKeyValuePairs(preset);

        var presetsParamsList = new List<FilamentFileParamUIModel>();
        foreach (var presetPairsKey in presetPairs.Keys)
        {
            presetsParamsList.Add(new FilamentFileParamUIModel()
            {
                Key = presetPairsKey,
                Value = presetPairs[presetPairsKey]
            });
        }

        PresetParamsList = new ObservableCollection<FilamentFileParamUIModel>(presetsParamsList);
    }

    public static Dictionary<string, string> ToKeyValuePairs(object obj)
    {
        var result = new Dictionary<string, string>();
        foreach (PropertyInfo prop in obj.GetType().GetProperties())
        {
            result[prop.Name] = prop.GetValue(obj, null).ToString();
        }
        return result;
    }


    public string FolderWithTemplatesPath
    {
        get => _folderWithTemplatesPath;
        set => SetProperty(ref _folderWithTemplatesPath, value);
    }

    public string OutputFolder
    {
        get => _outputFolder;
        set => SetProperty(ref _outputFolder, value);
    }
}