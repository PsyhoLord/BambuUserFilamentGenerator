// See https://aka.ms/new-console-template for more information

using BambuConfigGenerator.ExcelToJsonGenerator;

Console.WriteLine("Hello, World!");

MsOfficeService msOfficeService = new MsOfficeService();
msOfficeService.ReadDataFromExcel();
