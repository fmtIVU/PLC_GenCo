using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_GenCO_Generator.Inits
{
    class InitProgramsInfo
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
