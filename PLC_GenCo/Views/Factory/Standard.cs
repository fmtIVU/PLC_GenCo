using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PLC_GenCo.ViewModels
{
    public class Standard
    {
        public int Id { get; set; }
        public Enums.StandardComponent StandardComponent { get; set; }
        public Enums.ConnectionType ConnectionType { get; set; }

    }
}