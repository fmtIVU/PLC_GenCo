using PLC_GenCO_Generator.Inits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PLC_GenCO_Generator.Generators
{
    public class Generator
    {
        //readOnly
        public ControllerInfo ControllerInfo { get; }
        public DataTablesInfo DataTablesInfo { get; }
        public ModulesInfo ModulesInfo { get; }
        public AddOnDefinitionsInfo AddOnDefinitionsInfo { get; }
        public GlobalTagsInfo GlobalTagsInfo { get; }
        public ProgramsInfo ProgramsInfo { get; }
        public TasksInfo TasksInfo { get; }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------  
        //constructor
        public Generator(ControllerInfo controllerInfo, DataTablesInfo dataTablesInfo,
                          ModulesInfo modulesInfo, AddOnDefinitionsInfo addOnDefinitionsInfo,
                          GlobalTagsInfo globalTagsInfo, ProgramsInfo programsInfo, TasksInfo tasksInfo)
        {

            var initControllerInfo = new InitControllerInfo(controllerInfo);
            ControllerInfo = initControllerInfo.InitializedData();

            var initDataTablesInfo = new InitDataTablesInfo(dataTablesInfo);
            DataTablesInfo = initDataTablesInfo.InitializedData();

            var initModulesInfo = new InitModulesInfo(modulesInfo);
            ModulesInfo = initModulesInfo.InitializedData();

            var initAddOnDefinitionsInfo = new InitAddOnDefinitionsInfo(addOnDefinitionsInfo);
            AddOnDefinitionsInfo = initAddOnDefinitionsInfo.InitializedData();

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

            var generateDataTables = new GenerateDataTables(DataTablesInfo);
            
            var generateModules = GenerateModules(ModulesInfo);
            var generateAddOnDefinitions = GenerateAddOnDefinitions(AddOnDefinitionsInfo);
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
                    new XAttribute("ProcessorType", ControllerInfo.procesorType),
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

                    generateDataTables.GetDataTables(),
                    generateModules,
                    generateAddOnDefinitions,
                    generateGlobalTags,
                    generatePrograms,
                    generateTasks
                    )
                );
    


        return generatedProject;
        }
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------

        private XElement GenerateModules(ModulesInfo modulesInfo)
        {
            return new XElement("Modules");
        }

        private XElement GenerateAddOnDefinitions(AddOnDefinitionsInfo addOnDefinitionsInfo)
        {
            return new XElement("AddOnDefinitions");
        }

        private XElement GenerateGlobalTags(GlobalTagsInfo globalTagsInfo)
        {
            return new XElement("GlobalTags");
        }

        private XElement GeneratePrograms(ProgramsInfo programsInfo)
        {
            return new XElement("Programs");
        }

        private XElement GenerateTasks(TasksInfo tasksInfo)
        {
            return new XElement("Tasks");
        }


    }
}
