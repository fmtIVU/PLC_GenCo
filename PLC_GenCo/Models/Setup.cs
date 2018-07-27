using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Models
{
    public class Setup
    {
        public string AOIName { get; set; }
        public List<Parameter> Parameters { get; set; }
    }
}