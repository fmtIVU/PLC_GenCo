using PLC_GenCo.Models;
using PLC_GenCo.Models.Setups;
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
        public List<Component> Components;
        public List<Standard> Standards;
        public List<ComponentLocation> Locations;
        public List<IO> IOs;
        public List<Module> Modules;
        public List<DIPulseSetup> DIPulseSetups;
        public List<MotFrqSetup> MotFrqSetups;
        public List<DIAlarmSetup> DIAlarmSetups;
        public List<AIAlarmSetup> AIAlarmSetups;

        public bool ApplyLocationFilter;
    }

    public struct ModulesInfo
    {
        public ControllerInfo controller;
        public List<Module> modules;
        public List<MotFrqSetup> MotFrqSetups;
        public List<IO> IOs;
        public List<Component> Components;

    }
    public struct AddOnInstructionDefinitionsInfo
    {
        public List<Component> Components;
        public List<Standard> Standards;
    }

    public struct TagsInfo
    {

    }
    public struct ProgramsInfo
    {
        public List<Module> Modules;
        public List<IO> IOs;
        public GenerateTags GenerateTags;
        public List<AIAlarmSetup> AIAlarmSetups;
        public List<DIAlarmSetup> DIAlarmSetups;
        public List<DIPulseSetup> DIPulseSetups;
        public List<MDirSetup> MDirSetups;
        public List<MRevSetup> MRevSetups;
        public List<MotFrqSetup> MotFrqSetups;
        public List<StdVlvSetup> StdVlvSetups;
        public List<Component> Components;
    }
    public struct TasksInfo
    {

    }
}