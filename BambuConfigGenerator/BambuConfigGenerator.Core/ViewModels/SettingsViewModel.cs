using BambuConfigGenerator.Core.Models;
using BambuConfigGenerator.Core.Services.Interfaces;
using BambuConfigGenerator.Core.Services.PlatformSpecific;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Diagnostics;

namespace BambuConfigGenerator.Core.ViewModels;

public class SettingsViewModel : MvxViewModel
{
    private readonly IFileFolderPickerService _fileFolderPickerService;
    private readonly IUserSettingsService _userSettingsService;
    private string _userId;
    private string _folderToBambuLabApp;
    private string _folderToBambuLabUserApp;
    private string _folderToBambuLabUsers;
    private string _folderToBambuLabUserFilaments;

    public SettingsViewModel(IFileFolderPickerService fileFolderPickerService,
        IIOService ioService, ITemplateFolderAnalyserService templateFolderAnalyserService,
        IMvxNavigationService navigationService, IUserSettingsService userSettingsService)
    {
        _fileFolderPickerService = fileFolderPickerService;
        _userSettingsService = userSettingsService;

        SaveCommand = new MvxCommand(SaveUserSettings);
        SelectFolderToBambuLabAppCommand = new MvxAsyncCommand(SelectFolderToBambuLabApp);
        OpenFolderToBambuLabAppCommand = new MvxCommand(() => Process.Start("explorer.exe", FolderToBambuLabApp));
        ClearFolderToBambuLabAppCommand = new MvxCommand(() => FolderToBambuLabApp = string.Empty);

        SelectFolderToBambuLabUserAppCommand = new MvxAsyncCommand(SelectFolderToBambuLabUserApp);
        OpenFolderToBambuLabUserAppCommand = new MvxCommand(() => Process.Start("explorer.exe", FolderToBambuLabUserApp));
        ClearFolderToBambuLabUserAppCommand = new MvxCommand(() => FolderToBambuLabUserApp = string.Empty);

        SelectFolderToBambuLabUsersCommand = new MvxAsyncCommand(SelectFolderToBambuLabUsers);
        OpenFolderToBambuLabUsersCommand = new MvxCommand(() => Process.Start("explorer.exe", FolderToBambuLabUsers));
        ClearFolderToBambuLabUsersCommand = new MvxCommand(() => FolderToBambuLabUsers = string.Empty);

        SelectFolderToBambuLabUserFilamentsCommand = new MvxAsyncCommand(SelectFolderToBambuLabUserFilaments);
        OpenFolderToBambuLabUserFilamentsCommand = new MvxCommand(() => Process.Start("explorer.exe", FolderToBambuLabUserFilaments));
        ClearFolderToBambuLabUserFilamentsCommand = new MvxCommand(() => FolderToBambuLabUserFilaments = string.Empty);
        Init();
    }

    private async Task SelectFolderToBambuLabUserFilaments()
    {
        FolderToBambuLabUserFilaments = await SelectFolder(FolderToBambuLabUsers);
    }

    private async Task SelectFolderToBambuLabUsers()
    {
        FolderToBambuLabUsers = await SelectFolder(FolderToBambuLabUsers);
    }

    private async Task SelectFolderToBambuLabUserApp()
    {
        FolderToBambuLabApp = await SelectFolder(FolderToBambuLabUserApp);
    }

    private async Task SelectFolderToBambuLabApp()
    {
        FolderToBambuLabApp = await SelectFolder(FolderToBambuLabApp);
    }

    private async Task<string> SelectFolder(string initialPath)
    {
        return await _fileFolderPickerService.SelectFolder(initialPath);
    }

    private void SaveUserSettings()
    {
        var userSettings = new UserSettingsModel
        {
            UserId = UserId,
            FolderToBambuLabApp = FolderToBambuLabApp,
            FolderToBambuLabUserApp = FolderToBambuLabUserApp,
            FolderToBambuLabUsers = FolderToBambuLabUsers,
            FolderToBambuLabUserFilaments = FolderToBambuLabUserFilaments
        };

        _userSettingsService.UpdateUserConfig(userSettings);
    }

    private void Init()
    {
        var userSettings = _userSettingsService.UserSettings;

        UserId = userSettings.UserId;
        FolderToBambuLabApp = userSettings.FolderToBambuLabApp;
        FolderToBambuLabUserApp = userSettings.FolderToBambuLabUserApp;
        FolderToBambuLabUsers = userSettings.FolderToBambuLabUsers;
        FolderToBambuLabUserFilaments = userSettings.FolderToBambuLabUserFilaments;
    }

    public string UserId
    {
        get => _userId;
        set => SetProperty(ref _userId, value);
    }

    public string FolderToBambuLabApp
    {
        get => _folderToBambuLabApp;
        set => SetProperty(ref _folderToBambuLabApp, value);
    }

    public string FolderToBambuLabUserApp
    {
        get => _folderToBambuLabUserApp;
        set => SetProperty(ref _folderToBambuLabUserApp, value);
    }

    public string FolderToBambuLabUsers
    {
        get => _folderToBambuLabUsers;
        set => SetProperty(ref _folderToBambuLabUsers, value);
    }

    public string FolderToBambuLabUserFilaments
    {
        get => _folderToBambuLabUserFilaments;
        set => SetProperty(ref _folderToBambuLabUserFilaments, value);
    }



    public IMvxCommand SaveCommand { get; }

    public IMvxAsyncCommand SelectFolderToBambuLabAppCommand { get; }
    public IMvxCommand OpenFolderToBambuLabAppCommand { get; }
    public IMvxCommand ClearFolderToBambuLabAppCommand { get; }

    public IMvxAsyncCommand SelectFolderToBambuLabUserAppCommand { get; }
    public IMvxCommand OpenFolderToBambuLabUserAppCommand { get; }
    public IMvxCommand ClearFolderToBambuLabUserAppCommand { get; }

    public IMvxAsyncCommand SelectFolderToBambuLabUsersCommand { get; }
    public IMvxCommand OpenFolderToBambuLabUsersCommand { get; }
    public IMvxCommand ClearFolderToBambuLabUsersCommand { get; }

    public IMvxAsyncCommand SelectFolderToBambuLabUserFilamentsCommand { get; }
    public IMvxCommand OpenFolderToBambuLabUserFilamentsCommand { get; }
    public IMvxCommand ClearFolderToBambuLabUserFilamentsCommand { get; }
}