namespace BambuConfigGenerator.Core.Services.PlatformSpecific;

public interface IFileFolderPickerService
{
    public Task<string> SelectFile(string initialPath);

    public Task<string> SelectFolder(string initialPath);

    public List<string> GetListOfFoldersInDirectory(string path);

    public List<string> GetListOfFilesInFolder(string path, string format = null);
}