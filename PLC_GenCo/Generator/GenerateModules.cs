using PLC_GenCo.Models;
using System;
using System.Xml.Linq;

namespace PLC_GenCo.Generator
{
    internal class GenerateModules
    {
        private ModulesInfo _modulesInfo;
        private Data _data;

        public GenerateModules(ModulesInfo modulesInfo)
        {
            _modulesInfo = modulesInfo;
            _data = new Data();
        }

        public XElement GetModules()
        {
            var modules = new XElement("Modules");

            modules.Add(GetController());   // Add Controller
            modules.Add(GetEmbDiscIO());    // Add Embedded Discrete IO

            var portAddress = 1;            //init module address

            foreach (Module module in _modulesInfo.modules)
            {
                //skip embedded modules - they are added
                if (module.IOModulesType != ViewModels.Enums.IOModulesType.EmbDIx16 && module.IOModulesType != ViewModels.Enums.IOModulesType.EmbDOx16)
                {
                    portAddress++;

                    if (portAddress > 6)
                    {
                        throw new Exception("6 modules limit exceeded");
                    }

                    modules.Add(GetModule(module, portAddress));
                }
                
            }

           
            
                
                


            return modules;
        }

        private XElement GetController()
        {
            var controller = 
            new XElement("Module",
                new XAttribute("Name", "Local"),
                new XAttribute("CatalogNumber", _modulesInfo.controller.catalogNumber),
                new XAttribute("Vendor", "1"),
                new XAttribute("ProductType", "14"),
                new XAttribute("ProductCode", _modulesInfo.controller.productCode),
                new XAttribute("Major", _modulesInfo.controller.majorRev),
                new XAttribute("Minor", _modulesInfo.controller.minorRev),
                new XAttribute("ParentModule", "Local"),
                new XAttribute("ParentModPortId", "1"),
                new XAttribute("Inhibited", "false"),
                new XAttribute("MajorFault", "True"),
                
                new XElement("EKey",
                    new XAttribute("State", "Disabled")
                    ),
                new XElement("Ports",
                    new XElement("Port",
                        new XAttribute("Id", "1"),
                        new XAttribute("Address", "0"),
                        new XAttribute("Type", "PointIO"),
                        new XAttribute("Upstream", "false"),
                        
                        new XElement("Bus",
                            new XAttribute("Size", "8")
                        )
                    ),
                    new XElement("Port",
                        new XAttribute("Id", "2"),
                        new XAttribute("Type", "Ethernet"),
                        new XAttribute("Upstream", "false"),
                        
                        new XElement("Bus")
                    )
                )
           );

            return controller;
        }

        private XElement GetEmbDiscIO()
        {

            //Generate embedded DI and DO tags comments
            var embDiscIODIComments = new XElement("Comments");
            var embDiscIODOComments = new XElement("Comments");

            var commentsDI = new string[16];                //init array for comments - 16 DI/DO
            var commentsDO = new string[16];

            commentsDI = _modulesInfo.modules.Find(m => m.IOModulesType == ViewModels.Enums.IOModulesType.EmbDIx16).Comments;
            commentsDO = _modulesInfo.modules.Find(m => m.IOModulesType == ViewModels.Enums.IOModulesType.EmbDOx16).Comments;



            for (int i = 0; i < 16; i++)
            {
                embDiscIODIComments.Add(
                    new XElement("Comment",
                        new XAttribute("Operand", ".DATA."+ i.ToString()),
                        "<![CDATA[" + commentsDI[i] + "]]>"
                        )
                );

                embDiscIODOComments.Add(
                    new XElement("Comment",
                        new XAttribute("Operand", ".DATA." + i.ToString()),
                        "<![CDATA[" + commentsDO[i] + "]]>"
                        )
                );

            }

            // Generate embedded IO
            var embDiscIO =
            new XElement("Module",
                new XAttribute("Name", "Discrete_IO"),
                new XAttribute("CatalogNumber", "Embedded"),
                new XAttribute("Vendor", "1"),
                new XAttribute("ProductType", "7"),
                new XAttribute("ProductCode", "1140"),
                new XAttribute("Major", _modulesInfo.controller.majorRev),
                new XAttribute("Minor", _modulesInfo.controller.minorRev),
                new XAttribute("ParentModule", "Local"),
                new XAttribute("ParentModPortId", "1"),
                new XAttribute("Inhibited", "false"),
                new XAttribute("MajorFault", "True"),

                new XElement("EKey",
                    new XAttribute("State", "CompatibleModule")
                    ),
                new XElement("Ports",
                    new XElement("Port",
                        new XAttribute("Id", "1"),
                        new XAttribute("Address", "1"),
                        new XAttribute("Type", "PointIO"),
                        new XAttribute("Upstream", "true")
                    )
                ),
                new XElement("Communications",
                    new XElement("ConfigTag",
                        new XAttribute("ConfigSize", "80"),
                        new XAttribute("ExternalAccess", "Read/Write"),

                            new XElement("Data", _data.ConfigTagData),
                            new XElement("Data",
                                new XAttribute("Format", "Decorated"),
                                new XElement("Structure",
                                    new XAttribute("DataType", "AB:Embedded_DiscreteIO:C:0"),
                                    //----------------------------------------------------------------------------
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt00FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt00FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt01FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt01FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt02FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt02FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt03FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt03FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt04FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt04FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt05FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt05FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt06FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt06FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt07FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt07FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt08FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt08FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt09FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt09FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt10FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt10FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt11FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt11FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt12FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt12FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt13FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt13FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt14FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt14FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt15FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt15FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    //------------------------------------------------------------------------------------
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "FaultMode"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Binary"),
                                        new XAttribute("Value", "2#0000_0000_0000_0000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "FaultValue"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Binary"),
                                        new XAttribute("Value", "2#0000_0000_0000_0000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "ProgMode"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Binary"),
                                        new XAttribute("Value", "2#0000_0000_0000_0000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "ProgValue"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Binary"),
                                        new XAttribute("Value", "2#0000_0000_0000_0000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "COSOnOffEn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Binary"),
                                        new XAttribute("Value", "2#0000_0000_0000_0000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "COSOFFOnEn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Binary"),
                                        new XAttribute("Value", "2#0000_0000_0000_0000")
                                    )

                                )
                            )
                        ),
                    new XElement("Connections",
                        new XElement("Connection",
                            new XAttribute("Name", "Data"),
                            new XAttribute("RPI", "20000"),
                            new XAttribute("Type", "Output"),
                            new XAttribute("EventID", "0"),
                            new XAttribute("ProgrammaticallySendEventTrigger", "false"), 

                            new XElement("InputTag",
                                new XAttribute("ExternalAccess", "Read/Write"),
                                embDiscIODIComments,
                                new XElement("ForceData", _data.ForceDataDI),
                                new XElement("Data",
                                    new XAttribute("Format", "Decorated"),
                                    new XElement("Structure",
                                        new XAttribute("DataType", "AB:Embedded_DiscreteIO:I:0"),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Fault"),
                                            new XAttribute("DataType", "DINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000_0000_0000_0000_0000_0000_0000")
                                        ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Data"),
                                            new XAttribute("DataType", "INT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000_0000_0000")
                                        )
                                    )
                               )
                            ),
                            new XElement("OutputTag",
                            new XAttribute("ExternalAccess", "Read/Write"),
                                embDiscIODOComments,
                                new XElement("ForceData", _data.ForceDataDO),
                                new XElement("Data",
                                    new XAttribute("Format", "Decorated"),
                                    new XElement("Structure",
                                        new XAttribute("DataType", "AB:Embedded_DiscreteIO:O:0"),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Fault"),
                                            new XAttribute("DataType", "DINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000_0000_0000_0000_0000_0000_0000")
                                        ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Data"),
                                            new XAttribute("DataType", "INT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000_0000_0000")
                                        )
                                    )
                               )
                            )
                        )
                    )
                ),
                    new XElement("ExtendedProperties",
                        new XElement("public",
                            new XElement("ConfigID", "100"),
                            new XElement("CatNum", "Embedded")
                        )
                    )
              );

                return embDiscIO;
        }

        private XElement GetModule(Module IOmodule, int portAddress)
        {
            var module = new XElement("Module");

            switch (IOmodule.IOModulesType)
            {
                case ViewModels.Enums.IOModulesType.EmbDIx16:
                    throw new Exception("Generate modules: Embedded modules not filtered");
                    
                case ViewModels.Enums.IOModulesType.EmbDOx16:
                    throw new Exception("Generate modules: Embedded modules not filtered");
                   
                case ViewModels.Enums.IOModulesType.DIx4:
                    module = GetDIx4(portAddress);
                    break;
                case ViewModels.Enums.IOModulesType.DIx8:
                    module = GetDIx8(portAddress);
                    break;
                case ViewModels.Enums.IOModulesType.DOx4:
                    module = GetDOx4(portAddress);
                    break;
                case ViewModels.Enums.IOModulesType.DOx8:
                    module = GetDOx8(portAddress);
                    break;
                case ViewModels.Enums.IOModulesType.AIx4:
                    module = GetAIx4(portAddress);
                    break;
                case ViewModels.Enums.IOModulesType.AIx8:
                    module = GetAIx8(portAddress);
                    break;
                case ViewModels.Enums.IOModulesType.AOx4:
                    module = GetAOx4(portAddress);
                    break;
                default:
                    throw new Exception("Generate modules: Unknown module - update switch-case");
            }

            return module;
        }

        private XElement GetPSModbus()
        {
            throw new NotImplementedException();
        }

        private XElement GetAOx4(int portAddress)
        {
            var getAox4Module =
                new XElement("Module",
                    new XAttribute("Name", "AOx4"),
                    new XAttribute("CatalogNumber", "1734-OE4C/C"),
                    new XAttribute("Vendor", "1"),
                    new XAttribute("ProductType", "115"),
                    new XAttribute("ProductCode", "211"),
                    new XAttribute("Major", "3"),
                    new XAttribute("Minor", "1"),
                    new XAttribute("ParentModule", "Local"),
                    new XAttribute("ParentModPortId", "1"),
                    new XAttribute("Inhibited", "false"),
                    new XAttribute("MajorFault", "false"),

                    new XElement("EKey",
                        new XAttribute("State", "CompatibleModule")
                        ),
                    new XElement("Ports",
                        new XElement("Port",
                            new XAttribute("Id", "1"),
                            new XAttribute("Address", portAddress.ToString()),
                            new XAttribute("Type", "PointIO"),
                            new XAttribute("Upstream", "true")
                        )
                    ),
                    new XElement("Communications",
                        new XAttribute("CommMethod", "536870913"),
                        new XElement("ConfigTag",
                            new XAttribute("ConfigSize", "76"),
                            new XAttribute("ExternalAccess", "Read/Write"),

                            new XElement("Data", _data.DataAOx4),
                                new XElement("Data",
                                new XAttribute("Format", "Decorated"),

                                new XElement("Structure",
                                    new XAttribute("DataType", "AB:1734_OE4:C:0"),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0FaultValue"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0ProgValue"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0LowEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0HighEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0LowLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "-32768")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0HighLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "32768")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0RangeType"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0FaultMode"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0ProgMode"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0LimitAlarmLatch"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0AlarmDisable"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1FaultValue"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1ProgValue"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1LowEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1HighEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1LowLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "-32768")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1HighLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "32768")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1RangeType"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1FaultMode"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1ProgMode"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1LimitAlarmLatch"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1AlarmDisable"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2FaultValue"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2ProgValue"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2LowEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2HighEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2LowLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "-32768")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2HighLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "32768")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2RangeType"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2FaultMode"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2ProgMode"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2LimitAlarmLatch"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2AlarmDisable"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3FaultValue"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3ProgValue"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3LowEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3HighEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3LowLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "-32768")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3HighLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "32768")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3RangeType"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3FaultMode"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3ProgMode"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3LimitAlarmLatch"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3AlarmDisable"),
                                        new XAttribute("DataType", "SINT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    )
                                )
                            )
                        ),
                        new XElement("Connections",
                        new XElement("Connection",
                            new XAttribute("Name", "OutputData"),
                            new XAttribute("RPI", "80000"),
                            new XAttribute("Type", "Output"),
                            new XAttribute("EventID", "0"),
                            new XAttribute("ProgrammaticallySendEventTrigger", "false"),

                            new XElement("InputTag",
                                new XAttribute("ExternalAccess", "Read/Write"),
                                new XElement("ForceData", _data.ForceDataAOx4),
                                new XElement("Data",
                                    new XAttribute("Format", "Decorated"),
                                    new XElement("Structure",
                                        new XAttribute("DataType", "AB:1734_OE4:I:0"),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Fault"),
                                            new XAttribute("DataType", "DINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000_0000_0000_0000_0000_0000_0000")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0Status"),
                                            new XAttribute("DataType", "SINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0Fault"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0Calibration"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0LAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0HAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1Status"),
                                            new XAttribute("DataType", "SINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1Fault"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1Calibration"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1LAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1HAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2Status"),
                                            new XAttribute("DataType", "SINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2Fault"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2Calibration"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2LAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2HAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3Status"),
                                            new XAttribute("DataType", "SINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3Fault"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3Calibration"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3LAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3HAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            )
                                        )
                                    )
                                ),
                            new XElement("OutputTag",
                                new XAttribute("ExternalAccess", "Read/Write"),
                                new XElement("ForceData", _data.ForceDataAOx4),
                                new XElement("Data",
                                    new XAttribute("Format", "Decorated"),
                                    new XElement("Structure",
                                        new XAttribute("DataType", "AB:1734_OE4:O:0"),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0Data"),
                                            new XAttribute("DataType", "INT"),
                                            new XAttribute("Radix", "Decimal"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1Data"),
                                            new XAttribute("DataType", "INT"),
                                            new XAttribute("Radix", "Decimal"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2Data"),
                                            new XAttribute("DataType", "INT"),
                                            new XAttribute("Radix", "Decimal"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3Data"),
                                            new XAttribute("DataType", "INT"),
                                            new XAttribute("Radix", "Decimal"),
                                            new XAttribute("Value", "0")
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    ),
                    new XElement("ExtendedProperties",
                        new XElement("public",
                            new XElement("ConfigID", "400"),
                            new XElement("CatNum", "1734-OE4C")
                        )
                    )
                );





            return getAox4Module;
        }

        private XElement GetAIx8(int portAddress)
        {
            var getAix8Module =
            new XElement("Module",
                new XAttribute("Name", "AIx8"),
                new XAttribute("CatalogNumber", "1734-IE8C/C"),
                new XAttribute("Vendor", "1"),
                new XAttribute("ProductType", "115"),
                new XAttribute("ProductCode", "210"),
                new XAttribute("Major", "3"),
                new XAttribute("Minor", "1"),
                new XAttribute("ParentModule", "Local"),
                new XAttribute("ParentModPortId", "1"),
                new XAttribute("Inhibited", "false"),
                new XAttribute("MajorFault", "false"),

                new XElement("EKey",
                    new XAttribute("State", "CompatibleModule")
                    ),
                new XElement("Ports",
                    new XElement("Port",
                        new XAttribute("Id", "1"),
                        new XAttribute("Address", portAddress.ToString()),
                        new XAttribute("Type", "PointIO"),
                        new XAttribute("Upstream", "true")
                    )
                ),
                new XElement("Communications",
                    new XAttribute("CommMethod", "536870914"),
                    new XElement("ConfigTag",
                        new XAttribute("ConfigSize", "150"),
                        new XAttribute("ExternalAccess", "Read/Write"),

                        new XElement("Data", _data.DataAIx8),
                            new XElement("Data",
                            new XAttribute("Format", "Decorated"),

                            new XElement("Structure",
                                new XAttribute("DataType", "AB:1734_IE8:C:0"),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch0LowEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch0HighEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch0DigitalFilter"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch0LAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch0HAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch0LLAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch0HHAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch0RangeType"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "3")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch0LimitAlarmLatch"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch0AlarmDisable"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch1LowEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch1HighEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch1DigitalFilter"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch1LAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch1HAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch1LLAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch1HHAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch1RangeType"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "3")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch1LimitAlarmLatch"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch1AlarmDisable"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch2LowEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch2HighEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch2DigitalFilter"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch2LAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch2HAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch2LLAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch2HHAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch2RangeType"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "3")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch2LimitAlarmLatch"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch2AlarmDisable"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch3LowEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch3HighEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch3DigitalFilter"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch3LAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch3HAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch3LLAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch3HHAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch3RangeType"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "3")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch3LimitAlarmLatch"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch3AlarmDisable"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch4LowEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch4HighEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch4DigitalFilter"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch4LAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch4HAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch4LLAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch4HHAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch4RangeType"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "3")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch4LimitAlarmLatch"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch4AlarmDisable"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch5LowEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch5HighEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch5DigitalFilter"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch5LAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch5HAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch5LLAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch5HHAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch5RangeType"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "3")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch5LimitAlarmLatch"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch5AlarmDisable"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch6LowEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch6HighEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch6DigitalFilter"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch6LAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch6HAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch6LLAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch6HHAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch6RangeType"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "3")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch6LimitAlarmLatch"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch6AlarmDisable"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch7LowEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch7HighEngineering"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch7DigitalFilter"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch7LAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch7HAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch7LLAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch7HHAlarmLimit"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "10000")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch7RangeType"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "3")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch7LimitAlarmLatch"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                ),
                                new XElement("DataValueMember",
                                    new XAttribute("Name", "Ch7AlarmDisable"),
                                    new XAttribute("DataType", "INT"),
                                    new XAttribute("Radix", "Decimal"),
                                    new XAttribute("Value", "0")
                                )
                    )
                )
            ),
        new XElement("Connections",
            new XElement("Connection",
                new XAttribute("Name", "InputData"),
                new XAttribute("RPI", "80000"),
                new XAttribute("Type", "Input"),
                new XAttribute("EventID", "0"),
                new XAttribute("ProgrammaticallySendEventTrigger", "false"),

                new XElement("InputTag",
                    new XAttribute("ExternalAccess", "Read/Write"),
                    new XElement("ForceData", _data.ForceDataAIx8),
                    new XElement("Data",
                        new XAttribute("Format", "Decorated"),
                        new XElement("Structure",
                            new XAttribute("DataType", "AB:1734_IE8:I:0"),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Fault"),
                                new XAttribute("DataType", "DINT"),
                                new XAttribute("Radix", "Binary"),
                                new XAttribute("Value", "2#0000_0000_0000_0000_0000_0000_0000_0000")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch0Data"),
                                new XAttribute("DataType", "INT"),
                                new XAttribute("Radix", "Decimal"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch1Data"),
                                new XAttribute("DataType", "INT"),
                                new XAttribute("Radix", "Decimal"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch2Data"),
                                new XAttribute("DataType", "INT"),
                                new XAttribute("Radix", "Decimal"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch3Data"),
                                new XAttribute("DataType", "INT"),
                                new XAttribute("Radix", "Decimal"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch4Data"),
                                new XAttribute("DataType", "INT"),
                                new XAttribute("Radix", "Decimal"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch5Data"),
                                new XAttribute("DataType", "INT"),
                                new XAttribute("Radix", "Decimal"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch6Data"),
                                new XAttribute("DataType", "INT"),
                                new XAttribute("Radix", "Decimal"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch7Data"),
                                new XAttribute("DataType", "INT"),
                                new XAttribute("Radix", "Decimal"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch0Status"),
                                new XAttribute("DataType", "SINT"),
                                new XAttribute("Radix", "Binary"),
                                new XAttribute("Value", "2#0000_0000")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch0Fault"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch0Calibration"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch0LAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch0HAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch0LLAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch0HHAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch0Underrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch0Overrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch1Status"),
                                new XAttribute("DataType", "SINT"),
                                new XAttribute("Radix", "Binary"),
                                new XAttribute("Value", "2#0000_0000")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch1Fault"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch1Calibration"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch1LAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch1HAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch1LLAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch1HHAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch1Underrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch1Overrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch2Status"),
                                new XAttribute("DataType", "SINT"),
                                new XAttribute("Radix", "Binary"),
                                new XAttribute("Value", "2#0000_0000")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch2Fault"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch2Calibration"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch2LAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch2HAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch2LLAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch2HHAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch2Underrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch2Overrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch3Status"),
                                new XAttribute("DataType", "SINT"),
                                new XAttribute("Radix", "Binary"),
                                new XAttribute("Value", "2#0000_0000")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch3Fault"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch3Calibration"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch3LAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch3HAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch3LLAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch3HHAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch3Underrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch3Overrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch4Status"),
                                new XAttribute("DataType", "SINT"),
                                new XAttribute("Radix", "Binary"),
                                new XAttribute("Value", "2#0000_0000")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch4Fault"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch4Calibration"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch4LAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch4HAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch4LLAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch4HHAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch4Underrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch4Overrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch5Status"),
                                new XAttribute("DataType", "SINT"),
                                new XAttribute("Radix", "Binary"),
                                new XAttribute("Value", "2#0000_0000")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch5Fault"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch5Calibration"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch5LAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch5HAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch5LLAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch5HHAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch5Underrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch5Overrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch6Status"),
                                new XAttribute("DataType", "SINT"),
                                new XAttribute("Radix", "Binary"),
                                new XAttribute("Value", "2#0000_0000")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch6Fault"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch6Calibration"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch6LAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch6HAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch6LLAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch6HHAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch6Underrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch6Overrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch7Status"),
                                new XAttribute("DataType", "SINT"),
                                new XAttribute("Radix", "Binary"),
                                new XAttribute("Value", "2#0000_0000")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch7Fault"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch7Calibration"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch7LAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch7HAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch7LLAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch7HHAlarm"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch7Underrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                ),
                            new XElement("DataValueMember",
                                new XAttribute("Name", "Ch7Overrange"),
                                new XAttribute("DataType", "BOOL"),
                                new XAttribute("Value", "0")
                                )
                            )
                        )
                    )
                )
            )
        ),
        new XElement("ExtendedProperties",
            new XElement("public",
                new XElement("ConfigID", "101"),
                new XElement("CatNum", "1734-IE8C")
            )
        )
    );

            return getAix8Module;
        }

        private XElement GetAIx4(int portAddress)
        {
            var getAix4Module =
                new XElement("Module",
                    new XAttribute("Name", "AIx4"),
                    new XAttribute("CatalogNumber", "1734-IE4C/C"),
                    new XAttribute("Vendor", "1"),
                    new XAttribute("ProductType", "115"),
                    new XAttribute("ProductCode", "209"),
                    new XAttribute("Major", "3"),
                    new XAttribute("Minor", "1"),
                    new XAttribute("ParentModule", "Local"),
                    new XAttribute("ParentModPortId", "1"),
                    new XAttribute("Inhibited", "false"),
                    new XAttribute("MajorFault", "false"),

                    new XElement("EKey",
                        new XAttribute("State", "CompatibleModule")
                        ),
                    new XElement("Ports",
                        new XElement("Port",
                            new XAttribute("Id", "1"),
                            new XAttribute("Address", portAddress.ToString()),
                            new XAttribute("Type", "PointIO"),
                            new XAttribute("Upstream", "true")
                        )
                    ),
                    new XElement("Communications",
                        new XAttribute("CommMethod", "536870913"),
                        new XElement("ConfigTag",
                            new XAttribute("ConfigSize", "78"),
                            new XAttribute("ExternalAccess", "Read/Write"),

                            new XElement("Data", _data.DataAIx4),
                                new XElement("Data",
                                new XAttribute("Format", "Decorated"),

                                new XElement("Structure",
                                    new XAttribute("DataType", "AB:1734_IE4:C:0"),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0LowEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0HighEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0DigitalFilter"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0LAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0HAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0LLAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0HHAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0RangeType"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "3")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0LimitAlarmLatch"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch0AlarmDisable"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1LowEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1HighEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1DigitalFilter"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1LAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1HAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1LLAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1HHAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1RangeType"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "3")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1LimitAlarmLatch"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch1AlarmDisable"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2LowEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2HighEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2DigitalFilter"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2LAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2HAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2LLAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2HHAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2RangeType"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "3")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2LimitAlarmLatch"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch2AlarmDisable"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3LowEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3HighEngineering"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3DigitalFilter"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3LAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3HAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3LLAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3HHAlarmLimit"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "10000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3RangeType"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "3")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3LimitAlarmLatch"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Ch3AlarmDisable"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "0")
                                    )
                                )
                            )
                        ),
                    new XElement("Connections",
                        new XElement("Connection",
                            new XAttribute("Name", "InputData"),
                            new XAttribute("RPI", "80000"),
                            new XAttribute("Type", "Input"),
                            new XAttribute("EventID", "0"),
                            new XAttribute("ProgrammaticallySendEventTrigger", "false"),

                            new XElement("InputTag",
                                new XAttribute("ExternalAccess", "Read/Write"),
                                new XElement("ForceData", _data.ForceDataAIx4),
                                new XElement("Data",
                                    new XAttribute("Format", "Decorated"),
                                    new XElement("Structure",
                                        new XAttribute("DataType", "AB:1734_IE4:I:0"),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Fault"),
                                            new XAttribute("DataType", "DINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000_0000_0000_0000_0000_0000_0000")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0Data"),
                                            new XAttribute("DataType", "INT"),
                                            new XAttribute("Radix", "Decimal"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1Data"),
                                            new XAttribute("DataType", "INT"),
                                            new XAttribute("Radix", "Decimal"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2Data"),
                                            new XAttribute("DataType", "INT"),
                                            new XAttribute("Radix", "Decimal"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3Data"),
                                            new XAttribute("DataType", "INT"),
                                            new XAttribute("Radix", "Decimal"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0Status"),
                                            new XAttribute("DataType", "SINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0Fault"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0Calibration"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0LAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0HAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0LLAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0HHAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0Underrange"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch0Overrange"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1Status"),
                                            new XAttribute("DataType", "SINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1Fault"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1Calibration"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1LAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1HAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1LLAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1HHAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1Underrange"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch1Overrange"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2Status"),
                                            new XAttribute("DataType", "SINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2Fault"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2Calibration"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2LAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2HAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2LLAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2HHAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2Underrange"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch2Overrange"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3Status"),
                                            new XAttribute("DataType", "SINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3Fault"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3Calibration"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3LAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3HAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3LLAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3HHAlarm"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3Underrange"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Ch3Overrange"),
                                            new XAttribute("DataType", "BOOL"),
                                            new XAttribute("Value", "0")
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    ),
                    new XElement("ExtendedProperties",
                        new XElement("public",
                            new XElement("ConfigID", "100"),
                            new XElement("CatNum", "1734-IE4C")
                        )
                    )
                );

            return getAix4Module;
        }

        private XElement GetDOx8(int portAddress)
        {
            throw new NotImplementedException();
        }

        private XElement GetDOx4(int portAddress)
        {
            throw new NotImplementedException();
        }

        private XElement GetDIx4(int portAddress)
        {
            var getDix4Module =
                new XElement("Module",
                    new XAttribute("Name", "DIx4"),
                    new XAttribute("CatalogNumber", "1734-IB4/C"),
                    new XAttribute("Vendor", "1"),
                    new XAttribute("ProductType", "7"),
                    new XAttribute("ProductCode", "130"),
                    new XAttribute("Major", "3"),
                    new XAttribute("Minor", "1"),
                    new XAttribute("ParentModule", "Local"),
                    new XAttribute("ParentModPortId", "1"),
                    new XAttribute("Inhibited", "false"),
                    new XAttribute("MajorFault", "false"),

                    new XElement("EKey",
                        new XAttribute("State", "CompatibleModule")
                        ),
                    new XElement("Ports",
                        new XElement("Port",
                            new XAttribute("Id", "1"),
                            new XAttribute("Address", portAddress.ToString()), 
                            new XAttribute("Type", "PointIO"),
                            new XAttribute("Upstream", "true")
                        )
                    ),
                    new XElement("Communications",
                        new XAttribute("CommMethod", "536870913"),
                        new XElement("ConfigTag",
                            new XAttribute("ConfigSize", "20"),
                            new XAttribute("ExternalAccess", "Read/Write"),

                            new XElement("Data", _data.DataDIx4),
                                new XElement("Data",
                                new XAttribute("Format", "Decorated"),

                                new XElement("Structure",
                                    new XAttribute("DataType", "AB:1734_DI4:C:0"),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt00FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt00FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt01FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt01FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt02FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt02FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt03FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt03FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    )
                                )
                            )
                        ),
                    new XElement("Connections",
                        new XElement("Connection",
                            new XAttribute("Name", "InputData"),
                            new XAttribute("RPI", "20000"),
                            new XAttribute("Type", "Input"),
                            new XAttribute("EventID", "0"),
                            new XAttribute("ProgrammaticallySendEventTrigger", "false"),

                            new XElement("InputTag",
                                new XAttribute("ExternalAccess", "Read/Write"),
                                new XElement("ForceData", _data.ForceDataDIx4),
                                new XElement("Data",
                                    new XAttribute("Format", "Decorated"),
                                    new XElement("Structure",
                                        new XAttribute("DataType", "AB:1734_DI8:I:0"),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Fault"),
                                            new XAttribute("DataType", "DINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000_0000_0000_0000_0000_0000_0000")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Data"),
                                            new XAttribute("DataType", "SINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000")
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    ),
                    new XElement("ExtendedProperties",
                        new XElement("public",
                            new XElement("ConfigID", "200"),
                            new XElement("CatNum", "1734-IB4")
                        )
                    )
                );

            return getDix4Module;
        }

        private XElement GetDIx8(int portAddress)
        {
            var getDix8Module =
                new XElement("Module",
                    new XAttribute("Name", "DIx8"),
                    new XAttribute("CatalogNumber", "1734-IB8/C"),
                    new XAttribute("Vendor", "1"),
                    new XAttribute("ProductType", "7"),
                    new XAttribute("ProductCode", "216"),
                    new XAttribute("Major", "3"),
                    new XAttribute("Minor", "1"),
                    new XAttribute("ParentModule", "Local"),
                    new XAttribute("ParentModPortId", "1"),
                    new XAttribute("Inhibited", "false"),
                    new XAttribute("MajorFault", "false"),

                    new XElement("EKey",
                        new XAttribute("State", "CompatibleModule")
                        ),
                    new XElement("Ports",
                        new XElement("Port",
                            new XAttribute("Id", "1"),
                            new XAttribute("Address", portAddress.ToString()),
                            new XAttribute("Type", "PointIO"),
                            new XAttribute("Upstream", "true")
                        )
                    ),
                    new XElement("Communications",
                        new XAttribute("CommMethod", "536870913"),
                        new XElement("ConfigTag",
                            new XAttribute("ConfigSize", "36"),
                            new XAttribute("ExternalAccess", "Read/Write"),

                            new XElement("Data", _data.DataDIx8),
                                new XElement("Data",
                                new XAttribute("Format", "Decorated"),

                                new XElement("Structure",
                                    new XAttribute("DataType", "AB:1734_DI8:C:0"),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt00FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt00FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt01FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt01FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt02FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt02FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt03FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt03FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt04FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt04FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt05FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt05FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt06FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt06FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt07FilterOffOn"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    ),
                                    new XElement("DataValueMember",
                                        new XAttribute("Name", "Pt07FilterOnOff"),
                                        new XAttribute("DataType", "INT"),
                                        new XAttribute("Radix", "Decimal"),
                                        new XAttribute("Value", "1000")
                                    )
                                )
                            )
                        ),
                    new XElement("Connections",
                        new XElement("Connection",
                            new XAttribute("Name", "InputData"),
                            new XAttribute("RPI", "20000"),
                            new XAttribute("Type", "Input"),
                            new XAttribute("EventID", "0"),
                            new XAttribute("ProgrammaticallySendEventTrigger", "false"),

                            new XElement("InputTag",
                                new XAttribute("ExternalAccess", "Read/Write"),
                                new XElement("ForceData", _data.ForceDataDIx8),
                                new XElement("Data",
                                    new XAttribute("Format", "Decorated"),
                                    new XElement("Structure",
                                        new XAttribute("DataType", "AB:1734_DI8:I:0"),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Fault"),
                                            new XAttribute("DataType", "DINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000_0000_0000_0000_0000_0000_0000")
                                            ),
                                        new XElement("DataValueMember",
                                            new XAttribute("Name", "Data"),
                                            new XAttribute("DataType", "SINT"),
                                            new XAttribute("Radix", "Binary"),
                                            new XAttribute("Value", "2#0000_0000")
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    ),
                    new XElement("ExtendedProperties",
                        new XElement("public",
                            new XElement("ConfigID", "300"),
                            new XElement("CatNum", "1734-IB8")
                        )
                    )
                );

            return getDix8Module;
        }
    }
}