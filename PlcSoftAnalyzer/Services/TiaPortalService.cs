using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcSoftAnalyzer.Interfaces;
using Siemens.Engineering;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.SW;

namespace PlcSoftAnalyzer.Services
{
    public class TiaPortalService : ITiaPortalService
    {
        public TiaPortal TiaPortal { get; private set; }
        public Project CurrentProject { get; private set; }
        public DeviceItem CurrentCpu { get; private set; }
        public PlcSoftware CurrentPlcSoftware { get; private set; }

        public TiaPortalService() { }

        public void ConnectOpenedCpuProject()
        {
            TiaPortal = ConnectOpenedProccess();
            CurrentProject = TiaPortal.Projects.FirstOrDefault();
            CurrentCpu = GetCurrentCPUList(CurrentProject)[0];
            CurrentPlcSoftware = GetCurrentPlcSoftware(CurrentCpu);
        }

        public void DisconnectOpenedCpuProject()
        {
            TiaPortal.Dispose();
            CurrentProject = null;
            CurrentCpu = null;
            CurrentPlcSoftware = null;
        }
        public TiaPortal ConnectOpenedProccess()
        {
            int processId;
            var TiaPortalProcessList = new List<TiaPortalProcess>(TiaPortal.GetProcesses());
            if (TiaPortalProcessList.Count >= 1) processId = TiaPortalProcessList[0].Id;
            else throw new ArgumentException("No TiaPortal precess opened");
            return TiaPortal.GetProcess(processId).Attach();
        }

        /// <summary>
        /// Get list of CPU devices from the project.
        /// </summary>
        /// <param name="project">Tia Portal project</param>
        /// <returns>List of defined CPU</returns>
        public List<DeviceItem> GetCurrentCPUList(Project project)
        {
            var deviceList = new List<DeviceItem>();
            foreach (var device in project.Devices)
            {
                var deviceItemComposition = device.DeviceItems;
                foreach (var deviceItem in deviceItemComposition)
                {
                    if (deviceItem.Classification == DeviceItemClassifications.CPU)
                    {

                        deviceList.Add(deviceItem);
                    }
                }
            }
            return deviceList;
        }

        /// <summary>
        /// Get PLC Software object of the corresponding device.
        /// </summary>
        /// <param name="deviceItem">PLC device object.</param>
        /// <returns>PLC software object.</returns>
        public PlcSoftware GetCurrentPlcSoftware(DeviceItem deviceItem)
        {
            SoftwareContainer softwareContainer = ((IEngineeringServiceProvider)deviceItem).GetService<SoftwareContainer>();
            if (softwareContainer != null)
            {
                Software software = softwareContainer.Software;
                return software as PlcSoftware;
            }
            return null;
        }
    }
}
