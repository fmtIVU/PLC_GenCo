using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.Models
{
    public class IO
    {
        public int Id { get; set; }

        public string Location { get; set; }

        public ConnectionType ConnectionType { get; set; }

        public PLCAddress PLCAddress { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }

        //Matchup parameters
        public StandardComponent Standard { get; set; }
        public int? Parent { get; set; }
        public MatchStatus MatchStatus { get; set; }



    }
}