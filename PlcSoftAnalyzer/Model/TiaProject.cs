using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup.Localizer;
using Siemens.Engineering;
using Siemens.Engineering.CrossReference;
using Siemens.Engineering.HmiUnified.HmiLogging.HmiLoggingCommon;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.Tags;
using PlcSoftAnalyzer.ViewModel;

namespace PlcSoftAnalyzer.Model
{
    /// <summary>
    /// Wrapper class for working with Tia Portal via TiaOpenness API
    /// </summary>
    public static class TiaProject
    {

        public static readonly TagAddressType[] ReportDataTypes = new TagAddressType[2] { TagAddressType.Input, TagAddressType.Output }; 

        /// <summary>
        /// Connection to opened Tia Portal project.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throw if there are no opened Tia Portal processes</exception>
        public static TiaPortal ConnectTiaPortal()
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

        /// <summary>
        /// Get PLC Software object of the corresponding device.
        /// </summary>
        /// <param name="deviceItem">PLC device object.</param>
        /// <returns>PLC software object.</returns>
        public static PlcSoftware GetCurrentPlcSoftware(DeviceItem deviceItem)
        {
            SoftwareContainer softwareContainer = ((IEngineeringServiceProvider)deviceItem).GetService<SoftwareContainer>();
            if (softwareContainer != null)
            {
                Software software = softwareContainer.Software;
                return software as PlcSoftware;
            }
            return null;
        }

        /// <summary>
        /// Load all tag tables from PLC Software.
        /// </summary>
        /// <param name="plcSoftware"> PLC Software object</param>
        /// <returns>List of tab tables</returns>
        public static List<TagTable> GetAllTagTables(PlcSoftware plcSoftware)
        {
            //Get tag table IEnumerable<PlcTagTable>
            PlcTagTableComposition plcTagTables = plcSoftware.TagTableGroup.TagTables;
            var tagTables = plcTagTables.Select(table => new TagTable(table)).ToList();
            //Get tag table IEnumerable<PlcTagTableUserGroup>
            PlcTagTableUserGroupComposition plcTagTableGroups = plcSoftware.TagTableGroup.Groups;
            foreach (var group in plcTagTableGroups)
            {
                foreach (var table in group.TagTables)
                {
                    tagTables.Add(new TagTable(table));
                }
            }
            return tagTables;
          }
    }
}
