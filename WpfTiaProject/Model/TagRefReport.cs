using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siemens.Engineering.SW.Tags;

namespace WpfTiaProject.Model
{
    public class TagRefReport
    {
        public string TableName {  get; set; }
        public SortedDictionary<int, int> TagsRefRate { get; set; }
        public TagRefReport() { }
        public TagRefReport(string Name, SortedDictionary<int, int> tagsRefRate)
        {
            TableName = Name;
            TagsRefRate = tagsRefRate;
        }
    }
}
