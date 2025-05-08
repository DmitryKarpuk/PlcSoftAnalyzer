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

namespace PlcSoftAnalyzer.ViewModel
{
    public class TagRefReportViewModel : ViewModelBase
    {
        private FlowDocument _tagRefReport;
        //private Dictionary<int, int> _referenceTagsDict;
        private List<TagTableRefReport> reportSource;
        public FlowDocument TagRefReport
        {
            get { return _tagRefReport; }
            set
            {  _tagRefReport = value;
                OnPropertyChanged(nameof(TagRefReport));}
            }

        public TagRefReportViewModel() { }
        public TagRefReportViewModel(List<TagTableRefReport> report)
        {
            reportSource = report;
            GenerateReport();
        }
        public void GenerateReport()
        {
            FlowDocument doc = new FlowDocument();
            doc.Blocks.Add(new Paragraph(new Run("PLC soft analyzer report"))
            {
                FontSize = 20,
                FontWeight = System.Windows.FontWeights.Bold,
                TextAlignment = TextAlignment.Center
            });
            if (reportSource != null)
            {
                foreach (var source in reportSource)
                {
                    doc.Blocks.Add(new Paragraph(new Run($"{source.Name}")) { FontWeight = FontWeights.Bold });
                    var RefTypesMap = new Dictionary<TagAddressType, int>();
                    foreach (var tag in source.RefOutOfLimitData)
                    {
                        if (RefTypesMap.ContainsKey(tag.AddressType)) RefTypesMap[tag.AddressType]++;
                        else RefTypesMap[tag.AddressType] = 1;
                    }

                    RefTypesMap.ToList().ForEach(item =>
                    {
                        doc.Blocks.Add(new Paragraph(new Run($"\t {item.Value} {item.Key} tags references out of limit")));
                    });

                    if (source.TagsAmount > 0)
                    {
                        double invalidTagPercentage = ((double)source.RefOutOfLimitData.Count / source.TagsAmount) * 100.0;
                        doc.Blocks.Add(new Paragraph(new Run($"\t Summury: {invalidTagPercentage:F2}% " +
                            $"({source.RefOutOfLimitData.Count} out of {source.TagsAmount})")));
                    }
                }
            }
            TagRefReport = doc;
        }
        public void CleanReport()
        { 
            TagRefReport.Blocks.Clear(); 
        }
    }
}
