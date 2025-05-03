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
    public class TagRefReport
    {
        public string TableName {  get; set; }

        /// <summary>
        /// Gets or sets the reference data for tags.
        /// The outer dictionary is grouped by tag address type,
        /// the inner dictionary maps reference count to occurrence count.
        /// </summary>
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
