using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_GenCO_Generator.Inits
{
    class InitGlobalTagsInfo
    {
        public GlobalTagsInfo GlobalTagsInfo { get; }

        public InitGlobalTagsInfo(GlobalTagsInfo globalTagsInfo)
        {
            GlobalTagsInfo = globalTagsInfo;
        }

        public GlobalTagsInfo InitializedData()
        {

            return new GlobalTagsInfo();
        }
    }
}
