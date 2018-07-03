using PLC_GenCo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels
{
    public class EditIOIOListViewModel
    {
        public IO IO { get; set; }
        public IEnumerable<ComponentLocation> IOLocations { get; set; }
        public IEnumerable<Component> Parents { get; set; }
    }
}