using Microsoft.AspNet.Identity;
using PLC_GenCo.Models;
using PLC_GenCo.ViewModels;
using PLC_GenCo.XMLDB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace PLC_GenCo.Controllers
{
    public class IOListController : Controller
    {
        private ApplicationDbContext _context;

        public IOListController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        //==========================================================================================================
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

            var viewModel = new IOListViewModel
            {
                IOs = xmlDB.IOs,
                Components = xmlDB.Components,
                Standards = xmlDB.Standards,
                PageName = pageName
                
            };

            return View(viewModel);
        }
        //==========================================================================================================
        //Call form add/edit io
        public ActionResult IOForm(int id)
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

            var io = xmlDB.IOs.SingleOrDefault(c => c.Id == id);

            if (io == null)
                return HttpNotFound();

            var viewModel = new EditIOIOListViewModel
            {
                IO = io,
                IOLocations = xmlDB.Locations,
                Parents = xmlDB.Components.Where(c => c.Dependancy == Enums.Dependancy.Parent),
                PageName = pageName
            };

            return View("IOForm", viewModel);
        }
        //Save new/edited IO
        [HttpPost]
        public ActionResult Save(IO io)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            //Load page name
            String pageName;
            if (String.IsNullOrEmpty(userName))
            {
                pageName = "";
            }
            else
            {
                pageName = _context.Users.First(c => c.Name == userName).ActProject;
            }

            if (!ModelState.IsValid)
            {

                var viewModel = new EditIOIOListViewModel
                {
                    IO = io,
                    IOLocations = xmlDB.Locations.ToList(),
                    Parents = xmlDB.Components.Where(c => c.Dependancy == Enums.Dependancy.Parent),
                    PageName = pageName
                };

                return View("IOForm", viewModel);

            }

            //Component created by user -> set to match
            io.MatchStatus = Enums.MatchStatus.Match;

            if (String.IsNullOrWhiteSpace(io.ParentName) || String.IsNullOrEmpty(io.ParentName))
            {
                // UPDATE/ADD Single IOs Component
                var componentInDb = xmlDB.Components.SingleOrDefault(c => c.IOId == io.Id);

                if (componentInDb == null)
                {
                    // Create single component

                    var component = new Component();
                    component.IOId = io.Id;
                    component.Name = io.Name;
                    component.Comment = io.Comment;
                    component.Location = io.Location;
                    component.Dependancy = Enums.Dependancy.Single;

                    component.MatchStatus = Enums.MatchStatus.No_Match;


                    xmlDB.Components.Add(component);
                }
                else
                {
                    //Update single component
                    componentInDb.Comment = io.Comment;
                    componentInDb.Location = io.Location;
                    componentInDb.Name = io.Name;
                }
            }else
            {
                // Update/Add parent component
                var componentInDb = xmlDB.Components.SingleOrDefault(c => c.Name == io.ParentName);

                if (componentInDb == null)
                {
                    // Create parent component

                    var component = new Component();
                    component.IOId = io.Id;
                    component.Name = io.Name;
                    component.Comment = io.Comment;
                    component.Location = io.Location;
                    component.Dependancy = Enums.Dependancy.Parent;

                    component.MatchStatus = Enums.MatchStatus.No_Match;


                    xmlDB.Components.Add(component);
                }

            }

            xmlDB.Save();

            //UPDATE/ADD IO 
            var IOInDb = xmlDB.IOs.SingleOrDefault(c => c.Id == io.Id);

            if (IOInDb == null)
            {
                xmlDB.IOs.Add(io);
            }
            else
            {
                IOInDb.Name = io.Name;
                IOInDb.Comment = io.Comment;
                IOInDb.Location = io.Location;
                IOInDb.IOAddress = io.IOAddress;
                IOInDb.ComponentId = io.ComponentId;
                IOInDb.ConnectionType = io.ConnectionType;
                IOInDb.MatchStatus = io.MatchStatus;

            }
            xmlDB.Save();

            //UPDATE IDs
            var ioIDUpdate = xmlDB.IOs.First(c => c.Name == io.Name);
            var ComponentIDUpdate = new Component();

            // Search for component when it is single/parent
            if (String.IsNullOrEmpty(io.ParentName))
            {
                ComponentIDUpdate = xmlDB.Components.First(c => c.Name == io.Name);
                //Update single components IO ID
                ComponentIDUpdate.IOId = ioIDUpdate.Id;
            }else
            {
                ComponentIDUpdate = xmlDB.Components.First(c => c.Name == io.ParentName);
                //Parent component has on IO ID because they are multiple
            }
            

            ioIDUpdate.ComponentId = ComponentIDUpdate.Id;


            xmlDB.Save();

            return RedirectToAction("Index", "IOList");
        }

    }
}
