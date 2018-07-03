using PLC_GenCo.Models;
using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
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


        public ActionResult Index()
        {

            var viewModel = new IOListViewModel
            {
                IOs = _context.IOs.ToList(),
                Components = _context.Components.ToList()
            };

            return View(viewModel);
        }

        public ActionResult IOForm(int id)
        {

            var io = _context.IOs.SingleOrDefault(c => c.Id == id);

            if (io == null)
                return HttpNotFound();

            var viewModel = new EditIOIOListViewModel
            {
                IO = io,
                IOLocations = _context.ComponentLocations.ToList(),
                Parents = _context.Components.Where(c => c.IsParent == true)
            };

            return View("IOForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(IO io)
        {
            if (!ModelState.IsValid)
            {

                var viewModel = new EditIOIOListViewModel
                {
                    IO = io,
                    IOLocations = _context.ComponentLocations.ToList(),
                    Parents = _context.Components.Where(c => c.IsParent == true)
                };

                return View("IOForm", viewModel);

            }

            io.MatchStatus = Enums.MatchStatus.Match;
            if (io.Id == 1)
            {
                _context.IOs.Add(io);
                _context.Components.Add(new Component
                {
                    Name = io.Name,
                    Comment = io.Comment,
                    Location = io.Location,
                    ConnectionType = io.ConnectionType,
                    StandardComponent = io.Standard,
                    IOId = io.Id

                });
            }
            else
            {
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
                    IOInDb.Parent = io.Parent;
                    IOInDb.PLCAddress = io.PLCAddress;
                    IOInDb.Standard = io.Standard;
                    IOInDb.ConnectionType = io.ConnectionType;
                    IOInDb.MatchStatus = io.MatchStatus;

                }
                var ComponentInDb = _context.Components.SingleOrDefault(c => c.IOId == io.Id);

                if (ComponentInDb == null)
                {
                    _context.Components.Add(new Component
                    {
                        Name = io.Name,
                        Comment = io.Comment,
                        Location = io.Location,
                        ConnectionType = io.ConnectionType,
                        StandardComponent = io.Standard,
                        IOId = io.Id

                    });
                }
                else
                {
                    ComponentInDb.Name = io.Name;
                    ComponentInDb.Comment = io.Comment;
                    ComponentInDb.Location = io.Location;
                    ComponentInDb.ConnectionType = io.ConnectionType;
                    ComponentInDb.StandardComponent = io.Standard;
                    ComponentInDb.IOId = io.Id;

                }

            }
            _context.SaveChanges();

            return RedirectToAction("Index", "IOList");
        }
        //--------------------------------------------------------------------------------------------------------
        public ActionResult MatchToStandards()
        {
            CreateParentComponents();
            MatchIOToStandard();
            return RedirectToAction("Index", "IOList");
        }

        private void CreateParentComponents()
        {
            //Detect components with same name

            foreach (var io in _context.IOs.ToList())
            {
                if (_context.IOs.Where(c => c.Name == io.Name).Count() > 1  && !_context.Components.Any(c => c.Name == io.Name))
                {
                    var newComponent = new Component
                    {
                        Name = io.Name,
                        Comment = io.Name,
                        Location = io.Location,
                        IsParent = true
                    };

                    if (io.ConnectionType == Enums.ConnectionType.ETH_AI ||
                        io.ConnectionType == Enums.ConnectionType.ETH_DI ||
                        io.ConnectionType == Enums.ConnectionType.ETH_AO ||
                        io.ConnectionType == Enums.ConnectionType.ETH_DO || 
                        ((_context.IOs.Any( c=> c.ConnectionType == Enums.ConnectionType.ETH_AI && c.Name == io.Name) ||
                        _context.IOs.Any(c => c.ConnectionType == Enums.ConnectionType.ETH_DI && c.Name == io.Name) ||
                        _context.IOs.Any(c => c.ConnectionType == Enums.ConnectionType.ETH_DO && c.Name == io.Name) ||
                        _context.IOs.Any(c => c.ConnectionType == Enums.ConnectionType.ETH_AO && c.Name == io.Name))))
                    {
                        newComponent.ConnectionType = Enums.ConnectionType.ETH; 
                    }
                    else
                    {
                        newComponent.ConnectionType = Enums.ConnectionType.DIO;
                    }
                    _context.Components.Add(newComponent);
                }

                _context.SaveChanges();
            }

        }


        private void MatchIOToStandard()
        {

            foreach (var io in _context.IOs.ToList())
            {
                var nameCount = _context.IOs.Where(c => c.Name == io.Name).Count();
            

                switch (io.ConnectionType)
                {
                    case Enums.ConnectionType.AI:
                        if ( nameCount > 1)
                        {
                            io.Standard = Enums.StandardComponent.C_AI;         //AI input is child compoonent
                            io.Parent = _context.Components.Single(c => c.Name == io.Name).Id;
                        }
                        else
                        {
                            io.Standard = Enums.StandardComponent.AI_Alarm;     // AI input is Single component
                            _context.Components.Add(new Component {
                                Name = io.Name,
                                Comment = io.Comment,
                                Location = io.Location,
                                ConnectionType = io.ConnectionType,
                                StandardComponent = io.Standard,
                                IOId = io.Id
                                
                            });
                        }
                        io.MatchStatus = Enums.MatchStatus.Match;
                        _context.SaveChanges();

                        break;
                    case Enums.ConnectionType.AO:
                            io.Standard = Enums.StandardComponent.C_AO;         //AI input is child compoonent
                            io.Parent = _context.Components.Single(c => c.Name == io.Name).Id;
                            io.MatchStatus = Enums.MatchStatus.Match;
                        break;
                    case Enums.ConnectionType.DI:
                        if (nameCount > 1)
                        {
                            io.Standard = Enums.StandardComponent.C_DI;         //DI is child compoonent
                            io.Parent = _context.Components.Single(c => c.Name == io.Name).Id;
                            io.MatchStatus = Enums.MatchStatus.Match;
                        }
                        else
                        {
                            io.Standard = Enums.StandardComponent.DI_Alarm;     // DI is Single component
                            io.MatchStatus = Enums.MatchStatus.Check;
                            _context.Components.Add(new Component
                            {
                                Name = io.Name,
                                Comment = io.Comment,
                                Location = io.Location,
                                ConnectionType = io.ConnectionType,
                                StandardComponent = io.Standard,
                                IOId = io.Id

                            });
                        }
                        
                        _context.SaveChanges();
                        break;
                    case Enums.ConnectionType.DO:
                        if (nameCount > 1)
                        {
                            io.Standard = Enums.StandardComponent.C_DO;         //DO is child compoonent
                            io.Parent = _context.Components.Single(c => c.Name == io.Name).Id;
                        }
                        else
                        {
                            io.Standard = Enums.StandardComponent.DO;           // DO is Single component
                            _context.Components.Add(new Component
                            {
                                Name = io.Name,
                                Comment = io.Comment,
                                Location = io.Location,
                                ConnectionType = io.ConnectionType,
                                StandardComponent = io.Standard,
                                IOId = io.Id

                            });
                        }
                        io.MatchStatus = Enums.MatchStatus.Match;
                        _context.SaveChanges();
                        break;
                    case Enums.ConnectionType.ETH:
                        io.Parent = _context.Components.Single(c => c.Name == io.Name).Id;
                        break;
                    case Enums.ConnectionType.DIO:
                        io.Parent = _context.Components.Single(c => c.Name == io.Name).Id;
                        break;
                    case Enums.ConnectionType.ETH_AI:
                        if (nameCount > 1)
                        {
                            io.Standard = Enums.StandardComponent.C_AI;         //AI input is child compoonent
                            io.Parent = _context.Components.Single(c => c.Name == io.Name).Id;
                        }
                        else
                        {
                            io.Standard = Enums.StandardComponent.AI_Alarm;     // AI input is Single component
                            _context.Components.Add(new Component
                            {
                                Name = io.Name,
                                Comment = io.Comment,
                                Location = io.Location,
                                ConnectionType = io.ConnectionType,
                                StandardComponent = io.Standard,
                                IOId = io.Id

                            });
                        }
                        io.MatchStatus = Enums.MatchStatus.Match;
                        _context.SaveChanges();

                        break;
                    case Enums.ConnectionType.ETH_DI:
                        if (nameCount > 1)
                        {
                            io.Standard = Enums.StandardComponent.C_DI;         //AI input is child compoonent
                            io.Parent = _context.Components.Single(c => c.Name == io.Name).Id;
                        }
                        else
                        {
                            io.Standard = Enums.StandardComponent.DI_Alarm;     // AI input is Single component
                            _context.Components.Add(new Component
                            {
                                Name = io.Name,
                                Comment = io.Comment,
                                Location = io.Location,
                                ConnectionType = io.ConnectionType,
                                StandardComponent = io.Standard,
                                IOId = io.Id

                            });
                        }
                        io.MatchStatus = Enums.MatchStatus.Match;
                        _context.SaveChanges();

                        break;
                    case Enums.ConnectionType.ETH_DO:
                        if (nameCount > 1)
                        {
                            io.Standard = Enums.StandardComponent.C_DO;         //DO is child compoonent
                            io.Parent = _context.Components.Single(c => c.Name == io.Name).Id;
                        }
                        else
                        {
                            io.Standard = Enums.StandardComponent.DO;           // DO is Single component
                            _context.Components.Add(new Component
                            {
                                Name = io.Name,
                                Comment = io.Comment,
                                Location = io.Location,
                                ConnectionType = io.ConnectionType,
                                StandardComponent = io.Standard,
                                IOId = io.Id

                            });
                        }
                        io.MatchStatus = Enums.MatchStatus.Match;
                        _context.SaveChanges();
                        break;
                    case Enums.ConnectionType.ETH_AO:
                        io.Standard = Enums.StandardComponent.C_AO;         //AI input is child compoonent
                        io.Parent = _context.Components.Single(c => c.Name == io.Name).Id;
                        io.MatchStatus = Enums.MatchStatus.Match;
                        break;

                    default:
                        throw new Exception("Matching: Unknown connection type");
                    
                }

            }
        }

    }
}
