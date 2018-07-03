using PLC_GenCo.Generator;
using PLC_GenCo.Models;
using PLC_GenCo.Models.Setups;
using PLC_GenCo.ViewModels;
using PLC_GenCo.ViewModels.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace PLC_GenCo.Controllers
{
    public class FactoryController : Controller
    {
        private ApplicationDbContext _context;

        public FactoryController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        //------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------
        // GET: Factory/Index
        // Load main page
        public ActionResult Index()
        {

            var viewModel = new FactoryViewModel
            {
                Standards = _context.Standards.ToList(),
                Components = _context.Components.ToList()
            };

            return View(viewModel);
        }
        //------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------
        // Addcomponent
        public ActionResult AddComponent()
        {

            var addComponentFactoryViewModel = new AddComponentFactoryViewModel()
            {
                ComponentLocations = _context.ComponentLocations.ToList(),
                
            };

            return View("ComponentForm", addComponentFactoryViewModel);
        }
        //------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------
        // Save new or edited component
        [HttpPost]
        public ActionResult Save (Component component)
        {
            if (!ModelState.IsValid && component.Id != 0 && component.ConnectionType != 0)
            {

                var viewModel = new AddComponentFactoryViewModel
                {
                    Component = component,
                    ComponentLocations = _context.ComponentLocations.ToList(),
                };
                return View("ComponentForm", viewModel);

            }

            component.ConnectionType = _context.Standards.Single(c => c.StandardComponent == component.StandardComponent).ConnectionType;

            if (component.Id == 1)
            {
                _context.Components.Add(component);
            }
            else
            {
                var componentInDb = _context.Components.SingleOrDefault(c => c.Id == component.Id);
                if(componentInDb == null)
                {
                    _context.Components.Add(component);
                }
                else
                {
                    componentInDb.Name = component.Name;
                    componentInDb.Comment = component.Comment;
                    componentInDb.Location = component.Location;
                    componentInDb.StandardComponent = component.StandardComponent;
                    componentInDb.IsParent = component.IsParent;
                    componentInDb.IOId = component.IOId;
                    componentInDb.StandardId = component.StandardId;
                }

            }
            _context.SaveChanges();

            if (component.IsParent)
            {
                return RedirectToAction("Index", "IOList");
            }
            else
            {
                return RedirectToAction("Index", "Factory");
            }
            
        }
        //------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------
        // Edit component
        public ActionResult Edit(int id)
        {

            var component = _context.Components.SingleOrDefault(c => c.Id == id);

            if (component == null)
                return HttpNotFound();

            var viewModel = new AddComponentFactoryViewModel
            {
                Component = component,
                ComponentLocations = _context.ComponentLocations.ToList(),
            };

            return View("ComponentForm", viewModel);
        }
        //------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------
        // Download components
        public ActionResult Download()
        {

            var aoi = new XElement("AOI");

            var uri = new System.Uri(@"C:\Users\Ivan\Desktop\OP generator PLC koda\StandardAOI\Analog");

            aoi = XElement.Load(@"C:\Users\Ivan\Desktop\OP generator PLC koda\StandardAOI\Analog.L5X");
            /*
            var controllerInfo = new ControllerInfo();
            var datatypesInfo = new DataTypesInfo();
            datatypesInfo.components = _context.Components.ToList();
            datatypesInfo.standards = _context.Standards.ToList();
            datatypesInfo.locations = _context.ComponentLocations.ToList();
            datatypesInfo.applyLocationFilter = false;

            var modulesInfo = new ModulesInfo { modules = new List<Module>(), controller = new ControllerInfo()};
            modulesInfo.controller.name = "Pumpestation PLC";
            modulesInfo.controller.description = "Tavle PLC";

            var module0 = new Module {ModuleId = 1, Name = "Rack 1 Embedded DIx16", IOModulesType = Enums.IOModulesType.EmbDIx16, Comments = new string[16] };
            var module1 = new Module {ModuleId = 2, Name = "Rack 1 Embedded DOx16", IOModulesType = Enums.IOModulesType.EmbDOx16, Comments = new string[16] };
            var module2 = new Module {ModuleId = 3, Name = "Rack 1 DIx4", IOModulesType = Enums.IOModulesType.DIx4, Comments = new string[4] };
            var module3 = new Module {ModuleId = 4, Name = "Rack 1 AIx8", IOModulesType = Enums.IOModulesType.AIx8, Comments = new string[8] };
            var module4 = new Module { ModuleId = 5, Name = "Rack 1 AIx4", IOModulesType = Enums.IOModulesType.AIx4, Comments = new string[4] };
            var module5 = new Module { ModuleId = 6, Name = "Rack 1 DIx8", IOModulesType = Enums.IOModulesType.DIx8, Comments = new string[8] };
            var module6 = new Module { ModuleId = 7, Name = "Rack 1 AOx4", IOModulesType = Enums.IOModulesType.AOx4, Comments = new string[4] };

            modulesInfo.modules.Add(module0);
            modulesInfo.modules.Add(module1);
            modulesInfo.modules.Add(module2);
            modulesInfo.modules.Add(module3);
            modulesInfo.modules.Add(module4);
            modulesInfo.modules.Add(module5);
            modulesInfo.modules.Add(module6);




            var globalTagsInfo = new GlobalTagsInfo();
            var programsInfo = new ProgramsInfo();
            var tasksInfo = new TasksInfo();
            var addOnInstructionDefinitionsInfo = new AddOnInstructionDefinitionsInfo();



            var generator = new ProjectGenerator(controllerInfo, datatypesInfo, modulesInfo, addOnInstructionDefinitionsInfo, globalTagsInfo, programsInfo, tasksInfo);


            //Console.WriteLine(generator.GenerateProject());
            // Create a file to write to.

            string XMLVersion = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";

            string project = XMLVersion + Environment.NewLine + generator.GenerateProject().ToString();
            */
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=Project.xml");
            Response.ContentType = "xml";

            // Write all my data
            //Response.Write(project);
            Response.Write(aoi);
            Response.End();

            // Not sure what else to do here
            return RedirectToAction("Index", "Factory");
            //var generator = new Generator();
            //byte[] fileBytes = System.IO.File.ReadAllBytes(@"C:/Users/Ivan/Desktop/OP generator PLC koda/generated_files/generatedfile.xml");
            //string fileName = "generatedfile.xml";
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }

        public ActionResult Setup(int id)
        {
            var component = _context.Components.Single(c => c.Id == id);
            

            switch (component.StandardComponent)
            {
                case Enums.StandardComponent.No_Match:
                    return Content("Select standard first!");
                    
                case Enums.StandardComponent.DI_Alarm:

                    var DIAlarmSetupInDb = _context.DIAlarms.SingleOrDefault(c => c.IdComponent == id);

                    DIAlarmSetupViewModel viewModelDIAlarm = new DIAlarmSetupViewModel
                    {
                        Component = _context.Components.SingleOrDefault(c => c.Id == id),
                    };

                    //IF its edit use existing one
                    if (!(DIAlarmSetupInDb == null))
                    {
                        viewModelDIAlarm.DIAlarmSetup = DIAlarmSetupInDb;
                    }
                    else
                    {
                        viewModelDIAlarm.DIAlarmSetup = new DIAlarmSetup
                        {
                            IdComponent = component.Id,
                        };
                    }
                    

                    return View("SetupDIAlarm", viewModelDIAlarm);
                    
                case Enums.StandardComponent.DI_Pulse:

                    var DIPulseSetupInDb = _context.DIpulses.SingleOrDefault(c => c.IdComponent == id);

                    DIPulseSetupViewModel viewModelDIPulse = new DIPulseSetupViewModel
                    {
                        Component = _context.Components.SingleOrDefault(c => c.Id == id),
                    };

                    //IF its edit use existing one
                    if (!(DIPulseSetupInDb == null))
                    {
                        viewModelDIPulse.DIPulseSetup = DIPulseSetupInDb;
                    }
                    else
                    {
                        viewModelDIPulse.DIPulseSetup = new DIPulseSetup
                        {
                            IdComponent = component.Id,
                        };
                    }

                    return View("SetupDIPulse", viewModelDIPulse);
                    
                    
                case Enums.StandardComponent.DO:
                    return Content("No available setup!");
                   
                case Enums.StandardComponent.AI_Alarm:

                    var AIAlarmSetupInDb = _context.AIAlarms.SingleOrDefault(c => c.IdComponent == id);

                    AIAlarmSetupViewModel viewModelAIAlarm = new AIAlarmSetupViewModel
                    {
                        Component = _context.Components.SingleOrDefault(c => c.Id == id),
                    };

                    //IF its edit use existing one
                    if (!(AIAlarmSetupInDb == null))
                    {
                        viewModelAIAlarm.AIAlarmSetup = AIAlarmSetupInDb;
                    }
                    else
                    {
                        viewModelAIAlarm.AIAlarmSetup = new AIAlarmSetup
                        {
                            IdComponent = component.Id,
                            IdIO = component.IOId,
                        };
                    }

                    return View("SetupAIAlarm", viewModelAIAlarm);
                   
                case Enums.StandardComponent.P_Std_Motor_Dir:


                    var MDirSetupInDb = _context.MDirs.Include("INMeasurement01").
                                                      Include("INMeasurement02").
                                                      Include("INMeasurement03").
                                                      Include("INMeasurement04").
                                                      Include("INMeasurement05").
                                                      Include("INMeasurement06").SingleOrDefault(c => c.IdComponent == id);

                    MDirSetupViewModel viewModelMDir = new MDirSetupViewModel
                    {
                        Component = _context.Components.SingleOrDefault(c => c.Id == id),
                        Childs = _context.IOs.Where(c => c.Parent == component.Id).ToList()
                    };

                    //IF its edit use existing one
                    if (!(MDirSetupInDb == null))
                    {
                        viewModelMDir.MDirSetup = MDirSetupInDb;
                    }
                    else
                    {
                        viewModelMDir.MDirSetup = new MDirSetup
                        {
                            IdComponent = component.Id,
                        };
                    }

                    return View("SetupMDir", viewModelMDir);
                 /*   
                case Enums.StandardComponent.P_Std_Motor_Dir_Frq:
                    return View("SetupStdMotorDirFrq", viewModel);
                    
                case Enums.StandardComponent.P_Std_Motor_Rev:
                    return View("SetupStdMotorRev", viewModel);
                    
                case Enums.StandardComponent.P_Std_Motor_Rev_Frq:
                    return View("SetupStdMotorRevFrq", viewModel);
                    
                case Enums.StandardComponent.P_Std_Valve:
                    return View("SetupStdValve", viewModel);
                    
                case Enums.StandardComponent.C_AI:
                    return Content("Setup as part of parent component!");
                    
                case Enums.StandardComponent.C_AO:
                    return Content("Setup as part of parent component!");
                    
                case Enums.StandardComponent.C_DO:
                    return Content("Setup as part of parent component!");
                    
                case Enums.StandardComponent.C_DI:
                    return Content("Setup as part of parent component!");
                 */
                default:
                    return Content("Unknown standard!");
                    
            }

        }

        public ActionResult SaveDIAlarm(DIAlarmSetup DIAlarmSetup)
        {
            if (!ModelState.IsValid && DIAlarmSetup.Id != 0)
            {

                var viewModel = new DIAlarmSetupViewModel
                {
                    Component = _context.Components.SingleOrDefault(c=> c.Id == DIAlarmSetup.IdComponent),
                    DIAlarmSetup = DIAlarmSetup,
                   
                };
                return View("SetupDIAlarm", viewModel);

            }
            
            var DIAlarmInDb = _context.DIAlarms.SingleOrDefault(c => c.Id == DIAlarmSetup.Id);

            if (DIAlarmInDb == null)
            {
                _context.DIAlarms.Add(DIAlarmSetup);
            }
            else
            {
                DIAlarmInDb.IdComponent = DIAlarmSetup.IdComponent;
                DIAlarmInDb.InputType = DIAlarmSetup.InputType;
                DIAlarmInDb.TimeDelay = DIAlarmSetup.TimeDelay;
            }

        _context.SaveChanges();
        
        return RedirectToAction("Index", "Factory");
        }

        public ActionResult SaveDIPulse(DIPulseSetup DIPulseSetup)
        {
            if (!ModelState.IsValid && DIPulseSetup.Id != 0)
            {

                var viewModel = new DIPulseSetupViewModel
                {
                    Component = _context.Components.SingleOrDefault(c => c.Id == DIPulseSetup.IdComponent),
                    DIPulseSetup = DIPulseSetup,

                };
                return View("SetupDIPulse", viewModel);

            }

            var DIPulseInDb = _context.DIpulses.SingleOrDefault(c => c.Id == DIPulseSetup.Id);

            if (DIPulseInDb == null)
            {
                _context.DIpulses.Add(DIPulseSetup);
            }
            else
            {
                DIPulseInDb.IdComponent = DIPulseSetup.IdComponent;

            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Factory");
        }

        public ActionResult SaveAIAlarm(AIAlarmSetup AIAlarmSetup)
        {
            if (!ModelState.IsValid && AIAlarmSetup.Id != 0)
            {

                var viewModel = new AIAlarmSetupViewModel
                {
                    Component = _context.Components.SingleOrDefault(c => c.Id == AIAlarmSetup.IdComponent),
                    AIAlarmSetup = AIAlarmSetup,

                };
                return View("SetupAIAlarm", viewModel);

            }

            var AIAlarmInDb = _context.AIAlarms.SingleOrDefault(c => c.Id == AIAlarmSetup.Id);

            if (AIAlarmInDb == null)
            {
                _context.AIAlarms.Add(AIAlarmSetup);
            }
            else
            {
                AIAlarmInDb.IdComponent = AIAlarmSetup.IdComponent;
                AIAlarmInDb.IdIO = AIAlarmSetup.IdIO;
                AIAlarmInDb.AICType = AIAlarmSetup.AICType;
                AIAlarmInDb.TimeDelay = AIAlarmSetup.TimeDelay;
                AIAlarmInDb.ScaleMax = AIAlarmSetup.ScaleMax;
                AIAlarmInDb.ScaleMin = AIAlarmSetup.ScaleMin;
                AIAlarmInDb.AlarmHigh = AIAlarmSetup.AlarmHigh;
                AIAlarmInDb.AlarmLow = AIAlarmSetup.AlarmLow;
                AIAlarmInDb.AlarmEqual = AIAlarmSetup.AlarmEqual;
                AIAlarmInDb.UseAlarmHigh = AIAlarmSetup.UseAlarmHigh;
                AIAlarmInDb.UseAlarmLow = AIAlarmSetup.UseAlarmLow;
                AIAlarmInDb.UseAlarmEqual = AIAlarmSetup.UseAlarmEqual;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Factory");
        }

        public ActionResult SaveMDir(MDirSetup MDirSetup)
        {
            if (!ModelState.IsValid && MDirSetup.Id != 0)
            {

                var viewModel = new MDirSetupViewModel
                {
                    Component = _context.Components.SingleOrDefault(c => c.Id == MDirSetup.IdComponent),
                    MDirSetup = MDirSetup,

                };
                return View("SetupMDir", viewModel);

            }

            var MDirInDb = _context.MDirs.Include("INMeasurement01").
                                          Include("INMeasurement02").
                                          Include("INMeasurement03").
                                          Include("INMeasurement04").
                                          Include("INMeasurement05").
                                          Include("INMeasurement06").First(c => c.Id == MDirSetup.Id);


            //All AIAlarmSetups belong to this parent component
            MDirSetup.INMeasurement01.IdComponent = MDirSetup.IdComponent;
            MDirSetup.INMeasurement02.IdComponent = MDirSetup.IdComponent;
            MDirSetup.INMeasurement03.IdComponent = MDirSetup.IdComponent;
            MDirSetup.INMeasurement04.IdComponent = MDirSetup.IdComponent;
            MDirSetup.INMeasurement05.IdComponent = MDirSetup.IdComponent;
            MDirSetup.INMeasurement06.IdComponent = MDirSetup.IdComponent;

            if (MDirInDb == null)
            {
                _context.MDirs.Add(MDirSetup);
            }
            else
            {
                MDirInDb.IdComponent = MDirSetup.IdComponent;
                MDirInDb.INExtFault01 = MDirSetup.INExtFault01;
                MDirInDb.INExtFault02 = MDirSetup.INExtFault02;
                MDirInDb.INExtFault03 = MDirSetup.INExtFault03;
                MDirInDb.INExtFault04 = MDirSetup.INExtFault04;
                MDirInDb.INExtFault05 = MDirSetup.INExtFault05;
                MDirInDb.INExtFault06 = MDirSetup.INExtFault06;
                MDirInDb.INExtFault07 = MDirSetup.INExtFault07;
                MDirInDb.INExtFault08 = MDirSetup.INExtFault08;

                MDirInDb.InputType01 = MDirSetup.InputType01;
                MDirInDb.InputType02 = MDirSetup.InputType02;
                MDirInDb.InputType03 = MDirSetup.InputType03;
                MDirInDb.InputType04 = MDirSetup.InputType04;
                MDirInDb.InputType05 = MDirSetup.InputType05;
                MDirInDb.InputType06 = MDirSetup.InputType06;
                MDirInDb.InputType07 = MDirSetup.InputType07;
                MDirInDb.InputType08 = MDirSetup.InputType08;

                MDirInDb.INRunningFB = MDirSetup.INRunningFB;

                MDirInDb.OUTResetSignal = MDirSetup.OUTResetSignal;
                MDirInDb.OUTStartSignal = MDirSetup.OUTStartSignal;


                MDirInDb.INMeasurement01 = MDirSetup.INMeasurement01;
                MDirInDb.INMeasurement02 = MDirSetup.INMeasurement02;
                MDirInDb.INMeasurement03 = MDirSetup.INMeasurement03;
                MDirInDb.INMeasurement04 = MDirSetup.INMeasurement04;
                MDirInDb.INMeasurement05 = MDirSetup.INMeasurement05;
                MDirInDb.INMeasurement06 = MDirSetup.INMeasurement06;


            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Factory");
        }

        
    }
}