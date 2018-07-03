using PLC_GenCo.Models;
using PLC_GenCo.Models.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels.Setups
{
    public class MDirSetupViewModel
    {
        public MDirSetup MDirSetup { get; set; }
        public Component Component { get; set; }
        public List<IO> Childs { get;  set; }
        
    }
}