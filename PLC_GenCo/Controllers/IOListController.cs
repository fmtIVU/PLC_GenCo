using PLC_GenCo.Models;
using PLC_GenCo.ViewModels;
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

            var viewModel = new IOListViewModel
            {
                IOs = _context.IOs.ToList(),
                Components = _context.Components.ToList(),
                Standards = _context.Standards.ToList()

            };

            return View(viewModel);
        }
        //==========================================================================================================
        //Call form add/edit io
        public ActionResult IOForm(int id)
        {

            var io = _context.IOs.SingleOrDefault(c => c.Id == id);

            if (io == null)
                return HttpNotFound();

            var viewModel = new EditIOIOListViewModel
            {
                IO = io,
                IOLocations = _context.ComponentLocations.ToList(),
                Parents = _context.Components.Where(c => c.Depandancy == Enums.Dependancy.Parent)
            };

            return View("IOForm", viewModel);
        }
        //Save new/edited IO
        [HttpPost]
        public ActionResult Save(IO io)
        {
            if (!ModelState.IsValid)
            {

                var viewModel = new EditIOIOListViewModel
                {
                    IO = io,
                    IOLocations = _context.ComponentLocations.ToList(),
                    Parents = _context.Components.Where(c => c.Depandancy == Enums.Dependancy.Parent)
                };

                return View("IOForm", viewModel);

            }
            //Component created by user -> set to match
            io.MatchStatus = Enums.MatchStatus.Match;

            if (String.IsNullOrEmpty(io.ParentName))
            {
                // UPDATE/ADD Single IOs Component
                var componentInDb = _context.Components.SingleOrDefault(c => c.IOId == io.Id);

                if (componentInDb == null)
                {
                    // Create single component

                    var component = new Component();
                    component.IOId = io.Id;
                    component.Name = io.Name;
                    component.Comment = io.Comment;
                    component.Location = io.Location;
                    component.Depandancy = Enums.Dependancy.Single;

                    component.MatchStatus = Enums.MatchStatus.No_Match;


                    _context.Components.Add(component);
                    _context.SaveChanges();
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
                var componentInDb = _context.Components.SingleOrDefault(c => c.Name == io.ParentName);

                if (componentInDb == null)
                {
                    // Create parent component

                    var component = new Component();
                    component.IOId = io.Id;
                    component.Name = io.Name;
                    component.Comment = io.Comment;
                    component.Location = io.Location;
                    component.Depandancy = Enums.Dependancy.Parent;

                    component.MatchStatus = Enums.MatchStatus.No_Match;


                    _context.Components.Add(component);
                    _context.SaveChanges();
                }

            }
            

            //UPDATE/ADD IO 
            var IOInDb = _context.IOs.SingleOrDefault(c => c.Id == io.Id);

            if (IOInDb == null)
            {
                _context.IOs.Add(io);
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
            _context.SaveChanges();

            //UPDATE IDs
            var ioIDUpdate = _context.IOs.First(c => c.Name == io.Name);
            var ComponentIDUpdate = new Component();

            // Search for component when it is single/parent
            if (String.IsNullOrEmpty(io.ParentName))
            {
                ComponentIDUpdate = _context.Components.First(c => c.Name == io.Name);
                //Update single components IO ID
                ComponentIDUpdate.IOId = ioIDUpdate.Id;
            }else
            {
                ComponentIDUpdate = _context.Components.First(c => c.Name == io.ParentName);
                //Parent component has on IO ID because they are multiple
            }
            

            ioIDUpdate.ComponentId = ComponentIDUpdate.Id;
            

            _context.SaveChanges();

            return RedirectToAction("Index", "IOList");
        }

    }
}
