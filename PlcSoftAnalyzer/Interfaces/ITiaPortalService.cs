using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siemens.Engineering.HW;
using Siemens.Engineering;
using Siemens.Engineering.SW;

namespace PlcSoftAnalyzer.Interfaces
{
    public interface ITiaPortalService
    {
        public TiaPortal TiaPortal { get; }
        public Project CurrentProject { get; }
        public DeviceItem CurrentCpu { get; }
        public PlcSoftware CurrentPlcSoftware { get; }
        public void ConnectOpenedCpuProject();
        public void DisconnectOpenedCpuProject();
    }
}
