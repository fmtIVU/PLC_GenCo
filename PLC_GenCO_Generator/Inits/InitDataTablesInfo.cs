using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_GenCO_Generator.Inits
{
    class InitDataTablesInfo
    {
        public DataTablesInfo DataTablesInfo { get; }

        public InitDataTablesInfo(DataTablesInfo dataTablesInfo)
        {
            DataTablesInfo = dataTablesInfo;
        }

        public DataTablesInfo InitializedData()
        {
            //set default values

            return DataTablesInfo;
        }
           
    }

}
