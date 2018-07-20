using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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

            return View(_context.Standards.ToList());
        }

        public ActionResult LoadDefaultStandards()
        {
            // Loading default standards
            var standards = new List<Standard>();

            DirectoryInfo d = new DirectoryInfo(@"C:\Users\Ivan\Desktop\OP generator PLC koda\StandardAOI");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.L5X"); //Getting L5X files

            foreach (var file in Files)
            {

                switch (file.Name)
                {
                    case ("AlarmDi.L5X"):
                        standards.Add(new Standard
                        {
                            AOIName = file.Name.Substring(0, file.Name.Length - 4),
                            ConnectionType = Enums.ConnectionType.DI,
                            Group = "DIProtection"
                        });
                        break;
                    case ("Analog.L5X"):
                        standards.Add(new Standard
                        {
                            AOIName = file.Name.Substring(0, file.Name.Length - 4),
                            //Generated together with AlarmAI
                            ConnectionType = Enums.ConnectionType.AI,
                            Group = "AIProtection"
                        });
                        break;
                    case ("Average.L5X"):
                        standards.Add(new Standard
                        {
                            AOIName = file.Name.Substring(0, file.Name.Length - 4),
                            //Generated together with AlarmAI
                            ConnectionType = Enums.ConnectionType.No_Connection_Type,
                            Group = "Aux"
                        });
                        break;
                    case ("CLOCK.L5X"):
                        standards.Add(new Standard
                        {
                            AOIName = file.Name.Substring(0, file.Name.Length - 4),
                            //Generated always
                            ConnectionType = Enums.ConnectionType.No_Connection_Type,
                            Group = "Always"
                        });
                        break;
                    case ("CNT.L5X"):
                        standards.Add(new Standard
                        {
                            AOIName = file.Name.Substring(0, file.Name.Length - 4),
                            //Generated always
                            ConnectionType = Enums.ConnectionType.DI,
                            Group = "Always"
                        });
                        break;
                    case ("DanfossFC_FV2_0.L5X"):
                        standards.Add(new Standard
                        {
                            AOIName = file.Name.Substring(0, file.Name.Length - 4),
                            //Generated with FRQ Dir and Rev
                            ConnectionType = Enums.ConnectionType.No_Connection_Type,
                            Group = "Aux"
                        });
                        break;
                    case ("DriftPause.L5X"):
                        standards.Add(new Standard
                        {
                            AOIName = file.Name.Substring(0, file.Name.Length - 4),
                            //Generated on request with Motor
                            ConnectionType = Enums.ConnectionType.No_Connection_Type,
                            Group = "Aux"
                        });
                        break;
                    case ("MotorDir.L5X"):
                        standards.Add(new Standard
                        {
                            AOIName = file.Name.Substring(0, file.Name.Length - 4),
                            //Generated as parent
                            ConnectionType = Enums.ConnectionType.DIO,
                            Group = "Motor"
                        });
                        standards.Add(new Standard
                        {
                            AOIName = file.Name.Substring(0, file.Name.Length - 4),
                            //Generated as parent
                            ConnectionType = Enums.ConnectionType.ETH,
                            Group = "Motor"
                        });
                        break;
                    case ("MotorRev.L5X"):
                        standards.Add(new Standard
                        {
                            AOIName = file.Name.Substring(0, file.Name.Length - 4),
                            //Generated as parent
                            ConnectionType = Enums.ConnectionType.DIO,
                            Group = "Motor"
                        });
                        standards.Add(new Standard
                        {
                            AOIName = file.Name.Substring(0, file.Name.Length - 4),
                            //Generated as parent
                            ConnectionType = Enums.ConnectionType.ETH,
                            Group = "Motor"
                        });
                        break;
                    case ("ValveControl.L5X"):
                        standards.Add(new Standard
                        {
                            AOIName = file.Name.Substring(0, file.Name.Length - 4),
                            //Generated as parent
                            ConnectionType = Enums.ConnectionType.DIO,
                            Group = "Valve"
                        });
                        break;
                    case ("SCP.L5X"):
                        standards.Add(new Standard
                        {
                            AOIName = file.Name.Substring(0, file.Name.Length - 4),
                            //Generated as parent
                            ConnectionType = Enums.ConnectionType.No_Connection_Type,
                            Group = "Aux"
                        });
                        break;

                    default:
                        standards.Add(new Standard
                        {
                            AOIName = file.Name.Substring(0, file.Name.Length - 4),
                            //Generated as parent
                            ConnectionType = Enums.ConnectionType.No_Connection_Type,
                            Group = "Aux"
                        });
                        break;
                }
            }

            foreach (var standard in standards)
            {
                _context.Standards.Add(standard);
                _context.SaveChanges();
            }

            //Add DO
            _context.Standards.Add(new Standard {
                AOIName = "Std_DO_NoAOI",
                ConnectionType = Enums.ConnectionType.DO,
                Group = "Aux"
            });
            _context.SaveChanges();

            return RedirectToAction("Index", "Standards");
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