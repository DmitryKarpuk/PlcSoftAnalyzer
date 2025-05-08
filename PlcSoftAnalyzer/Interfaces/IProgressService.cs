using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlcSoftAnalyzer.Interfaces
{
    public interface IProgressService
    {
        Task RunWithProgressWindowAsync(Func<CancellationToken, Task> operation);
    }
}
