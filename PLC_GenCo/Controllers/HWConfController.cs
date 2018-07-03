using PLC_GenCo.Models;
using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
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
                
                module.ModuleAddress = _context.Modules.Count();   //TODO if controller not first---- embDI + embDO = 2 rest start with 2

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
                    ModuleAddress = 1
                    };
                var embDO = new Module
                {
                    Name = "PLC_Emb_DO",
                    IOModulesType = Enums.IOModulesType.EmbDOx16,
                    ModuleAddress = 1
                };

                _context.Modules.Add(embDI);
                _context.Modules.Add(embDO);

            }

            _context.SaveChanges();

            return RedirectToAction("Index", "HWConf");
        }
        //-----------------------------------------------------------------------------------------------------------
        public ActionResult AddLocation()           //Add Location
        {

            return View("LocationForm");
        }
        public ActionResult AddModule()           //Add Module
        {

            return View("ModuleForm");
        }
        public ActionResult AddPLC()           //Add Location
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
        //-----------------------------------------------------------------------------------------------------------
        public ActionResult LocationForm(int id) //Edit location
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

        public ActionResult ModuleForm(int id) //Edit module
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
        //-----------------------------------------------------------------------------------------------------------
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
                var ms = new MemoryStream();
                
                file.InputStream.CopyTo(ms);
                byte[] array = ms.GetBuffer();
                string fileAsString = Encoding.UTF8.GetString(array);

                String[] CSVrows = fileAsString.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Skip(1).ToArray();

                

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
                        case ("ETH_DI"):
                            io.ConnectionType = Enums.ConnectionType.ETH_DI;
                            break;
                        case ("ETH_AI"):
                            io.ConnectionType = Enums.ConnectionType.ETH_AI;
                            break;
                        case ("ETH_DO"):
                            io.ConnectionType = Enums.ConnectionType.ETH_DO;
                            break;
                        case ("ETH_AO"):
                            io.ConnectionType = Enums.ConnectionType.ETH_AO;
                            break;
                        case ("DIO"):
                            io.ConnectionType = Enums.ConnectionType.DIO;
                            break;
                        default:
                            throw new Exception("Error reading PLC IO type");
                    }

                    io.PLCAddress = new PLCAddress(IOmembers[2]);
                    io.Name = IOmembers[3];
                    io.Comment = IOmembers[4];
                    
                    _context.IOs.Add(io);
                    _context.SaveChanges();
                }

                
                 
       
            }
            DetectLocations(); //Add locations to database
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");
        }
        //-----------------------------------------------------------------------------------------------------------
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

    }
}
