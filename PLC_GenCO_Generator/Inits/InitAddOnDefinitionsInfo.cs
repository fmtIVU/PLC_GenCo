using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_GenCO_Generator.Inits
{
    class InitAddOnDefinitionsInfo
    {
        public AddOnDefinitionsInfo AddOnDefinitionsInfo { get; }

        public InitAddOnDefinitionsInfo(AddOnDefinitionsInfo addOnDefinitionsInfo)
        {
            AddOnDefinitionsInfo = addOnDefinitionsInfo;
        }

        public AddOnDefinitionsInfo InitializedData()
        {

            return new AddOnDefinitionsInfo();
        }
    }
}
