using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Models
{
    public class Task
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public List<Program> Programs { get; set; }
    }
}