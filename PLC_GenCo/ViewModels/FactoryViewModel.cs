using PLC_GenCo.Models;
using PLC_GenCo.Models.Setups;
using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels
{
    public class FactoryViewModel : BaseViewModel
    {
        public List<Component> Components { get; set; }
        public List<Standard> Standnards { get; set; }
        public List<IO> IOs { get; set; }
        public List<AIAlarmSetup> AIAlarms { get; set; }
        public List<DIAlarmSetup> DIAlarms { get; set; }

    }
}