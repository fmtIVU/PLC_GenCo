using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels
{
    public class FactoryViewModel
    {
        public List<Standard> Standards { get; set; }
        public List<Component> Components { get; set; }
    }
}