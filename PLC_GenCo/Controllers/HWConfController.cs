using Microsoft.AspNet.Identity;
using PLC_GenCo.Models;
using PLC_GenCo.ViewModels;
using PLC_GenCo.XMLDB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PLC_GenCo.Controllers
{
    public class HWConfController : Controller
    {
        private ApplicationDbContext _context;

        public HWConfController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        //====================================================================================================================
        //MAIN PAGE
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

            var viewModel = new HWConfViewModel
            {
                Modules = xmlDB.Modules,
                Locations = xmlDB.Locations,
                PLC = xmlDB.PLC,
                PageName = pageName
            };
            return View(viewModel);
        }
        //====================================================================================================================
        //UPLOAD FILE
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            var ioList = new List<IO>();

            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                //var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                //var path = Path.Combine(Server.MapPath("C:/Users/Ivan/Desktop/OP generator PLC koda/uploads"), fileName);
                //file.SaveAs(path);

                //Parsing file
                var ms = new MemoryStream();
                file.InputStream.CopyTo(ms);
                byte[] array = ms.GetBuffer();
                string fileAsString = Encoding.UTF8.GetString(array);
                //Split by lines
                String[] CSVrows = fileAsString.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Skip(1).ToArray();

                
                //Each line split by ';' -- parse values to database
                foreach (string str in CSVrows)
                {
                    if (str.Length < 10)
                        continue;

                    var IOmembers = str.Split(';');

                    var io = new IO();

                    io.Location = IOmembers[0];

                    switch (IOmembers[1])
                    {
                        case ("AI"):
                            io.ConnectionType = Enums.ConnectionType.AI;
                            break;
                        case ("AO"):
                            io.ConnectionType = Enums.ConnectionType.AO;
                            break;
                        case ("DI"):
                            io.ConnectionType = Enums.ConnectionType.DI;
                            break;
                        case ("DO"):
                            io.ConnectionType = Enums.ConnectionType.DO;
                            break;
                        case ("DIO"):
                            io.ConnectionType = Enums.ConnectionType.DIO;
                            break;
                        default:
                            throw new Exception("Error reading PLC IO type");
                    }

                    io.IOAddress = new IOAddress(IOmembers[2]);
                    io.Name = IOmembers[3];
                    io.ParentName = IOmembers[4];
                    io.Comment = IOmembers[5];


                    //Add io
                    ioList.Add(io);
                }




            }

            foreach (var io in ioList)
            {
                xmlDB.IOs.Add(io);
            }

            xmlDB.Save();
            //Add locations to database
            DetectLocations();
            CreateComponents();
            
             

            return RedirectToAction("Index");
        }
        //====================================================================================================================
        //LOCATION HANDLING
        //Call location form
        public ActionResult LocationForm(int id)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            var location = xmlDB.Locations.SingleOrDefault(c => c.Id == id);

            if (location == null)
                return HttpNotFound();

            var viewModel = new LocationFormViewModel
            {
                Location = location,
            };

            return View("LocationForm", viewModel);
        }
        //Add location button
        public ActionResult AddLocation()
        {

            return View("LocationForm");
        }
        //Save new/edited location
        [HttpPost]
        public ActionResult SaveLocation(ComponentLocation location)
        {
            if (!ModelState.IsValid && location.Id != 0)
            {

                var viewModel = new LocationFormViewModel
                {
                    Location = location,
                };
                return View("LocationForm", viewModel);

            }

            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            //Choose between add and update
            if (location.Id == 0)
            {
                xmlDB.Locations.Add(location);
            }else
            {
                var locationInDB = xmlDB.Locations.Single(c => c.Id == location.Id);
                locationInDB.Name = location.Name;
                locationInDB.Prefix = location.Prefix;
            }
            
            xmlDB.Save();

            return RedirectToAction("Index", "HWConf");
        }
        //====================================================================================================================
        //MODULE HANDLING
        //Call module form
        public ActionResult ModuleForm(int id)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            var module = xmlDB.Modules.SingleOrDefault(c => c.Id == id);

            if (module == null)
                return HttpNotFound();

            var viewModel = new ModuleFormViewModel
            {
                Module = module,
            };

            return View("ModuleForm", viewModel);
        }
        //Add module button
        public ActionResult AddModule()
        {

            return View("ModuleForm");
        }
        //Save new/edited modlue
        [HttpPost]
        public ActionResult SaveModule(Module module)
        {
            if (!ModelState.IsValid && module.Id != 0)
            {

                var viewModel = new ModuleFormViewModel
                {
                    Module = module,
                };
                return View("ModuleForm", viewModel);

            }

            

            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            // Create new or update
            if (module.Id == 0)
            {
                //new
                module.Address = xmlDB.Modules.Count();
                xmlDB.Modules.Add(module);
            }
            else
            {
                //Update
                var moduleInDB = xmlDB.Modules.Single(c => c.Id == module.Id);
                moduleInDB.Name = module.Name;
                moduleInDB.IOModulesType = module.IOModulesType;

            }
            
            xmlDB.Save();

            return RedirectToAction("Index", "HWConf");
        }
        //====================================================================================================================
        //PLC HANDLING
        //Call PLC form
        public ActionResult AddPLC()
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            if (xmlDB.PLC ==  null)
            {
                return View("PLCForm");
            }
            else
            {
                var viewModel = new PLCFormViewModel
                {
                    PLC = xmlDB.PLC
                };

                return View("PLCForm", viewModel);
            }

        }
        //Save new/edited PLC
        [HttpPost]
        public ActionResult SavePLC(PLC PLC)
        {
            if (!ModelState.IsValid && PLC.Id != 0)
            {

                var viewModel = new PLCFormViewModel
                {
                    PLC = PLC,
                };
                return View("PLCForm", viewModel);

            }
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            xmlDB.PLC = PLC;
            xmlDB.Save();
            //------------------------------------------------------------------------
            //Add embedded IO modules

            if (PLC.ProductType == Enums.ControllerType.L16ER || PLC.ProductType == Enums.ControllerType.L18ER && 
                (xmlDB.Modules.Where(c => ((c.IOModulesType == Enums.IOModulesType.EmbDIx16) || (c.IOModulesType == Enums.IOModulesType.EmbDOx16))).Count() == 0))
            {
                var embDI = new Module
                {
                    Name = "PLC_Emb_DI",
                    IOModulesType = Enums.IOModulesType.EmbDIx16,
                    Address = 1
                };
                var embDO = new Module
                {
                    Name = "PLC_Emb_DO",
                    IOModulesType = Enums.IOModulesType.EmbDOx16,
                    Address = 1
                };

                xmlDB.Modules.Add(embDI);
                xmlDB.Modules.Add(embDO);
                xmlDB.Save();

            }
            


            return RedirectToAction("Index", "HWConf");
        }
        //====================================================================================================================
        //ADD LOCATIONS
        private void DetectLocations()
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            foreach (IO io in xmlDB.IOs.ToList())
            {
                //Add location if not duplicate
                if (xmlDB.Locations.SingleOrDefault(c => c.Name == io.Location) == null) //Add location if it doesnt exist and if it is not empty or null
                {
                    xmlDB.Locations.Add(new ComponentLocation { Name = io.Location });
                    xmlDB.Save();
                }
            }

            return;
        }

        //==========================================================================================================
        //Match IO to Standard
        private void CreateComponents()
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            foreach (var io in xmlDB.IOs.ToList())
            {

                if (!String.IsNullOrWhiteSpace(io.ParentName))
                {
                    //IO is part of parent component
                    io.MatchStatus = Enums.MatchStatus.Check;

                    //If parent component doesnt exist create it
                    if (!xmlDB.Components.Any(c => c.Name == io.ParentName))
                    {
                        var component = new Component();
                        component.Name = io.ParentName;
                        component.Comment = io.Comment;
                        component.Location = io.Location;
                        component.Dependancy = Enums.Dependancy.Parent;

                        component.MatchStatus = Enums.MatchStatus.No_Match;

                        if (io.IOAddress.Type == Enums.IOType.IO)
                        {
                            component.ConnectionType = Enums.ConnectionType.DIO;
                        }
                        else
                        {
                            component.ConnectionType = Enums.ConnectionType.ETH;
                        }

                        xmlDB.Components.Add(component);

                    }else
                    {
                        //if it exist check connection type - if any child IO is IP(ETH) type -> switch components connection type
                        if (io.IOAddress.Type == Enums.IOType.IP)
                        {
                            var componentDB = xmlDB.Components.Single(c => c.Name == io.ParentName);
                            componentDB.ConnectionType = Enums.ConnectionType.ETH;
                        }
                    }

                }
                else
                {

                    switch (io.ConnectionType)
                    {
                        case Enums.ConnectionType.AI:
                            // Create single component

                            var componentAI = new Component();
                            componentAI.IOId = io.Id;
                            componentAI.Name = io.Name;
                            componentAI.Comment = io.Comment;
                            componentAI.Location = io.Location;
                            componentAI.Dependancy = Enums.Dependancy.Single;
                            componentAI.ConnectionType = io.ConnectionType;

                            //Matching
                            var possibleStdAIAOI = xmlDB.Standards.Where(c => c.ConnectionType == io.ConnectionType).ToList();

                            switch (possibleStdAIAOI.Count())
                            {
                                case (0):
                                    componentAI.StandardId = null;
                                    componentAI.MatchStatus = Enums.MatchStatus.No_Match;
                                    io.MatchStatus = Enums.MatchStatus.No_Match;
                                    break;
                                    
                                case (1):
                                    componentAI.StandardId = possibleStdAIAOI.ElementAt(0).Id;
                                    componentAI.MatchStatus = Enums.MatchStatus.Match;
                                    io.MatchStatus = Enums.MatchStatus.Match;
                                    break;

                                    // Case more than one
                                default:
                                    // take first one - standards should be sorted from most used to least used
                                    componentAI.StandardId = possibleStdAIAOI.ElementAt(0).Id;
                                    componentAI.MatchStatus = Enums.MatchStatus.Check;
                                    io.MatchStatus = Enums.MatchStatus.Check;
                                    break;
                            }

                            xmlDB.Components.Add(componentAI);

                            break;
                        //=====================================================================================
                        case Enums.ConnectionType.AO:
                            //Always part of parent component
                            throw new Exception("Analog output must be part of parent component");
                        //=====================================================================================
                        case Enums.ConnectionType.DI:
                            // Create single component

                            var componentDI = new Component();
                            componentDI.IOId = io.Id;
                            componentDI.Name = io.Name;
                            componentDI.Comment = io.Comment;
                            componentDI.Location = io.Location;
                            componentDI.Dependancy = Enums.Dependancy.Single;
                            componentDI.ConnectionType = io.ConnectionType;

                            //Matching
                            var possibleStdDIAOI = xmlDB.Standards.Where(c => c.ConnectionType == io.ConnectionType).ToList();

                            switch (possibleStdDIAOI.Count())
                            {
                                case (0):
                                    componentDI.StandardId = null;
                                    componentDI.MatchStatus = Enums.MatchStatus.No_Match;
                                    io.MatchStatus = Enums.MatchStatus.No_Match;
                                    break;

                                case (1):
                                    componentDI.StandardId = possibleStdDIAOI.ElementAt(0).Id;
                                    componentDI.MatchStatus = Enums.MatchStatus.Match;
                                    io.MatchStatus = Enums.MatchStatus.Match;
                                    break;

                                // Case more than one
                                default:
                                    // take first one - standards should be sorted from most used to least used
                                    componentDI.StandardId = possibleStdDIAOI.ElementAt(0).Id;
                                    componentDI.MatchStatus = Enums.MatchStatus.Check;
                                    io.MatchStatus = Enums.MatchStatus.Check;
                                    break;
                            }


                            xmlDB.Components.Add(componentDI);

                            break;
                        //=====================================================================================
                        case Enums.ConnectionType.DO:
                            // Create single component

                            var componentDO = new Component();
                            componentDO.IOId = io.Id;
                            componentDO.Name = io.Name;
                            componentDO.Comment = io.Comment;
                            componentDO.Location = io.Location;
                            componentDO.Dependancy = Enums.Dependancy.Single;
                            componentDO.ConnectionType = io.ConnectionType;

                            //Matching
                            var possibleStdDOAOI = xmlDB.Standards.Where(c => c.ConnectionType == io.ConnectionType).ToList();

                            switch (possibleStdDOAOI.Count())
                            {
                                case (0):
                                    componentDO.StandardId = null;
                                    componentDO.MatchStatus = Enums.MatchStatus.No_Match;
                                    io.MatchStatus = Enums.MatchStatus.No_Match;
                                    break;

                                case (1):
                                    componentDO.StandardId = possibleStdDOAOI.ElementAt(0).Id;
                                    componentDO.MatchStatus = Enums.MatchStatus.Match;
                                    io.MatchStatus = Enums.MatchStatus.Match;
                                    break;

                                // Case more than one
                                default:
                                    // take first one - standards should be sorted from most used to least used
                                    componentDO.StandardId = possibleStdDOAOI.ElementAt(0).Id;
                                    componentDO.MatchStatus = Enums.MatchStatus.Check;
                                    io.MatchStatus = Enums.MatchStatus.Check;
                                    break;
                            }


                            xmlDB.Components.Add(componentDO);

                            break;
                        //=====================================================================================
                        case Enums.ConnectionType.ETH:
                            //ETH and DIO reserved for parent components
                            throw new Exception("ETH and DIO connection types reserved for parent components");
                        case Enums.ConnectionType.DIO:
                            //ETH and DIO reserved for parent components
                            throw new Exception("ETH and DIO connection types reserved for parent components");
                        default:
                            throw new Exception("Matching: Unknown connection type");

                    }

                    
                }
            }

            xmlDB.Save();

            //Fill up io.ComponentId
            foreach (var io in xmlDB.IOs)
            {
                var component = xmlDB.Components.FirstOrDefault(c => c.IOId == io.Id);
                //If component exist link with IO
                if (component != null)
                {
                    io.ComponentId = component.Id;
                }else
                {
                    //For child components
                    io.ComponentId = xmlDB.Components.First(c => c.Name == io.ParentName).Id;
                }
            }

            xmlDB.Save();
        }

    }
}
