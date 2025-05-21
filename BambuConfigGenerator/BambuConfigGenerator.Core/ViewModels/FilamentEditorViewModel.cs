using System.IO;
using System.Windows;
using BambuConfigGenerator.Core.Models.UIModels;
using BambuConfigGenerator.Core.Services.Interfaces;
using BambuConfigGenerator.Core.Services.PlatformSpecific;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace BambuConfigGenerator.Core.ViewModels;

public class FilamentEditorViewModel : MvxViewModel
{
    private readonly IFileFolderPickerService _fileFolderPickerService;
    private readonly IIOService _ioService;
    private readonly IUserSettingsService _userSettingsService;
    private MvxObservableCollection<FileUIModel> _fileList;
    private MvxObservableCollection<FilamentFileParamUIModel> _paramsCollection;
    private int _selectedFileIndex;

    private string _selectedFolder;

    public FilamentEditorViewModel(IFileFolderPickerService fileFolderPickerService,
        IIOService ioService, ITemplateFolderAnalyserService templateFolderAnalyserService,
        IMvxNavigationService navigationService, IUserSettingsService userSettingsService)
    {
        _fileFolderPickerService = fileFolderPickerService;
        _ioService = ioService;
        _userSettingsService = userSettingsService;

        SelectFolderCommand = new MvxAsyncCommand(SelectOutputFolder);
        SaveCommand = new MvxCommand(SaveFilamentConfig);
        OpenFolderCommand = new MvxCommand(OpenFolder);

        var config = _userSettingsService.UserSettings;
        SelectedFolder = config.FolderToBambuLabUserFilaments;
    }

    public string SelectedFolder
    {
        get => _selectedFolder;
        set => SetProperty(ref _selectedFolder, value);
    }

    public MvxObservableCollection<FileUIModel> FileList
    {
        get => _fileList;
        set => SetProperty(ref _fileList, value);
    }

    public int SelectedFileIndex
    {
        get => _selectedFileIndex;
        set
        {
            SetProperty(ref _selectedFileIndex, value);
            ReadFilamentConfig();
        }
    }

    public IMvxAsyncCommand SelectFolderCommand { get; }

    public IMvxCommand SaveCommand { get; }

    public IMvxCommand OpenFolderCommand { get; }

    public MvxObservableCollection<FilamentFileParamUIModel> FilamentParamsCollection
    {
        get => _paramsCollection;
        set => SetProperty(ref _paramsCollection, value);
    }

    private void OpenFolder()
    {
        var files = _fileFolderPickerService.GetListOfFilesInFolder(SelectedFolder, "*.json");
        var fileList = new MvxObservableCollection<FileUIModel>();
        foreach (var file in files)
            fileList.Add(new FileUIModel
            {
                FileName = Path.GetFileName(file),
                FullFilePath = file
            });

        FileList = fileList;
        SelectedFileIndex = 0;
        ReadFilamentConfig();
    }

    private void ReadFilamentConfig()
    {
        var filamentConfig = _ioService.GetFilamentConfigValuePairs(FileList[SelectedFileIndex].FullFilePath);
        var filamentConfigUiModel = new MvxObservableCollection<FilamentFileParamUIModel>();

        foreach (var config in filamentConfig)
            filamentConfigUiModel.Add(new FilamentFileParamUIModel
            {
                Key = config.Key,
                Value = config.Value
            });

        FilamentParamsCollection = filamentConfigUiModel;
    }

    private void SaveFilamentConfig()
    {
        _ioService.SaveFilamentConfigValuePairs(FileList[SelectedFileIndex].FullFilePath, FilamentParamsCollection);
        MessageBox.Show("Success!!!", "WooHoo!!!", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private async Task SelectOutputFolder()
    {
        var path = await _fileFolderPickerService.SelectFolder(SelectedFolder);
        if(string.IsNullOrEmpty(path))
            return;
        SelectedFolder = path;
        OpenFolder();
    }
}