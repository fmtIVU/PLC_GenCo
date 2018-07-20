using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Models
{
    public class Program
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Routine> Routines { get; set; }
    }
}