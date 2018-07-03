using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Generator.Inits
{
    public class InitDataTypesInfo
    {
        public DataTypesInfo DataTablesInfo { get; }

        public InitDataTypesInfo(DataTypesInfo dataTablesInfo)
        {
            DataTablesInfo = dataTablesInfo;
        }

        public DataTypesInfo InitializedData()
        {
            //set default values

            return DataTablesInfo;
        }
    }
}