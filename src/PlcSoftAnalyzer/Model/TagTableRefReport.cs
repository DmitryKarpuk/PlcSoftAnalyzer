using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siemens.Engineering.SW.Tags;

namespace PlcSoftAnalyzer.Model
{
    /// <summary>
    /// Represents a reference report for a PLC tag table.
    /// Holds the table name and structured reference data grouped by tag address type.
    /// </summary>
    public class TagTableRefReport
    {
        public string Name {  get; set; }
        public int TagsAmount { get; set; }

        public List<PlcTagInfo> RefOutOfLimitData { get; set; }
        public TagTableRefReport() { }
        public TagTableRefReport(string name, int tagsAmount)
        {
            Name = name;
            TagsAmount = tagsAmount;
            RefOutOfLimitData = new List<PlcTagInfo>();
        }
        public TagTableRefReport(string Name, List<PlcTagInfo> refOutOfLimitData)
        {
            this.Name = Name;
            RefOutOfLimitData = refOutOfLimitData;
        }
    }
}
