using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels
{
    public class IndexHomeViewModel : BaseViewModel
    {
        public List<String> Projects { get; set; }
        public string UserName { get; set; }
        public string ProjectName { get; set; }
    }
}