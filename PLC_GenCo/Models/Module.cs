using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.Models
{
    public class Module
    {
        public int Id { get; set; }

        public int ModuleAddress { get; set; }

        public IOModulesType IOModulesType { get; set; }

        [Required]
        public string Name { get; set; }

        public string[] Comments { get; set; }

        //[Required]
        //public string Location { get; set; }

    }
}