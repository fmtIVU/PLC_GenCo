using PLC_GenCo.Models.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels
{
    public class DuplicateViewModel : BaseViewModel
    {
        public Component ComponentToCopy { get; set; }
        public List<DIAlarmSetup> DIAlarmSetups { get; set; }
        public List<AIAlarmSetup> AIAlarmSetups { get; set; }
        public List<Component> ComponentsForCopy { get; set; }
        
    }
}