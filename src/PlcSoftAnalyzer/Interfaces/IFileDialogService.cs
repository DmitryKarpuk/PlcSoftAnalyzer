using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcSoftAnalyzer.Interfaces
{
    public interface IFileDialogService
    {
        string FilePath { get; set; }
        bool OpenFileDialog();
        bool SaveFileDialog();

    }
}
