using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Generator.Inits
{
    public class InitTagsInfo
    {
        public TagsInfo GlobalTagsInfo { get; }

        public InitTagsInfo(TagsInfo globalTagsInfo)
        {
            GlobalTagsInfo = globalTagsInfo;
        }

        public TagsInfo InitializedData()
        {

            return new TagsInfo();
        }
    }
}