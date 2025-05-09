using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using PlcSoftAnalyzer.ViewModel;
using PlcSoftAnalyzer.Views;
using PlcSoftAnalyzer.Services;
using PlcSoftAnalyzer.Interfaces;
using System.Threading;
using PlcSoftAnalyzer.Model;

namespace PlcSoftAnalyzer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static readonly Dictionary<Model.TagAddressType, int> LimitsMap = new Dictionary<Model.TagAddressType, int>()
        {
            {TagAddressType.Input, 0 },
            {TagAddressType.Output, 0 }
        };
        protected override void OnStartup(StartupEventArgs e)
        {
            var propgressService = new ProgressService<ProgressWindow>(new ProgressViewModel());
            var refAnalyzerServie = new TagRefAnylizerService(LimitsMap);
            var fileDialogService = new FileDialogService();
            var excelReportService = new ExcelReportService();
            var dataContext = new MainViewModel(propgressService, refAnalyzerServie, fileDialogService, excelReportService);
            MainWindow = new MainWindow()
            {
                DataContext = dataContext
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
