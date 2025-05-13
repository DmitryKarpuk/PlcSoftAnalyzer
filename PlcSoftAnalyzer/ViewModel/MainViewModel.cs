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
using PlcSoftAnalyzer.Model;
using PlcSoftAnalyzer.Services;
using PlcSoftAnalyzer.Interfaces;
using PlcSoftAnalyzer.Views;


namespace PlcSoftAnalyzer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool _isReportDone;
        private ProjectInfoViewModel _projectInfoViewMode;
        private ObservableCollection<TagTableViewModel> _tagTables;
        private TagRefReportViewModel _tagRefReportViewModel;
        private bool _isTagCheckSelected;
        private bool _isTiaConnected;
        private int _selectedInputLimit;
        private int _selectedOutputLimit;
        private IProgressService _progressService;
        private ITagRefAnalyzerService _referencesAnalyzerService;
        private IFileDialogService _fileDialogService;
        private IExcelReportService _excelReportService;
        private IMessageService _messageService;
        private ITiaPortalService _tiaPortalService;
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
        public int[] InputLimits => new int[5] {1, 2, 3, 4, 5};
        public int SelectedInputLimit
        {
            get
            {
                return _selectedInputLimit;
            }
            set
            {
                _selectedInputLimit = value;
                _referencesAnalyzerService.LimitsMap[TagAddressType.Input] = _selectedInputLimit;
            }
        }
        public int[] OutputLimits => new int[5] { 1, 2, 3, 4, 5 };
        public int SelectedOutputLimit
        {
            get
            {
                return _selectedOutputLimit;
            }
            set
            {
                _selectedOutputLimit = value;
                _referencesAnalyzerService.LimitsMap[TagAddressType.Output] = _selectedOutputLimit;
            }
        }
        public bool IsReportDone
        {
            get 
            {
                return _isReportDone; 
            }
            set
            {
                _isReportDone = value;
                OnPropertyChanged(nameof(IsReportDone));
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

        /// <summary>
        /// Print full report in excel
        /// </summary>
        public ICommand PrintReport { get; }

        public MainViewModel(IProgressService progressService, ITagRefAnalyzerService tagRefAnalyzerService, 
                            IFileDialogService fileDialogService, IExcelReportService excelReportService,
                            IMessageService messageService, ITiaPortalService tiaPortalService)
        {
            IsReportDone = false;
            _progressService = progressService;
            _fileDialogService = fileDialogService;
            _referencesAnalyzerService = tagRefAnalyzerService;
            _excelReportService = excelReportService;
            _messageService = messageService;
            _tiaPortalService = tiaPortalService;
            SelectedInputLimit = 2;
            SelectedOutputLimit = 1;
            ConnectTia = new DelegateCommand(
                (parameter) =>
                {
                    try
                    {
                        _tiaPortalService.ConnectOpenedCpuProject();
                        ProjectInfoViewModel = new ProjectInfoViewModel(_tiaPortalService.CurrentProject, _tiaPortalService.CurrentCpu);
                        IsTiaConnected = true;
                        _messageService.ShowInformation("Connected", "Connect Tia Portal");
                    }
                    catch (Exception ex)
                    {
                        _messageService.ShowError(ex, "Connect Tia Portal");
                    }
                });
            DisconnectTia = new DelegateCommand(
                (parameter) =>
                {
                    try
                    {
                        if (_tiaPortalService.TiaPortal != null)
                        {
                            _tiaPortalService.DisconnectOpenedCpuProject();
                            ProjectInfoViewModel = null;
                            IsTiaConnected = false;
                            IsTagCheckSelected = false;
                            IsReportDone = false;
                            TagTables?.Clear();
                            _messageService.ShowInformation("Disconnected", "Disconnect Tia Portal");
                        }
                    }
                    catch (Exception ex)
                    {
                        _messageService.ShowError(ex, "Disconnect Tia Portal");
                    }
                });
            LoadTagTables = new DelegateCommand(
                (parameter) =>
                {
                    try
                    {
                        if (parameter is bool == true)
                        {
                            var tagTables = _tiaPortalService.CurrentSoftwareTagTables;
                            TagTables = new ObservableCollection<TagTableViewModel>(tagTables.Select(table => new TagTableViewModel(table)));
                        }
                        else TagTables = null;
                    }
                    catch (Exception ex)
                    {
                        _messageService.ShowError(ex, "Loading tag tables");
                    }
                }
            );
            LoadTagReferencesReport = new DelegateCommand(
                (parameter) =>
                {
                    try
                    {
                        _progressService.RunWithProgressWindowAsync(ExecuteDataLoad);
                        _messageService.ShowInformation("Report generated", "Generate report");
                    }
                    catch (Exception ex)
                    {
                        _messageService.ShowError(ex, "Generate report");
                    }
                });
            PrintReport = new DelegateCommand(
                (parameter) =>
                {
                    if (_fileDialogService.SaveFileDialog() == true)
                    {
                        try
                        {
                            _excelReportService.PrintRefAnaylyzerReport(_fileDialogService.FilePath, _referencesAnalyzerService.TagTableRefReportSource);
                            _messageService.ShowInformation("Report printed", "Print report");
                        }
                        catch (Exception ex)
                        {
                            _messageService.ShowError(ex, "Print report");
                        }
                    }
                }
                );
           
        }

        private async Task ExecuteDataLoad(CancellationToken token)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {

                    var selectedTables = TagTables
                            .Where(table => table.IsSelected)
                            .Select(table => table.TagTable)
                            .ToList();
                    _referencesAnalyzerService.LoadTagRefOutOfLimitData(selectedTables, token);
                    TagRefReportViewModel = new TagRefReportViewModel();
                    _referencesAnalyzerService.TagTableRefReportSource.ForEach(table => {
                        TagRefReportViewModel.Items.Add(table);
                        });
                    ;
                    IsReportDone = true;                
            });
        }
    }
}
