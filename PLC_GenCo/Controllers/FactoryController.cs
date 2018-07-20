using PLC_GenCo.Generator;
using PLC_GenCo.Models;
using PLC_GenCo.Models.Setups;
using PLC_GenCo.ViewModels;
using PLC_GenCo.ViewModels.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

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

            var viewModel = new FactoryViewModel
            {
                Components = _context.Components.ToList(),
                IOs = _context.IOs.ToList(),
                AIAlarms = _context.AIAlarms.ToList(),
                DIAlarms = _context.DIAlarms.ToList(),
                DIPulses = _context.DIpulses.ToList(),
                MDirs = _context.MDirs.ToList(),
                MRevs = _context.MRevs.ToList(),
                MotFrqs = _context.MotFrqs.ToList(),
                StdVlvs = _context.StdVlvs.ToList(),
                Standnards = _context.Standards.ToList()
            };

            return View(viewModel);
        }
        //====================================================================================================================
        // ADD COMPONENT
        public ActionResult AddComponent()
        {

            var addComponentFactoryViewModel = new AddComponentFactoryViewModel()
            {
                ComponentLocations = _context.ComponentLocations.ToList(),
                Standards = _context.Standards.ToList()

            };

            return View("ComponentForm", addComponentFactoryViewModel);
        }
        //====================================================================================================================
        // SAVE NEW/EDITED COMPONENT
        [HttpPost]
        public ActionResult Save(Component component)
        {
            if (!ModelState.IsValid && component.Id != 0 && component.ConnectionType != 0)
            {

                var viewModel = new AddComponentFactoryViewModel
                {
                    Component = component,
                    Standards = _context.Standards.ToList(),
                    ComponentLocations = _context.ComponentLocations.ToList(),
                };
                return View("ComponentForm", viewModel);

            }

            component.ConnectionType = _context.Standards.First(c => c.Id == component.StandardId).ConnectionType;
            component.MatchStatus = Enums.MatchStatus.Match;

            if (component.Id == 1)
            {
                _context.Components.Add(component);
            }
            else
            {
                var componentInDb = _context.Components.SingleOrDefault(c => c.Id == component.Id);
                if (componentInDb == null)
                {
                    _context.Components.Add(component);
                }
                else
                {
                    componentInDb.Name = component.Name;
                    componentInDb.Comment = component.Comment;
                    componentInDb.Location = component.Location;
                    componentInDb.Depandancy = component.Depandancy;
                    componentInDb.IOId = component.IOId;
                    componentInDb.StandardId = component.StandardId;
                    componentInDb.MatchStatus = component.MatchStatus;
                    componentInDb.ConnectionType = component.ConnectionType;
                }

            }
            foreach (var io in _context.IOs.Where(c=>c.ComponentId == component.Id))
            {
                io.MatchStatus = Enums.MatchStatus.Match;
            }
            _context.SaveChanges();


            return RedirectToAction("Index", "IOList");

        }
        //====================================================================================================================
        // EDIT COMPONENT
        public ActionResult Edit(int id)
        {

            var component = _context.Components.SingleOrDefault(c => c.Id == id);

            if (component == null)
                return HttpNotFound();

            var viewModel = new AddComponentFactoryViewModel
            {
                Component = component,
                ComponentLocations = _context.ComponentLocations.ToList(),
                Standards = _context.Standards.Where(c=>c.ConnectionType == component.ConnectionType)
            };

            return View("ComponentForm", viewModel);
        }
        //====================================================================================================================
        //====================================================================================================================
        //CALL COMPONENT SETUP - REDIRECTING

        public ActionResult Setup(int id)
        {
            var component = _context.Components.Single(c => c.Id == id);

            //In case clicking before choosing standard
            if (component.MatchStatus == Enums.MatchStatus.No_Match)
            {
                return Content("Select standard first!");
            }

            //IN case clicking on No standard
            if (!component.StandardId.HasValue)
            {
                return Content("No configuration for standard which doesn't exist :)");
            }


            var standard = _context.Standards.Single(c => c.Id == component.StandardId);

            switch (standard.AOIName)
            {
                case ("AlarmDi"):

                    var DIAlarmSetupInDb = _context.DIAlarms.SingleOrDefault(c => c.IdComponent == id);

                    DIAlarmSetupViewModel viewModelDIAlarm = new DIAlarmSetupViewModel
                    {
                        Component = _context.Components.SingleOrDefault(c => c.Id == id),
                    };

                    //IF it is edit use existing one
                    if (!(DIAlarmSetupInDb == null))
                    {
                        viewModelDIAlarm.DIAlarmSetup = DIAlarmSetupInDb;
                    }
                    else
                    {
                        viewModelDIAlarm.DIAlarmSetup = new DIAlarmSetup
                        {
                            IdComponent = component.Id,
                            IdIO = component.IOId,
                            Comment = component.Comment
                        };
                    }


                    return View("SetupDIAlarm", viewModelDIAlarm);
                //==============================================================================================
                case ("CNT"):

                    var DIPulseSetupInDb = _context.DIpulses.SingleOrDefault(c => c.IdComponent == id);

                    DIPulseSetupViewModel viewModelDIPulse = new DIPulseSetupViewModel
                    {
                        Component = _context.Components.SingleOrDefault(c => c.Id == id),
                    };

                    //IF its edit use existing one
                    if (!(DIPulseSetupInDb == null))
                    {
                        viewModelDIPulse.DIPulseSetup = DIPulseSetupInDb;
                    }
                    else
                    {
                        viewModelDIPulse.DIPulseSetup = new DIPulseSetup
                        {
                            IdComponent = component.Id,
                            IdIO = component.IOId
                        };
                    }

                    return View("SetupDIPulse", viewModelDIPulse);

                //=========================================================================================
                case ("Analog"):

                    var AIAlarmSetupInDb = _context.AIAlarms.SingleOrDefault(c => c.IdComponent == id);

                    AIAlarmSetupViewModel viewModelAIAlarm = new AIAlarmSetupViewModel
                    {
                        Component = _context.Components.SingleOrDefault(c => c.Id == id),
                    };

                    //IF its edit use existing one
                    if (!(AIAlarmSetupInDb == null))
                    {
                        viewModelAIAlarm.AIAlarmSetup = AIAlarmSetupInDb;
                    }
                    else
                    {
                        viewModelAIAlarm.AIAlarmSetup = new AIAlarmSetup
                        {
                            IdComponent = component.Id,
                            IdIO = component.IOId,
                            Comment = component.Comment
                        };
                    }

                    return View("SetupAIAlarm", viewModelAIAlarm);
                //=================================================================================================
                case ("MotorDir"):

                    if (standard.ConnectionType == Enums.ConnectionType.DIO)
                    {
                        var MDirSetupInDb = _context.MDirs.SingleOrDefault(c => c.IdComponent == id);

                        MDirSetupViewModel viewModelMDir = new MDirSetupViewModel
                        {
                            Component = _context.Components.SingleOrDefault(c => c.Id == id),
                            Childs = _context.IOs.Where(c => c.ComponentId == component.Id).ToList(),
                            DIAlarms = _context.DIAlarms.Where(c => c.IdComponent == component.Id).ToList(),
                            AIAlarms = _context.AIAlarms.Where(c => c.IdComponent == component.Id).ToList(),
                            DIAlarm = new DIAlarmSetup(),
                            AIAlarm = new AIAlarmSetup()
                        };

                        //IF its edit use existing one
                        if (!(MDirSetupInDb == null))
                        {
                            viewModelMDir.MDirSetup = MDirSetupInDb;
                        }
                        else
                        {
                            viewModelMDir.MDirSetup = new MDirSetup
                            {
                                IdComponent = component.Id,
                            };
                        }

                        return View("SetupMDir", viewModelMDir);
                    }
                    else
                    {
                        var MotFrqSetupInDb = _context.MotFrqs.SingleOrDefault(c => c.IdComponent == id);

                        MotFrqSetupViewModel viewModelMotFrq = new MotFrqSetupViewModel
                        {
                            Component = _context.Components.SingleOrDefault(c => c.Id == id),
                            Childs = _context.IOs.Where(c => c.ComponentId == component.Id).ToList(),
                            DIAlarms = _context.DIAlarms.Where(c => c.IdComponent == component.Id).ToList(),
                            AIAlarms = _context.AIAlarms.Where(c => c.IdComponent == component.Id).ToList(),
                            DIAlarm = new DIAlarmSetup(),
                            AIAlarm = new AIAlarmSetup()
                        };

                        //IF its edit use existing one
                        if (!(MotFrqSetupInDb == null))
                        {
                            viewModelMotFrq.MotFrqSetup = MotFrqSetupInDb;
                        }
                        else
                        {
                            viewModelMotFrq.MotFrqSetup = new MotFrqSetup
                            {
                                IdComponent = component.Id,
                            };
                        }

                        return View("SetupMotFrq", viewModelMotFrq);
                    }
                    
                //=====================================================================================================
                
                case ("MotorRev"):


                    if (standard.ConnectionType == Enums.ConnectionType.DIO)
                    {
                        var MRevSetupInDb = _context.MRevs.SingleOrDefault(c => c.IdComponent == id);

                        MRevSetupViewModel viewModelMRev = new MRevSetupViewModel
                        {
                            Component = _context.Components.SingleOrDefault(c => c.Id == id),
                            Childs = _context.IOs.Where(c => c.ComponentId == component.Id).ToList(),
                            DIAlarms = _context.DIAlarms.Where(c => c.IdComponent == component.Id).ToList(),
                            AIAlarms = _context.AIAlarms.Where(c => c.IdComponent == component.Id).ToList(),
                            DIAlarm = new DIAlarmSetup(),
                            AIAlarm = new AIAlarmSetup()
                        };

                        //IF its edit use existing one
                        if (!(MRevSetupInDb == null))
                        {
                            viewModelMRev.MRevSetup = MRevSetupInDb;
                        }
                        else
                        {
                            viewModelMRev.MRevSetup = new MRevSetup
                            {
                                IdComponent = component.Id,
                            };
                        }

                        return View("SetupMRev", viewModelMRev);
                    }else
                    {
                        var MotFrqSetupInDb = _context.MotFrqs.SingleOrDefault(c => c.IdComponent == id);

                        MotFrqSetupViewModel viewModelMotFrq = new MotFrqSetupViewModel
                        {
                            Component = _context.Components.SingleOrDefault(c => c.Id == id),
                            Childs = _context.IOs.Where(c => c.ComponentId == component.Id).ToList(),
                            DIAlarms = _context.DIAlarms.Where(c => c.IdComponent == component.Id).ToList(),
                            AIAlarms = _context.AIAlarms.Where(c => c.IdComponent == component.Id).ToList(),
                            DIAlarm = new DIAlarmSetup(),
                            AIAlarm = new AIAlarmSetup()
                        };

                        //IF its edit use existing one
                        if (!(MotFrqSetupInDb == null))
                        {
                            viewModelMotFrq.MotFrqSetup = MotFrqSetupInDb;
                        }
                        else
                        {
                            viewModelMotFrq.MotFrqSetup = new MotFrqSetup
                            {
                                IdComponent = component.Id,
                            };
                        }

                        return View("SetupMotFrq", viewModelMotFrq);
                    }
                        
                //===============================================================================================
                case ("ValveControl"):

                    var StdVlvSetupInDb = _context.StdVlvs.SingleOrDefault(c => c.IdComponent == id);

                    StdVlvSetupViewModel viewModelStdVlv = new StdVlvSetupViewModel
                    {
                        Component = _context.Components.SingleOrDefault(c => c.Id == id),
                        Childs = _context.IOs.Where(c => c.ComponentId == component.Id).ToList(),
                        DIAlarms = _context.DIAlarms.Where(c => c.IdComponent == component.Id).ToList(),
                        AIAlarms = _context.AIAlarms.Where(c => c.IdComponent == component.Id).ToList(),
                        DIAlarm = new DIAlarmSetup(),
                        AIAlarm = new AIAlarmSetup()
                    };

                    //IF its edit use existing one
                    if (!(StdVlvSetupInDb == null))
                    {
                        viewModelStdVlv.StdVlvSetup = StdVlvSetupInDb;
                    }
                    else
                    {
                        viewModelStdVlv.StdVlvSetup = new StdVlvSetup
                        {
                            IdComponent = component.Id,
                        };
                    }

                    return View("SetupStdVlv", viewModelStdVlv);

                default:
                    return Content("Unknown standard!");

            }

        }

        //====================================================================================================================
        //SAVE COMPONENT SETUP

        public ActionResult SaveDIAlarm(DIAlarmSetup DIAlarmSetup)
        {
            if (!ModelState.IsValid && DIAlarmSetup.Id != 0)
            {

                var viewModel = new DIAlarmSetupViewModel
                {
                    Component = _context.Components.SingleOrDefault(c => c.Id == DIAlarmSetup.IdComponent),
                    DIAlarmSetup = DIAlarmSetup,

                };
                return View("SetupDIAlarm", viewModel);

            }

            var DIAlarmInDb = _context.DIAlarms.SingleOrDefault(c => c.Id == DIAlarmSetup.Id);

            if (DIAlarmInDb == null)
            {
                _context.DIAlarms.Add(DIAlarmSetup);
            }
            else
            {
                DIAlarmInDb.IdComponent = DIAlarmSetup.IdComponent;
                DIAlarmInDb.Comment = DIAlarmSetup.Comment;
                DIAlarmInDb.InputType = DIAlarmSetup.InputType;
                DIAlarmInDb.TimeDelay = DIAlarmSetup.TimeDelay;
                DIAlarmInDb.IdIO = DIAlarmSetup.IdIO;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Factory");
        }

        public ActionResult SaveDIPulse(DIPulseSetup DIPulseSetup)
        {
            if (!ModelState.IsValid && DIPulseSetup.Id != 0)
            {

                var viewModel = new DIPulseSetupViewModel
                {
                    Component = _context.Components.SingleOrDefault(c => c.Id == DIPulseSetup.IdComponent),
                    DIPulseSetup = DIPulseSetup,

                };
                return View("SetupDIPulse", viewModel);

            }

            var DIPulseInDb = _context.DIpulses.SingleOrDefault(c => c.Id == DIPulseSetup.Id);


            if (DIPulseInDb == null)
            {
                _context.DIpulses.Add(DIPulseSetup);
            }
            else
            {
                DIPulseInDb.IdComponent = DIPulseSetup.IdComponent;
                DIPulseInDb.IdIO = DIPulseSetup.IdIO;
                DIPulseInDb.PulsesPerUnit = DIPulseSetup.PulsesPerUnit;

            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Factory");
        }

        public ActionResult SaveAIAlarm(AIAlarmSetup AIAlarmSetup)
        {
            if (!ModelState.IsValid && AIAlarmSetup.Id != 0)
            {

                var viewModel = new AIAlarmSetupViewModel
                {
                    Component = _context.Components.SingleOrDefault(c => c.Id == AIAlarmSetup.IdComponent),
                    AIAlarmSetup = AIAlarmSetup,

                };
                return View("SetupAIAlarm", viewModel);

            }

            var AIAlarmInDb = _context.AIAlarms.SingleOrDefault(c => c.Id == AIAlarmSetup.Id);

            if (AIAlarmInDb == null)
            {
                _context.AIAlarms.Add(AIAlarmSetup);
            }
            else
            {
                AIAlarmInDb.IdComponent = AIAlarmSetup.IdComponent;
                AIAlarmInDb.Comment = AIAlarmSetup.Comment;
                AIAlarmInDb.IdIO = AIAlarmSetup.IdIO;
                AIAlarmInDb.AICType = AIAlarmSetup.AICType;
                AIAlarmInDb.TimeDelay = AIAlarmSetup.TimeDelay;
                AIAlarmInDb.ScaleMax = AIAlarmSetup.ScaleMax;
                AIAlarmInDb.ScaleMin = AIAlarmSetup.ScaleMin;
                AIAlarmInDb.AlarmHH = AIAlarmSetup.AlarmHH;
                AIAlarmInDb.AlarmH = AIAlarmSetup.AlarmH;
                AIAlarmInDb.AlarmL = AIAlarmSetup.AlarmL;
                AIAlarmInDb.AlarmLL = AIAlarmSetup.AlarmLL;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Factory");
        }

        public ActionResult SaveMDir(MDirSetup MDirSetup)
        {

            if (!ModelState.IsValid && MDirSetup.Id != 0)
            {

                var viewModelBAD = new MDirSetupViewModel
                {
                    Component = _context.Components.SingleOrDefault(c => c.Id == MDirSetup.IdComponent),
                    MDirSetup = MDirSetup,

                };
                viewModelBAD.Childs = _context.IOs.Where(c => c.ComponentId == viewModelBAD.Component.Id).ToList();
                viewModelBAD.DIAlarms = _context.DIAlarms.Where(c => c.IdComponent == viewModelBAD.Component.Id).ToList();
                viewModelBAD.AIAlarms = _context.AIAlarms.Where(c => c.IdComponent == viewModelBAD.Component.Id).ToList();
                return View("SetupMDir", viewModelBAD);

            }

            var MDirInDb = _context.MDirs.FirstOrDefault(c => c.IdComponent == MDirSetup.IdComponent);

            if (MDirInDb == null)
            {
                _context.MDirs.Add(MDirSetup);
            }
            else
            {
                MDirInDb.IdComponent = MDirSetup.IdComponent;
                MDirInDb.INRunningFB = MDirSetup.INRunningFB;
                MDirInDb.OUTResetSignal = MDirSetup.OUTResetSignal;
                MDirInDb.OUTStartSignal = MDirSetup.OUTStartSignal;
            }

            _context.SaveChanges();

            //Init viewmodel data
            var viewModelOK = new MDirSetupViewModel
            {
                Component = _context.Components.SingleOrDefault(c => c.Id == MDirSetup.IdComponent),
                MDirSetup = MDirSetup,
                DIAlarm = new DIAlarmSetup(),
                AIAlarm = new AIAlarmSetup(),

            };

            viewModelOK.Childs = _context.IOs.Where(c => c.ComponentId == viewModelOK.Component.Id).ToList();
            viewModelOK.DIAlarms = _context.DIAlarms.Where(c => c.IdComponent == viewModelOK.Component.Id).ToList();
            viewModelOK.AIAlarms = _context.AIAlarms.Where(c => c.IdComponent == viewModelOK.Component.Id).ToList();

            return View("SetupMDir", viewModelOK);
        }

        public ActionResult SaveMotFrq(MotFrqSetup MotFrqSetup)
        {
            if (!ModelState.IsValid && MotFrqSetup.Id != 0)
            {

                var viewModelBAD = new MotFrqSetupViewModel
                {
                    Component = _context.Components.SingleOrDefault(c => c.Id == MotFrqSetup.IdComponent),
                    MotFrqSetup = MotFrqSetup,
                    

                };
                viewModelBAD.Childs = _context.IOs.Where(c => c.ComponentId == viewModelBAD.Component.Id).ToList();
                viewModelBAD.DIAlarms = _context.DIAlarms.Where(c => c.IdComponent == viewModelBAD.Component.Id).ToList();
                viewModelBAD.AIAlarms = _context.AIAlarms.Where(c => c.IdComponent == viewModelBAD.Component.Id).ToList();

                return View("SetupMotFrq", viewModelBAD);

            }

            var MotFrqInDb = _context.MotFrqs.FirstOrDefault(c => c.IdComponent == MotFrqSetup.IdComponent);

            if (MotFrqInDb == null)
            {
                _context.MotFrqs.Add(MotFrqSetup);
            }
            else
            {
                MotFrqInDb.IdComponent = MotFrqSetup.IdComponent;
                MotFrqInDb.FrqType = MotFrqSetup.FrqType;
                MotFrqInDb.IPAddress = MotFrqSetup.IPAddress;
                MotFrqInDb.NominalSpeed = MotFrqSetup.NominalSpeed;
            }



            _context.SaveChanges();

            //Init viewmodel data
            var viewModelOK = new MotFrqSetupViewModel
            {
                Component = _context.Components.SingleOrDefault(c => c.Id == MotFrqSetup.IdComponent),
                MotFrqSetup = MotFrqSetup,
                DIAlarm = new DIAlarmSetup(),
                AIAlarm = new AIAlarmSetup()

            };
            viewModelOK.Childs = _context.IOs.Where(c => c.ComponentId == viewModelOK.Component.Id).ToList();
            viewModelOK.DIAlarms = _context.DIAlarms.Where(c => c.IdComponent == viewModelOK.Component.Id).ToList();
            viewModelOK.AIAlarms = _context.AIAlarms.Where(c => c.IdComponent == viewModelOK.Component.Id).ToList();

            return View("SetupMotFrq", viewModelOK);

        }

        public ActionResult SaveMRev(MRevSetup MRevSetup)
        {

            if (!ModelState.IsValid && MRevSetup.Id != 0)
            {

                var viewModelBAD = new MRevSetupViewModel
                {
                    Component = _context.Components.SingleOrDefault(c => c.Id == MRevSetup.IdComponent),
                    MRevSetup = MRevSetup,

                };
                viewModelBAD.Childs = _context.IOs.Where(c => c.ComponentId == viewModelBAD.Component.Id).ToList();
                viewModelBAD.DIAlarms = _context.DIAlarms.Where(c => c.IdComponent == viewModelBAD.Component.Id).ToList();
                viewModelBAD.AIAlarms = _context.AIAlarms.Where(c => c.IdComponent == viewModelBAD.Component.Id).ToList();
                return View("SetupMRev", viewModelBAD);

            }

            var MRevInDb = _context.MRevs.FirstOrDefault(c => c.IdComponent == MRevSetup.IdComponent);

            if (MRevInDb == null)
            {
                _context.MRevs.Add(MRevSetup);
            }
            else
            {
                MRevInDb.IdComponent = MRevSetup.IdComponent;
                MRevInDb.INRunningFBFW = MRevSetup.INRunningFBFW;
                MRevInDb.INRunningFBBW = MRevSetup.INRunningFBBW;
                MRevInDb.OUTResetSignal = MRevSetup.OUTResetSignal;
                MRevInDb.OUTStartSignalFW = MRevSetup.OUTStartSignalFW;
                MRevInDb.OUTStartSignalBW = MRevSetup.OUTStartSignalBW;
            }

            _context.SaveChanges();

            //Init viewmodel data
            var viewModelOK = new MRevSetupViewModel
            {
                Component = _context.Components.SingleOrDefault(c => c.Id == MRevSetup.IdComponent),
                MRevSetup = MRevSetup,
                DIAlarm = new DIAlarmSetup(),
                AIAlarm = new AIAlarmSetup(),

            };

            viewModelOK.Childs = _context.IOs.Where(c => c.ComponentId == viewModelOK.Component.Id).ToList();
            viewModelOK.DIAlarms = _context.DIAlarms.Where(c => c.IdComponent == viewModelOK.Component.Id).ToList();
            viewModelOK.AIAlarms = _context.AIAlarms.Where(c => c.IdComponent == viewModelOK.Component.Id).ToList();

            return View("SetupMRev", viewModelOK);

        }

        public ActionResult SaveStdVlv(StdVlvSetup StdVlvSetup)
        {

            if (!ModelState.IsValid && StdVlvSetup.Id != 0)
            {

                var viewModelBAD = new StdVlvSetupViewModel
                {
                    Component = _context.Components.SingleOrDefault(c => c.Id == StdVlvSetup.IdComponent),
                    StdVlvSetup = StdVlvSetup,

                };
                viewModelBAD.Childs = _context.IOs.Where(c => c.ComponentId == viewModelBAD.Component.Id).ToList();
                viewModelBAD.DIAlarms = _context.DIAlarms.Where(c => c.IdComponent == viewModelBAD.Component.Id).ToList();
                viewModelBAD.AIAlarms = _context.AIAlarms.Where(c => c.IdComponent == viewModelBAD.Component.Id).ToList();
                return View("SetupStdVlv", viewModelBAD);

            }

            var StdVlvInDb = _context.StdVlvs.FirstOrDefault(c => c.IdComponent == StdVlvSetup.IdComponent);

            if (StdVlvInDb == null)
            {
                _context.StdVlvs.Add(StdVlvSetup);
            }
            else
            {
                StdVlvInDb.IdComponent = StdVlvSetup.IdComponent;
                StdVlvInDb.INClosedFB = StdVlvSetup.INClosedFB;
                StdVlvInDb.INOpenedFB = StdVlvSetup.INOpenedFB;
                StdVlvInDb.OUTResetSignal = StdVlvSetup.OUTResetSignal;
                StdVlvInDb.OUTOpenSignal = StdVlvSetup.OUTOpenSignal;
                StdVlvInDb.OUTCloseSignal = StdVlvSetup.OUTCloseSignal;
            }

            _context.SaveChanges();

            //Init viewmodel data
            var viewModelOK = new StdVlvSetupViewModel
            {
                Component = _context.Components.SingleOrDefault(c => c.Id == StdVlvSetup.IdComponent),
                StdVlvSetup = StdVlvSetup,
                DIAlarm = new DIAlarmSetup(),
                AIAlarm = new AIAlarmSetup(),

            };

            viewModelOK.Childs = _context.IOs.Where(c => c.ComponentId == viewModelOK.Component.Id).ToList();
            viewModelOK.DIAlarms = _context.DIAlarms.Where(c => c.IdComponent == viewModelOK.Component.Id).ToList();
            viewModelOK.AIAlarms = _context.AIAlarms.Where(c => c.IdComponent == viewModelOK.Component.Id).ToList();

            return View("SetupStdVlv", viewModelOK);
        }
        //====================================================================================================================

    }
}