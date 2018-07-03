using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Generator.Inits
{
    public class InitProgramsInfo
    {
        public ProgramsInfo ProgramsInfo { get; }

        public InitProgramsInfo(ProgramsInfo programsInfo)
        {
            ProgramsInfo = programsInfo;
        }

        public ProgramsInfo InitializedData()
        {

            return new ProgramsInfo();
        }
    }
}