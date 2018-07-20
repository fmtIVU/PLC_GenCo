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
            //Clean up danish letters for udt-s
            foreach (var io in DataTablesInfo.IOs)
            {
                if (io.Name.Contains('å'))
                {
                    io.Name = io.Name.Replace("å", "aa");
                }

                if (io.Name.Contains('æ'))
                {
                    io.Name = io.Name.Replace("æ", "ae");
                }

                if (io.Name.Contains('ø'))
                {
                    io.Name = io.Name.Replace("ø", "oe");
                }

            }
            


            return DataTablesInfo;
        }
    }
}