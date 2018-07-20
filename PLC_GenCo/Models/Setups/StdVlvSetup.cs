using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.Models.Setups
{
    public class StdVlvSetup
    {
        public int Id { get; set; }
        public int IdComponent { get; set; }

        public int? OUTOpenSignal { get; set; }
        public int? OUTCloseSignal { get; set; }

        public int? OUTResetSignal { get; set; }

        public int? INOpenedFB { get; set; }
        public int? INClosedFB { get; set; }

    }
}