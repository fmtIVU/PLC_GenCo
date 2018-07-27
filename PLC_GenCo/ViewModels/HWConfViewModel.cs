using PLC_GenCo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels
{
    public class HWConfViewModel : BaseViewModel
    {
        public List<Module> Modules { get; set; }
        public List<ComponentLocation> Locations { get; set; }
        public PLC PLC { get; set; }
    }
}