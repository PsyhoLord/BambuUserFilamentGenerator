namespace BambuConfigGenerator.Core.Services.PlatformSpecific;

public interface IFileFolderPickerService
{
    public Task<string> SelectFile(string initialPath);

    public Task<string> SelectFolder(string initialPath);
}