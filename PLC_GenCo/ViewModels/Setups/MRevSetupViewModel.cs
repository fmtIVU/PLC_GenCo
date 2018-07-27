using PLC_GenCo.Models;
using PLC_GenCo.Models.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels.Setups
{
    public class MRevSetupViewModel : BaseViewModel
    {
        public MRevSetup MRevSetup { get; set; }
        public Component Component { get; set; }
        public List<IO> Childs { get;  set; }
        public AIAlarmSetup AIAlarm { get; set; }
        public DIAlarmSetup DIAlarm { get; set; }
        public List<DIAlarmSetup> DIAlarms { get; set; }
        public List<AIAlarmSetup> AIAlarms { get; set; }

    }
}