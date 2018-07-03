using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Generator.Inits
{
    public class InitAddOnInstructionDefinitionsInfo
    {
        public AddOnInstructionDefinitionsInfo AddOnInstructionDefinitionsInfo { get; }

        public InitAddOnInstructionDefinitionsInfo(AddOnInstructionDefinitionsInfo addOnInstructionDefinitionsInfo)
        {
            AddOnInstructionDefinitionsInfo = addOnInstructionDefinitionsInfo;
        }

        public AddOnInstructionDefinitionsInfo InitializedData()
        {

            return AddOnInstructionDefinitionsInfo;
        }
    }
}