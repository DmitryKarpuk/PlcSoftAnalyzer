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
using WpfTiaProject.Model;

namespace WpfTiaProject.ViewModel
{
    public class TagRefReportViewModel : ViewModelBase
    {
        private FlowDocument _tagRefReport;
        //private Dictionary<int, int> _referenceTagsDict;
        private List<TagRefReport> reportSource;
        public FlowDocument TagRefReport
        {
            get { return _tagRefReport; }
            set
            {  _tagRefReport = value;
                OnPropertyChanged(nameof(TagRefReport));}
            }

        public TagRefReportViewModel() { }
        public TagRefReportViewModel(List<TagRefReport> report)
        {
            reportSource = report;
            GenerateReport();
        }
        private void GenerateReport()
        {
            FlowDocument doc = new FlowDocument();
            doc.Blocks.Add(new Paragraph(new Run("Tag References Report"))
            {
                FontSize = 20,
                FontWeight = System.Windows.FontWeights.Bold,
                TextAlignment = TextAlignment.Center
            });
            foreach (var source in reportSource)
            {
                    doc.Blocks.Add(new Paragraph(new Run($"{source.TableName}")) { FontWeight=FontWeights.Bold});
                    foreach (var type in source.TagsRefData)
                    {
                        doc.Blocks.Add(new Paragraph(new Run($"\t{type.Key} tags:")));
                        foreach(var item in type.Value)
                            doc.Blocks.Add(new Paragraph(new Run($"\t\t{item.Value} tags have {item.Key} references")) { FontWeight = FontWeights.Thin });
                    }
                    doc.Blocks.Add(new Paragraph(new Run("")));
            }
            TagRefReport = doc;
        }
    }
}
