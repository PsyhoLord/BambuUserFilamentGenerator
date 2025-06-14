using BambuConfigGenerator.Contracts;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using Newtonsoft.Json;
using Excel = Microsoft.Office.Interop.Excel;

namespace BambuConfigGenerator.ExcelToJsonGenerator;

public class MsOfficeService // : IDisposable
{
    private string _filamentPresetFilePath = "FilamentPresets.xlsx";
    private string _filamentOutputFilePath = "FilamentPresets.json";

    public CorrectionParametersModel ReadDataFromExcel()
    {
        ProcessWorkbook();

        return null;
    }

    public void ProcessWorkbook()
    {
        var inputFilePath = @$"C:\Repos\BambuUserFilamentGenerator\Resources\{_filamentPresetFilePath}";
        var outputFilePath = @$"C:\Repos\BambuUserFilamentGenerator\Resources\{_filamentOutputFilePath}";
        Console.WriteLine(inputFilePath);

        Application excel = null;
        Workbook wkb = null;

        try
        {

            excel = new Application();

            var workbook = OpenBook(excel, inputFilePath, false, true, false);

            var plaCorrections = ReadCorrectionsFromExcel(workbook, "PLA");
            var petgCorrections = ReadCorrectionsFromExcel(workbook, "PETG");

            var allCorrections = new List<ExcelFilamentRowDataModel>();
            allCorrections.AddRange(plaCorrections);
            allCorrections.AddRange(petgCorrections);

            var jsonFileContent = JsonConvert.SerializeObject(allCorrections, Formatting.Indented);
            File.WriteAllText(outputFilePath, jsonFileContent);
        }
        catch (Exception ex)
        {
            //if you need to handle stuff
            Console.WriteLine(ex.Message);
        }
        finally
        {
            if (wkb != null)
                ReleaseRCM(wkb);

            if (excel != null)
                ReleaseRCM(excel);

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    private List<ExcelFilamentRowDataModel> ReadCorrectionsFromExcel(Workbook workbook, string sheetName)
    {
        var sheet = workbook?.Sheets[sheetName] as Worksheet;

        if(sheet == null)
            throw new ArgumentException($"Sheet with name {sheetName} does not exist in the workbook.");

        var listOfCorrections = new List<ExcelFilamentRowDataModel>();
        Excel.Range range = null;
        try
        {
            var dataPresent = true;
            var y = 2;
            do
            {
                range = sheet.Range[$"A{y}:J{y}", Missing.Value];
                if (range.Cells[1, 2].Value == null)
                {
                    dataPresent = false;
                }

                if (dataPresent)
                {
                    var row = new ExcelFilamentRowDataModel
                    {
                        Brand = range.Cells[1, 2].Value,
                        Type = range.Cells[1, 3].Value,
                        Serial = range.Cells[1, 4].Value,
                        RecommendedTempMin = range.Cells[1, 5].Value,
                        RecommendedTempMax = range.Cells[1, 6].Value,
                        NozzleInitialLayerTemp = range.Cells[1, 7].Value,
                        NozzleLayersTemp = range.Cells[1, 8].Value,
                        FFR = range.Cells[1, 9].Value,
                        K = range.Cells[1, 10].Value
                    };
                    listOfCorrections.Add(row);
                }
                ReleaseRCM(range);
                y++;
            } while (dataPresent);
            return listOfCorrections;
        }
        catch (Exception e)
        {
            throw;
        }
        finally
        {
            if (range != null)
                ReleaseRCM(range);
        }
    }

    public static Workbook OpenBook(Application excelInstance, string fileName, bool readOnly, bool editable,
        bool updateLinks)
    {
        Workbook book = excelInstance.Workbooks.Open(
            fileName, updateLinks, readOnly,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
            Type.Missing, editable, Type.Missing, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing);
        return book;
    }

    public static void ReleaseRCM(object o)
    {
        try
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
        }
        catch
        {
        }
        finally
        {
            o = null;
        }
    }
}