using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLC_GenCo.Controllers
{
    public class StandardsController : Controller
    {
        private ApplicationDbContext _context;

        public StandardsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ViewResult Index()
        {
            var standards = _context.Standards.ToList(); 

            return View(standards);
        }

        public ActionResult Details(int id)
        {
            var standard = _context.Standards.SingleOrDefault(c => c.Id == id);

            if (standard == null)
                return HttpNotFound();

            return View(standard);
        }

        

    }
}