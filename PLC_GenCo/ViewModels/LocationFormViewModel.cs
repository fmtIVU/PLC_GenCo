﻿using PLC_GenCo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels 
{
    public class LocationFormViewModel : BaseViewModel
    {
        public ComponentLocation Location { get; set; }
    }
}