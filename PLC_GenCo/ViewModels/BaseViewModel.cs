using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels
{
    public class BaseViewModel : IHeaderInfo
    {
        public string PageName { get; set; }
    }
}