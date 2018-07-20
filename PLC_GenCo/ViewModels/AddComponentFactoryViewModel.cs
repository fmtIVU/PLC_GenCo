using PLC_GenCo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels
{
    public class AddComponentFactoryViewModel
    {
        public IEnumerable<ComponentLocation> ComponentLocations { get; set; }
        public IEnumerable<Standard> Standards { get; set; }
        public Component Component { get; set;}


    }
}