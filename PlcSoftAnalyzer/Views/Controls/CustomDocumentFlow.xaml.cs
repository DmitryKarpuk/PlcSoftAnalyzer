using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using PlcSoftAnalyzer.Model;

namespace PlcSoftAnalyzer.Views.Controls
{
    public partial class CustomDocumentFlow
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof (ItemsSource), 
            typeof (ObservableCollection<TagTableRefReport>), 
            typeof (CustomDocumentFlow), 
            new FrameworkPropertyMetadata(null, OnItemsSourceChanged));
        
        public ObservableCollection<TagTableRefReport> ItemsSource
        {
            set
            {
                if (value == null)
                    ClearValue(ItemsSourceProperty);
                else
                    SetValue(ItemsSourceProperty, value);
            }
        }

        public CustomDocumentFlow()
        {
            InitializeComponent();

            //незнаю надо это или нет попробуй убрать
            Document = new FlowDocument();
            
            Document.Blocks.Add(new Paragraph(new Run("PLC soft analyzer report"))
            {
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center
            });
        }
        
        private static void OnItemsSourceChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is not CustomDocumentFlow control)
            {
                return;
            }
            
            control.ItemsSourceChanged(eventArgs.OldValue, eventArgs.NewValue);
        }

        private void ItemsSourceChanged(object oldValue, object newValue)
        {
            if (oldValue is ObservableCollection<TagTableRefReport> oldCollection)
            {
                oldCollection.CollectionChanged -= CollectionOnCollectionChanged;
                return;
            }
            
            if (newValue is not ObservableCollection<TagTableRefReport> collection)
            {
                return;
            }
            
            collection.CollectionChanged += CollectionOnCollectionChanged;
        }

        private void CollectionOnCollectionChanged(object _, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems is not ObservableCollection<TagTableRefReport> colection)
            {
                return;
            }
            
            foreach (var report in colection)
            {
                Document.Blocks.Add(new Paragraph(new Run($"{report.Name}")) { FontWeight = FontWeights.Bold });
                var refTypesMap = new Dictionary<TagAddressType, int>();
                foreach (var tag in report.RefOutOfLimitData)
                {
                    if (refTypesMap.ContainsKey(tag.AddressType)) refTypesMap[tag.AddressType]++;
                    else refTypesMap[tag.AddressType] = 1;
                }

                refTypesMap
                    .ToList()
                    .ForEach(item =>
                    {
                        Document.Blocks.Add(new Paragraph(new Run($"\t {item.Value} {item.Key} tags references out of limit")));
                    });

                if (report.TagsAmount <= 0)
                {
                    continue;
                }
                var invalidTagPercentage = (double)report.RefOutOfLimitData.Count / report.TagsAmount * 100.0;
                Document.Blocks.Add(new Paragraph(new Run($"\t Summury: {invalidTagPercentage:F2}% " +
                                                         $"({report.RefOutOfLimitData.Count} out of {report.TagsAmount})")));
            }
        }

        private void ProduceItems()
        {
            
        }
    }
}