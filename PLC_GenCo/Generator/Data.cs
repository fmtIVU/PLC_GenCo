using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Generator
{
    public class Data
    {
        const string configTagData = "54 00 00 00 71 00 00 00 01 00 00 00 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 00 00 00 00 00 00 00 00 00 00 00 00";
        const string forceDataDI = "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
        const string forceDataDO = "00 00 00 00 00 00 00 00 00 00 00 00";
        const string dataDIx8 = "28 00 00 00 67 00 00 00 01 00 00 00 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03";
        const string forceDataDIx8 = "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
        const string dataDIx4 = "18 00 00 00 67 00 00 00 01 00 00 00 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03 E8 03";
        const string forceDataDIx4 = "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
        const string dataAIx4 = "52 00 00 00 7B 00 00 00 01 00 00 00 CD 0C FF 3F 00 00 29 0C A3 40 33 0B 99 41 03 00 00 00 CD 0C FF 3F 00 00 29 0C A3 40 33 0B 99 41 03 00 00 00 CD 0C FF 3F 00 00 29 0C A3 40 33 0B 99 41 03 00 00 00 CD 0C FF 3F 00 00 29 0C A3 40 33 0B 99 41 03 00 00 00 64 00 00 00";
        const string forceDataAIx4 = "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
        const string dataAIx8 = "9A 00 00 00 7B 00 00 00 01 00 00 00 CD 0C FF 3F 00 00 29 0C A3 40 33 0B 99 41 03 00 00 00 CD 0C FF 3F 00 00 29 0C A3 40 33 0B 99 41 03 00 00 00 CD 0C FF 3F 00 00 29 0C A3 40 33 0B 99 41 03 00 00 00 CD 0C FF 3F 00 00 29 0C A3 40 33 0B 99 41 03 00 00 00 CD 0C FF 3F 00 00 29 0C A3 40 33 0B 99 41 03 00 00 00 CD 0C FF 3F 00 00 29 0C A3 40 33 0B 99 41 03 00 00 00 CD 0C FF 3F 00 00 29 0C A3 40 33 0B 99 41 03 00 00 00 CD 0C FF 3F 00 00 29 0C A3 40 33 0B 99 41 03 00 00 00 64 00 00 00";
        const string forceDataAIx8 = "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
        const string dataAOx4 = "50 00 00 00 7B 00 00 00 01 00 00 00 00 00 00 00 66 06 FF 1F 00 80 FF 7F 00 01 01 00 00 00 00 00 00 00 66 06 FF 1F 00 80 FF 7F 00 01 01 00 00 00 00 00 00 00 66 06 FF 1F 00 80 FF 7F 00 01 01 00 00 00 00 00 00 00 66 06 FF 1F 00 80 FF 7F 00 01 01 00 00 00";
        const string forceDataAOx4 = "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";

        public string ConfigTagData { get;}
        public string ForceDataDI { get;}
        public string ForceDataDO { get;}
        public string DataDIx8 { get;}
        public string ForceDataDIx8 { get; }
        public string DataDIx4 { get; }
        public string ForceDataDIx4 { get; }
        public string DataAIx4 { get; }
        public string ForceDataAIx4 { get; }
        public string DataAIx8 { get; }
        public string ForceDataAIx8 { get; }
        public string DataAOx4 { get; }
        public string ForceDataAOx4 { get; }

        public Data()
        {
            ConfigTagData = configTagData;
            ForceDataDI = forceDataDI;
            ForceDataDO = forceDataDO;
            DataDIx8 = dataDIx8;
            ForceDataDIx8 = ForceDataDIx8;
            DataDIx4 = dataDIx4;
            ForceDataDIx4 = ForceDataDIx4;
            DataAIx4 = dataAIx4;
            ForceDataAIx4 = ForceDataAIx4;
            DataAIx8 = dataAIx8;
            ForceDataAIx8 = ForceDataAIx8;
            DataAOx4 = dataAOx4;
            ForceDataAOx4 = ForceDataAOx4;

        }
        
    }
}