using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.Models.Setups
{
    public class MDirSetup
    {
        public int Id { get; set; }
        public int IdComponent { get; set; }

        public int? OUTStartSignal { get; set; }
        public int? OUTResetSignal { get; set; }

        public int? INRunningFB { get; set; }

        public InputType InputType01 { get; set; }
        public InputType InputType02 { get; set; }
        public InputType InputType03 { get; set; }
        public InputType InputType04 { get; set; }
        public InputType InputType05 { get; set; }
        public InputType InputType06 { get; set; }
        public InputType InputType07 { get; set; }
        public InputType InputType08 { get; set; }

        public int? INExtFault01 { get; set; }
        public int? INExtFault02 { get; set; }
        public int? INExtFault03 { get; set; }
        public int? INExtFault04 { get; set; }
        public int? INExtFault05 { get; set; }
        public int? INExtFault06 { get; set; }
        public int? INExtFault07 { get; set; }
        public int? INExtFault08 { get; set; }

        public AIAlarmSetup INMeasurement01 { get; set; }
        public AIAlarmSetup INMeasurement02 { get; set; }
        public AIAlarmSetup INMeasurement03 { get; set; }
        public AIAlarmSetup INMeasurement04 { get; set; }
        public AIAlarmSetup INMeasurement05 { get; set; }
        public AIAlarmSetup INMeasurement06 { get; set; }

    }
}