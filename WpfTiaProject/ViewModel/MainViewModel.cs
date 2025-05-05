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
using WpfTiaProject.Services;
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
        private TagRefReportViewModel _tagRefReportViewModel;
        private List<TagRefReport> _tagRefReports;
        private bool _isTagCheckSelected;
        private bool _isTiaConnected;
        private IProgressService _progressService;
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
        public bool IsTagCheckSelected
        {
            get { return _isTagCheckSelected; }
            set
            {
                _isTagCheckSelected = value;
                OnPropertyChanged(nameof(IsTagCheckSelected));
            }
        }
        public bool IsTiaConnected
        {
            get { return _isTiaConnected; }
            set
            {
                _isTiaConnected = value;
                OnPropertyChanged(nameof(IsTiaConnected));
            }
        }

        /// <summary>
        /// Command for connecting to opened Tia Portal project. 
        /// </summary>
        public ICommand ConnectTia { get; }

        /// <summary>
        /// Command for terminating connection to Tia Portal
        /// </summary>
        public ICommand DisconnectTia { get; }

        /// <summary>
        /// Load PLC tag tables from the project.
        /// </summary>
        public ICommand LoadTagTables { get; }

        /// <summary>
        /// Load and generate report of tag references.
        /// </summary>
        public ICommand LoadTagReferencesReport { get; }
        public MainViewModel(IProgressService progressService)
        {
            var cts = new CancellationToken();
            ProjectInfoViewModel = null;
            TagTables = null;
            TagRefReportViewModel = null;
            _progressService = progressService;
            ConnectTia = new DelegateCommand(
                (parameter) =>
                {
                    _tiaPortal = TiaProject.ConnectTiaPortal();
                    _currentProject = _tiaPortal.Projects.FirstOrDefault();
                    _currentCPU = TiaProject.GetCurrentCPUList(_currentProject)[0];
                    _currentPlcSoftware = TiaProject.GetCurrentPlcSoftware(_currentCPU);
                    ProjectInfoViewModel = new ProjectInfoViewModel(_currentProject, _currentCPU);
                    IsTiaConnected = true;
                        
                    });

            DisconnectTia = new DelegateCommand(
                (parameter) =>
                {
                    _tiaPortal.Dispose();
                    _tiaPortal = null;
                    _currentProject = null;
                    ProjectInfoViewModel = null;
                    IsTiaConnected = false;
                    IsTagCheckSelected = false;
                    TagTables.Clear();
                });


            LoadTagTables = new DelegateCommand(
                (parameter) =>
                {
                    if (parameter is bool == true)
                    {
                        var tagTables = TiaProject.GetAllTagTables(_currentPlcSoftware);
                        TagTables = new ObservableCollection<TagTableViewModel>(tagTables.Select(table => new TagTableViewModel(table)));
                    }
                    else TagTables = null;
    }
    );
            LoadTagReferencesReport = new DelegateCommand(
                (parameter) =>
                {
                    _progressService.RunWithProgressWindowAsync(ExecuteDataLoad);
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
    }
}
