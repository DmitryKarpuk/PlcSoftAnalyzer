using System;
using System.Collections.Generic;
using ClosedXML.Excel;
using PlcSoftAnalyzer.Model;

namespace PlcSoftAnalyzer.Services
{
    public class ExcelReportService
    {
        public void PrintRefAnaylyzerReport(string fileName, List<TagTableRefReport> TagTableRefReportSource)
        {
            var wb = new XLWorkbook();
            foreach(var table in TagTableRefReportSource)
            {
                wb.Worksheets.Add(table.Name);              
            }
        }
    }
}
