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
                    doc.Blocks.Add(new Paragraph(new Run($"Tag table: {source.TableName}")));
                    foreach (var item in source.TagsRefRate)
                    {
                        doc.Blocks.Add(new Paragraph(new Run($"{item.Value} tags have {item.Key} references")));
                    }
                    doc.Blocks.Add(new Paragraph(new Run("")));
            }

            #region comments
            //Table table = new Table();
            //table.Columns.Add(new TableColumn { Width = new GridLength(100) });
            //table.Columns.Add(new TableColumn { Width = new GridLength(200)});
            //table.Columns.Add(new TableColumn { Width = new GridLength(100)});

            ////Header
            //TableRowGroup headerGroup = new TableRowGroup();
            //TableRow headerRow = new TableRow();
            //headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Reference numbers"))) { FontWeight = System.Windows.FontWeights.Bold });
            //headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Table Name"))) { FontWeight = System.Windows.FontWeights.Bold });
            //headerRow.Cells.Add(new TableCell(new Paragraph(new Run("Table selected"))) { FontWeight = System.Windows.FontWeights.Bold });
            //headerGroup.Rows.Add(headerRow);
            //table.RowGroups.Add(headerGroup);

            //// Data rows
            //int id = 0;
            //TableRowGroup dataGroup = new TableRowGroup();
            //foreach (var item in TagTables)
            //{
            //    if (item.IsSelected)
            //    {
            //        TableRow row = new TableRow();
            //        row.Cells.Add(new TableCell(new Paragraph(new Run(id.ToString()))));
            //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.Name))));
            //        row.Cells.Add(new TableCell(new Paragraph(new Run(item.IsSelected ? "x" : "0"))));
            //        dataGroup.Rows.Add(row);
            //    }
            //}
            //table.RowGroups.Add(dataGroup);
            //doc.Blocks.Add(table);
            #endregion  

            TagRefReport = doc;
        }
    }
}
