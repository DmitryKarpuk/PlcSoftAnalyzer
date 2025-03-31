using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTiaProject.Model;

namespace WpfTiaProject.ViewModel
{
    public class TagTableViewModel
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public TagTableViewModel(TagTable TagTable, bool isSelected = false)
        {
            IsSelected = isSelected;
            Name = TagTable.PlcTagTable.Name;
        }
    }
}
