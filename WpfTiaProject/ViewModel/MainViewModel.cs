using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Siemens.Engineering;
using Siemens.Engineering.SW;
using WpfTiaProject.Model;


namespace WpfTiaProject.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private TiaPortal _tiaPortal;
        private Project _project;
        private ProjectInfoViewModel _projectInfoViewMode;
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
        public ICommand ConnectTia { get; }
        public ICommand DisconnectTia { get; }
        public MainViewModel()
        {
            ProjectInfoViewModel = new ProjectInfoViewModel();
            ConnectTia = new DelegateCommand(
                (parameter) =>
                {
                    _tiaPortal = TiaProject.ConnectTiaPortal();
                    _project = _tiaPortal.Projects.FirstOrDefault();
                    ProjectInfoViewModel = new ProjectInfoViewModel(_project);
                });
            DisconnectTia = new DelegateCommand(
                (parameter) =>
                {
                    _tiaPortal.Dispose();
                    _tiaPortal = null;  
                    _project = null;
                    ProjectInfoViewModel = null;
                });
        }
    }
}
