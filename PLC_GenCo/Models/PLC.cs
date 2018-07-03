using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.Models
{
    public class PLC
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ControllerType ProductType { get; set; }
    }
}