using PLC_GenCo.Models;
using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.Generator
{
    public struct ControllerInfo
    {
        public string name;
        public ControllerType procesorType;
        public string catalogNumber;
        public string productCode;
        public string majorRev;
        public string minorRev;
        public string description;
    }

    public struct DataTypesInfo
    {
        public List<Component> components;
        public List<Standard> standards;
        public List<ComponentLocation> locations;
        public bool applyLocationFilter;
    }

    public struct ModulesInfo
    {
        public ControllerInfo controller;
        public List<Module> modules;
        
    }
    public struct AddOnInstructionDefinitionsInfo
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