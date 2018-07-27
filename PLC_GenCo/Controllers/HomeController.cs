using Microsoft.AspNet.Identity;
using PLC_GenCo.Models;
using PLC_GenCo.Models.Setups;
using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

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
            List<String> projects;
            var userName = User.Identity.GetUserName();
            String pageName;

            if (String.IsNullOrEmpty(userName))
            {
                projects = new List<String>();
                pageName = "";
            }
            else
            {
                projects = System.IO.Directory.GetDirectories(@"C:\Users\Ivan\Desktop\OP generator PLC koda\Profiles\" + userName).ToList();
                pageName = _context.Users.First(c => c.Name == userName).ActProject;
            }

            for (int i = 0; i < projects.Count(); i++)
            {
                projects[i] = projects[i].Split('\\').Last();
            }

            var viewModel = new IndexHomeViewModel
            {
                Projects = projects,
                UserName = userName,
                PageName = pageName,
            };


            return View(viewModel);
        }

        public ActionResult New(IndexHomeViewModel viewModel)
        {
            var userName = User.Identity.GetUserName();
            System.IO.Directory.CreateDirectory(@"C:\Users\Ivan\Desktop\OP generator PLC koda\Profiles\" + userName + @"\" + viewModel.ProjectName);
            System.IO.Directory.CreateDirectory(@"C:\Users\Ivan\Desktop\OP generator PLC koda\Profiles\" + userName + @"\" + viewModel.ProjectName + @"\Standards");

            var XMLProject = new XElement("Project",
                new XAttribute("Name", viewModel.ProjectName),
                new XAttribute("ComponentId", "1"),
                new XAttribute("StandardId", "1"),
                new XAttribute("ModuleId", "1"),
                new XAttribute("LocationId", "1"),
                new XAttribute("IOId", "1"),
               

                new XElement("PLC"),
                new XElement("Components"),
                new XElement("Locations"),
                new XElement("IOs"),
                new XElement("Modules"),
                new XElement("Standards")
                );

            XMLProject.Save(@"C:\Users\Ivan\Desktop\OP generator PLC koda\Profiles\" + userName + @"\" + viewModel.ProjectName + @"\" + viewModel.ProjectName + @".xml");

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Delete(string name)
        {
            var userName = User.Identity.GetUserName();
            System.IO.Directory.Delete(@"C:\Users\Ivan\Desktop\OP generator PLC koda\Profiles\" + userName + @"\" + name, true);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Open(string name)
        {
            var userName = User.Identity.GetUserName();
            var user = _context.Users.First(c => c.Name == userName);
            user.ActProject = name;
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Close()
        {
            var userName = User.Identity.GetUserName();
            var user = _context.Users.First(c => c.Name == userName);
            user.ActProject = null;
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        //====================================================================================================================
        //CONTACT PAGE
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //====================================================================================================================
       
    }
}