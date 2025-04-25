using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static List<TagTable> GetAllTagTables(PlcSoftware plcSoftware)
        {
            if (plcSoftware != null)
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
            else throw new ArgumentException("Tia project not connected");
        }

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

        public static SourceObjectComposition GetSources(PlcTag tag)
        {
            var tagRefService = tag.GetService<CrossReferenceService>();
            var tagRefResult = tagRefService.GetCrossReferences(CrossReferenceFilter.AllObjects);
            SourceObjectComposition tagRefSources = tagRefResult.Sources;
            return tagRefSources;
        }

        public static List<TagRefReport> GeTableTagsRefData(List<PlcTagTable> tables)
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
