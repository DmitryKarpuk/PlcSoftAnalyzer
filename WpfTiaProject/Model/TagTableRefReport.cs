using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siemens.Engineering.SW.Tags;

namespace WpfTiaProject.Model
{
    /// <summary>
    /// Represents a reference report for a PLC tag table.
    /// Holds the table name and structured reference data grouped by tag address type.
    /// </summary>
    public class TagTableRefReport
    {
        public string TableName {  get; set; }
        public int TagsAmount { get; set; }

        public Dictionary<TagAddressType, List<PlcTag>> RefOutOfLimitData { get; set; }
        public TagTableRefReport() { }
        public TagTableRefReport(string name, int tagsAmount)
        {
            TableName = name;
            TagsAmount = tagsAmount;
            RefOutOfLimitData = new Dictionary<TagAddressType, List<PlcTag>>();
        }
        public TagTableRefReport(string Name, Dictionary<TagAddressType, List<PlcTag>> refOutOfLimitData)
        {
            TableName = Name;
            RefOutOfLimitData = refOutOfLimitData;
        }
    }
}
