using Microsoft.AspNet.Identity;
using PLC_GenCo.Generator;
using PLC_GenCo.Models;
using PLC_GenCo.Models.Setups;
using PLC_GenCo.ViewModels;
using PLC_GenCo.ViewModels.Setups;
using PLC_GenCo.XMLDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using static PLC_GenCo.ViewModels.Enums;

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
        //====================================================================================================================
        // MAIN COMPONENT SETUP PAGE
        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            //Load pagename
            String pageName;
            if (String.IsNullOrEmpty(userName))
            {
                pageName = "";
            }
            else
            {
                pageName = _context.Users.First(c => c.Name == userName).ActProject;
            }

            var viewModel = new FactoryViewModel
            {
                Components = xmlDB.Components.ToList(),
                IOs = xmlDB.IOs.ToList(),
                //AIAlarms = xmlDB.AIAlarms.ToList(),
                //DIAlarms = xmlDB.DIAlarms.ToList(),
                Standnards = xmlDB.Standards.ToList(),
                PageName = pageName
            };

            return View(viewModel);
        }
        //====================================================================================================================
        // ADD COMPONENT
        public ActionResult AddComponent()
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            //Load pagename
            String pageName;
            if (String.IsNullOrEmpty(userName))
            {
                pageName = "";
            }
            else
            {
                pageName = _context.Users.First(c => c.Name == userName).ActProject;
            }

            var addComponentFactoryViewModel = new AddComponentFactoryViewModel()
            {
                ComponentLocations = xmlDB.Locations.ToList(),
                Standards = xmlDB.Standards.ToList(),
                PageName = pageName

            };

            return View("ComponentForm", addComponentFactoryViewModel);
        }
        //====================================================================================================================
        // SAVE NEW/EDITED COMPONENT
        [HttpPost]
        public ActionResult Save(Component component)
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

            // Input validation TODO


            //If standard is changed
            if (component.StandardId != null && component.StandardId != 0)
            {
                component.ConnectionType = xmlDB.Standards.First(c => c.Id == component.StandardId).ConnectionType;
                component.MatchStatus = Enums.MatchStatus.Match;
            }
            
            //Add/Update component
            if (component.Id == 0)
            {
                xmlDB.Components.Add(component);
            }
            else
            {
                var componentInDb = xmlDB.Components.SingleOrDefault(c => c.Id == component.Id);

                if (componentInDb == null)
                {
                    xmlDB.Components.Add(component);
                }
                else
                {
                    componentInDb.Name = component.Name;
                    componentInDb.Comment = component.Comment;
                    componentInDb.Location = component.Location;
                    componentInDb.Dependancy = component.Dependancy;
                    componentInDb.IOId = component.IOId;
                    componentInDb.StandardId = component.StandardId;
                    componentInDb.MatchStatus = component.MatchStatus;
                    componentInDb.ConnectionType = component.ConnectionType;

                    if (componentInDb.Setup != null)
                    {
                        //Update parameter values (setup)
                        for (int i = 0; i < componentInDb.Setup.Parameters.Count; i++)
                        {
                            if (component.Setup.Parameters[i] != null)
                            {

                                //Choose from which AuxValue take value
                                if (componentInDb.Setup.Parameters[i].Type == ParType.Parameter)
                                {//Case Parameter

                                    if (componentInDb.Setup.Parameters[i].DataType == DataType.BOOL)
                                    {//Case Parameter - BOOL

                                        componentInDb.Setup.Parameters[i].Value = component.Setup.Parameters[i].AuxValueBOOL ? "1" : "0";

                                    }

                                    if (componentInDb.Setup.Parameters[i].DataType == DataType.REAL)
                                    {//Case Parameter - Real
                                        if (component.Setup.Parameters[i].AuxValueFLOAT == null)
                                        {
                                            componentInDb.Setup.Parameters[i].Value = componentInDb.Setup.Parameters[i].DefaultValue;
                                        }
                                        else
                                        {
                                            componentInDb.Setup.Parameters[i].Value = component.Setup.Parameters[i].AuxValueFLOAT.ToString();
                                        }
                                    }

                                }
                                else
                                {//case IO

                                    if (component.Setup.Parameters[i].AuxValueINT == null)
                                    {
                                        componentInDb.Setup.Parameters[i].Value = componentInDb.Setup.Parameters[i].DefaultValue;
                                    }
                                    else
                                    {
                                        componentInDb.Setup.Parameters[i].Value = component.Setup.Parameters[i].AuxValueINT.ToString();
                                    }

                                }
                            }
                        }
                    }
                    
                    
                }

            }

            //Set all components childs to Match
            foreach (var io in xmlDB.IOs.Where(c=>c.ComponentId == component.Id))
            {
                io.MatchStatus = Enums.MatchStatus.Match;
            }
            xmlDB.Save();


            return RedirectToAction("Index", "IOList");

        }
        //====================================================================================================================
        // EDIT COMPONENT
        public ActionResult Edit(int id)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            //Load pagename
            String pageName;
            if (String.IsNullOrEmpty(userName))
            {
                pageName = "";
            }
            else
            {
                pageName = _context.Users.First(c => c.Name == userName).ActProject;
            }

            var component = xmlDB.Components.SingleOrDefault(c => c.Id == id);

            if (component == null)
                return HttpNotFound();

            //Two cases - setup component with and without standard
            if (component.StandardId != null && component.StandardId != 0)
            {
                //Convert String Value to AuxValueX
                foreach (var parameter in component.Setup.Parameters.ToList())
                {
                    //Do that only for IO and Parameter Types
                    if (parameter.Type != ParType.IO && parameter.Type != ParType.Parameter)
                        continue;

                    if (parameter.Type == ParType.Parameter)
                    {//Case Parameter
                        if (parameter.DataType == DataType.BOOL)
                        {
                            parameter.AuxValueBOOL = Convert.ToBoolean(Convert.ToInt32(parameter.Value));
                        }else
                        {
                            if (parameter.Value == "TODO")
                            {
                                parameter.AuxValueFLOAT = null;
                            }
                            else
                            {
                                parameter.AuxValueFLOAT = (float)Convert.ToDouble(parameter.Value);
                            }
                            
                        }

                    }else
                    {//case IO
                        if (parameter.Value == "TODO" || parameter.Value == "AlwaysOFF" || parameter.Value == "AlwaysON")
                        {
                            parameter.AuxValueINT = null;
                        }else
                        {
                            parameter.AuxValueINT = Convert.ToInt32(parameter.Value);
                        }
                        
                    }

                }

            }
            
            var viewModel = new AddComponentFactoryViewModel
            {
                Component = component,
                ComponentLocations = xmlDB.Locations.ToList(),
                Standards = xmlDB.Standards,
                DIChilds = xmlDB.IOs.Where(c => (c.ConnectionType == Enums.ConnectionType.DI && c.ComponentId == component.Id)).ToList(),
                DOChilds = xmlDB.IOs.Where(c => (c.ConnectionType == Enums.ConnectionType.DO && c.ComponentId == component.Id)).ToList(),
                AIChilds = xmlDB.IOs.Where(c => (c.ConnectionType == Enums.ConnectionType.AI && c.ComponentId == component.Id)).ToList(),
                AOChilds = xmlDB.IOs.Where(c => (c.ConnectionType == Enums.ConnectionType.AO && c.ComponentId == component.Id)).ToList(),
                PageName = pageName
            };

            return View("ComponentForm", viewModel);

        }

        [HttpPost]
        public ActionResult Approve(int stdId, int IOId)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            var ioInDb = xmlDB.IOs.First(c => c.Id == IOId);
            var componentInDb = xmlDB.Components.First(c => c.Id == ioInDb.ComponentId);

            componentInDb.StandardId = stdId;
            ioInDb.MatchStatus = Enums.MatchStatus.Match;
            componentInDb.MatchStatus = Enums.MatchStatus.Match;

            xmlDB.Save(); 

            return RedirectToAction("Index", "IOList");
        }
    }
}