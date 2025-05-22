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
using Microsoft.Extensions.DependencyInjection;

namespace PlcSoftAnalyzer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        static readonly Dictionary<Model.TagAddressType, int> LimitsMap = new Dictionary<Model.TagAddressType, int>()
        {
            {TagAddressType.Input, 0 },
            {TagAddressType.Output, 0 }
        };
        protected override void OnStartup(StartupEventArgs e)
        {
            //var tiaPortalService = new TiaPortalService();
            //var progressService = new ProgressService<ProgressWindow>(new ProgressViewModel());
            //var refAnalyzerServie = new TagRefAnylizerService(LimitsMap);
            //var fileDialogService = new FileDialogService();
            //var excelReportService = new ExcelReportService();
            //var messageService = new MessageService();
            //var dataContext = new MainViewModel(progressService, refAnalyzerServie, fileDialogService,
            //                                    excelReportService, messageService, tiaPortalService);
            //MainWindow = new MainWindow()
            //{
            //    DataContext = dataContext
            //};
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            //mainWindow.DataContext = _serviceProvider.GetRequiredService<MainViewModel>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // DI registrations
            services.AddSingleton<ITiaPortalService, TiaPortalService>();
            services.AddTransient<ProgressViewModel>();
            services.AddTransient<IProgressService, ProgressService<ProgressWindow>>(
                s => new ProgressService<ProgressWindow>(s.GetRequiredService<ProgressViewModel>()));           
            services.AddSingleton<IFileDialogService, FileDialogService>();
            services.AddSingleton<IExcelReportService, ExcelReportService>();
            services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<ITagRefAnalyzerService, TagRefAnylizerService>(sp =>
                new TagRefAnylizerService(LimitsMap));
            // ViewModels
            services.AddSingleton<MainViewModel>();

            // Views
            services.AddSingleton<MainWindow>(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });
        }
    }
}
