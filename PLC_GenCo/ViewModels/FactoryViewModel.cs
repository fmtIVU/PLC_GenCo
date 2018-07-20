using PLC_GenCo.Models;
using PLC_GenCo.Models.Setups;
using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels
{
    public class FactoryViewModel
    {
        public List<Component> Components { get; set; }
        public List<Standard> Standnards { get; set; }
        public List<IO> IOs { get; set; }
        public List<AIAlarmSetup> AIAlarms { get; set; }
        public List<DIAlarmSetup> DIAlarms { get; set; }
        public List<DIPulseSetup> DIPulses { get; set; }
        public List<MDirSetup> MDirs { get; set; }
        public List<MRevSetup> MRevs { get; set; }
        public List<MotFrqSetup> MotFrqs { get; set; }
        public List<StdVlvSetup> StdVlvs { get; set; }

    }
}