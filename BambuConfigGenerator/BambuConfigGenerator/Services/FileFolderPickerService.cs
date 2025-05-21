using BambuConfigGenerator.Core.Services.PlatformSpecific;
using Microsoft.Win32;
using System.Windows;

namespace BambuConfigGenerator.Services;

public class FileFolderPickerService : IFileFolderPickerService
{
    public async Task<string> SelectFile(string initialPath)
    {
        var openFileDialog = new OpenFileDialog
        {
            InitialDirectory = initialPath,
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            string selectedFilePath = openFileDialog.FileName;
            MessageBox.Show($"Selected file: {selectedFilePath}", "File Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            return selectedFilePath;
        }

        return null;
    }

    public List<string> GetListOfFoldersInDirectory(string path)
    {
        var folders = new List<string>();
        try
        {
            folders = System.IO.Directory.GetDirectories(path).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        return folders;
    }

    public List<string> GetListOfFilesInFolder(string path, string format = null)
    {
        var files = new List<string>();
        try
        {
            files = string.IsNullOrEmpty(format)
               ? System.IO.Directory.GetFiles(path).ToList()
               : System.IO.Directory.GetFiles(path, format).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        return files;
    }

    public async Task<string> SelectFolder(string initialPath)
    {
        // Create a "Save As" dialog for selecting a directory (HACK)
        var dialog = new SaveFileDialog
        {
            // dialog.InitialDirectory = textbox.Text; // Use current value for initial dir
            Title = "Select a Directory", // instead of default "Save As"
            Filter = "Directory|*.this.directory", // Prevents displaying files
            FileName = "select" // Filename will then be "select.this.directory"
        };

        if (dialog.ShowDialog() == true)
        {
            string path = dialog.FileName;
            // Remove fake filename from resulting path
            path = path.Replace("\\select.this.directory", "");
            path = path.Replace(".this.directory", "");
            // If user has changed the filename, create the new directory
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            // Our final value is in path
            // textbox.Text = path;
            return path;
        }

        return string.Empty;
    }
}