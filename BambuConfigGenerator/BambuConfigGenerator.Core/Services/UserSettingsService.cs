using System.IO;
using BambuConfigGenerator.Core.Models;
using BambuConfigGenerator.Core.Services.Interfaces;
using BambuConfigGenerator.Core.Services.PlatformSpecific;

namespace BambuConfigGenerator.Core.Services;

public class UserSettingsService : IUserSettingsService
{
    private readonly IFileFolderPickerService _fileFolderPickerService;
    private readonly IIOService _ioService;
    private const string DefaultFolderToBambuLabApp = @"C:\Program Files\Bambu Studio";
    private string DefaultFolderToBambuLabUserApp = @$"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\BambuStudio";
    private string DefaultFolderToBambuLabUsers => $@"{DefaultFolderToBambuLabUserApp}\user";

    public string CurrentUserId { get; set; }

    public UserSettingsModel UserSettings { get; set; }

    public UserSettingsService(IFileFolderPickerService fileFolderPickerService, IIOService ioService)
    {
        _fileFolderPickerService = fileFolderPickerService;
        _ioService = ioService;

        if (!LoadUserConfigFromFile())
        {
            LoadDefaults();
        }
    }

    private void LoadDefaults()
    {
        UserSettings = new UserSettingsModel();

        var currentUserId = GetCurrentUserId();
        var folderToBambuLabUserFilament = string.Empty;
        if (!string.IsNullOrEmpty(currentUserId))
        {
            CurrentUserId = currentUserId;

            var filamentDirectory = @$"{DefaultFolderToBambuLabUsers}\{currentUserId}\filament";

            if (Directory.Exists(filamentDirectory))
            {
                var filamentBase = @$"{filamentDirectory}\base";
                if (Directory.Exists(filamentBase))
                {
                    folderToBambuLabUserFilament = filamentBase;
                }
                else
                {
                    Directory.CreateDirectory(filamentBase);
                    folderToBambuLabUserFilament = filamentBase;
                }
            }
        }

        UserSettings.UserId = currentUserId;
        UserSettings.FolderToBambuLabApp = DefaultFolderToBambuLabApp;
        UserSettings.FolderToBambuLabUserApp = DefaultFolderToBambuLabUserApp;
        UserSettings.FolderToBambuLabUsers = DefaultFolderToBambuLabUsers;
        UserSettings.FolderToBambuLabUserFilaments = folderToBambuLabUserFilament;
        SaveUserConfig();
    }

    public void UpdateUserConfig(UserSettingsModel settings)
    {
        _ioService.SaveUserSettings(settings);
    }

    private void SaveUserConfig()
    {
        _ioService.SaveUserSettings(UserSettings);
    }

    private bool LoadUserConfigFromFile()
    {
        try
        {
            var userSettings = _ioService.LoadUserSettingsModel();
            if (userSettings == null)
                return false;

            UserSettings = userSettings;

            return true;
        }
        catch (Exception e)
        {
            //ignore
            return false;
        }
    }

    private string GetCurrentUserId()
    {
        var folders = _fileFolderPickerService.GetListOfFoldersInDirectory(DefaultFolderToBambuLabUsers);
        
        if (folders.Count > 2)
            return string.Empty;

        var userFolderPath = folders.FirstOrDefault(f => f != "default");

        return Path.GetFileName(userFolderPath);
    }
}