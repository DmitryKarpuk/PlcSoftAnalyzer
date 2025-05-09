using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using PlcSoftAnalyzer.Interfaces;
using PlcSoftAnalyzer.Model;

namespace PlcSoftAnalyzer.Services
{
    public class ExcelReportService : IExcelReportService
    {
        public ExcelReportService() { }
        public void PrintRefAnaylyzerReport(string fileName, List<TagTableRefReport> TagTableRefReportSource)
        {
            string[] titles = new string[4] { "Tag name", "HW type", "HW address", "Amount of references" };
            var wb = new XLWorkbook();
            foreach(var table in TagTableRefReportSource)
            {
                if (table.RefOutOfLimitData.Count > 0)
                {
                    var ws = wb.AddWorksheet(table.Name);
                    ws = AddTitles(ws, titles);
                    ws.Cell("A2").InsertData(table.RefOutOfLimitData);
                }
            }
            wb.SaveAs(fileName);
        }

        private IXLWorksheet AddTitles(IXLWorksheet ws, string[] titles)
        {
            for (int i = 0; i < titles.Count(); i++)
            {
                ws.Cell(1, i + 1).Value = titles[i];
                ws.Cell(1, i + 1).Style.Font.SetBold(true);
                ws.Column(i + 1).Width = titles[i].Length + 5;
            }
            return ws;

        }
    }
}
