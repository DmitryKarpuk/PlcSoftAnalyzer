using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using Siemens.Engineering;
using Siemens.Engineering.Hmi.Tag;
using Siemens.Engineering.HW;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.Tags;
using WpfTiaProject.Model;
using WpfTiaProject.View;


namespace WpfTiaProject.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private TiaPortal _tiaPortal;
        private Project _currentProject;
        private DeviceItem _currentCPU;
        private ProjectInfoViewModel _projectInfoViewMode;
        private PlcSoftware _currentPlcSoftware;
        private ObservableCollection<TagTableViewModel> _tagTables;
        private List<Model.TagTable> _TagTables;
        private bool _tagCheckSelected;
        private TagRefReportViewModel _tagRefReportViewModel;
        private List<TagRefReport> _tagRefReports;
        public int ThreadId => Thread.CurrentThread.ManagedThreadId;
        public ObservableCollection<TagTableViewModel> TagTables
        { get { return _tagTables; }
          set
            {
                _tagTables = value;
                OnPropertyChanged(nameof(TagTables));
            }
        }
        public ProjectInfoViewModel ProjectInfoViewModel
        {
            get
            {
                return _projectInfoViewMode;
            }
            set
            {
                _projectInfoViewMode = value;
                OnPropertyChanged(nameof(ProjectInfoViewModel));
            }
        }
        //public PlcSoftware PlcSoftware { get; set; }
        public TagRefReportViewModel TagRefReportViewModel
        {
            get { return _tagRefReportViewModel; }
            set
            {
                _tagRefReportViewModel = value;
                OnPropertyChanged(nameof(TagRefReportViewModel));
            }
        }
        public bool TagCheckSelected
        {
            get { return _tagCheckSelected; }
            set
            {
                _tagCheckSelected = value;
                OnPropertyChanged(nameof(TagCheckSelected));
            }
        }
        public ICommand ConnectTia { get; }
        public ICommand DisconnectTia { get; }
        public ICommand GetTagTables { get; }
        public ICommand GetTagReferencesReport { get; }
        public MainViewModel()
        {
            ProjectInfoViewModel = null;
            TagTables = null;
            TagRefReportViewModel = null;
            ConnectTia = new DelegateCommand(
                (parameter) =>
                {
                    _tiaPortal = TiaProject.ConnectTiaPortal();
                    _currentProject = _tiaPortal.Projects.FirstOrDefault();
                    _currentCPU = TiaProject.GetCurrentCPUList(_currentProject)[0];
                    _currentPlcSoftware = TiaProject.GetCurrentPlcSoftware(_currentCPU);
                    ProjectInfoViewModel = new ProjectInfoViewModel(_currentProject, _currentCPU);
                });
            DisconnectTia = new DelegateCommand(
                (parameter) =>
                {
                    _tiaPortal.Dispose();
                    _tiaPortal = null;  
                    _currentProject = null;
                    ProjectInfoViewModel = null;
                });
            GetTagTables = new DelegateCommand(
                (parameter) =>
                {
                    if (parameter is bool == true)
                    {
                        _TagTables = TiaProject.GetAllTagTables(_currentPlcSoftware);
                        TagTables = new ObservableCollection<TagTableViewModel>(_TagTables.Select(table => new TagTableViewModel(table)));
                    }
                    else TagTables = null;
    }
    );
            GetTagReferencesReport = new DelegateCommand(
                (parameter) =>
                {
                    OpenNewThreadWindow(ExecuteDataLoad);
                });
        }

        private async Task ExecuteDataLoad(CancellationToken token)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                _tagRefReports = TiaProject.GeTableTagsRefData(TagTables.Where(table => table.IsSelected).Select(table => table.TagTable).ToList());
                TagRefReportViewModel = new TagRefReportViewModel(_tagRefReports);
            });
        }

        private async void OpenNewThreadWindow(Func<CancellationToken, Task> operation)
        {
            CancellationTokenSource _cts = new CancellationTokenSource();
            ProgressWindow window = null;
            Thread thread = new Thread(() =>
            {
                window = new ProgressWindow(new ProgressViewModel());
                window.Closed += (s, e) =>
                { 
                    _cts?.Cancel();
                    Dispatcher.CurrentDispatcher.InvokeShutdown(); };
                window.Show();
                Dispatcher.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
            await operation(_cts.Token);
            if (window != null && window.Dispatcher.Thread.IsAlive)
            {
                window.Dispatcher.Invoke(() => window.Close());              
            }
        }
    }
}
