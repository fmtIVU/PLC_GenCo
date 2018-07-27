using Microsoft.AspNet.Identity;
using PLC_GenCo.Generator;
using PLC_GenCo.Models;
using PLC_GenCo.ViewModels;
using PLC_GenCo.XMLDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace PLC_GenCo.Controllers
{
    public class ExportController : Controller
    {
        private ApplicationDbContext _context;

        public ExportController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Export
        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            String pageName;

            if (String.IsNullOrEmpty(userName))
            {
                pageName = "";
            }
            else
            {
                pageName = _context.Users.First(c => c.Name == userName).ActProject;
            }

            var viewModel = new ExportViewModel
            {
                Controller = xmlDB.PLC,
                Modules = xmlDB.Modules,
                UDTs = new List<UDT>(),
                AOIs = new List<Standard>(),
                Tags = new List<Tag>(),
                Tasks = new List<Task>()
            };

            var project = Generate();

            foreach (var udt in project.Element("Controller").Element("DataTypes").Elements())
            {
                var addUDT = new UDT
                {
                    Name = udt.Attribute("Name").Value,
                };

                if (udt.Elements("Description").Any())
                {
                    addUDT.Description = udt.Element("Description").Value;
                }
                else
                {
                    addUDT.Description = "No description";
                }

                viewModel.UDTs.Add(addUDT);

            }

            foreach (var tag in project.Element("Controller").Element("Tags").Elements())
            {
                var addTag = new Tag
                {
                    Name = tag.Attribute("Name").Value,
                    Type = tag.Attribute("DataType").Value,
                };

                if (tag.Elements("Description").Any())
                {
                    addTag.Description = tag.Element("Description").Value;
                }
                else
                {
                    addTag.Description = "No description";
                }

                viewModel.Tags.Add(addTag);

            }

            foreach (var aoi in project.Element("Controller").Element("AddOnInstructionDefinitions").Elements())
            {
                var addStandard = new Standard
                {
                    AOIName = aoi.Attribute("Name").Value,
                };

                if (aoi.Elements("Description").Any())
                {
                    addStandard.Description = aoi.Element("Description").Value;
                }
                else
                {
                    addStandard.Description = "No description";
                }

                String name = aoi.Attribute("Name").Value;
                addStandard.Group = xmlDB.Standards.First(c => c.AOIName == name).Group;

                viewModel.AOIs.Add(addStandard);

            }

            foreach (var task in project.Element("Controller").Element("Tasks").Elements())
            {
                var addTask = new Task
                {
                    Name = task.Attribute("Name").Value,
                    Type = task.Attribute("Type").Value,
                    Programs = new List<Program>(),
                };

                if (task.Elements("Description").Any())
                {
                    addTask.Description = task.Element("Description").Value;
                }
                else
                {
                    addTask.Description = "No description";
                }

                // Add all tasks programs
                foreach (var program in task.Element("ScheduledPrograms").Elements())
                {
                    var addProgram = new Program
                    {
                        Name = program.Attribute("Name").Value,
                        Routines = new List<Routine>()
                    };

                    var locatedProgram = project.Element("Controller").Element("Programs").Elements().First(c => c.Attribute("Name").Value == program.Attribute("Name").Value);

                    if (locatedProgram.Elements("Description").Any())
                    {
                        addProgram.Description = locatedProgram.Element("Description").Value;
                    }
                    else
                    {
                        addProgram.Description = "No description";
                    }

                    // Add all programs routines
                    foreach (var routine in locatedProgram.Element("Routines").Elements())
                    {
                        var addRoutine = new Routine
                        {
                            Name = routine.Attribute("Name").Value,
                        };

                        if (routine.Elements("Description").Any())
                        {
                            addRoutine.Description = routine.Element("Description").Value;
                        }
                        else
                        {
                            addRoutine.Description = "No description";
                        }

                        if (routine.Elements("RLLContent").Any())
                        {
                            addRoutine.Type = "LAD";
                        }
                        else
                        {
                            addRoutine.Type = "STL";
                        }

                        addProgram.Routines.Add(addRoutine);
                    }
                    addTask.Programs.Add(addProgram);
                }
                viewModel.Tasks.Add(addTask);

            }




            return View(viewModel);
        }

        public ActionResult Download()
        {
            
            string XMLVersion = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";

            string project = XMLVersion + Environment.NewLine + Generate().ToString();

            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=GenCoTest" + DateTime.Now.ToString("yyyyMMddhhmm") + ".xml");
            Response.ContentType = "xml";

            // Write all data
            Response.Write(project);
            Response.End();

            return new EmptyResult();
        }

        private XElement Generate()
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            String pageName;

            if (String.IsNullOrEmpty(userName))
            {
                pageName = "";
            }
            else
            {
                pageName = _context.Users.First(c => c.Name == userName).ActProject;
            }

            var controllerInfo = new ControllerInfo();
            var datatypesInfo = new DataTypesInfo
            {
                Components = xmlDB.Components.ToList(),
                Standards = xmlDB.Standards.ToList(),
                Locations = xmlDB.Locations.ToList(),
                IOs = xmlDB.IOs.ToList(),
                //DIPulseSetups = _context.DIpulses.ToList(),
                Modules = xmlDB.Modules.ToList(),
                //MotFrqSetups = _context.MotFrqs.ToList(),
                //DIAlarmSetups = _context.DIAlarms.ToList(),
                //AIAlarmSetups = _context.AIAlarms.ToList(),
                ApplyLocationFilter = false

            };

            var modulesInfo = new ModulesInfo
            {
                modules = xmlDB.Modules.ToList(),
                controller = new ControllerInfo
                {
                    name = "StdPLC",
                    description = "Standard controller"                   //TODO From database
                },
                IOs = xmlDB.IOs.ToList(),
                Components = xmlDB.Components.ToList(),
                //MotFrqSetups = xmlDB.MotFrqs.ToList()

            };

            var tagsInfo = new TagsInfo();

            var programsInfo = new ProgramsInfo
            {
                IOs = xmlDB.IOs.ToList(),
                Modules = xmlDB.Modules.ToList(),
                //AIAlarmSetups = xmlDB.AIAlarms.ToList(),
                //DIAlarmSetups = xmlDB.DIAlarms.ToList(),
                //DIPulseSetups = xmlDB.DIpulses.ToList(),
                //MDirSetups = xmlDB.MDirs.ToList(),
                //MRevSetups = xmlDB.MRevs.ToList(),
                //MotFrqSetups = xmlDB.MotFrqs.ToList(),
                //StdVlvSetups = xmlDB.StdVlvs.ToList(),
                Components = xmlDB.Components.ToList()

            };

            var tasksInfo = new TasksInfo();
            var addOnInstructionDefinitionsInfo = new AddOnInstructionDefinitionsInfo
            {
                Components = xmlDB.Components.ToList(),
                Standards = xmlDB.Standards.ToList()

            };



            var generator = new ProjectGenerator(controllerInfo, datatypesInfo, modulesInfo, addOnInstructionDefinitionsInfo, tagsInfo, programsInfo, tasksInfo);


            return generator.GenerateProject();
        }

        public ActionResult ExportRoutine(string name)
        {
            // Locate Routine
            var routine = new XElement("Empty");

            //Search through all programs for routine with name X
            foreach (var program in Generate().Element("Controller").Element("Programs").Elements())
            {
                foreach (var routineXE in program.Element("Routines").Elements())
                {
                    if (routineXE.Attribute("Name").Value == name)
                    {
                        routine = routineXE;
                        break;
                    }
                }
            }

            // Use = target mandatory
            if (routine.Attributes().Any(c => c.Name == "Use"))
            {
                routine.Attribute("Use").Value = "Target";
            }else
            {
                routine.Add(new XAttribute("Use", "Target"));
            }
            

            //Pack-up in project
            var exportRoutine = new XElement("RSLogix5000Content",
                new XAttribute("ExportOptions", "References DecoratedData Context Dependencies ForceProtectedEncoding AllProjDocTrans"),
                new XAttribute("ExportDate", "Thu Jul 19 14:02:34 2018"),
                new XAttribute("Owner", "Windows User, Frontmatec"),
                new XAttribute("ContainsContext", "true"),
                new XAttribute("TargetSubType", "RLL"),
                new XAttribute("TargetType", "Routine"),
                new XAttribute("TargetName", name),
                new XAttribute("SoftwareRevision", "24.00"),
                new XAttribute("SchemaRevision", "1.0"),

                new XElement("Controller",
                    new XAttribute("Name","PLC"),
                    new XAttribute("Use", "Context"),

                    new XElement("Programs",
                        new XAttribute("Use", "Context"),

                        new XElement("Program",
                            new XAttribute("Use", "Context"),

                            new XElement("Routines",
                                new XAttribute("Use", "Context")
                                )
                            )
                        )
                    )
                );

            exportRoutine.Element("Controller").Element("Programs").Element("Program").Element("Routines").Add(routine);

            //Write to file
            string XMLVersion = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";

            string project = XMLVersion + Environment.NewLine + exportRoutine.ToString();

            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + name + ".L5X");
            Response.ContentType = "xml";

            // Write all data
            Response.Write(project);
            Response.End();

            return new EmptyResult();
        }

        public ActionResult ExportProgram(string name)
        {
            // Locate Program
            var program = Generate().Element("Controller").Element("Programs").Elements().First(c => c.Attribute("Name").Value == name);

            // Use = target mandatory
            if (program.Attributes().Any(c => c.Name == "Use"))
            {
                program.Attribute("Use").Value = "Target";
            }
            else
            {
                program.Add(new XAttribute("Use", "Target"));
            }

            //Pack-up in project
            var exportProgram = new XElement("RSLogix5000Content",
                new XAttribute("ExportOptions", "References DecoratedData Context Dependencies ForceProtectedEncoding AllProjDocTrans"),
                new XAttribute("ExportDate", "Thu Jul 19 14:02:34 2018"),
                new XAttribute("Owner", "Windows User, Frontmatec"),
                new XAttribute("ContainsContext", "true"),
                new XAttribute("TargetSubType", "RLL"),
                new XAttribute("TargetType", "Program"),
                new XAttribute("TargetName", name),
                new XAttribute("SoftwareRevision", "24.00"),
                new XAttribute("SchemaRevision", "1.0"),

                new XElement("Controller",
                    new XAttribute("Name", "PLC"),
                    new XAttribute("Use", "Context"),

                    new XElement("Programs",
                        new XAttribute("Use", "Context")
                        )
                    )
                );

            exportProgram.Element("Controller").Element("Programs").Add(program);

            //Write to file
            string XMLVersion = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";

            string project = XMLVersion + Environment.NewLine + exportProgram.ToString();

            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + name + ".L5X");
            Response.ContentType = "xml";

            // Write all data
            Response.Write(project);
            Response.End();

            return new EmptyResult();
        }

        public ActionResult ExportUDT(string name)
        {
            // Locate UDT
            var UDT = Generate().Element("Controller").Element("DataTypes").Elements().First(c => c.Attribute("Name").Value == name);

            // Use = target mandatory
            if (UDT.Attributes().Any(c => c.Name == "Use"))
            {
                UDT.Attribute("Use").Value = "Target";
            }
            else
            {
                UDT.Add(new XAttribute("Use", "Target"));
            }

            //Pack-up in project
            var exportUDT = new XElement("RSLogix5000Content",
                new XAttribute("ExportOptions", "References DecoratedData Context Dependencies ForceProtectedEncoding AllProjDocTrans"),
                new XAttribute("ExportDate", "Thu Jul 19 14:02:34 2018"),
                new XAttribute("Owner", "Windows User, Frontmatec"),
                new XAttribute("ContainsContext", "true"),
                new XAttribute("TargetSubType", "RLL"),
                new XAttribute("TargetType", "DataType"),
                new XAttribute("TargetName", name),
                new XAttribute("SoftwareRevision", "24.00"),
                new XAttribute("SchemaRevision", "1.0"),

                new XElement("Controller",
                    new XAttribute("Name", "PLC"),
                    new XAttribute("Use", "Context"),

                    new XElement("DataTypes",
                        new XAttribute("Use", "Context")
                        )
                    )
                );

            exportUDT.Element("Controller").Element("DataTypes").Add(UDT);

            //Write to file
            string XMLVersion = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";

            string project = XMLVersion + Environment.NewLine + exportUDT.ToString();

            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + name + ".L5X");
            Response.ContentType = "xml";

            // Write all data
            Response.Write(project);
            Response.End();

            return new EmptyResult();
        }
    }
}