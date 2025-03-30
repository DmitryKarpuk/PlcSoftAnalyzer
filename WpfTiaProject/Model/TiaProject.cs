using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siemens.Engineering;
using Siemens.Engineering.HmiUnified.HmiLogging.HmiLoggingCommon;
using Siemens.Engineering.HW;
using Siemens.Engineering.SW;
using WpfTiaProject.ViewModel;

namespace WpfTiaProject.Model
{
    public static class TiaProject
    {
        public static TiaPortal ConnectTiaPortal()
        {
            int processId;
            //Get processes list
            var TiaPortalProcessList = new List<TiaPortalProcess>(TiaPortal.GetProcesses());
            // Get process Id of first Tia process
            if (TiaPortalProcessList.Count >= 1) processId = TiaPortalProcessList[0].Id;
            else throw new ArgumentException("No TiaPortal precess opened");
            // Get TiaPortal instance of first process Id
            return TiaPortal.GetProcess(processId).Attach();
        }

        public static List<DeviceItem> GetCurrentCPUList(Project project)
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

    }
}
