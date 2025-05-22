using BambuConfigGenerator.Core.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace BambuConfigGenerator.Core.Services;

public class MsOfficeService
{
    private string _filamentPresetFilePath = "FilamentPreset.xlsx";

    public CorrectionParametersModel ReadDataFromExcel()
    {
        ProcessWorkbook();

        return null;
    }

    public void ProcessWorkbook()
    {
        string file = @"C:\Users\Chris\Desktop\TestSheet.xls";
        Console.WriteLine(file);

        Excel.Application excel = null;
        Excel.Workbook wkb = null;

        try
        {
            excel = new Excel.Application();

            wkb = OpenBook(excel, file, false, true, false);

            Excel.Worksheet sheet = wkb.Sheets["PLA"] as Excel.Worksheet;

            Excel.Range range = null;

            // Excel.Range range1 = sheet.Range["A1:A5", Missing.Value];
            Excel.Range range1 = sheet.Range["A1", "A5"];

            //if (range1 != null)
            //    foreach (Excel.Range r in range1)
            //    {
            //        string user = r.Text;
            //        string value = r.Value2;
            //    }

            /*if (sheet != null)
                range = sheet.get_Range("A1", Missing.Value);

            string A1 = String.Empty;

            if (range != null)
                A1 = range.Text.ToString();*/

            Console.WriteLine("A1 value: {0}", A1);
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
        }
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