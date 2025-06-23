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

        /// <summary>
        /// Write in the excel information of all tags out
        /// of the references limit.
        /// </summary>
        /// <param name="fileName"> Result file path</param>
        /// <param name="TagTableRefReportSource"> Data source of the tags</param>
        public void PrintRefAnaylyzerReport(string fileName, List<TagTableRefReport> TagTableRefReportSource)
        {
            string[] titles = new string[4] { "Tag name", "HW type", "HW address", "Amount of references" };
            var wb = new XLWorkbook();
            foreach(var table in TagTableRefReportSource)
            {
                var sortedTable = table.RefOutOfLimitData.OrderBy(t => t.ReferenceAmount)
                                                         .ToList();
                if (table.RefOutOfLimitData.Count > 0)
                {
                    var ws = wb.AddWorksheet(table.Name);
                    ws = AddTitles(ws, titles);
                    ws.Cell("A2").InsertData(sortedTable);
                }
            }
            wb.SaveAs(fileName);
        }

        /// <summary>
        /// Add title to the Excel sheet.
        /// </summary>
        /// <param name="ws"> Excel sheet.</param>
        /// <param name="titles">Titles</param>
        /// <returns></returns>
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
