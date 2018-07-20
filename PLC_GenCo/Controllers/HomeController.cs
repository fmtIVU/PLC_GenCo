using PLC_GenCo.Models;
using PLC_GenCo.Models.Setups;
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
        //====================================================================================================================
        //MAIN PAGE
        public ActionResult Index()
        {
            return View();
        }
        //====================================================================================================================
        //CONTACT PAGE
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //====================================================================================================================
        //RESET DATABASE COMMAND
        public ActionResult ResetDatabase()
        {
    
            //Delete components
            foreach (Component component in _context.Components.ToList())
            {
                _context.Components.Remove(component);
            }
            _context.SaveChanges();
            //Delete IOs
            foreach (IO io in _context.IOs.ToList())
            {
                _context.IOs.Remove(io);
            }
            _context.SaveChanges();
            //Delete locations
            foreach (ComponentLocation location in _context.ComponentLocations.ToList())
            {
                _context.ComponentLocations.Remove(location);
            }
            _context.SaveChanges();
            //Delete modules
            foreach (Module module in _context.Modules.ToList())
            {
                _context.Modules.Remove(module);
            }
            _context.SaveChanges();
            //delete PLC
            foreach (PLC PLC in _context.PLC.ToList())
            {
                _context.PLC.Remove(PLC);
            }
            _context.SaveChanges();
            //======================================================================================================
            //DELETE SETUPS
            //Delete AI Alarm setups
            foreach (AIAlarmSetup AIAlarm in _context.AIAlarms.ToList())
            {
                _context.AIAlarms.Remove(AIAlarm);
            }
            _context.SaveChanges();
            //Delete DI Alarm setups
            foreach (DIAlarmSetup DIAlarm in _context.DIAlarms.ToList())
            {
                _context.DIAlarms.Remove(DIAlarm);
            }
            _context.SaveChanges();
            //Delete DI pulse setups
            foreach (DIPulseSetup DIPulse in _context.DIpulses.ToList())
            {
                _context.DIpulses.Remove(DIPulse);
            }
            _context.SaveChanges();
            //Delete motor single direction setups
            foreach (MDirSetup MDir in _context.MDirs.ToList())
            {
                _context.MDirs.Remove(MDir);
            }
            _context.SaveChanges();
            //Delete motor two direction setups
            foreach (MRevSetup MRev in _context.MRevs.ToList())
            {
                _context.MRevs.Remove(MRev);
            }
            _context.SaveChanges();
            //Delete motor with frequencyconverter setups
            foreach (MotFrqSetup MotFrq in _context.MotFrqs.ToList())
            {
                _context.MotFrqs.Remove(MotFrq);
            }
            _context.SaveChanges();
            //Delete mstandard valve setups
            foreach (StdVlvSetup StdVlv in _context.StdVlvs.ToList())
            {
                _context.StdVlvs.Remove(StdVlv);
            }
            _context.SaveChanges();

            return Content("Done!");
        }
    }
}