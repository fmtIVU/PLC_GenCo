using PLC_GenCo.Models;
using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_GenCO_Generator
{
    public struct ControllerInfo
    {
        public string name;
        public string procesorType;
        public string majorRev;
        public string minorRev;
        public string description;
    }

    public struct DataTablesInfo
    {
        public List<Component> components;
        public List<Standard> standards;
        public List<ComponentLocation> locations;
    }

    public struct ModulesInfo
    {

    }
    public struct AddOnDefinitionsInfo
    {

    }

    public struct GlobalTagsInfo
    {

    }
    public struct ProgramsInfo
    {

    }
    public struct TasksInfo
    {

    }
}
