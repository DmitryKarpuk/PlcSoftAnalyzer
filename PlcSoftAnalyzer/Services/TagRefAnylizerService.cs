using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Siemens.Engineering.CrossReference;
using Siemens.Engineering.SW.Tags;
using PlcSoftAnalyzer.Model;
using PlcSoftAnalyzer.Interfaces;

namespace PlcSoftAnalyzer.Services
{
    public class TagRefAnylizerService : ITagRefAnalyzerService
    {
        public List<TagTableRefReport> TagTableRefReportSource {  get; private set; }
        public  Dictionary<TagAddressType, int> LimitsMap { get; set; }

        public TagRefAnylizerService()
        {
            TagTableRefReportSource = new List<TagTableRefReport>();
        }
        public TagRefAnylizerService(Dictionary<TagAddressType, int> limitsMap)
        {
            LimitsMap = limitsMap;
            TagTableRefReportSource = new List<TagTableRefReport>();
        }

        public void LoadTagRefOutOfLimitData(List<PlcTagTable> tables, CancellationToken token)
        {
            foreach (var table in tables)
            {
                if (table != null)
                {
                    var reportItem = new TagTableRefReport(table.Name, table.Tags.Count);
                    foreach (var tag in table.Tags)
                    {
                        if (token.IsCancellationRequested) return ;
                        var tagType = DefineTagType(tag);
                        if (LimitsMap.ContainsKey(tagType))
                        {
                            int typeLimit = LimitsMap[tagType];
                            var tagReferences = CalculateReferences(tag);
                            if (tagReferences > typeLimit || tagReferences <= 0)
                            {
                                var tagInfo = new PlcTagInfo(tag.Name, tag.LogicalAddress, tagReferences, tagType);
                                reportItem.RefOutOfLimitData.Add(tagInfo);
                            }
                        }
                        else continue;
                    }
                    TagTableRefReportSource.Add(reportItem);
                }               
            }
        }



        /// <summary>
        /// Calculate cross references of the PLC tag.
        /// </summary>
        /// <param name="tag">PLC tag object</param>
        /// <returns>Amount of references.</returns>
        private int CalculateReferences(PlcTag tag)
        {
            int referenceCount = 0;
            var sources = GetSources(tag);
            foreach (var source in sources)
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
        private SourceObjectComposition GetSources(PlcTag tag)
        {
            var tagRefService = tag.GetService<CrossReferenceService>();
            var tagRefResult = tagRefService.GetCrossReferences(CrossReferenceFilter.AllObjects);
            SourceObjectComposition tagRefSources = tagRefResult.Sources;
            return tagRefSources;
        }

        /// <summary>
        /// Define tag type depend of address.
        /// </summary>
        /// <param name="tag">PLC tag</param>
        /// <returns>Taf type.</returns>
        private static TagAddressType DefineTagType(PlcTag tag)
        {
            var tagAddress = tag.LogicalAddress;
            switch (tagAddress.ToLower()[1])
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
