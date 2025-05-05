using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfTiaProject.ViewModel;
using WpfTiaProject.View;
using WpfTiaProject.Services;

namespace WpfTiaProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var propgressService = new ProgressService<ProgressWindow>(new ProgressViewModel());
            var dataContext = new MainViewModel(propgressService);
            MainWindow = new MainWindow()
            {
                DataContext = dataContext
            };
            //var controlProjectInfo = MainWindow.FindName("projectInfoView") as ProjectInfoView;
            //if (controlProjectInfo != null) 
            //    controlProjectInfo.DataContext = dataContext.ProjectInfoViewModel;
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
