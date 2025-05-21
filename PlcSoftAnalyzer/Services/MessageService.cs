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
        /// <summary>
        /// Show standard message window.
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="title">Title of the message window</param>
        /// <param name="image">Image of the window</param>
        public void ShowMessage(string message, string title, MessageBoxImage image)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, image);
        }

        /// <summary>
        /// Show confirmation window.
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="title">Title of the message window</param>
        /// <param name="buttons">Configuration of windows buttons</param>
        /// <returns></returns>
        public bool ShowConfirmation(string message, string title, MessageBoxButton buttons = MessageBoxButton.YesNo)
        {
            var result = MessageBox.Show(message, title, buttons, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Show error message window.
        /// </summary>
        /// <param name="message">Error text</param>
        /// <param name="title">Title of the message window</param>
        public void ShowError(string message, string title)
        {
            ShowMessage(message, title, MessageBoxImage.Error);
        }

        /// <summary>
        /// Show error message window.
        /// </summary>
        /// <param name="error">Catched exception</param>
        /// <param name="title">Title of the message window</param>
        public void ShowError(Exception error, string title)
        {
            ShowMessage($"{error.Message}\n{error.InnerException?.Message}\n{ error.InnerException?.StackTrace}", title, 
                MessageBoxImage.Error);
        }

        /// <summary>
        /// Show information message window.
        /// </summary>
        /// <param name="message">Information text</param>
        /// <param name="title">Title of the message window</param>
        public void ShowInformation(string message, string title)
        {
            ShowMessage(message, title, MessageBoxImage.Information);
        }

        /// <summary>
        /// Show warning message window.
        /// </summary>
        /// <param name="message">Warning text</param>
        /// <param name="title">Title of the message window</param>
        public void ShowWarning(string message, string title)
        {
            ShowMessage(message, title, MessageBoxImage.Warning);
        }
    }
}
