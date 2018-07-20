using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Generator.Inits
{
    public class InitProgramsInfo
    {
        public ProgramsInfo _programsInfo { get; }

        public InitProgramsInfo(ProgramsInfo programsInfo)
        {
            _programsInfo = programsInfo;
        }

        public ProgramsInfo InitializedData()
        {

            return _programsInfo;
        }
    }
}