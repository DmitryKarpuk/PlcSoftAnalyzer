using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Siemens.Engineering.SW.Tags;
using Siemens.Engineering.VersionControl;
using PlcSoftAnalyzer.Model;
using DocumentFormat.OpenXml.Bibliography;

namespace PlcSoftAnalyzer.ViewModel
{
    public class TagRefReportViewModel : ViewModelBase
    {
        private FlowDocument _tagRefReport;
        
        public ObservableCollection<TagTableRefReport> Items { get;}
        public FlowDocument TagRefReport
        {
            get => _tagRefReport;
            set
            {  
                _tagRefReport = value;
                OnPropertyChanged(nameof(TagRefReport));
            }
        }

        public TagRefReportViewModel()
        {
            Items = new ObservableCollection<TagTableRefReport>();
        }
        
        public TagRefReportViewModel(List<TagTableRefReport> report)
        {
            Items = new ObservableCollection<TagTableRefReport>(report);
        }
        
        //для клина создать команду
        public void CleanReport()
        { 
            TagRefReport.Blocks.Clear(); 
        }
    }
}
