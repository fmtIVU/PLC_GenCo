using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Generator.Inits
{
    public class InitGlobalTagsInfo
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