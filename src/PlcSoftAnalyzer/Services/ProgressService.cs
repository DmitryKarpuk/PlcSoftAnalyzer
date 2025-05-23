using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using PlcSoftAnalyzer.Interfaces;
using PlcSoftAnalyzer.ViewModel;

namespace PlcSoftAnalyzer.Services
{
    public class ProgressService<TProgressWindow> : IProgressService
        where TProgressWindow : Window, new()
    {
        public TProgressWindow ProgressWindow { get; set; }
        private ViewModelBase _progressViewModel { get; set; }
        public ProgressService(ViewModelBase viewModel)
        {
            _progressViewModel = viewModel;
        }
        /// <summary>
        /// Run window with progressbar in a new Thread and 
        /// simultaneously run long time operation in the main thread.
        /// </summary>
        /// <param name="operation">Long time running operation</param>
        /// <returns></returns>
        public async Task RunWithProgressWindowAsync(Func<CancellationToken, Task> operation)
        {
            var cts = new CancellationTokenSource();
            Thread thread = new Thread(() =>
            {
                ProgressWindow = new TProgressWindow();
                ProgressWindow.DataContext = _progressViewModel;
                ProgressWindow.Closed += (s, e) =>
                {
                    cts?.Cancel();
                    Dispatcher.CurrentDispatcher.InvokeShutdown();
                    cts.Dispose();
                };
                ProgressWindow.Show();
                Dispatcher.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
            await operation(cts.Token);
            if (ProgressWindow != null && ProgressWindow.Dispatcher.Thread.IsAlive)
            {
                ProgressWindow.Dispatcher.Invoke(() => ProgressWindow.Close());
            }
        }
    }
}
