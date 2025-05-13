using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Siemens.Engineering.SW.Tags;
using PlcSoftAnalyzer.Model;

namespace PlcSoftAnalyzer.Interfaces
{
    public interface ITagRefAnalyzerService
    {
        List<TagTableRefReport> TagTableRefReportSource { get; }
        Dictionary<TagAddressType, int> LimitsMap { get; }
        void LoadTagRefOutOfLimitData(List<PlcTagTable> tables, CancellationToken token);
    }
}
