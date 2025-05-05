using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfTiaProject.View;
using WpfTiaProject.ViewModel;

namespace WpfTiaProject.Services
{
    public class ProgressService<TProgressWindow> : IProgressService
        where TProgressWindow : Window, new()
    {
        public TProgressWindow ProgressWindow { get; set; }
        private ViewModelBase _progressViewModel { get; set; }
        private CancellationToken _cts { get; set; }
        public ProgressService(ViewModelBase viewModel, CancellationToken cts)
        {
            _progressViewModel = viewModel;
            _cts = cts;
        }
        public async Task RunWithProgressWindowAsync(Func<CancellationToken, Task> operation)
        {
            CancellationTokenSource _cts = new CancellationTokenSource();
            Thread thread = new Thread(() =>
            {
                ProgressWindow = new TProgressWindow();
                ProgressWindow.DataContext = _progressViewModel;
                ProgressWindow.Closed += (s, e) =>
                {
                    _cts?.Cancel();
                    Dispatcher.CurrentDispatcher.InvokeShutdown();
                };
                ProgressWindow.Show();
                Dispatcher.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
            await operation(_cts.Token);
            if (ProgressWindow != null && ProgressWindow.Dispatcher.Thread.IsAlive)
            {
                ProgressWindow.Dispatcher.Invoke(() => ProgressWindow.Close());
            }
        }
    }
}
