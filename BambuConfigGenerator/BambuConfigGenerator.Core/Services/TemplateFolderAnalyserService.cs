using System.Collections.ObjectModel;
using System.IO;
using BambuConfigGenerator.Core.Models.UIModels;
using BambuConfigGenerator.Core.Services.Interfaces;
using Path = System.IO.Path;
using Enums = BambuConfigGenerator.Contracts.Enums; // Assuming Enums is in the Contracts namespace

namespace BambuConfigGenerator.Core.Services;

public class TemplateFolderAnalyserService : ITemplateFolderAnalyserService
{
    private List<string> _listOfFilesInFolder;

    public string PathToTemplates { get; set; }

    public bool IsValid { get; set; }

    public ObservableCollection<FilamentTypeUIModel> FilamentTypes { get; set; }

    public bool SetFolderPath(string path)
    {
        PathToTemplates = path;
        Validate();

        if (IsValid)
            GetFilamentTypes();

        return IsValid;
    }

    private void Validate()
    {
        ListFilesInTemplatesFolder(PathToTemplates);


        if (_listOfFilesInFolder?.Count == 0) IsValid = false;

        IsValid = true;
    }

    private void ListFilesInTemplatesFolder(string folderPath)
    {
        _listOfFilesInFolder = default;

        try
        {
            _listOfFilesInFolder = Directory.GetFiles(folderPath, "*.json").ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private List<string> GetUniqueSecondWords(List<string> filesList)
    {
        if (filesList == null || filesList.Count == 0) return new List<string>();

        var filamentWords = filesList
            .Select(file => Path.GetFileNameWithoutExtension(file)) // Get file name without extension
            .Select(GetFilamentTypeWord) // Extract the second word
            .Distinct() // Ensure uniqueness
            .ToList();

        return filamentWords;
    }

    private string GetFilamentTypeWord(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input string cannot be null or empty.", nameof(input));

        var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (words.Length < 8) throw new InvalidOperationException("The input string does not contain a second word.");

        return words[1]; // Return the second word (index 1)
    }

    private void GetFilamentTypes()
    {
        var uniqueSecondWords = GetUniqueSecondWords(_listOfFilesInFolder);

        if (uniqueSecondWords.Count == 0) IsValid = false;

        FilamentTypes = new ObservableCollection<FilamentTypeUIModel>();

        foreach (var word in uniqueSecondWords)
            switch (word)
            {
                case "ABS":
                    FilamentTypes.Add(new FilamentTypeUIModel { Name = "ABS", FilamentType = Enums.FilamentTypes.ABS });
                    break;
                case "ABS-GF":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "ABS-GF", FilamentType = Enums.FilamentTypes.ABS_GF });
                    break;
                case "PETG":
                    FilamentTypes.Add(
                        new FilamentTypeUIModel { Name = "PETG", FilamentType = Enums.FilamentTypes.PETG });
                    break;
                case "PET-CF":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "PET-CF", FilamentType = Enums.FilamentTypes.PET_CF });
                    break;
                case "PLA":
                    FilamentTypes.Add(new FilamentTypeUIModel { Name = "PLA", FilamentType = Enums.FilamentTypes.PLA });
                    break;
                case "PA-CF":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "PA-CF", FilamentType = Enums.FilamentTypes.PA_CF });
                    break;
                case "PC":
                    FilamentTypes.Add(new FilamentTypeUIModel { Name = "PC", FilamentType = Enums.FilamentTypes.PC });
                    break;
                case "ASA":
                    FilamentTypes.Add(new FilamentTypeUIModel { Name = "ASA", FilamentType = Enums.FilamentTypes.ASA });
                    break;
                case "ASA-CF":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "ASA-CF", FilamentType = Enums.FilamentTypes.ASA_CF });
                    break;
                case "ASA-AERO":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "ASA-AERO", FilamentType = Enums.FilamentTypes.ASA_AERO });
                    break;
                case "PA6-CF":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "PA6-CF", FilamentType = Enums.FilamentTypes.PA6_CF });
                    break;
                case "PA—GF":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "PA—GF", FilamentType = Enums.FilamentTypes.PA_GF });
                    break;
                case "PETG-CF":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "PETG-CF", FilamentType = Enums.FilamentTypes.PETG_CF });
                    break;
                case "PLA-AERO":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "PLA-AERO", FilamentType = Enums.FilamentTypes.PLA_AERO });
                    break;
                case "PLA-CF":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "PLA-CF", FilamentType = Enums.FilamentTypes.PLA_CF });
                    break;
                case "PPS":
                    FilamentTypes.Add(new FilamentTypeUIModel { Name = "PPS", FilamentType = Enums.FilamentTypes.PPS });
                    break;
                case "PPA-CF":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "PPA-CF", FilamentType = Enums.FilamentTypes.PPA_CF });
                    break;
                case "PPA-GF":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "PPA-GF", FilamentType = Enums.FilamentTypes.PPA_GF });
                    break;
                case "PPS-CF":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "PPS-CF", FilamentType = Enums.FilamentTypes.PPS_CF });
                    break;
                case "BVOH":
                    FilamentTypes.Add(
                        new FilamentTypeUIModel { Name = "BVOH", FilamentType = Enums.FilamentTypes.BVOH });
                    break;
                case "PVA":
                    FilamentTypes.Add(new FilamentTypeUIModel { Name = "PVA", FilamentType = Enums.FilamentTypes.PVA });
                    break;
                case "PA":
                    FilamentTypes.Add(new FilamentTypeUIModel { Name = "PA", FilamentType = Enums.FilamentTypes.PA });
                    break;
                case "TPU":
                    FilamentTypes.Add(new FilamentTypeUIModel { Name = "TPU", FilamentType = Enums.FilamentTypes.TPU });
                    break;
                case "TPU-AMS":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "TPU-AMS", FilamentType = Enums.FilamentTypes.TPU_AMS });
                    break;
                case "EVA":
                    FilamentTypes.Add(new FilamentTypeUIModel { Name = "EVA", FilamentType = Enums.FilamentTypes.EVA });
                    break;
                case "HIPS":
                    FilamentTypes.Add(
                        new FilamentTypeUIModel { Name = "HIPS", FilamentType = Enums.FilamentTypes.HIPS });
                    break;
                case "PCTG":
                    FilamentTypes.Add(
                        new FilamentTypeUIModel { Name = "PCTG", FilamentType = Enums.FilamentTypes.PCTG });
                    break;
                case "PE—CF":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "PE—CF", FilamentType = Enums.FilamentTypes.PE_CF });
                    break;
                case "PHA":
                    FilamentTypes.Add(new FilamentTypeUIModel { Name = "PHA", FilamentType = Enums.FilamentTypes.PHA });
                    break;
                case "PP—CF":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "PP—CF", FilamentType = Enums.FilamentTypes.PP_CF });
                    break;
                case "PP":
                    FilamentTypes.Add(new FilamentTypeUIModel { Name = "PP", FilamentType = Enums.FilamentTypes.PP });
                    break;
                case "PP—GF":
                    FilamentTypes.Add(new FilamentTypeUIModel
                        { Name = "PP—GF", FilamentType = Enums.FilamentTypes.PP_GF });
                    break;
            }
    }
}