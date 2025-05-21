using BambuConfigGenerator.Core.Models;

namespace BambuConfigGenerator.Core.Services.Interfaces;

public interface IUserSettingsService
{
    string CurrentUserId { get; set; }
    UserSettingsModel UserSettings { get; set; }
    void UpdateUserConfig(UserSettingsModel settings);
}