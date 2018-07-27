using PLC_GenCo.Models.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels.Setups
{
    public class DIPulseSetupViewModel : BaseViewModel
    {
        public DIPulseSetup DIPulseSetup { get; set; }
        public Component Component { get; set; }
    }
}