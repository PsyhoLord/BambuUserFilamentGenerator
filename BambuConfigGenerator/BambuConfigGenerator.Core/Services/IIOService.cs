using BambuConfigGenerator.Models;
using System.IO;
using System.Text.Json;

namespace BambuConfigGenerator.Core.Services;

internal interface IIOService
{
    public FilamentModel GetDefaultFilamentModel(string filePath);
}