using Microsoft.AspNet.Identity;
using PLC_GenCo.ViewModels;
using PLC_GenCo.XMLDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using static PLC_GenCo.ViewModels.Enums;

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

            var viewModel = new StandardViewModel {
                PageName = pageName,
                Standards = xmlDB.Standards
            };

            return View(viewModel);
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
                            Group = "DIAlarm"
                        });

                        break;
                    case ("Analog.L5X"):
                        standards.Add(new Standard
                        {
                            AOIName = file.Name.Substring(0, file.Name.Length - 4),
                            //Generated together with AlarmAI
                            ConnectionType = Enums.ConnectionType.AI,
                            Group = "AIAlarm"
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
                            Group = "Counter"
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
                        /*
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
                        */
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

                        break;
                }
            }

            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            foreach (var standard in standards)
            {
                //Add to file
                xmlDB.Standards.Add(standard);
            }

            /*//Add DO
            xmlDB.Standards.Add(new Standard {
                AOIName = "Std_DO_NoAOI",
                ConnectionType = Enums.ConnectionType.DO,
                Group = "Aux"
            });*/

            xmlDB.Save();

            // CREATE DEFAULT CONFIG FILES

            //Create config file AlarmDi
            xmlDB.AddConfigFile("AlarmDi", GenerateAlarmDiConfig());
            //Create config file Analog
            xmlDB.AddConfigFile("Analog", GenerateAnalogConfig());
            //Create config file CLOCK
            xmlDB.AddConfigFile("CLOCK", GenerateCLOCKConfig());
            //Create config file CNT
            xmlDB.AddConfigFile("CNT", GenerateCNTConfig());
            //Create config file DanfossFC_FV2_0
            xmlDB.AddConfigFile("DanfossFC_FV2_0", GenerateDanfossFC_FV2_0Config());
            //Create config file MotorDir
            xmlDB.AddConfigFile("MotorDir", GenerateMotorDirConfig());
            //Create config file ValveControl
            xmlDB.AddConfigFile("ValveControl", GenerateValveControlConfig());

            return RedirectToAction("Index", "Standards");
        }

        public ActionResult Details(int id)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            var standard = xmlDB.Standards.SingleOrDefault(c => c.Id == id);

            if (standard == null)
                return HttpNotFound();

            return View(standard);
        }

        private XElement AddParameter(string name, string shortName, DataType datatype, InOut inout, ParType parType, string defaultVal)
        {
            var parameter= new XElement("Parameter",
                new XAttribute("Name", name),
                new XAttribute("ShortName", shortName),
                new XAttribute("DataType", datatype.ToString()),
                new XAttribute("Usage", inout.ToString()),
                new XAttribute("Type", parType.ToString()),
                new XAttribute("DefaultValue", defaultVal),
                new XAttribute("Value", defaultVal)
                );

            return parameter;
        }

        private XElement GenerateAlarmDiConfig()
        {
            var config = new XElement("Configuration",
                new XAttribute("AOIName", "AlarmDi"),
                new XAttribute("Name", "TODO"),
                new XAttribute("Location", "TODO")
                );

            config.Add(AddParameter("Input", "IN", DataType.BOOL, InOut.Input, ParType.IO, "TODO"));
            config.Add(AddParameter("SekundPuls", "SP", DataType.BOOL, InOut.Input, ParType.Constant, "Clock.SekundPuls"));
            config.Add(AddParameter("InverseAlarm", "NO/NC", DataType.BOOL, InOut.Input, ParType.Constant, "1"));
            config.Add(AddParameter("DelayPRE", "TD", DataType.INT, InOut.Input, ParType.Parameter, "0"));

            return config;
        }

        private XElement GenerateAnalogConfig()
        {
            var config = new XElement("Configuration",
                new XAttribute("AOIName", "Analog"),
                new XAttribute("Name", "TODO"),
                new XAttribute("Location", "TODO")
                );

            config.Add(AddParameter("InputInt", "IN", DataType.INT, InOut.Input, ParType.IO, "TODO"));
            config.Add(AddParameter("IntSelect", "I", DataType.BOOL, InOut.Input, ParType.Constant, "1"));
            config.Add(AddParameter("MinScaleIn", "M", DataType.INT, InOut.Input, ParType.Constant, "3227"));
            config.Add(AddParameter("MaxScaleIn", "M", DataType.INT, InOut.Input, ParType.Constant, "16383"));
            config.Add(AddParameter("MinScale", "Min", DataType.REAL, InOut.Input, ParType.Parameter, "TODO"));
            config.Add(AddParameter("MaxScale", "Max", DataType.REAL, InOut.Input, ParType.Parameter, "TODO"));
            config.Add(AddParameter("BlokerLavL", "LL", DataType.BOOL, InOut.Input, ParType.Parameter, "1"));
            config.Add(AddParameter("AlarmLavLLim", "LL", DataType.REAL, InOut.Input, ParType.Parameter, "0"));
            config.Add(AddParameter("BlokerLav", "L", DataType.BOOL, InOut.Input, ParType.Parameter, "1"));
            config.Add(AddParameter("AlarmLavLim", "L", DataType.REAL, InOut.Input, ParType.Parameter, "0"));
            config.Add(AddParameter("BlokerHoj", "H", DataType.BOOL, InOut.Input, ParType.Parameter, "1"));
            config.Add(AddParameter("AlarmHojLim", "H", DataType.REAL, InOut.Input, ParType.Parameter, "0"));
            config.Add(AddParameter("BlokerHojH", "HH", DataType.BOOL, InOut.Input, ParType.Parameter, "1"));
            config.Add(AddParameter("AlarmHojHLim", "HH", DataType.REAL, InOut.Input, ParType.Parameter, "0"));
            config.Add(AddParameter("Middel", "M", DataType.REAL, InOut.Output, ParType.Constant, "IO.AI.NAME"));

            return config;
        }

        private XElement GenerateCLOCKConfig()
        {
            return new XElement("Configuration",
                new XAttribute("AOIName", "CLOCK"),
                new XAttribute("Name", "Clock"),
                new XAttribute("Location", "TODO")
                );
        }

        private XElement GenerateCNTConfig()
        {
            var config = new XElement("Configuration",
                new XAttribute("AOIName", "CNT"),
                new XAttribute("Name", "TODO"),
                new XAttribute("Location", "TODO")
                );

            config.Add(AddParameter("Value", "PPU", DataType.REAL, InOut.Input, ParType.Parameter, "1"));
            config.Add(AddParameter("Puls", "IN", DataType.BOOL, InOut.Input, ParType.IO, "TODO"));
            config.Add(AddParameter("MinutePulse5", "I", DataType.BOOL, InOut.Input, ParType.Constant, "Clock.MinutePulse5"));
            config.Add(AddParameter("HourPulse", "I", DataType.BOOL, InOut.Input, ParType.Constant, "Clock.HourPulse"));
            config.Add(AddParameter("DayPulse", "I", DataType.BOOL, InOut.Input, ParType.Constant, "Clock.DayPulse"));
            config.Add(AddParameter("WeekPulse", "I", DataType.BOOL, InOut.Input, ParType.Constant, "Clock.WeekPulse"));
            config.Add(AddParameter("MonthPulse", "I", DataType.BOOL, InOut.Input, ParType.Constant, "Clock.MonthPulse"));
            config.Add(AddParameter("YearPulse", "I", DataType.BOOL, InOut.Input, ParType.Constant, "Clock.YearPulse"));

            return config;
        }

        private XElement GenerateDanfossFC_FV2_0Config()
        {
            var config = new XElement("Configuration",
                new XAttribute("AOIName", "DanfossFC_FV2_0"),
                new XAttribute("Name", "TODO"),
                new XAttribute("Location", "TODO")
                );

            config.Add(AddParameter("DataIn", "IN", DataType.INT, InOut.Input, ParType.Constant, "NAME:I1.Data"));
            config.Add(AddParameter("DataOut", "OUT", DataType.INT, InOut.Input, ParType.Constant, "NAME:O1.Data"));
            config.Add(AddParameter("InConnectionFaulted", "CF", DataType.BOOL, InOut.Input, ParType.Constant, "NAME:I1.ConnectionFaulted"));
            config.Add(AddParameter("InReset", "Res", DataType.BOOL, InOut.Input, ParType.Constant, "_IMotor.NAME.Fault_Reset"));
            config.Add(AddParameter("InSpeedReq", "SP", DataType.REAL, InOut.Input, ParType.Constant, "_IMotor.NAME.SpeedSP"));

            return config;
        }
        
        private XElement GenerateMotorDirConfig()
        {
            var config = new XElement("Configuration",
                new XAttribute("AOIName", "MotorDir"),
                new XAttribute("Name", "TODO"),
                new XAttribute("Location", "TODO")
                );

            config.Add(AddParameter("Enable", "EN", DataType.BOOL, InOut.Input, ParType.Constant, "StartUpTimer.DN"));
            config.Add(AddParameter("interLockOk1", "EN", DataType.BOOL, InOut.Input, ParType.Constant, "StartUpTimer.DN"));
            config.Add(AddParameter("Ext. Falut 01", "ExtF01", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 02", "ExtF01", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 03", "ExtF01", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 04", "ExtF01", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 05", "ExtF01", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 06", "ExtF01", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 07", "ExtF01", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 08", "ExtF01", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Condition1", "", DataType.BOOL, InOut.Input, ParType.Interface, "StartFW"));
            config.Add(AddParameter("Running1", "fbFW", DataType.BOOL, InOut.Input, ParType.IO, "TODO"));
            config.Add(AddParameter("Auto", "A", DataType.BOOL, InOut.Input, ParType.Constant, "AlwaysON"));
            config.Add(AddParameter("Start1", "cmdFW", DataType.BOOL, InOut.Output, ParType.IO, "TODO"));
            config.Add(AddParameter("Scantime_ms", "I", DataType.REAL, InOut.Input, ParType.Constant, "Clock.ScanTimeReal"));
            config.Add(AddParameter("FaultNegMask", "cmdFW", DataType.INT, InOut.Input, ParType.TODO, "TODO"));
            config.Add(AddParameter("DelayAfterStart_ms", "c", DataType.INT, InOut.Input, ParType.Constant, "2000"));
            config.Add(AddParameter("StartPulsetime", "I", DataType.Other, InOut.Input, ParType.Constant, "StartUpTimer"));

            return config;
        }

        private XElement GenerateValveControlConfig()
        {
            var config = new XElement("Configuration",
                new XAttribute("AOIName", "ValveControl"),
                new XAttribute("Name", "TODO"),
                new XAttribute("Location", "TODO")
                );

            config.Add(AddParameter("Enable", "EN", DataType.BOOL, InOut.Input, ParType.Constant, "StartUpTimer.DN"));
            config.Add(AddParameter("interLockOkOpen", "EN", DataType.BOOL, InOut.Input, ParType.Constant, "StartUpTimer.DN"));
            config.Add(AddParameter("interLockOkOpen", "EN", DataType.BOOL, InOut.Input, ParType.Constant, "StartUpTimer.DN"));
            config.Add(AddParameter("Open FB", "FB-O", DataType.BOOL, InOut.Input, ParType.IO, "TODO"));
            config.Add(AddParameter("Close FB", "FB-C", DataType.BOOL, InOut.Input, ParType.IO, "TODO"));
            config.Add(AddParameter("Ext. Falut 01", "ExtF01", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 02", "ExtF02", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 03", "ExtF03", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 04", "ExtF04", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 05", "ExtF05", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 06", "ExtF06", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 07", "ExtF07", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 08", "ExtF08", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 09", "ExtF09", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("Ext. Falut 10", "ExtF10", DataType.BOOL, InOut.Input, ParType.IO, "AlwaysOFF"));
            config.Add(AddParameter("OpenCondition", "O", DataType.BOOL, InOut.Input, ParType.Interface, "Open"));
            config.Add(AddParameter("CloseCondition", "C", DataType.BOOL, InOut.Input, ParType.Interface, "Close"));
            config.Add(AddParameter("OpenActive", "O", DataType.BOOL, InOut.Input, ParType.Constant, "AlwaysON"));
            config.Add(AddParameter("CloseActive", "C", DataType.BOOL, InOut.Input, ParType.Constant, "AlwaysON"));
            config.Add(AddParameter("Auto", "A", DataType.BOOL, InOut.Input, ParType.Constant, "AlwaysON"));
            config.Add(AddParameter("Open CMD", "CMD-O", DataType.BOOL, InOut.Output, ParType.IO, "TODO"));
            config.Add(AddParameter("Close CMD", "CMD-C", DataType.BOOL, InOut.Output, ParType.IO, "TODO"));
            config.Add(AddParameter("Scantime_ms", "I", DataType.REAL, InOut.Input, ParType.Constant, "Clock.ScanTimeReal"));
            config.Add(AddParameter("FaultNegMask", "cmdFW", DataType.INT, InOut.Input, ParType.TODO, "TODO"));

            return config;
        }
    }
}