using PLC_GenCo.Models;
using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLC_GenCo.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        //--------------------------------------------------------
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ResetDatabase()
        {
    

            foreach (Component component in _context.Components.ToList())
            {
                _context.Components.Remove(component);
            }
            _context.SaveChanges();

            foreach (IO io in _context.IOs.ToList())
            {
                _context.IOs.Remove(io);
            }
            _context.SaveChanges();

            foreach (ComponentLocation location in _context.ComponentLocations.ToList())
            {
                _context.ComponentLocations.Remove(location);
            }
            _context.SaveChanges();

            foreach (Module module in _context.Modules.ToList())
            {
                _context.Modules.Remove(module);
            }
            _context.SaveChanges();

            foreach (PLC PLC in _context.PLC.ToList())
            {
                _context.PLC.Remove(PLC);
            }
            _context.SaveChanges();

            return Content("Done!");
        }
    }
}