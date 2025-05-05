using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Documents;
using WpfTiaProject.Model;
using System.Collections.ObjectModel;
using System.Collections;
using Siemens.Engineering.SW.Tags;
using System.Windows.Threading;

namespace WpfTiaProject.ViewModel
{
    public class ProgressViewModel : ViewModelBase
    {
        public int ThreadId => Thread.CurrentThread.ManagedThreadId;
        public ProgressViewModel()
        {}
    }
}
