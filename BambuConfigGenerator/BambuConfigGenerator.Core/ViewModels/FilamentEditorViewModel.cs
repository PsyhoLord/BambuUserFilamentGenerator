using BambuConfigGenerator.Core.Models.UIModels;
using BambuConfigGenerator.Core.Services;
using BambuConfigGenerator.Core.Services.Interfaces;
using BambuConfigGenerator.Core.Services.PlatformSpecific;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Windows;

namespace BambuConfigGenerator.Core.ViewModels;

public class FilamentEditorViewModel : MvxViewModel
{
    private readonly IFileFolderPickerService _fileFolderPickerService;
    private readonly IIOService _ioService;
    private MvxObservableCollection<FileUIModel> _fileList;
    private int _selectedFileIndex;

    private string _selectedFolder;
    private MvxObservableCollection<FilamentFileParamUIModel> _paramsCollection;

    public FilamentEditorViewModel(IFileFolderPickerService fileFolderPickerService,
        IIOService ioService, ITemplateFolderAnalyserService templateFolderAnalyserService,
        IMvxNavigationService navigationService)
    {
        _fileFolderPickerService = fileFolderPickerService;
        _ioService = ioService;

        SelectFolderCommand = new MvxAsyncCommand(SelectOutputFolder);
        SaveCommand = new MvxCommand(SaveFilamentConfig);
        OpenFolderCommand = new MvxCommand(OpenFolder);
    }

    private void OpenFolder()
    {
        var files = _fileFolderPickerService.GetListOfFilesInFolder(SelectedFolder, "*.json");
        var fileList = new MvxObservableCollection<FileUIModel>();
        foreach (var file in files)
        {
            fileList.Add(new FileUIModel
            {
                FileName = System.IO.Path.GetFileName(file),
                FullFilePath = file
            });
        }

        FileList = fileList;
        SelectedFileIndex = 0;
        ReadFilamentConfig();
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

    private void ReadFilamentConfig()
    {
        var filamentConfig = _ioService.GetFilamentConfigValuePairs(FileList[SelectedFileIndex].FullFilePath);
        var filamentConfigUiModel = new MvxObservableCollection<FilamentFileParamUIModel>();

        foreach (var config in filamentConfig)
        {
            filamentConfigUiModel.Add(new FilamentFileParamUIModel
            {
                Key = config.Key,
                Value = config.Value
            });
        }

        FilamentParamsCollection = filamentConfigUiModel;
    }

    private void SaveFilamentConfig()
    {
        _ioService.SaveFilamentConfigValuePairs(FileList[SelectedFileIndex].FullFilePath, FilamentParamsCollection);
        MessageBox.Show("Success!!!", "WooHoo!!!", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public IMvxAsyncCommand SelectFolderCommand { get; }
    public IMvxCommand SaveCommand { get; }

    public IMvxCommand OpenFolderCommand { get; }

    private async Task SelectOutputFolder()
    {
        var path = await _fileFolderPickerService.SelectFolder(string.Empty);
        SelectedFolder = path;
        OpenFolder();
    }

    public MvxObservableCollection<FilamentFileParamUIModel> FilamentParamsCollection
    {
        get => _paramsCollection;
        set => SetProperty(ref _paramsCollection, value);
    }
}