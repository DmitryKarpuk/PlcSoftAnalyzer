using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;

namespace PlcSoftAnalyzer.Interfaces
{
    public interface IMessageService
    {
        void ShowMessage(string message, string title, MessageBoxImage image);
        void ShowError(string message, string title);
        void ShowError(Exception message, string title);
        void ShowWarning(string message, string title);
        void ShowInformation(string message, string title);
        bool ShowConfirmation(string message, string title, MessageBoxButton buttons = MessageBoxButton.YesNo);

    }
}
