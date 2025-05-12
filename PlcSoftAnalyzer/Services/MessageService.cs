using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PlcSoftAnalyzer.Interfaces;

namespace PlcSoftAnalyzer.Services
{
    public class MessageService : IMessageService
    {
        public void ShowMessage(string message, string title, MessageBoxImage image)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, image);
        }
        public bool ShowConfirmation(string message, string title, MessageBoxButton buttons = MessageBoxButton.YesNo)
        {
            var result = MessageBox.Show(message, title, buttons, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        public void ShowError(string message, string title)
        {
            ShowMessage(message, title, MessageBoxImage.Error);
        }

        public void ShowError(Exception error, string title)
        {
            ShowMessage(error.Message, title, MessageBoxImage.Error);
        }

        public void ShowInformation(string message, string title)
        {
            ShowMessage(message, title, MessageBoxImage.Information);
        }

        public void ShowWarning(string message, string title)
        {
            ShowMessage(message, title, MessageBoxImage.Warning);
        }
    }
}
