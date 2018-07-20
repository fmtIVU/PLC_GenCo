using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.Models.Setups
{
    public class DIAlarmSetup
    {
        public int Id { get; set; }
        public int IdComponent { get; set; }
        public int? IdIO { get; set; }
        public string Comment { get; set; }
        public int? TimeDelay { get; set; }
        public InputType InputType { get; set; }
    }
}