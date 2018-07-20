using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.Models.Setups
{
    public class MRevSetup
    {
        public int Id { get; set; }
        public int IdComponent { get; set; }

        public int? OUTStartSignalFW { get; set; }
        public int? OUTStartSignalBW { get; set; }

        public int? OUTResetSignal { get; set; }

        public int? INRunningFBFW { get; set; }
        public int? INRunningFBBW { get; set; }

    }
}