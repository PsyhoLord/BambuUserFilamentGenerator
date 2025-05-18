using System.Collections.ObjectModel;
using BambuConfigGenerator.Core.Models.UIModels;

namespace BambuConfigGenerator.Core.Services;

public interface ITemplateFolderAnalyserService
{
    public string PathToTemplates { get; set; }

    public bool IsValid { get; set; }

    public ObservableCollection<FilamentTypeUIModel> FilamentTypes { get; set; }

    public bool SetFolderPath(string path);
}