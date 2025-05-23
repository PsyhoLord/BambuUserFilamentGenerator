using System.Reflection;
using BambuConfigGenerator.Core.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace BambuConfigGenerator.Core.Services;

public class MsOfficeService // : IDisposable
{
    private string _filamentPresetFilePath = "Copy of FilamentPresets.xlsx";

    public CorrectionParametersModel ReadDataFromExcel()
    {
        ProcessWorkbook();

        return null;
    }

    public void ProcessWorkbook()
    {
        string file = @$"C:\Repos\BambuUserFilamentGenerator\Resources\{_filamentPresetFilePath}";
        Console.WriteLine(file);

        Excel.Application excel = null;
        Excel.Workbook wkb = null;

        try
        {

            excel = new Excel.Application();

            wkb = OpenBook(excel, file, false, true, false);

            /*Excel.Worksheet sheet = wkb.Sheets["PLA"] as Excel.Worksheet;
            Excel.Range range = sheet.Range["A1", "J1"];
            
            if (range != null)
                foreach (Excel.Range r in range)
                {
                    var x = r.Value;
                    Console.WriteLine(r.Text);
                    Console.WriteLine(r.Value);
                }

            ReleaseRCM(range);*/

            ReadPlaWorkSheet(wkb);
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

    private void ReadPlaWorkSheet(Excel.Workbook workbook)
    {
        Excel.Worksheet sheet = workbook.Sheets["PLA"] as Excel.Worksheet;

        var listOfCorrections = new List<ExcelFilamentRowDataModel>();

        var startIndex = 2;
        var lineIndex = 2;

        do
        {
            var range = sheet.Range[$"A{lineIndex}", $"J{lineIndex}"];

            try
            {
                if (range != null)
                {
                    var row = new ExcelFilamentRowDataModel
                    {
                        Brand = range.Cells[1, 1].Value,
                        Type = range.Cells[1, 2].Value,
                        Serial = range.Cells[1, 3].Value,
                        RecommendedTempMin = range.Cells[1, 4].Value,
                        RecommendedTempMax = range.Cells[1, 5].Value,
                        NozzleInitialLayerTemp = range.Cells[1, 6].Value,
                        NozzleLayersTemp = range.Cells[1, 7].Value,
                        FFR = range.Cells[1, 8].Value,
                        K = range.Cells[1, 9].Value
                        //Brand = range[0].Value,
                        //Type = range[1].Value,
                        //Serial = range[2].Value,
                        //RecommendedTempMin = range[3].Value,
                        //RecommendedTempMax = range[4].Value,
                        //NozzleInitialLayerTemp = range[5].Value,
                        //NozzleLayersTemp = range[6].Value,
                        //FFR = range[7].Value,
                        //K = range[8].Value
                    };
                    listOfCorrections.Add(row);
                    lineIndex++;
                }
                else
                {
                    break;
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                ReleaseRCM(range);
            }

            lineIndex++;
        } while (lineIndex < 2);
    }

    public static Excel.Workbook OpenBook(Excel.Application excelInstance, string fileName, bool readOnly, bool editable,
        bool updateLinks)
    {
        Excel.Workbook book = excelInstance.Workbooks.Open(
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