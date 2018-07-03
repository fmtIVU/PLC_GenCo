using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.Models.Setups
{
    public class AIAlarmSetup
    {
        public int Id { get; set; }
        public int IdComponent { get; set; }
        public int? IdIO { get; set; }
        public AICType AICType { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float? ScaleMax { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float? ScaleMin { get; set; }

        public int? TimeDelay { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float? AlarmHigh { get; set; }
        public bool UseAlarmHigh { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float? AlarmEqual { get; set; }
        public bool UseAlarmEqual { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float? AlarmLow { get; set; }
        public bool UseAlarmLow { get; set; }


    }
}