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

            if (_dataTypesInfo.applyLocationFilter)
            {
                dataTypes.Add(GetPumpDataType());                                                       //Add udt_Pump 
                dataTypes.Add(GetValveDataType());                                                      //Add udt_Valve

                foreach (ComponentLocation location in _dataTypesInfo.locations)
                {
                    var componentsInSameLocation = _dataTypesInfo.components.Where(c => c.Location == location.Name).ToList();

                    var DIDataType = GetDIDataType(componentsInSameLocation, location.Prefix);                           //Add udt_DI if DI exist
                    if (DIDataType.Name != "Empty")
                        dataTypes.Add(DIDataType);

                    var DODataType = GetDODataType(componentsInSameLocation, location.Prefix);                           //Add udt_DO if DO exist
                    if (DODataType.Name != "Empty")
                        dataTypes.Add(DODataType);

                    var AIDataType = GetAIDataType(componentsInSameLocation, location.Prefix);                           //Add udt_AI if AI exist
                    if (AIDataType.Name != "Empty")
                        dataTypes.Add(AIDataType);

                    var AODataType = GetAODataType(componentsInSameLocation, location.Prefix);                           //Add udt_AO if AO exist
                    if (AODataType.Name != "Empty")
                        dataTypes.Add(AODataType);

                    if ((AODataType.Name != "Empty") || (AIDataType.Name != "Empty") || (DODataType.Name != "Empty") || (DIDataType.Name != "Empty"))
                        dataTypes.Add(GetIODataType(DIDataType, DODataType, AIDataType, AODataType, componentsInSameLocation, location.Prefix));

                    var pumpsDataType = GetPumpsDataType(componentsInSameLocation, location.Prefix);                     //Add udt_Pumps if pumps exist
                    if (pumpsDataType.Name != "Empty")
                        dataTypes.Add(pumpsDataType);

                    var valvesDataType = GetValvesDataType(componentsInSameLocation, location.Prefix);                   //Add udt_Valves if valves exist
                    if (valvesDataType.Name != "Empty")
                        dataTypes.Add(valvesDataType);

                }

            }
            else
            {
                var DIDataType = GetDIDataType(_dataTypesInfo.components, "");                                          //Add udt_DI if DI exist
                if (DIDataType.Name != "Empty")
                    dataTypes.Add(DIDataType);

                var DODataType = GetDODataType(_dataTypesInfo.components, "");                                          //Add udt_DO if DO exist
                if (DODataType.Name != "Empty")
                    dataTypes.Add(DODataType);

                var AIDataType = GetAIDataType(_dataTypesInfo.components, "");                                          //Add udt_AI if AI exist
                if (AIDataType.Name != "Empty")
                    dataTypes.Add(AIDataType);

                var AODataType = GetAODataType(_dataTypesInfo.components, "");                                          //Add udt_AO if AO exist
                if (AODataType.Name != "Empty")
                    dataTypes.Add(AODataType);

                if ((AODataType.Name != "Empty") || (AIDataType.Name != "Empty") || (DODataType.Name != "Empty") || (DIDataType.Name != "Empty"))
                    dataTypes.Add(GetIODataType(DIDataType, DODataType, AIDataType, AODataType, _dataTypesInfo.components, ""));

                dataTypes.Add(GetPumpDataType());                                                                       //Add udt_Pump 
                dataTypes.Add(GetValveDataType());                                                                      //Add udt_Valve

                var pumpsDataType = GetPumpsDataType(_dataTypesInfo.components, "");                                    //Add udt_Pumps if pumps exist
                if (pumpsDataType.Name != "Empty")
                    dataTypes.Add(pumpsDataType);

                var valvesDataType = GetValvesDataType(_dataTypesInfo.components, "");                                  //Add udt_Valves if valves exist
                if (valvesDataType.Name != "Empty")
                    dataTypes.Add(valvesDataType);
            }

            return dataTypes;

        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //generate udt_IO
        private XElement GetIODataType(XElement XEDI, XElement XEDO, XElement XEAI, XElement XEAO, List<Component> components, string prefix)
        {
            var IOMembers = new List<XElement>();

            if (XEDI.Name != "Empty" )
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

                new XElement("Description", "<![CDATA[" + prefix + " I/O]]>"),

                new XElement("Members",
                    IOMembers
                    )
                );
            return IODataType;
        }

        //generate udt_DI
        private XElement GetDIDataType(List<Component> components, string prefix)
        {
            //filter DI components
            var DIComponents = components.Where(c => c.ConnectionType == Enums.ConnectionType.DI).ToList();

            if (DIComponents.Count == 0)
            {
                return new XElement("Empty");
            }

            //Generate members
            var DIMembers = new List<XElement>();
            var counter = 0; //counter for boolean members
            var postFix = 0; //postfix for SINT members
            foreach (Component component in DIComponents)
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
                    new XAttribute("Name", component.Name),
                    new XAttribute("DataType", "BIT"),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("Target", "ZZZZZZZZZZudt_DI" + prefix + postFix.ToString()),
                    new XAttribute("BitNumber", (counter % 8).ToString()),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", "<![CDATA[" + component.Comment + "]]>")
                    );
                DIMembers.Add(xElement);

                counter++;
            }

            var DIDataType = new XElement("DataType",
                new XAttribute("Name", "udt_DI" + prefix),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "<![CDATA[" + prefix + " Digital inputs]]>"),

                new XElement("Members",
                    DIMembers
                    )
                );

            return DIDataType;
        }

        //generate udt_DO
        private XElement GetDODataType(List<Component> components, string prefix)
        {
            //filter DO components
            var DOComponents = components.Where(c => (c.ConnectionType == Enums.ConnectionType.DO)).ToList();

            if (DOComponents.Count == 0)
            {
                return new XElement("Empty");
            }

            //Generate members
            var DOMembers = new List<XElement>();
            var counter = 0; //counter for boolean members
            var postFix = 0; //postfix for SINT members
            foreach (Component component in DOComponents)
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
                    new XAttribute("Name", component.Name),
                    new XAttribute("DataType", "BIT"),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("Target", "ZZZZZZZZZZudt_DO" + prefix + postFix.ToString()),
                    new XAttribute("BitNumber", (counter % 8).ToString()),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", "<![CDATA[" + component.Comment + "]]>")
                    );
                DOMembers.Add(xElement);

                counter++;
            }

        var DODataType = new XElement("DataType",
                new XAttribute("Name", "udt_DO" + prefix),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "<![CDATA[" + prefix + " Digital outputs]]>"),

                new XElement("Members",
                    DOMembers
                    )
                );

            return DODataType;
        }

        //generate udt_AI
        private XElement GetAIDataType(List<Component> components, string prefix)
        {
            //filter AI components
            var AIComponents = components.Where(c => c.ConnectionType == Enums.ConnectionType.AI).ToList();

            if (AIComponents.Count == 0)
            {
                return new XElement("Empty");
            }

            //Generate members
            var AIMembers = new List<XElement>();
            foreach (Component component in AIComponents)
            {

                var xElement = new XElement("Member",
                    new XAttribute("Name", component.Name),
                    new XAttribute("DataType", "INT"),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", "<![CDATA[" + component.Comment + "]]>")
                    );
                AIMembers.Add(xElement);
            }

            var AIDataType = new XElement("DataType",
                new XAttribute("Name", "udt_AI" + prefix),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "<![CDATA[" + prefix + "Analog inputs]]>"),

                new XElement("Members",
                    AIMembers
                    )
                );

            return AIDataType;
        }

        //generate udt_AO
        private XElement GetAODataType(List<Component> components, string prefix)
        {
            //filter AI components
            var AOComponents = components.Where(c => c.ConnectionType == Enums.ConnectionType.AO).ToList();

            if (AOComponents.Count == 0)
            {
                return new XElement("Empty");
            }

            //Generate members
            var AOMembers = new List<XElement>();
            foreach (Component component in AOComponents)
            {

                var xElement = new XElement("Member",
                    new XAttribute("Name", component.Name),
                    new XAttribute("DataType", "INT"),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", "<![CDATA[" + component.Comment + "]]>")
                    );
                AOMembers.Add(xElement);
            }

            var AODataType = new XElement("DataType",
                new XAttribute("Name", "udt_AO" + prefix),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "<![CDATA[" + prefix + "Analog outputs]]>"),

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

                new XElement("Description", "<![CDATA[Standard pump interface]]>"),

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
                        new XAttribute("Name", "Start"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Pump0"),
                        new XAttribute("BitNumber", "0"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "<![CDATA[IN - Start pump command]]>")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "Stop"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Pump0"),
                        new XAttribute("BitNumber", "1"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "<![CDATA[IN - Stop pump command]]>")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "Ready"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Pump0"),
                        new XAttribute("BitNumber", "2"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "<![CDATA[OUT - Pump ready to run]]>")
                    ),
                    new XElement("Member",
                        new XAttribute("Name", "Running_FB"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Pump0"),
                        new XAttribute("BitNumber", "3"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "<![CDATA[OUT - Running feedback]]>")
                           ),
                    new XElement("Member",
                        new XAttribute("Name", "Fault_Reset"),
                        new XAttribute("DataType", "BIT"),
                        new XAttribute("Dimension", "0"),
                        new XAttribute("Radix", "Decimal"),
                        new XAttribute("Hidden", "false"),
                        new XAttribute("Target", "ZZZZZZZZZZudt_Pump0"),
                        new XAttribute("BitNumber", "4"),
                        new XAttribute("ExternalAccess", "Read/Write"),
                        new XElement("Description", "<![CDATA[OUT - Fault reset for VLT]]>")
                           )
                       )
                   
                );


            return stdPumpDataType;
        }

        //generate udt_Pumps
        private XElement GetPumpsDataType(List<Component> components, string prefix)
        {
            //filter pump components
            var pumpsComponents = components.Where((c => c.StandardComponent == Enums.StandardComponent.P_Std_Motor_Dir ||
                                                         c.StandardComponent == Enums.StandardComponent.P_Std_Motor_Rev ||
                                                         c.StandardComponent == Enums.StandardComponent.P_Std_Motor_Dir_Frq ||
                                                         c.StandardComponent == Enums.StandardComponent.P_Std_Motor_Rev_Frq)).ToList();

            if (pumpsComponents.Count == 0)
            {
                return new XElement("Empty");
            }

            //Generate members
            var pumpsMembers = new List<XElement>();
            foreach (Component component in pumpsComponents)
            {

                var xElement = new XElement("Member",
                    new XAttribute("Name", component.Name),
                    new XAttribute("DataType", "udt_Pump"),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", "<![CDATA[" + component.Comment + "]]>")
                    );
                pumpsMembers.Add(xElement);
            }

            var pumpsDataType = new XElement("DataType",
                new XAttribute("Name", "udt_Pumps" + prefix),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "<![CDATA["+ prefix + " Pump interfaces]]>"),

                new XElement("Members",
                    pumpsMembers
                    )
                );

            return pumpsDataType;
        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------

        //generate udt_Valve
        private XElement GetValveDataType()
        {
            var stdValveMembers = new List<XElement>();

            var stdValveDataType = new XElement("DataType",
                new XAttribute("Name", "udt_Valve"),
                new XAttribute("Use", "Target"),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "<![CDATA[Standard valve interface]]>"),

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
                        new XElement("Description", "<![CDATA[IN - Open valve command]]>")
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
                        new XElement("Description", "<![CDATA[IN - Close valve command]]>")
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
                        new XElement("Description", "<![CDATA[OUT - Valve ready]]>")
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
                        new XElement("Description", "<![CDATA[OUT - Opened feedback]]>")
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
                        new XElement("Description", "<![CDATA[OUT - Closed feedback]]>")
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
                        new XElement("Description", "<![CDATA[OUT - Fault reset to valve]]>")
                           )
                       )
                );


            return stdValveDataType;
        }

        //generate udt_Valves
        private XElement GetValvesDataType(List<Component> components, string prefix)
        {
            //filter pump components
            var valvesComponents = components.Where(c => c.StandardComponent == Enums.StandardComponent.P_Std_Valve).ToList();

            if (valvesComponents.Count == 0)
            {
                return new XElement("Empty");
            }

            //Generate members
            var valvesMembers = new List<XElement>();
            foreach (Component component in valvesComponents)
            {

                var xElement = new XElement("Member",
                    new XAttribute("Name", component.Name),
                    new XAttribute("DataType", "udt_Valve"),
                    new XAttribute("Dimension", "0"),
                    new XAttribute("Radix", "Decimal"),
                    new XAttribute("Hidden", "false"),
                    new XAttribute("ExternalAccess", "Read/Write"),

                    new XElement("Description", "<![CDATA[" + component.Comment + "]]>")
                    );
                valvesMembers.Add(xElement);
            }

            var valvesDataType = new XElement("DataType",
                new XAttribute("Name", "udt_Valves" + prefix),
                new XAttribute("Family", "NoFamily"),
                new XAttribute("Class", "User"),

                new XElement("Description", "<![CDATA[" + prefix + "Valve interfaces]]>"),

                new XElement("Members",
                    valvesMembers
                    )
                );

            return valvesDataType;
        }
    }
}