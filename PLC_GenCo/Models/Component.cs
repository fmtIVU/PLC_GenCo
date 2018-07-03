using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels
{
    public class Component
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }
        public int StandardId { get; set; }
        public Enums.StandardComponent StandardComponent { get; set; }
        public Enums.ConnectionType ConnectionType { get; set; }

        public bool IsParent { get; set; }

        public int IOId { get; set; }

    }
}