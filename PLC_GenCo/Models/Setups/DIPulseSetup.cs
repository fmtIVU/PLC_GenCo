using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Models.Setups
{
    public class DIPulseSetup
    {
        public int Id { get; set; }
        public int IdComponent { get; set; }
        public int? IdIO { get; set; }
        public int? PulsesPerUnit { get; set; }
    }
}