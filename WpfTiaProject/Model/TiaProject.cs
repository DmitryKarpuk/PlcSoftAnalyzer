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
using WpfTiaProject.ViewModel;

namespace WpfTiaProject.Model
{
    /// <summary>
    /// Wrapper class for working with Tia Portal via TiaOpenness API
    /// </summary>
    public static class TiaProject
    {
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

        /// <summary>
        /// Calculate cross references of the PLC tag.
        /// </summary>
        /// <param name="tag">PLC tag object</param>
        /// <returns>Amount of references.</returns>
        public static int CalculateReferences(PlcTag tag)
        {
            int referenceCount = 0;
            var sources = GetSources(tag);
            foreach( var source in sources)
            {
                referenceCount += source.References.Count;
            }
            return referenceCount;
        }

        /// <summary>
        /// Get all cross reference sources of the tag
        /// </summary>
        /// <param name="tag">PLC tag object.</param>
        /// <returns>Cross reference sources of the tag.</returns>
        public static SourceObjectComposition GetSources(PlcTag tag)
        {
            var tagRefService = tag.GetService<CrossReferenceService>();
            var tagRefResult = tagRefService.GetCrossReferences(CrossReferenceFilter.AllObjects);
            SourceObjectComposition tagRefSources = tagRefResult.Sources;
            return tagRefSources;
        }

        /// <summary>
        /// Generate tag reference report data for each PLC tag table.
        /// </summary>
        /// <param name="tables">List of Plc tag rables.</param>
        /// <param name="token">Token to cancel operation.</param>
        /// <returns>List of reports for each table.</returns>
        /// 
        public static List<TagRefReport> GeTableTagsRefData(List<PlcTagTable> tables, CancellationToken token)
        {
            var result = new List<TagRefReport>();
            foreach (var table in tables)
            {
                if (table != null)
                {
                    var reportItem = new TagRefReport(table.Name);
                    var tagsRefData = reportItem.TagsRefData;
                    foreach (var tag in table.Tags)
                    {
                        if (token.IsCancellationRequested) return null;
                        var tagReferences = TiaProject.CalculateReferences(tag);
                        TagAddressType tagType = DefineTagType(tag);                       
                        if (tagsRefData[tagType].ContainsKey(tagReferences)) tagsRefData[tagType][tagReferences]++;
                        else tagsRefData[tagType][tagReferences] = 1;
                    }
                    reportItem.TagsRefData = tagsRefData;
                    result.Add(reportItem);
                }
            }
            return result;
        }

        /// <summary>
        /// Define tag type depend of address.
        /// </summary>
        /// <param name="tag">PLC tag</param>
        /// <returns>Taf type.</returns>
        private static TagAddressType DefineTagType(PlcTag tag)
        {
            string tagAdress = tag.LogicalAddress;
            switch (tagAdress.ToLower()[1])
            {
                case 'i': return TagAddressType.Input;
                case 'q': return TagAddressType.Output;
                case 'm': return TagAddressType.Merker;
                case 't': return TagAddressType.Timer;
                default: return TagAddressType.Undefined;
            }
        }

    }
}
