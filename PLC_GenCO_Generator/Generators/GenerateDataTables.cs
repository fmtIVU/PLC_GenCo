using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PLC_GenCO_Generator.Generators
{
    class GenerateDataTables
    {
        private DataTablesInfo _dataTablesInfo;

        public GenerateDataTables(DataTablesInfo dataTablesInfo)
        {
            _dataTablesInfo = dataTablesInfo;
        }

        public XElement GetDataTables()
        {
            var dataTables = new XElement("DataTables",
                GetDIDataType(),
                GetDODataType(),
                GetAIDataType(),
                GetAODataType(),
                GetIODataType(),
                GetPumpDataType(),
                GetPumpsDataType(),
                GetValveDataType(),
                GetValvesDataType()
                );

                return dataTables;

        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //generate udt_IO
        private XElement GetIODataType()
        {
            var IODataType = new XElement("IODataType");

            return IODataType;
        }

        //generate udt_DI
        private XElement GetDIDataType()
        {
            //filter DI components
            var DIComponents = _dataTablesInfo.components.Where(c => (c.ConnectionType == Enums.ConnectionType.AI_and_DIPoulse || c.ConnectionType == Enums.ConnectionType.Digital_Input || c.ConnectionType == Enums.ConnectionType.Digital_Poulse)).ToList();

            //Generate members
            var DIMembers = new List<XElement>();
            foreach(Component component in DIComponents)
            {

                var xElement = new XElement("Member",
                    new XAttribute("Name", component.Name),
                    new XAttribute("DataType", "BIT"),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("Target", "ZZZZZZZZZZudt_DO" + (((DIMembers.Count / 8) * 9)).ToString()),
                    new XAttribute("BitNumber", ((DIMembers.Count % 8)).ToString() ),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", "<![CDATA[" + component.Comment + "]]>")
                    );
                DIMembers.Add(xElement);
            }

            var DIDataType = new XElement("DataType",
                new XAttribute("Name", "udt_DI"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),
                DIMembers,
                new XElement("Description", "<![CDATA[Digital inputs]]>")
                );

            return DIDataType;
        }

        //generate udt_DO
        private XElement GetDODataType()
        {
            var DODataType = new XElement("DODataType");

            return DODataType;
        }

        //generate udt_AI
        private XElement GetAIDataType()
        {
            var AIDataType = new XElement("AIDataType");

            return AIDataType;
        }

        //generate udt_AO
        private XElement GetAODataType()
        {
            var AODataType = new XElement("AODataType");

            return AODataType;
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //generate udt_Pump
        private XElement GetPumpDataType()
        {
            var pumpDataType = new XElement("PumpDataType");

            return pumpDataType;
        }

        //generate udt_Pumps
        private XElement GetPumpsDataType()
        {
            var pumpsDataType = new XElement("PumpsDataType");

            return pumpsDataType;
        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------

        //generate udt_Valve
        private XElement GetValveDataType()
        {
            var valveDataType = new XElement("ValveDataType");

            return valveDataType;
        }

        //generate udt_Valves
        private XElement GetValvesDataType()
        {
            var valvesDataType = new XElement("ValvesDataType");

            return valvesDataType;
        }
    }
}
