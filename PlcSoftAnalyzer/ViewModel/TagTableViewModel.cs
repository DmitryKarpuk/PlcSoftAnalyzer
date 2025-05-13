using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siemens.Engineering.HmiUnified.UI.Events;
using Siemens.Engineering.SW.Tags;
using PlcSoftAnalyzer.Model;

namespace PlcSoftAnalyzer.ViewModel
{
    public class TagTableViewModel
    {
        public PlcTagTable TagTable { get; private set; }
        public bool IsSelected { get; set; }
        public string Name { get; set; }

        public TagTableViewModel(PlcTagTable table, bool isSelected=false)
        {
            TagTable = table;
            IsSelected = isSelected;
            Name = table.Name;
        }
    }
}
