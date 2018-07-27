using PLC_GenCo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels
{
    public class IOListViewModel : BaseViewModel
    {
        public List<IO> IOs { get; set; }
        public List<Component> Components{ get; set; }
        public List<Standard> Standards { get; set; }
        public Component Component { get; set; }


    }
}