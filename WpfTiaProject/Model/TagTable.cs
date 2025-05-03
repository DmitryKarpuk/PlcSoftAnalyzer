using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siemens.Engineering;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.Tags;

namespace WpfTiaProject.Model
{
    /// <summary>
    /// Represents a PLC tag table with a selection state, used in UI.
    /// </summary>
    public class TagTable
    {
        public bool IsSelected {  get; set; }
        public PlcTagTable PlcTagTable { get; set; }
        public TagTable(PlcTagTable plcTagTable = null, bool isSelected = false)
        {
            IsSelected = isSelected;
            PlcTagTable = plcTagTable;
        }
    }
}
