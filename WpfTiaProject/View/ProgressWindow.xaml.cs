using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfTiaProject.ViewModel;

namespace WpfTiaProject.View
{
    public partial class ProgressWindow : Window
    {
        public ProgressViewModel ViewModel { get; set; }
        public ProgressWindow(ProgressViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel;
            ViewModel.CloseAction = () => this.Close();
            this.ShowDialog();
        }
    }
}
