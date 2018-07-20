using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.Generator
{
    public class GenerateAddOnInstructionDefinitions
    {
        private AddOnInstructionDefinitionsInfo _addOnInstructionDefinitionsInfo;

        public GenerateAddOnInstructionDefinitions(AddOnInstructionDefinitionsInfo addOnInstructionDefinitionsInfo)
        {
            _addOnInstructionDefinitionsInfo = addOnInstructionDefinitionsInfo;
        }

        public XElement GetAddInstructionOnDefinitions()
        {
            
            var requiredStandards = new List<Standard>();
            var AddOnInstructionDefinitions = new XElement("AddOnInstructionDefinitions");

            //Filter required standards (remove duplicates)
            foreach (var component in _addOnInstructionDefinitionsInfo.Components)
            {
                //Skip components without standard ex. DO ----- TODO Warning
                if (component.StandardId == null)
                    continue;


                var standard = _addOnInstructionDefinitionsInfo.Standards.Single(c => c.Id == component.StandardId);

                //Filter => add uniques only ---- Filter by name, some standards use same AOI
                if (requiredStandards.Count == 0 || !requiredStandards.Any(c => c.AOIName == standard.AOIName))
                {
                    requiredStandards.Add(_addOnInstructionDefinitionsInfo.Standards.FirstOrDefault(c => c.Id == component.StandardId));
                }

            }

            //Add always used and depandant AOIs  ----------TODO - hard coded, update info
            //Always add CLOCK
            requiredStandards.Insert(0, _addOnInstructionDefinitionsInfo.Standards.FirstOrDefault(c => c.AOIName == "CLOCK"));
            //If frequency converters are used add handler
            if (requiredStandards.Any(c => ((c.AOIName == "MotorDir" || c.AOIName == "MotorRev") && c.ConnectionType == ConnectionType.ETH)))
            {
                requiredStandards.Add(_addOnInstructionDefinitionsInfo.Standards.FirstOrDefault(c => c.AOIName == "DanfossFC_FV2_0"));
            }

            //If analog alarms are used, add scaling AOI
            if(requiredStandards.Any(c => c.AOIName == "Analog"))
            {
                //put on beginning of the list -- some other AOIs need it as dependancy
                requiredStandards.Insert(0, _addOnInstructionDefinitionsInfo.Standards.FirstOrDefault(c => c.AOIName == "SCP"));
            }

            //Load required standards
            foreach (var standard in requiredStandards)
            {
                //Dummy DO standard
                if (standard.AOIName == "Std_DO_NoAOI")
                    continue;

                var uri = new System.Uri(@"C:\Users\Ivan\Desktop\OP generator PLC koda\StandardAOI\" + standard.AOIName + ".L5X");
                var aoi = XElement.Load(uri.ToString());

                //Navigate to child element "AddOnInstruction" ----- Skip export/project info
                AddOnInstructionDefinitions.Add(aoi.Descendants("AddOnInstructionDefinition"));

            }
            

            return AddOnInstructionDefinitions;
        }
    }


}