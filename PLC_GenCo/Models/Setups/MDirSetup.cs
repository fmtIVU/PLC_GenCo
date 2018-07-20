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

    }
}