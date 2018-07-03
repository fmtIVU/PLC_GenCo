using PLC_GenCo.Generator.Inits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace PLC_GenCo.Generator
{
    public class ProjectGenerator
    {
        //readOnly
        public ControllerInfo ControllerInfo { get; }
        public DataTypesInfo DataTypesInfo { get; }
        public ModulesInfo ModulesInfo { get; }
        public AddOnInstructionDefinitionsInfo AddOnInstructionDefinitionsInfo { get; }
        public GlobalTagsInfo GlobalTagsInfo { get; }
        public ProgramsInfo ProgramsInfo { get; }
        public TasksInfo TasksInfo { get; }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------  
        //constructor
        public ProjectGenerator(ControllerInfo controllerInfo, DataTypesInfo dataTablesInfo,
                          ModulesInfo modulesInfo, AddOnInstructionDefinitionsInfo addOnInstructionDefinitionsInfo,
                          GlobalTagsInfo globalTagsInfo, ProgramsInfo programsInfo, TasksInfo tasksInfo)
        {

            var initControllerInfo = new InitControllerInfo(controllerInfo);
            ControllerInfo = initControllerInfo.InitializedData();

            var initDataTypesInfo = new InitDataTypesInfo(dataTablesInfo);
            DataTypesInfo = initDataTypesInfo.InitializedData();

            var initModulesInfo = new InitModulesInfo(modulesInfo);
            ModulesInfo = initModulesInfo.InitializedData();

            var initAddOnInstructionDefinitionsInfo = new InitAddOnInstructionDefinitionsInfo(addOnInstructionDefinitionsInfo);
            AddOnInstructionDefinitionsInfo = initAddOnInstructionDefinitionsInfo.InitializedData();

            var initGlobalTagsInfo = new InitGlobalTagsInfo(globalTagsInfo);
            GlobalTagsInfo = initGlobalTagsInfo.InitializedData();

            var initProgramsInfo = new InitProgramsInfo(programsInfo);
            ProgramsInfo = initProgramsInfo.InitializedData();

            var initTasksInfo = new InitTasksInfo(tasksInfo);
            TasksInfo = initTasksInfo.InitializedData();

        }

        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        public XElement GenerateProject()
        {

            var generateDataTypes = new GenerateDataTypes(DataTypesInfo);

            var generateModules = new GenerateModules(ModulesInfo);
            var generateAddOnDefinitions = new AddOnInstructionDefinitions(AddOnInstructionDefinitionsInfo);
            var generateGlobalTags = GenerateGlobalTags(GlobalTagsInfo);
            var generatePrograms = GeneratePrograms(ProgramsInfo);
            var generateTasks = GenerateTasks(TasksInfo);


            var generatedProject = new XElement("RSLogix5000Content",
                new XAttribute("SchemaRevision", "1.0"),
                new XAttribute("SoftwareRevision", "24.00"),
                new XAttribute("TargetName", ControllerInfo.name),
                new XAttribute("TargetType", "Controller"),
                new XAttribute("ContainsContext", "false"),
                new XAttribute("Owner", "Windows User, Frontmatec"),
                new XAttribute("ExportDate", "Tue Jun 19 12:28:13 2018"),                   //TODO datetime.now + format
                new XAttribute("ExportOptions", "DecoratedData ForceProtectedEncoding AllProjDocTrans"),

                new XElement("Controller",
                    new XAttribute("Use", "Target"),
                    new XAttribute("Name", ControllerInfo.name),
                    new XAttribute("ProcessorType", ControllerInfo.catalogNumber),
                    new XAttribute("MajorRev", ControllerInfo.majorRev),
                    new XAttribute("MinorRev", ControllerInfo.minorRev),
                    new XAttribute("TimeSlice", "20"),
                    new XAttribute("ShareUnusedTimeSlice", "1"),
                    new XAttribute("ProjectCreationDate", "Wed Nov 05 19:48:49 2014"),       //TODO datetime.now + format
                    new XAttribute("LastModifiedDate", "Mon Jun 18 15:18:24 2018"),         //TODO datetime.now + format
                    new XAttribute("SFCExecutionControl", "CurrentActive"),
                    new XAttribute("SFCRestartPosition", "MostRecent"),
                    new XAttribute("SFCLastScan", "DontScan"),
                    new XAttribute("CommPath", "ADD/COMM/PATH"),
                    new XAttribute("ProjectSN", "16#60bf_9370"),                            //TODO random gen SN
                    new XAttribute("MatchProjectToController", "false"),
                    new XAttribute("CanUseRPIFromProducer", "false"),
                    new XAttribute("InhibitAutomaticFirmwareUpdate", "0"),
                    new XAttribute("PassThroughConfiguration", "EnabledWithAppend"),
                    new XAttribute("DownloadProjectDocumentationAndExtendedProperties", "true"),

                    new XElement("Description", "<![CDATA[" + ControllerInfo.description + "]]>"),

                    new XElement("RedundancyInfo",
                        new XAttribute("Enabled", "false"),
                        new XAttribute("KeepTestEditsOnSwitchOver", "false"),
                        new XAttribute("IOMemoryPadPercentage", "90"),
                        new XAttribute("DataTablePadPercentage", "50")
                        ),


                    new XElement("Security",
                        new XAttribute("Code", "0"),
                        new XAttribute("ChangesToDetect", "16#ffff_ffff_ffff_ffff")
                        ),


                    new XElement("SafetyInfo"),     //add blank?

                    generateDataTypes.GetDataTypes(),
                    generateModules.GetModules(),
                    generateAddOnDefinitions.GetAddInstructionOnDefinitions(),
                    generateGlobalTags,
                    generatePrograms,
                    generateTasks,
                    new XElement("CST",
                        new XAttribute("MasterID", "0")
                    ),
                    new XElement("WallClockTime",
                        new XAttribute("LocalTimeAdjustment", "0"),
                        new XAttribute("TimeZone", "0")
                        ),
                    new XElement("Trends"),
                    new XElement("DataLogs"),
                    new XElement("TimeSynchronize",
                        new XAttribute("Priority1", "128"),
                        new XAttribute("Priority2", "128"),
                        new XAttribute("PTPEnable", "false")
                        ),
                    new XElement("InternetProtocol",
                        new XAttribute("ConfigType", "BOOTP")
                        ),
                    new XElement("EthernetPorts",
                        new XElement("EthernetPort",
                            new XAttribute("Port", "1"),
                            new XAttribute("PortEnabled", "true"),
                            new XAttribute("AutoNegotiateEnabled", "true")
                            ),
                        new XElement("EthernetPort",
                            new XAttribute("Port", "2"),
                            new XAttribute("PortEnabled", "true"),
                            new XAttribute("AutoNegotiateEnabled", "true")
                            )
                        ),
                    new XElement("EthernetNetwork",
                        new XAttribute("SupervisorModeEnabled", "false"),
                        new XAttribute("SupervisorPrecedence", "0"),
                        new XAttribute("BeaconInterval", "400"),
                        new XAttribute("BeaconTimeout", "1960"),
                        new XElement("VLANID", "0")
                        )
                    )
                );



            return generatedProject;
        }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------

        private XElement GenerateGlobalTags(GlobalTagsInfo globalTagsInfo)
        {
            return new XElement("Tags");
        }

        private XElement GeneratePrograms(ProgramsInfo programsInfo)
        {
            var programs =
                new XElement("Programs",
                    new XElement("Program",
                        new XAttribute("Name", "MainProgram"),
                        new XAttribute("TestEdits", "false"),
                        new XAttribute("MainRoutineName", "MainRoutine"),
                        new XAttribute("Disabled", "false"),
                        new XAttribute("UseAsFolder", "false"),
                        new XElement("Tags"),
                        new XElement("Routines",
                            new XElement("Routine",
                                new XAttribute("Name", "MainRoutine"),
                                new XAttribute("Type", "RLL")
                                )
                            )
                    )
                );
            return new XElement("Programs");
        }

        private XElement GenerateTasks(TasksInfo tasksInfo)
        {
            var tasks =
                new XElement("Tasks",
                    new XElement("Task",
                        new XAttribute("Name", "MainTask"),
                        new XAttribute("Type", "CONTINUOUS"),
                        new XAttribute("Priority", "10"),
                        new XAttribute("Watchdog", "500"),
                        new XAttribute("DisableUpdateOutputs", "false"),
                        new XAttribute("InhibitTask", "false"),

                        new XElement("ScheduledProgram",
                            new XAttribute("Name", "MainProgram")
                        )
                    )
                );
            return new XElement("Tasks");
        }


    }
}