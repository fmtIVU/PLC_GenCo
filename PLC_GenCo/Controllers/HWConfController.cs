using PLC_GenCo.Models;
using PLC_GenCo.ViewModels;
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
            var viewModel = new HWConfViewModel
            {
                Modules = _context.Modules.ToList(),
                Locations = _context.ComponentLocations.ToList(),
                PLC = _context.PLC.FirstOrDefault()
            };
            return View(viewModel);
        }
        //====================================================================================================================
        //UPLOAD FILE
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            var IOList = new List<IO>();
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

                    _context.IOs.Add(io);
                    _context.SaveChanges();
                }




            }

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

            var location = _context.ComponentLocations.SingleOrDefault(c => c.Id == id);

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

            if (location.Id == 1)
            {
                _context.ComponentLocations.Add(location);
            }
            else
            {
                var locationInDb = _context.ComponentLocations.SingleOrDefault(c => c.Id == location.Id);
                if (locationInDb == null)
                {
                    _context.ComponentLocations.Add(location);
                }
                else
                {
                    locationInDb.Name = location.Name;
                    locationInDb.Prefix = location.Prefix;
                }

            }
            _context.SaveChanges();

            return RedirectToAction("Index", "HWConf");
        }
        //====================================================================================================================
        //MODULE HANDLING
        //Call module form
        public ActionResult ModuleForm(int id)
        {

            var module = _context.Modules.SingleOrDefault(c => c.Id == id);

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


            var moduleInDb = _context.Modules.SingleOrDefault(c => c.Id == module.Id);
            if (moduleInDb == null)
            {
                
                module.Address = _context.Modules.Count();   //TODO if controller not first---- embDI + embDO = 2 rest start with 2

                _context.Modules.Add(module);
            }
            else
            {
                moduleInDb.Name = module.Name;
                moduleInDb.IOModulesType = module.IOModulesType;
                moduleInDb.Comments = module.Comments;
            }

            
            _context.SaveChanges();

            return RedirectToAction("Index", "HWConf");
        }
        //====================================================================================================================
        //PLC HANDLING
        //Call PLC form
        public ActionResult AddPLC()
        {
            if (_context.PLC.Count() < 1)
            {
                return View("PLCForm");
            }
            else
            {
                var viewModel = new PLCFormViewModel
                {
                    PLC = _context.PLC.FirstOrDefault()
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


            var PLCInDb = _context.PLC.FirstOrDefault();

            if (_context.PLC.Count() < 1)
            {
                _context.PLC.Add(PLC);
            }
            else
            {
                PLCInDb.Name = PLC.Name;
                PLCInDb.ProductType = PLC.ProductType;
                PLCInDb.Description = PLC.Description;
            }


            _context.SaveChanges();
            //------------------------------------------------------------------------
            //Add embedded IO modules

            if ((PLC.ProductType == Enums.ControllerType.L16ER || PLC.ProductType == Enums.ControllerType.L18ER) &&
                _context.Modules.Where(c => ((c.IOModulesType == Enums.IOModulesType.EmbDIx16) || (c.IOModulesType == Enums.IOModulesType.EmbDOx16))).Count() == 0)
            {
                var embDI = new Module {
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

                _context.Modules.Add(embDI);
                _context.Modules.Add(embDO);

            }

            _context.SaveChanges();

            return RedirectToAction("Index", "HWConf");
        }
        //====================================================================================================================
        //ADD LOCATIONS
        private void DetectLocations()
        {
            var locations = new List<ComponentLocation>();
            var IOs = _context.IOs.ToList();

            foreach (IO io in IOs)
            {
                if (_context.ComponentLocations.SingleOrDefault(c => c.Name == io.Location) == null && !String.IsNullOrEmpty(io.Location)) //Add location if it doesnt exist and if it is not empty or null
                {
                    _context.ComponentLocations.Add(new ComponentLocation { Name = io.Location });
                    _context.SaveChanges();
                }
            }

            return;
        }

        //==========================================================================================================
        //Match IO to Standard
        private void CreateComponents()
        {

            foreach (var io in _context.IOs.ToList())
            {

                if (!String.IsNullOrEmpty(io.ParentName))
                {
                    //IO is part of parent component
                    io.MatchStatus = Enums.MatchStatus.Check;

                    //If parent component doesnt exist create it
                    if (!_context.Components.Any(c => c.Name == io.ParentName))
                    {
                        var component = new Component();
                        component.IOId = io.Id;
                        component.Name = io.ParentName;
                        component.Comment = io.Comment;
                        component.Location = io.Location;
                        component.Depandancy = Enums.Dependancy.Parent;

                        component.MatchStatus = Enums.MatchStatus.No_Match;

                        if (io.IOAddress.Type == Enums.IOType.IO)
                        {
                            component.ConnectionType = Enums.ConnectionType.DIO;
                        }
                        else
                        {
                            component.ConnectionType = Enums.ConnectionType.ETH;
                        }


                        try
                        {
                            _context.Components.Add(component);
                            _context.SaveChanges();
                        }
                        catch (DbEntityValidationException dbEx)
                        {
                            foreach (var validationErrors in dbEx.EntityValidationErrors)
                            {
                                foreach (var validationError in validationErrors.ValidationErrors)
                                {
                                    Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                                }
                            }

                        }


                    }else
                    {
                        //if it exist check connection type - if any child IO is IP(ETH) type -> switch components connection type
                        if (io.IOAddress.Type == Enums.IOType.IP)
                        {
                            _context.Components.Single(c => c.Name == io.ParentName).ConnectionType = Enums.ConnectionType.ETH;
                            _context.SaveChanges();
                        }
                    }

                    //Link IO to parent component
                    io.ComponentId = _context.Components.Single(c => c.Name == io.ParentName).Id;
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
                            componentAI.Depandancy = Enums.Dependancy.Single;
                            componentAI.ConnectionType = io.ConnectionType;

                            //Matching
                            var possibleStdAIAOI = _context.Standards.Where(c => c.ConnectionType == io.ConnectionType).ToList();

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

                            _context.Components.Add(componentAI);
                            _context.SaveChanges();

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
                            componentDI.Depandancy = Enums.Dependancy.Single;
                            componentDI.ConnectionType = io.ConnectionType;

                            //Matching
                            var possibleStdDIAOI = _context.Standards.Where(c => c.ConnectionType == io.ConnectionType).ToList();

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


                            _context.Components.Add(componentDI);
                            _context.SaveChanges();


                            break;
                        //=====================================================================================
                        case Enums.ConnectionType.DO:
                            // Create single component

                            var componentDO = new Component();
                            componentDO.IOId = io.Id;
                            componentDO.Name = io.Name;
                            componentDO.Comment = io.Comment;
                            componentDO.Location = io.Location;
                            componentDO.Depandancy = Enums.Dependancy.Single;
                            componentDO.ConnectionType = io.ConnectionType;

                            //Matching
                            var possibleStdDOAOI = _context.Standards.Where(c => c.ConnectionType == io.ConnectionType).ToList();

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


                            _context.Components.Add(componentDO);
                            _context.SaveChanges();

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

                    io.ComponentId = _context.Components.First(c => c.IOId == io.Id).Id;
                    _context.SaveChanges();
                }




            }
        }
    }
}
