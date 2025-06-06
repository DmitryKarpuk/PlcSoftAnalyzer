﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows;
using PlcSoftAnalyzer.Interfaces;

namespace PlcSoftAnalyzer.Services
{
    public class FileDialogService : IFileDialogService
    {
        public string FilePath { get; set; }

        /// <summary>
        /// Open standard OpenFileDialog window and return file path result.
        /// </summary>
        /// <returns></returns>
        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// Open standard SaveFileDialog window and return file path result.
        /// </summary>
        /// <returns></returns>
        public bool SaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "PlcSoftAnalyzerReport";
            saveFileDialog.DefaultExt = ".xlsx";
            saveFileDialog.Filter = "Excel Worksheets |*.xlsx; *.xlsm";
            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                return true;
            }
            else { return false; }
        }
    }
}
