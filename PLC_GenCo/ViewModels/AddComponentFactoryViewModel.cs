using PLC_GenCo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels
{
    public class AddComponentFactoryViewModel : BaseViewModel
    {
        public IEnumerable<ComponentLocation> ComponentLocations { get; set; }
        public IEnumerable<Standard> Standards { get; set; }
        public Component Component { get; set;}
        public List<IO> DIChilds { get; set; }
        public List<IO> DOChilds { get; set; }
        public List<IO> AIChilds { get; set; }
        public List<IO> AOChilds { get; set; }

    }
}