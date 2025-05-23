using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using Siemens.Engineering.SW.Tags;

namespace PlcSoftAnalyzer.Model
{
    public class PlcTagInfo
    {
        public string Name { get; private set; }
        public TagAddressType AddressType { get; private set; }
        public string Address { get; private set; }
        //public string Comment { get; private set; }
        public int ReferenceAmount { get; set;}

        public PlcTagInfo(string name, string address, int referenceAmount, TagAddressType addressType) 
        {
            Name = name;
            Address = address;
            ReferenceAmount = referenceAmount;
            AddressType = addressType;
        }
    }
}
