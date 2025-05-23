using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcSoftAnalyzer.Model;

namespace PlcSoftAnalyzer.Interfaces
{
    public interface IExcelReportService
    {
        public void PrintRefAnaylyzerReport(string fileName, List<TagTableRefReport> TagTableRefReportSource);
    }
}
