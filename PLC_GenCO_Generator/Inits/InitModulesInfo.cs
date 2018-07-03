using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_GenCO_Generator.Inits
{
    class InitModulesInfo
    {
        public ModulesInfo ModulesInfo { get; }

        public InitModulesInfo(ModulesInfo modulesInfo)
        {
            ModulesInfo = modulesInfo;
        }

        public ModulesInfo InitializedData()
        {

            return new ModulesInfo();
        }
    }
}
