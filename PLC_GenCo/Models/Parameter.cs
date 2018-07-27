using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.Models
{
    public class Parameter
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Value { get; set; }
        public string DefaultValue { get; set; }
        public ParType Type { get; set; }
        public InOut Usage { get; set; }
        public DataType DataType { get; set; }

        public int? AuxValueINT { get; set; }
        public bool AuxValueBOOL { get; set; }
        public float? AuxValueFLOAT { get; set; }

    }
}