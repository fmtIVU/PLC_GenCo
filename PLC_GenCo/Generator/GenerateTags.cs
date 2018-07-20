using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace PLC_GenCo.Generator
{
    public class GenerateTags
    {
        private TagsInfo _tagsInfo;
        private XElement _tags;

        public GenerateTags(TagsInfo tagsInfo)
        {
            _tagsInfo = tagsInfo;


            _tags = new XElement("Tags",
                new XElement("Tag",
                new XAttribute("Name", "IO"),
                new XAttribute("TagType", "Base"),
                new XAttribute("DataType", "udt_IO"),
                new XAttribute("Constant", "false"),
                new XAttribute("ExternalAccess", "Read/Write")
                    ),
                new XElement("Tag",
                new XAttribute("Name", "Disp"),
                new XAttribute("TagType", "Base"),
                new XAttribute("DataType", "udt_Disp"),
                new XAttribute("Constant", "false"),
                new XAttribute("ExternalAccess", "Read/Write")
                    ),
                new XElement("Tag",
                new XAttribute("Name", "ModuleStatus"),
                new XAttribute("TagType", "Base"),
                new XAttribute("DataType", "udt_Modules"),
                new XAttribute("Constant", "false"),
                new XAttribute("ExternalAccess", "Read/Write")
                    ),
                new XElement("Tag",
                new XAttribute("Name", "Counter"),
                new XAttribute("TagType", "Base"),
                new XAttribute("DataType", "udt_CNT"),
                new XAttribute("Constant", "false"),
                new XAttribute("ExternalAccess", "Read/Write")
                    ),
                new XElement("Tag",
                new XAttribute("Name", "Watchdog"),
                new XAttribute("TagType", "Base"),
                new XAttribute("DataType", "udt_WD"),
                new XAttribute("Constant", "false"),
                new XAttribute("ExternalAccess", "Read/Write"),

                new XElement("Data", "00 00 00 00 10 27 00 00 01 00 00 00 00 00 00 00 10 27 00 00 00 00 00 00 A3 07 00 00 00 00 00 00"),
                new XElement("Data",
                    new XAttribute("Format", "Decorated"),

                    new XElement("Structure",
                        new XAttribute("DataType", "udt_WD"),
                        InitInStructTimer("PLC_WD_T", 10),
                        InitInStructTimer("SCADA_WD_T", 10),
                        InitDataValueMember("TestAlarmTime", "DINT", 1955),
                        InitDataValueMember("Test_Alarm", "BOOL", 0),
                        InitDataValueMember("PLC_WD_Alarm", "BOOL", 0),
                        InitDataValueMember("SCADA_Handshake", "BOOL", 0)
                                )
                            )
                        ),
                new XElement("Tag",
                new XAttribute("Name", "Frq"),
                new XAttribute("TagType", "Base"),
                new XAttribute("DataType", "udt_AOIFrq"),
                new XAttribute("Constant", "false"),
                new XAttribute("ExternalAccess", "Read/Write")
                ),
                new XElement("Tag",
                new XAttribute("Name", "Motor"),
                new XAttribute("TagType", "Base"),
                new XAttribute("DataType", "udt_AOIPumps"),
                new XAttribute("Constant", "false"),
                new XAttribute("ExternalAccess", "Read/Write")
                ),
                new XElement("Tag",
                new XAttribute("Name", "Valve"),
                new XAttribute("TagType", "Base"),
                new XAttribute("DataType", "udt_AOIValves"),
                new XAttribute("Constant", "false"),
                new XAttribute("ExternalAccess", "Read/Write")
                ),
                new XElement("Tag",
                new XAttribute("Name", "_IMotor"),
                new XAttribute("TagType", "Base"),
                new XAttribute("DataType", "udt_Pumps"),
                new XAttribute("Constant", "false"),
                new XAttribute("ExternalAccess", "Read/Write")
                ),
                new XElement("Tag",
                new XAttribute("Name", "_IValve"),
                new XAttribute("TagType", "Base"),
                new XAttribute("DataType", "udt_Valves"),
                new XAttribute("Constant", "false"),
                new XAttribute("ExternalAccess", "Read/Write")
                ),
                new XElement("Tag",
                new XAttribute("Name", "TODO"),
                new XAttribute("TagType", "Base"),
                new XAttribute("DataType", "udt_TODO"),
                new XAttribute("Constant", "false"),
                new XAttribute("ExternalAccess", "Read/Write")
                ),
                new XElement("Tag",
                new XAttribute("Name", "AlarmDI"),
                new XAttribute("TagType", "Base"),
                new XAttribute("DataType", "udt_DIAlarms"),
                new XAttribute("Constant", "false"),
                new XAttribute("ExternalAccess", "Read/Write")
                ),
                new XElement("Tag",
                new XAttribute("Name", "AlarmAI"),
                new XAttribute("TagType", "Base"),
                new XAttribute("DataType", "udt_AIAlarms"),
                new XAttribute("Constant", "false"),
                new XAttribute("ExternalAccess", "Read/Write")
                )

            );
        }
        //=======================================================================================================
        public XElement GetTags()
        {

            return _tags;
        }
        //=======================================================================================================
        public bool AddTags(XElement Tags)
        {
            //Input validation
            if (Tags == null)
            {
                return false;
            }

            switch (Tags.Name.ToString())
            {
                //Handle adding single tag
                case ("Tag"):
                    //Add if it doesnt exist
                    if (!_tags.Descendants("Tag").Any(c=>c.Attribute("Name").Value == Tags.Attribute("Name").Value))
                        _tags.Add(Tags);
                    break;
                //Handle adding multiple tags
                case ("Tags"):
                    foreach (var tag in Tags.Descendants("Tag"))
                    {
                        //Add if it doesnt exist
                        if (!_tags.Descendants("Tag").Any(c => c.Attribute("Name").Value == tag.Attribute("Name").Value))
                            _tags.Add(tag);
                    }
                    break;
                default:
                    return false;     
            }

            return true;

        }

        private XElement InitInStructTimer(string name, int presetSec)
        {
            return new XElement("StructureMember",
                            new XAttribute("Name", name),
                            new XAttribute("DataType", "TIMER"),
                                InitDataValueMember("PRE", "DINT", (presetSec * 1000)),
                                InitDataValueMember("ACC", "DINT", 0),
                                InitDataValueMember("EN", "BOOL", 0),
                                InitDataValueMember("TT", "BOOL", 0),
                                InitDataValueMember("DN","BOOL", 0)
        
                                );
        } 

        private XElement InitDataValueMember(string name, string datatype, int value)
        {
            if (datatype == "BOOL")
            {
                if (value != 0 || value != 1)
                    value = 0;

                return new XElement("DataValueMember",
                                    new XAttribute("Name", name),
                                    new XAttribute("DataType", datatype),
                                    new XAttribute("Value", value.ToString())
                                    );
            }else
            {
                return new XElement("DataValueMember",
                                    new XAttribute("Name", name),
                                    new XAttribute("DataType", datatype),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", value.ToString())
                                    );
            }
            
        }
    }
}