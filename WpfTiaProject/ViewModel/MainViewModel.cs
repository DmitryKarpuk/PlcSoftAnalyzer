using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using Siemens.Engineering;
using Siemens.Engineering.Hmi.Tag;
using Siemens.Engineering.HW;
using Siemens.Engineering.SW;
using WpfTiaProject.Model;


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
        private bool _tagCheckSelected;
        private TagRefReportViewModel _tagRefReportViewModel;
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
                        var tagTableViewModelList = TiaProject.GetAllTagTables(_currentPlcSoftware).Select(table => new TagTableViewModel(table.PlcTagTable.Name)).ToList();
                        TagTables = new ObservableCollection<TagTableViewModel>(tagTableViewModelList);
                    }
                    else TagTables = null;
    }
    );
            GetTagReferencesReport = new DelegateCommand(
                (parameter) =>
                {
                    TagRefReportViewModel = new TagRefReportViewModel(TagTables);
                });
        }
    }
}
