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
        public Dictionary<TagAddressType, SortedDictionary<int, int>> TagsRefData { get; set; }
        public TagRefReport() { }
        public TagRefReport(string Name)
        {
            TableName = Name;
            TagsRefData = new Dictionary<TagAddressType, SortedDictionary<int, int>>();
            foreach(var type in Enum.GetValues(typeof(TagAddressType)))
            {
                TagsRefData.Add((TagAddressType)type, new SortedDictionary<int, int>());
            }

        }
        public TagRefReport(string Name, Dictionary<TagAddressType, SortedDictionary<int, int>> tagsRefRate)
        {
            TableName = Name;
            TagsRefData = tagsRefRate;
        }
    }
}
