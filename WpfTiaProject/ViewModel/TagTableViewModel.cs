using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siemens.Engineering.HmiUnified.UI.Events;
using WpfTiaProject.Model;

namespace WpfTiaProject.ViewModel
{
    public class TagTableViewModel
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }

        public TagTableViewModel(string name, bool isSelected = false)
        {
            IsSelected = isSelected;
            Name = name;
        }
    }
}
