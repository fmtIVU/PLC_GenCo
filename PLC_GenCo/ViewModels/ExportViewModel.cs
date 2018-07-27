using PLC_GenCo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.ViewModels
{
    public class ExportViewModel : BaseViewModel
    {
        public PLC Controller { get; set; }
        public List<Module> Modules { get; set; }
        public List<UDT> UDTs { get; set; }
        public List<Standard> AOIs { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Task> Tasks { get; set; }
    }
}