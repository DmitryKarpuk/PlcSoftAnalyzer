using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Documents;
using WpfTiaProject.Model;
using System.Collections.ObjectModel;
using System.Collections;
using Siemens.Engineering.SW.Tags;
using System.Windows.Threading;

namespace WpfTiaProject.ViewModel
{
    public class ProgressViewModel : ViewModelBase
    {
        public List<TagRefReport> Result {  get; set; }
        public List<TagTableViewModel> DataSource { get; set; }
        public Action CloseAction { get; set; }
        public ICommand Cancel { get; }
        public ProgressViewModel(List<TagTableViewModel> data)
        {
            DataSource = data;
            Cancel = new DelegateCommand(
                (parameter) => CloseAction?.Invoke());
        }

        public void DoWork()
        {
            var result = new List<TagRefReport>();
            foreach (TagTableViewModel table in DataSource)
            {
                if (table != null)
                {
                    var item = new TagRefReport()
                    {
                        TableName = table.Name,
                    };
                    var tableReferences = new SortedDictionary<int, int>();
                    foreach (var tag in table.TagTable.Tags)
                    {
                        var tagReferences = TiaProject.CalculateReferences(tag);
                        if (tableReferences.ContainsKey(tagReferences)) tableReferences[tagReferences]++;
                        else tableReferences[tagReferences] = 1;
                    }
                    item.TagsRefRate = tableReferences;
                    result.Add(item);
                }
            }
            Result = result;
            CloseAction?.Invoke();
            MessageBox.Show("Report generated");

        }
    }
}
