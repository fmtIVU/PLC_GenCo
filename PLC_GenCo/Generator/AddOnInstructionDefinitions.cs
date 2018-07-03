using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace PLC_GenCo.Generator
{
    public class AddOnInstructionDefinitions
    {
        private AddOnInstructionDefinitionsInfo _addOnInstructionDefinitionsInfo;

        public AddOnInstructionDefinitions(AddOnInstructionDefinitionsInfo addOnInstructionDefinitionsInfo)
        {
            _addOnInstructionDefinitionsInfo = addOnInstructionDefinitionsInfo;
        }

        public XElement GetAddInstructionOnDefinitions()
        {
            var aoi = new XElement("AOI");

            var uri = new System.Uri(@"C:\Users\Ivan\Desktop\OP generator PLC koda\StandardAOI\Analog");

            aoi = XElement.Load(uri.ToString());

            return new XElement("AddOnInstructionDefinitions");
        }
    }


}