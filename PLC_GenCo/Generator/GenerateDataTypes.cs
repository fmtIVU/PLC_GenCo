using PLC_GenCo.Models;
using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace PLC_GenCo.Generator
{
    public class GenerateDataTypes
    {
        private DataTypesInfo _dataTypesInfo;

        public GenerateDataTypes(DataTypesInfo dataTypesInfo)
        {
            _dataTypesInfo = dataTypesInfo;
        }

        public XElement GetDataTypes()
        {
            var dataTypes = new XElement("DataTypes",
                new XAttribute("Use", "Context")
                );

            if (_dataTypesInfo.ApplyLocationFilter)
            {
                dataTypes.Add(GetPumpDataType());                                                                                        //Add udt_Pump 
                dataTypes.Add(GetValveDataType());                                                                                       //Add udt_Valve

                foreach (ComponentLocation location in _dataTypesInfo.Locations)
                {
                    var iosInSameLocation = _dataTypesInfo.IOs.Where(c => c.Location == location.Name).ToList();

                    var DIDataType = GetDIDataType(iosInSameLocation, location.Prefix);                                                  //Add udt_DI if DI exist
                    if (DIDataType.Name != "Empty")
                        dataTypes.Add(DIDataType);

                    var DODataType = GetDODataType(iosInSameLocation, location.Prefix);                                                  //Add udt_DO if DO exist
                    if (DODataType.Name != "Empty")
                        dataTypes.Add(DODataType);

                    var AIDataType = GetAIDataType(iosInSameLocation, location.Prefix);                                                  //Add udt_AI if AI exist
                    if (AIDataType.Name != "Empty")
                        dataTypes.Add(AIDataType);

                    var AODataType = GetAODataType(iosInSameLocation, location.Prefix);                                                  //Add udt_AO if AO exist
                    if (AODataType.Name != "Empty")
                        dataTypes.Add(AODataType);

                    if ((AODataType.Name != "Empty") || (AIDataType.Name != "Empty") || (DODataType.Name != "Empty") || (DIDataType.Name != "Empty"))
                        dataTypes.Add(GetIODataType(DIDataType, DODataType, AIDataType, AODataType, location.Prefix));

                }


                foreach (ComponentLocation location in _dataTypesInfo.Locations)
                {
                    var componentsInSameLocation = _dataTypesInfo.Components.Where(c => c.Location == location.Name).ToList();

                    var pumpStandards = _dataTypesInfo.Standards.Where(c => c.Group == "Motor").Select(c => c.Id).ToArray(); 
                    var pumpsDataType = GetPumpsDataType(componentsInSameLocation, pumpStandards, location.Prefix);                      //Add udt_Pumps if pumps exist
                    if (pumpsDataType.Name != "Empty")
                    {
                        dataTypes.Add(pumpsDataType.Elements("DataType").First(c => c.Attribute("Name").Value == "udt_Pumps"));
                        dataTypes.Add(pumpsDataType.Elements("DataType").First(c => c.Attribute("Name").Value == "ust_AOIPumps"));
                    }
                        

                    var valveStandards = _dataTypesInfo.Standards.Where(c => c.Group == "Valve").Select(c => c.Id).ToArray();
                    var valvesDataType = GetValvesDataType(componentsInSameLocation, valveStandards, location.Prefix);                   //Add udt_Valves if valves exist
                    if (valvesDataType.Name != "Empty")
                    {
                        dataTypes.Add(valvesDataType.Elements("DataType").First(c => c.Attribute("Name").Value == "udt_Valves"));
                        dataTypes.Add(valvesDataType.Elements("DataType").First(c => c.Attribute("Name").Value == "udt_AOIValves"));
                    }
                }

            }
            else
            {
                var DIDataType = GetDIDataType(_dataTypesInfo.IOs, "");                                                                  //Add udt_DI if DI exist
                if (DIDataType.Name != "Empty")
                    dataTypes.Add(DIDataType);

                var DODataType = GetDODataType(_dataTypesInfo.IOs, "");                                                                  //Add udt_DO if DO exist
                if (DODataType.Name != "Empty")
                    dataTypes.Add(DODataType);

                var AIDataType = GetAIDataType(_dataTypesInfo.IOs, "");                                                                  //Add udt_AI if AI exist
                if (AIDataType.Name != "Empty")
                    dataTypes.Add(AIDataType);

                var AODataType = GetAODataType(_dataTypesInfo.IOs, "");                                                                  //Add udt_AO if AO exist
                if (AODataType.Name != "Empty")
                    dataTypes.Add(AODataType);

                if ((AODataType.Name != "Empty") || (AIDataType.Name != "Empty") || (DODataType.Name != "Empty") || (DIDataType.Name != "Empty"))
                    dataTypes.Add(GetIODataType(DIDataType, DODataType, AIDataType, AODataType, ""));

                dataTypes.Add(GetPumpDataType());                                                                                        //Add udt_Pump 
                dataTypes.Add(GetValveDataType());                                                                                       //Add udt_Valve

                var pumpStandards = _dataTypesInfo.Standards.Where(c => c.Group == "Motor").Select(c => c.Id).ToArray();
                var pumpsDataType = GetPumpsDataType(_dataTypesInfo.Components, pumpStandards, "");                                      //Add udt_Pumps if pumps exist
                if (pumpsDataType.Name != "Empty")
                {
                    dataTypes.Add(pumpsDataType.Elements("DataType").First(c => c.Attribute("Name").Value == "udt_Pumps"));
                    dataTypes.Add(pumpsDataType.Elements("DataType").First(c => c.Attribute("Name").Value == "udt_AOIPumps"));
                }

                var valveStandards = _dataTypesInfo.Standards.Where(c => c.Group == "Valve").Select(c => c.Id).ToArray();
                var valvesDataType = GetValvesDataType(_dataTypesInfo.Components, valveStandards, "");                                   //Add udt_Valves if valves exist
                if (valvesDataType.Name != "Empty")
                {
                    dataTypes.Add(valvesDataType.Elements("DataType").First(c => c.Attribute("Name").Value == "udt_Valves"));
                    dataTypes.Add(valvesDataType.Elements("DataType").First(c => c.Attribute("Name").Value == "udt_AOIValves"));
                }
            }


            dataTypes.Add(GetDisp());                           //udt used to mark not used I/Os

            var cnt = GetCNT();
            if (cnt.Name != "Empty")
            {
                dataTypes.Add(cnt);                             //udt used for pulse counters
            }

            dataTypes.Add(GetModuleStatus());                   //udt used for General->Module status

            var modules = GetModules();
            if (modules.Name != "Empty")
            {
                dataTypes.Add(modules);                         //udt used for General->Module status
            }

            dataTypes.Add(GetUDTfromFile("udt_WD"));            //Load watchdog udt

            dataTypes.Add(GetAOIFrq());                         //udt with all Frqhandler AOIs

            dataTypes.Add(GetTODODataType());                   //udt TODO

            var DIAlarms = GetDIAlarms();
            if (DIAlarms.Name != "Empty")
            {
                dataTypes.Add(DIAlarms);                        //List of DI alarms
            }

            var AIAlarms = GetAIAlarms();
            if (AIAlarms.Name != "Empty")
            {
                dataTypes.Add(AIAlarms);                        //List of AI alarms
            }

            return dataTypes;

        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //generate udt_IO
        private XElement GetIODataType(XElement XEDI, XElement XEDO, XElement XEAI, XElement XEAO, string prefix)
        {
            var IOMembers = new List<XElement>();

            if (XEDI.Name != "Empty")
            {
                IOMembers.Add(new XElement("Member",
                    new XAttribute("Name", "DI"),
                    new XAttribute("DataType", "udt_DI" + prefix),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "NullType"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("ExternalAccess", "Read/Write")
                    ));
            }

            if (XEDO.Name != "Empty")
            {
                IOMembers.Add(new XElement("Member",
                    new XAttribute("Name", "DO"),
                    new XAttribute("DataType", "udt_DO" + prefix),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "NullType"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("ExternalAccess", "Read/Write")
                    ));
            }

            if (XEAI.Name != "Empty")
            {
                IOMembers.Add(new XElement("Member",
                    new XAttribute("Name", "AI"),
                    new XAttribute("DataType", "udt_AI" + prefix),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "NullType"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("ExternalAccess", "Read/Write")
                    ));
            }

            if (XEAO.Name != "Empty")
            {
                IOMembers.Add(new XElement("Member",
                    new XAttribute("Name", "AO"),
                    new XAttribute("DataType", "udt_AO" + prefix),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "NullType"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("ExternalAccess", "Read/Write")
                    ));
            }


            var IODataType = new XElement("DataType",
                new XAttribute("Name", "udt_IO" + prefix),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", prefix + " I/O"),

                new XElement("Members",
                    IOMembers
                    )
                );
            return IODataType;
        }

        //generate udt_DI
        private XElement GetDIDataType(List<IO> IOs, string prefix)
        {
            //filter DI components
            var DIComponents = IOs.Where(c => c.ConnectionType == Enums.ConnectionType.DI).ToList();

            if (DIComponents.Count == 0)
            {
                return new XElement("Empty");
            }

            //Generate members
            var DIMembers = new List<XElement>();
            var counter = 0; //counter for boolean members
            var postFix = 0; //postfix for SINT members
            foreach (IO io in DIComponents)
            {

                if ((counter % 8) == 0)
                {

                    if (counter == 0)
                    {
                        postFix = 0;
                    }
                    else
                    {
                        postFix = counter + 1;
                    }

                    var xElementSINT = new XElement("Member",
                        new XAttribute("Name", "ZZZZZZZZZZudt_DI" + prefix + postFix.ToString()),
                        new XAttribute("DataType", "SINT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "true"),
                        new XAttribute("ExternalAccess", "Read/Write")

                        );

                    DIMembers.Add(xElementSINT);
                }

                var xElement = new XElement("Member",
                    new XAttribute("Name", io.Name),
                    new XAttribute("DataType", "BIT"),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("Target", "ZZZZZZZZZZudt_DI" + prefix + postFix.ToString()),
                    new XAttribute("BitNumber", (counter % 8).ToString()),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", io.Comment)
                    );
                DIMembers.Add(xElement);

                counter++;
            }

            var DIDataType = new XElement("DataType",
                new XAttribute("Name", "udt_DI" + prefix),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", prefix + " Digital inputs"),

                new XElement("Members",
                    DIMembers
                    )
                );

            return DIDataType;
        }

        //generate udt_DO
        private XElement GetDODataType(List<IO> IOs, string prefix)
        {
            //filter DO components
            var DOComponents = IOs.Where(c => c.ConnectionType == Enums.ConnectionType.DO).ToList();

            if (DOComponents.Count == 0)
            {
                return new XElement("Empty");
            }

            //Generate members
            var DOMembers = new List<XElement>();
            var counter = 0; //counter for boolean members
            var postFix = 0; //postfix for SINT members
            foreach (IO io in DOComponents)
            {

                if ((counter % 8) == 0)
                {

                    if (counter == 0)
                    {
                        postFix = 0;
                    }
                    else
                    {
                        postFix = counter + 1;
                    }

                    var xElementSINT = new XElement("Member",
                        new XAttribute("Name", "ZZZZZZZZZZudt_DO" + prefix + postFix.ToString()),
                        new XAttribute("DataType", "SINT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "true"),
                        new XAttribute("ExternalAccess", "Read/Write")

                        );

                    DOMembers.Add(xElementSINT);
                }

                var xElement = new XElement("Member",
                    new XAttribute("Name", io.Name),
                    new XAttribute("DataType", "BIT"),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("Target", "ZZZZZZZZZZudt_DO" + prefix + postFix.ToString()),
                    new XAttribute("BitNumber", (counter % 8).ToString()),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", io.Comment)
                    );
                DOMembers.Add(xElement);

                counter++;
            }

            var DODataType = new XElement("DataType",
                    new XAttribute("Name", "udt_DO" + prefix),
                    new XAttribute("Use", "Target"),
                    new XAttribute("Family", "NoFamily"),
                    new XAttribute("Class", "User"),

                    new XElement("Description", prefix + " Digital outputs"),

                    new XElement("Members",
                        DOMembers
                        )
                    );

            return DODataType;
        }

        //generate udt_AI
        private XElement GetAIDataType(List<IO> IOs, string prefix)
        {
            //filter AI components
            var AIComponents = IOs.Where(c => c.ConnectionType == Enums.ConnectionType.AI ).ToList();

            if (AIComponents.Count == 0)
            {
                return new XElement("Empty");
            }

            //Generate members
            var AIMembers = new List<XElement>();
            foreach (IO io in AIComponents)
            {

                var xElement = new XElement("Member",
                    new XAttribute("Name", io.Name),
                    new XAttribute("DataType", "REAL"),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", io.Comment)
                    );
                AIMembers.Add(xElement);
            }

            var AIDataType = new XElement("DataType",
                new XAttribute("Name", "udt_AI" + prefix),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", prefix + " Analog inputs"),

                new XElement("Members",
                    AIMembers
                    )
                );

            return AIDataType;
        }

        //generate udt_AO
        private XElement GetAODataType(List<IO> IOs, string prefix)
        {
            //filter AI components
            var AOComponents = IOs.Where(c => (c.ConnectionType == Enums.ConnectionType.AO || c.ConnectionType == Enums.ConnectionType.AO)).ToList();

            if (AOComponents.Count == 0)
            {
                return new XElement("Empty");
            }

            //Generate members
            var AOMembers = new List<XElement>();
            foreach (IO io in AOComponents)
            {

                var xElement = new XElement("Member",
                    new XAttribute("Name", io.Name),
                    new XAttribute("DataType", "INT"),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", io.Comment)
                    );
                AOMembers.Add(xElement);
            }

            var AODataType = new XElement("DataType",
                new XAttribute("Name", "udt_AO" + prefix),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", prefix + " Analog outputs"),

                new XElement("Members",
                    AOMembers
                    )
                );

            return AODataType;
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //generate udt_Pump
        private XElement GetPumpDataType()
        {
            var stdPumpMembers = new List<XElement>();

            var stdPumpDataType = new XElement("DataType",
                new XAttribute("Name", "udt_Pump"),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "Standard pump interface"),

                new XElement("Members",
                        new XElement("Member",
                        new XAttribute("Name", "ZZZZZZZZZZudt_Pump0"),
                        new XAttribute("DataType", "SINT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "true"),
                        new XAttribute("ExternalAccess", "Read/Write")
                        ),
                    new XElement("Member",
                        new XAttribute("Name", "StartFW"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Pump0"),
                        new XAttribute("BitNumber", "0"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "IN - Start pump command")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "StartBW"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Pump0"),
                        new XAttribute("BitNumber", "1"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "IN - Start pump command")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "Stop"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Pump0"),
                        new XAttribute("BitNumber", "2"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "IN - Stop pump command")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "Ready"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Pump0"),
                        new XAttribute("BitNumber", "3"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "OUT - Pump ready to run")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "RunningFW_FB"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Pump0"),
                        new XAttribute("BitNumber", "4"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "OUT - Running feedback")
                           ),
                    new XElement("Member",
                        new XAttribute("Name", "RunningBW_FB"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Pump0"),
                        new XAttribute("BitNumber", "5"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "OUT - Running feedback")
                           ),
                    new XElement("Member",
                        new XAttribute("Name", "Fault_Reset"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Pump0"),
                        new XAttribute("BitNumber", "6"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "OUT - Fault reset for VLT")
                       ),
                    new XElement("Member",
                        new XAttribute("Name", "SpeedSP"),
                        new XAttribute("DataType", "REAL"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "IN - Speed Set-Point")
                       )
                   )   
              );


            return stdPumpDataType;
        }

        //generate udt_Pumps & udt_AOIPumps
        private XElement GetPumpsDataType(List<Component> components, int[] standardIds, string prefix)
        {
            //filter pump components

            var pumpComponents = new List<Component>();

            foreach (var component in components)
            {
                foreach (var std in standardIds)
                {
                    if (std == component.StandardId)
                    {
                        pumpComponents.Add(component);
                        break;
                    }
                }
            }


            if (pumpComponents.Count == 0)
            {
                return new XElement("Empty");
            }

            //Generate members
            var pumpsMembers = new List<XElement>();
            foreach (Component component in pumpComponents)
            {

                var xElement = new XElement("Member",
                    new XAttribute("Name", component.Name),
                    new XAttribute("DataType", "udt_Pump"),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", component.Comment)
                    );
                pumpsMembers.Add(xElement);
            }

            var pumpsDataType = new XElement("DataType",
                new XAttribute("Name", "udt_Pumps" + prefix),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", prefix + " Pump interfaces"),

                new XElement("Members",
                    pumpsMembers
                    )
                );

            //Generate members for aoi
            var pumpsAOIDataType = new XElement("DataType",
                new XAttribute("Name", "udt_AOIPumps"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "Motor AOIs"),

                new XElement("Members")
                );

            foreach (Component component in pumpComponents)
            {
                //Get AOI name
                var aoiName = _dataTypesInfo.Standards.Single(c => c.Id == component.StandardId).AOIName;

                var xElement = new XElement("Member",
                    new XAttribute("Name", component.Name),
                    new XAttribute("DataType", aoiName),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", component.Comment)
                    );

                pumpsAOIDataType.Element("Members").Add(xElement);
            }


            var pumpsDataTypes = new XElement("DataTypes");
            pumpsDataTypes.Add(pumpsDataType);
            pumpsDataTypes.Add(pumpsAOIDataType);

            return pumpsDataTypes;
        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------

        //generate udt_Valve
        private XElement GetValveDataType()
        {

            var stdValveDataType = new XElement("DataType",
                new XAttribute("Name", "udt_Valve"),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "Standard valve interface"),

                new XElement("Members",
                    new XElement("Member",
                        new XAttribute("Name", "ZZZZZZZZZZudt_Valve0"),
                        new XAttribute("DataType", "SINT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "true"),
                        new XAttribute("ExternalAccess", "Read/Write")
                        ),
                    new XElement("Member",
                        new XAttribute("Name", "Open"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Valve0"),
                        new XAttribute("BitNumber", "0"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "IN - Open valve command")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "Close"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Valve0"),
                        new XAttribute("BitNumber", "1"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "IN - Close valve command")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "Ready"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Valve0"),
                        new XAttribute("BitNumber", "2"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "OUT - Valve ready")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "Opened_FB"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Valve0"),
                        new XAttribute("BitNumber", "3"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "OUT - Opened feedback")
                           ),
                    new XElement("Member",
                        new XAttribute("Name", "Closed_FB"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Valve0"),
                        new XAttribute("BitNumber", "4"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "OUT - Closed feedback")
                           ),
                    new XElement("Member",
                        new XAttribute("Name", "Fault_Reset"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Valve0"),
                        new XAttribute("BitNumber", "5"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "OUT - Fault reset to valve")
                           )
                       )
                );


            return stdValveDataType;
        }

        //generate udt_Valves & udt_AOIValves
        private XElement GetValvesDataType(List<Component> components, int[] standardIds, string prefix)
        {

            //filter valve components
            var valveComponents = new List<Component>();

            foreach (var component in components)
            {
                foreach (var std in standardIds)
                {
                    if (std == component.StandardId)
                    {
                        valveComponents.Add(component);
                        break;
                    }
                }
            }


            if (valveComponents.Count == 0)
            {
                return new XElement("Empty");
            }

            //Generate members for interface
            var valvesMembers = new List<XElement>();
            foreach (Component component in valveComponents)
            {

                var xElement = new XElement("Member",
                    new XAttribute("Name", component.Name),
                    new XAttribute("DataType", "udt_Valve"),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", component.Comment)
                    );
                valvesMembers.Add(xElement);
            }

            var valvesDataType = new XElement("DataType",
                new XAttribute("Name", "udt_Valves" + prefix),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", prefix + " Valve interfaces"),

                new XElement("Members",
                    valvesMembers
                    )
                );

            //Generate members for aoi
            var valvesAOIDataType = new XElement("DataType",
                new XAttribute("Name", "udt_AOIValves"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", " Valve AOIs"),

                new XElement("Members")
                );

            foreach (Component component in valveComponents)
            {

                var xElement = new XElement("Member",
                    new XAttribute("Name", component.Name),
                    new XAttribute("DataType", "ValveControl"),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", component.Comment)
                    );
                valvesAOIDataType.Element("Members").Add(xElement);
            }


            var valvesDataTypes = new XElement("DataTypes");
            valvesDataTypes.Add(valvesDataType);
            valvesDataTypes.Add(valvesAOIDataType);


            return valvesDataTypes;
        }

        private XElement GetDisp()
        {
            var dispDataType = new XElement("DataType",
                new XAttribute("Name", "udt_Disp"),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "Not used I/Os"),

                new XElement("Members",
                    new XElement("Member",
                        new XAttribute("Name", "DI"),
                        new XAttribute("DataType", "BOOL"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "Not used DI")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "DO"),
                        new XAttribute("DataType", "BOOL"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "Not used DO")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "AI"),
                        new XAttribute("DataType", "INT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "Not used AI")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "AO"),
                        new XAttribute("DataType", "INT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "Not used AO")
                    )
                )
            );

            return dispDataType;
        }

        private XElement GetCNT()
        {

            if (_dataTypesInfo.DIPulseSetups.Count <= 0)
                return new XElement("Empty");

            var cntDataType = new XElement("DataType",
                new XAttribute("Name", "udt_CNT"),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "Pulse counter:"),

                new XElement("Members"
                )
            );

            foreach (var DIpulse in _dataTypesInfo.DIPulseSetups)
            {
                var io = _dataTypesInfo.IOs.Single(c => c.Id == DIpulse.IdIO);

                var member = new XElement("Member",
                        new XAttribute("Name", io.Name),
                        new XAttribute("DataType", "CNT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", io.Comment)
                        );


                cntDataType.Element("Members").Add(member);
            }

            return cntDataType;
        }

        private XElement GetModuleStatus()
        {
            var modulestatusDataType = new XElement("DataType",
                new XAttribute("Name", "udt_ModuleStatus"),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "Module status"),

                new XElement("Members",
                    new XElement("Member",
                        new XAttribute("Name", "FaultCode"),
                        new XAttribute("DataType", "DINT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "Fault code")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "Alarm"),
                        new XAttribute("DataType", "BOOL"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "Alarm")
                    )
                )
            );

                    return modulestatusDataType;
        }

        private XElement GetModules()
        {

            if (_dataTypesInfo.Modules.Where(c => (c.IOModulesType != Enums.IOModulesType.EmbDIx16) && (c.IOModulesType != Enums.IOModulesType.EmbDOx16)).Count() <= 0)
                return new XElement("Empty");

            var modulesDataType = new XElement("DataType",
                new XAttribute("Name", "udt_Modules"),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "Module status:"),

                new XElement("Members"
                )
            );

            //Add all PLC IO modules
            foreach (var module in _dataTypesInfo.Modules)
            {
                XElement member;

                if (module.IOModulesType == Enums.IOModulesType.EmbDIx16 || module.IOModulesType == Enums.IOModulesType.EmbDIx16)
                {
                    member = new XElement("Member",
                        new XAttribute("Name", "Discrete_IO"),
                        new XAttribute("DataType", "udt_ModuleStatus"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "Address: 1 Type: PLC Embedded IO")
                        );
                }
                else
                {
                     member = new XElement("Member",
                        new XAttribute("Name", module.Name),
                        new XAttribute("DataType", "udt_ModuleStatus"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "Address: " + module.Address.ToString() + " Type: " + module.IOModulesType.ToString())
                        );
                }

                


                modulesDataType.Element("Members").Add(member);
            }

            //Add all ethernet modules
            foreach (var motFrqSetup in _dataTypesInfo.MotFrqSetups)
            {
                XElement member;
                //Find component which owns setup
                var setupOwner = _dataTypesInfo.Components.Single(c => c.Id == motFrqSetup.IdComponent);


                member = new XElement("Member",
                        new XAttribute("Name", setupOwner.Name),
                        new XAttribute("DataType", "udt_ModuleStatus"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "Address: " + motFrqSetup.IPAddress + " Type: Ethernet module - " + motFrqSetup.FrqType.ToString())
                        );


                modulesDataType.Element("Members").Add(member);
            }

            return modulesDataType;
        }

        private XElement GetUDTfromFile(string name)
        {

            var uri = new System.Uri(@"C:\Users\Ivan\Desktop\OP generator PLC koda\StandardTemplate\UserDefinedDatatypes\" + name + ".L5X");
          
            var udtFile = XElement.Load(uri.ToString());

            return udtFile.Element("Controller").Element("DataTypes").Element("DataType");
        }


        private XElement GetAOIFrq()
        {
            var frqAOIDataType = new XElement("DataType",
                new XAttribute("Name", "udt_AOIFrq"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", " Frequencyconverter AOIs"),

                new XElement("Members")
                );

            foreach (var frq in _dataTypesInfo.MotFrqSetups)
            {

                var setupOwner = _dataTypesInfo.Components.Single(c => c.Id == frq.IdComponent); 

                var xElement = new XElement("Member",
                    new XAttribute("Name", setupOwner.Name),
                    new XAttribute("DataType", "DanfossFC_FV2_0"),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", setupOwner.Comment)
                    );

                frqAOIDataType.Element("Members").Add(xElement);

            }

            return frqAOIDataType;
        }

        //generate udt_TODO
        private XElement GetTODODataType()
        {

            var todoDataType = new XElement("DataType",
                new XAttribute("Name", "udt_TODO"),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "To-Do - generated warnings"),

                new XElement("Members",
                    new XElement("Member",
                        new XAttribute("Name", "BOOL"),
                        new XAttribute("DataType", "BOOL"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", " ")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "DINT"),
                        new XAttribute("DataType", "DINT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", " ")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "INT"),
                        new XAttribute("DataType", "INT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", " ")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "REAL"),
                        new XAttribute("DataType", "REAL"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", " ")
                    )

                )
             );


            return todoDataType;
        }

        private XElement GetDIAlarms()
        {
            if (_dataTypesInfo.DIAlarmSetups.Count <= 0)
                return new XElement("Empty");

            var DIAlarmDataType = new XElement("DataType",
                new XAttribute("Name", "udt_DIAlarms"),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "Digital alarm:"),

                new XElement("Members"
                )
            );

            foreach (var DIAlarm in _dataTypesInfo.DIAlarmSetups)
            {
                var io = _dataTypesInfo.IOs.Single(c => c.Id == DIAlarm.IdIO);

                var member = new XElement("Member",
                        new XAttribute("Name", io.Name),
                        new XAttribute("DataType", "AlarmDi"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", io.Comment)
                        );


                DIAlarmDataType.Element("Members").Add(member);
            }

            return DIAlarmDataType;
        }

        private XElement GetAIAlarms()
        {
            if (_dataTypesInfo.AIAlarmSetups.Count <= 0)
                return new XElement("Empty");

            var AIAlarmDataType = new XElement("DataType",
                new XAttribute("Name", "udt_AIAlarms"),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "Analog alarm:"),

                new XElement("Members"
                )
            );

            foreach (var AIAlarm in _dataTypesInfo.AIAlarmSetups)
            {
                var io = _dataTypesInfo.IOs.Single(c => c.Id == AIAlarm.IdIO);

                var member = new XElement("Member",
                        new XAttribute("Name", io.Name),
                        new XAttribute("DataType", "Analog"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", io.Comment)
                        );


                AIAlarmDataType.Element("Members").Add(member);
            }

            return AIAlarmDataType;
        }
    }
}