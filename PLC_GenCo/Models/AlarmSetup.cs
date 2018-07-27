using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Models
{
    public class AlarmSetup
    {
        public int IOId { get; set; }
        public int IOComment { get; set; }
        public int ComponentId { get; set; }
        public string StandardId { get; set; }
        public List<Parameter> Parameters { get; set; }
    }
}