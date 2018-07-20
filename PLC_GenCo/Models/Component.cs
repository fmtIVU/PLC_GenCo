using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.ViewModels
{
    public class Component
    {
        public int Id { get; set; }
        public int? StandardId { get; set; }
        public int? IOId { get; set; }


        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }
        
        public Dependancy Depandancy { get; set; }
        public MatchStatus MatchStatus { get; set; }
        public ConnectionType ConnectionType { get; set; }

        

    }
}