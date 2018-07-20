using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.Models.Setups
{
    public class MotFrqSetup
    {
        public int Id { get; set; }
        public int IdComponent { get; set; }

        public FrqType FrqType { get; set; }

        public string IPAddress { get; set; }

        public int? NominalSpeed { get; set; }

    }
}