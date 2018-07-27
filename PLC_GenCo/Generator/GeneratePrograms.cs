using PLC_GenCo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.Generator
{
    public class GeneratePrograms
    {
        private ProgramsInfo _programsInfo;

        public GeneratePrograms(ProgramsInfo programsInfo)
        {
            _programsInfo = programsInfo;
        }
        
        public XElement GetPrograms()
        {
            var programs = new XElement("Programs");

            programs.Add(GetProgramInput());
            programs.Add(GetProgramGeneral());
            programs.Add(GetProgramControl());
            programs.Add(GetProgramComponent());
            programs.Add(GetProgramOutput());
            programs.Add(GetProgramAnalog());

            return programs;
        }
        //======================================================================================================
        //=======================================--------PROGRAM---------=======================================
        public XElement GetProgramInput()
        {
            var programInput = new XElement("Program",
                new XAttribute("Name", "Input"),
                new XAttribute("TestEdits", "false"),
                new XAttribute("MainRoutineName", "A00_MainRoutine"),
                new XAttribute("Disabled", "false"),
                new XAttribute("UseAsFolder", "false"),

                new XElement("Description", "Copy data from physical layer"),

                    new XElement("Tags"),
                    new XElement("Routines",
                        GetRoutineDigitalInput()
                        
                    )
                );
            //Navigate to Routines --> add main routine
            programInput.Element("Routines").Add(AddMainRoutine(programInput.Element("Routines")));


            return programInput;
        }
        //=======================================        ROUTINES        =======================================
        private XElement GetRoutineDigitalInput()
        {
            var routineInput = new XElement("Routine",
                new XAttribute("Name", "A01_DigitalInput"),
                new XAttribute("Type", "ST"),
                new XElement("Description", "Copy DI data from physical layer to IO tag")
            );

            var stContent = new XElement("STContent");

            stContent.Add(AddLine(0,"//             DIGITAL INPUT DATA UNMASKING"));
            stContent.Add(AddLine(1, "//"));
            stContent.Add(AddLine(2, "//"));


            foreach (Module module in _programsInfo.Modules)
            {
                switch (module.IOModulesType)
                {
                    case ViewModels.Enums.IOModulesType.EmbDIx16:
                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//    Rack:01    Module:0" + module.Address + "    PLC embeded DIx16"));
                        stContent.Add(AddLine(stContent.Descendants("Line").Count(),"//"));

                        //Filter IOs from this module - NOTE! same module has DO too!
                        var DIEmbIOs = _programsInfo.IOs.Where(c => (c.IOAddress.Module == module.Address && c.ConnectionType == ViewModels.Enums.ConnectionType.DI));

                        //Copy each IO channel to IO, or copy do Disp.
                        for (int i = 0; i < 16; i++)
                        {
                            var io = DIEmbIOs.SingleOrDefault(c => c.IOAddress.Channel == i);
                            String line = System.String.Empty;
                            String comment = System.String.Empty;
                            if (io == null)
                            {
                                //No IO on this input add Disp.
                                line += "Disp.DI:=Local:" + module.Address.ToString() + ":I.Data." + i.ToString() + ";";
                                comment = "Disponsibel IO";
                            }
                            else
                            {
                                line += "IO.DI." + io.Name + ":=Local:" + module.Address.ToString() + ":I.Data." + i.ToString() + ";";
                                comment = io.Comment;
                            }

                            //Fill with spaces until comment-all comments start in same place
                            int count = line.Count();
                            for (int j = count; j < 70 ; j++)
                            {
                                line += " ";
                            }

                            line += "//    CH:" + i.ToString("D2") + "    " + comment; 

                            stContent.Add(AddLine(stContent.Descendants("Line").Count(), line ));

                        }

                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));
                        break;
                    case ViewModels.Enums.IOModulesType.EmbDOx16:
                        continue;//Handled in Program:Output Routine:A01_DigitalOutput
                    case ViewModels.Enums.IOModulesType.DIx4:
                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//    Rack:01    Module:0" + module.Address + "    DIx4 module: " + module.Name));
                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));

                        //Filter IOs from this module - NOTE! same module has DO too!
                        var DIx4IOs = _programsInfo.IOs.Where(c => (c.IOAddress.Module == module.Address && c.ConnectionType == ViewModels.Enums.ConnectionType.DI));

                        //Copy each IO channel to IO, or copy do Disp.
                        for (int i = 0; i < 4; i++)
                        {
                            var io = DIx4IOs.SingleOrDefault(c => c.IOAddress.Channel == i);
                            String line = System.String.Empty;
                            String comment = System.String.Empty;
                            if (io == null)
                            {
                                //No IO on this input add Disp.
                                line += "Disp.DI:=Local:" + module.Address.ToString() + ":I.Data." + i.ToString() + ";";
                                comment = "Disponsibel IO";
                            }
                            else
                            {
                                line += "IO.DI." + io.Name + ":=Local:" + module.Address.ToString() + ":I.Data." + i.ToString() + ";";
                                comment = io.Comment;
                            }

                            //Fill with spaces until comment - comments start in same line
                            int count = line.Count();
                            for (int j = count; j < 70; j++)
                            {
                                line += " ";
                            }

                            line += "//    CH:" + i.ToString("D2") + "    " + comment;

                            stContent.Add(AddLine(stContent.Descendants("Line").Count(), line));

                        }

                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));
                        break;
                    case ViewModels.Enums.IOModulesType.DIx8:
                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//    Rack:01    Module:0" + module.Address + "    DIx8 module: " + module.Name));
                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));

                        //Filter IOs from this module
                        var DIx8IOs = _programsInfo.IOs.Where(c => c.IOAddress.Module == module.Address);

                        for (int i = 0; i < 8; i++)
                        {
                            var io = DIx8IOs.SingleOrDefault(c => c.IOAddress.Channel == i);
                            String line = System.String.Empty;
                            String comment = System.String.Empty;
                            if (io == null)
                            {
                                //No IO on this input add Disp.
                                line += "Disp.DI:=Local:" + module.Address.ToString() + ":I.Data." + i.ToString() + ";";
                                comment = "Disponsibel IO";
                            }
                            else
                            {
                                line += "IO.DI." + io.Name + ":=Local:" + module.Address.ToString() + ":I.Data." + i.ToString() + ";";
                                comment = io.Comment;
                            }

                            //Fill with spaces until comment
                            int count = line.Count();
                            for (int j = count; j < 70; j++)
                            {
                                line += " ";
                            }

                            line += "//    CH:" + i.ToString("D2") + "    " + comment;

                            stContent.Add(AddLine(stContent.Descendants("Line").Count(), line));

                        }

                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));
                        break;
                    case ViewModels.Enums.IOModulesType.DOx4:
                        continue;//Handled in Program:Output Routine:A01_DigitalOutput
                    case ViewModels.Enums.IOModulesType.DOx8:
                        continue;//Handled in Program:Output Routine:A01_DigitalOutput
                    case ViewModels.Enums.IOModulesType.AIx4:
                        continue;//Handled in Program:Analog Routine:A01_AnalogInput
                    case ViewModels.Enums.IOModulesType.AIx8:
                        continue;//Handled in Program:Analog Routine:A01_AnalogInput
                    case ViewModels.Enums.IOModulesType.AOx4:
                        continue;//Handled in Program:Output Routine:A02_AnalogOutput
                    default:
                        throw new Exception("New module not added to MainTask/Input/A01_Input routine generator");
                        
                }
            }

            //Add ETH_DI as TODO

            //Filter ETH_DI
            var ETH_DIios = _programsInfo.IOs.Where(c => c.ConnectionType == ConnectionType.DI && c.IOAddress.Type == IOType.IP).ToList();

            if (ETH_DIios.Count != 0)
            {
                stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//    Custom ethernet module I/Os"));
                stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));

                foreach (var io in ETH_DIios)
                {
                    String line = System.String.Empty;

                    line += "IO.DI." + io.Name + ":=TODO.BOOL;";

                    //Fill with spaces until comment
                    int count = line.Count();
                    for (int j = count; j < 70; j++)
                    {
                        line += " ";
                    }

                    line += "//    CH:ETH    " + io.Comment;

                    stContent.Add(AddLine(stContent.Descendants("Line").Count(), line));
                }
            }

            routineInput.Add(stContent);
            return routineInput;
        }
        //======================================================================================================
        //=======================================--------PROGRAM---------=======================================
        public XElement GetProgramGeneral()
        {

            var generalProgram = new XElement("Program",
                new XAttribute("Name", "General"),
                new XAttribute("TestEdits", "false"),
                new XAttribute("MainRoutineName", "A00_MainRoutine"),
                new XAttribute("Disabled", "false"),
                new XAttribute("UseAsFolder", "false"),

                new XElement("Description", "Basic init, clock, counters, DIalarms"),

                    new XElement("Tags"),
                    new XElement("Routines")
                    );

            //LOAD template A01_Initialization file 
            var initRouitne = new UploadRoutine(@"C:\Users\Ivan\Desktop\OP generator PLC koda\StandardTemplate\General\", "A01_Initialization");

            if (initRouitne.Read())
            {
                //Add routine
                generalProgram.Element("Routines").Add(initRouitne.Routine);
                //Add global tags
                if (initRouitne.Tags != null)
                    _programsInfo.GenerateTags.AddTags(initRouitne.Tags);
            }
            //===================================================================================================
            //Get A02_ModuleStatus
            generalProgram.Element("Routines").Add(GetRoutineModulStatus());
            //==================================================================================================
            var clockRouitne = new UploadRoutine(@"C:\Users\Ivan\Desktop\OP generator PLC koda\StandardTemplate\General\", "A03_Clock");

            if (clockRouitne.Read())
            {
                //Add routine
                generalProgram.Element("Routines").Add(clockRouitne.Routine);
                //Add global tags
                if (clockRouitne.Tags != null)
                    _programsInfo.GenerateTags.AddTags(clockRouitne.Tags);
            }
            //=================================================================================================
            //GET A04_Counters 
            generalProgram.Element("Routines").Add(GetRoutineCounters());

            //GET A05_DIAlarms 
            generalProgram.Element("Routines").Add(GetRoutineDIAlarm());


            //Navigate to Routines --> add main routine
            generalProgram.Element("Routines").Add(AddMainRoutine(generalProgram.Element("Routines")));
               

            return generalProgram;
        }
        //=======================================        ROUTINES        =======================================
        private XElement GetRoutineModulStatus()
        {
            var moduleStatusRoutine = new XElement("Routine",
                new XAttribute("Name", "A02_ModuleStatus"),
                new XAttribute("Type", "RLL"),
                new XElement("Description", "Read module status for each module"),

                new XElement("RLLContent")
            );
            int i = 0; //Rung counter
            foreach (var module in _programsInfo.Modules)
            {
                //Add embedded adicrete module once
                if (module.IOModulesType == ViewModels.Enums.IOModulesType.EmbDIx16 || module.IOModulesType == ViewModels.Enums.IOModulesType.EmbDOx16)
                {
                    //Skip DO, DI case made for both
                    if (module.IOModulesType == ViewModels.Enums.IOModulesType.EmbDOx16)
                        continue;

                    String commentEmb = "Module status -- Name: PLC Embedded discrete IO";
                    //Add title to first rung
                    if (i == 0)
                    {
                        moduleStatusRoutine.Element("RLLContent").Add(AddRungWithTitle(i, "MODULE STATUS", commentEmb, "[XIC(ScanCount." + i.ToString() + ") GSV(Module,Discrete_IO,FaultCode,ModuleStatus.Discrete_IO.FaultCode), GRT(ModuleStatus.Discrete_IO.FaultCode,0) OTE(ModuleStatus.Discrete_IO.Alarm) ];"));
                    }else
                    {
                        moduleStatusRoutine.Element("RLLContent").Add(AddRung(i, commentEmb, "[XIC(ScanCount." + i.ToString() + ") GSV(Module,Discrete_IO,FaultCode,ModuleStatus.Discrete_IO.FaultCode), GRT(ModuleStatus.Discrete_IO.FaultCode,0) OTE(ModuleStatus.Discrete_IO.Alarm) ];"));
                    }
                    
                }
                else
                {
                    String comment = "Module status -- Name: " + module.Name + "  -  Address: " + module.Address + "  -  Type: " + module.IOModulesType.ToString();
                    //Add title to first rung
                    if (i == 0)
                    {
                        moduleStatusRoutine.Element("RLLContent").Add(AddRungWithTitle(i, "MODULE STATUS", comment, "[XIC(ScanCount." + i.ToString() + ") GSV(Module," + module.Name + ",FaultCode,ModuleStatus." + module.Name + ".FaultCode), GRT(ModuleStatus." + module.Name + ".FaultCode,0) OTE(ModuleStatus." + module.Name + ".Alarm) ];"));
                    }else
                    {
                        moduleStatusRoutine.Element("RLLContent").Add(AddRung(i, comment, "[XIC(ScanCount." + i.ToString() + ") GSV(Module," + module.Name + ",FaultCode,ModuleStatus." + module.Name + ".FaultCode), GRT(ModuleStatus." + module.Name + ".FaultCode,0) OTE(ModuleStatus." + module.Name + ".Alarm) ];"));
                    }
                    
                }
                i++;

            }

            foreach (var motFrqSetup in _programsInfo.MotFrqSetups)
            {
                //Find component which owns setup
                var setupOwner = _programsInfo.Components.Single(c => c.Id == motFrqSetup.IdComponent);

                String comment = "Module status -- Name: " + setupOwner.Name + "  -  Address: " + motFrqSetup.IPAddress + "  -  Type: Frequency converter " + motFrqSetup.FrqType.ToString();

                moduleStatusRoutine.Element("RLLContent").Add(AddRung(i, comment, "[XIC(ScanCount." + i.ToString() + ") GSV(Module," + setupOwner.Name + ",FaultCode,ModuleStatus." + setupOwner.Name + ".FaultCode), GRT(ModuleStatus." + setupOwner.Name + ".FaultCode,0) OTE(ModuleStatus." + setupOwner.Name + ".Alarm) ];"));

                i++;
            }

            //WATCHDOG Function
            moduleStatusRoutine.Element("RLLContent").Add(AddRung(i, HeaderComment("WATCHDOG"), "NOP();"));
            i++;
            
            moduleStatusRoutine.Element("RLLContent").Add(AddRung(i, "PLC Watchdog", "[XIO(Watchdog.PLC_WD_T.DN) TON(Watchdog.PLC_WD_T,?,?) ,[XIC(Watchdog.PLC_WD_T.DN) XIO(Watchdog.PLC_WD_Alarm) ,XIO(Watchdog.PLC_WD_T.DN) XIC(Watchdog.PLC_WD_Alarm) ] OTE(Watchdog.PLC_WD_Alarm) ];"));
            i++;
            moduleStatusRoutine.Element("RLLContent").Add(AddRung(i, "Test alarm", "EQU(Clock.HHMM,Watchdog.TestAlarmTime)OTE(Watchdog.Test_Alarm);"));
            i++;
            moduleStatusRoutine.Element("RLLContent").Add(AddRung(i, "SCADA Watchdog", "TON(Watchdog.SCADA_WD_T,?,?)XIC(Watchdog.SCADA_Handshake)OTU(Watchdog.SCADA_Handshake)RES(Watchdog.SCADA_WD_T);"));
            i++;



            return moduleStatusRoutine;
        }
        //======================================================================================================
        private XElement GetRoutineCounters()
        {
            var countersRoutine = new XElement("Routine",
                new XAttribute("Name", "A04_Counters"),
                new XAttribute("Type", "RLL"),

                new XElement("Description", "Counter handler (AOI) calls"),

                new XElement("RLLContent")
            );
            int i = 0;
            foreach (var DIpuls in _programsInfo.DIPulseSetups)
            {
                var io =_programsInfo.IOs.FirstOrDefault(c => c.Id == DIpuls.IdIO);
                String comment;
                if (io != null)
                {
                    comment = "Pulse counter - " + io.Comment;
                }
                else
                {
                    comment = "Pulse counter - component unknown - possible ERROR";
                }
                //Add rung - First one with main title
                if (i == 0)
                {
                    countersRoutine.Element("RLLContent").Add(AddRungWithTitle(i, "COUNTERS" , comment, "CNT(Counter." + io.Name + ", " + Convert.ToDouble(1.0f / DIpuls.PulsesPerUnit).ToString("n4") + ", IO.DI." + io.Name + ", Clock.MinutePulse5, Clock.HourPulse, Clock.DayPulse, Clock.Weekpulse, Clock.MonthPulse, Clock.YearPulse);"));
                }
                else
                {
                    countersRoutine.Element("RLLContent").Add(AddRung(i, comment, "CNT(Counter." + io.Name + ", " + Convert.ToDouble(1.0f / DIpuls.PulsesPerUnit).ToString("n4") + ", IO.DI." + io.Name + ", Clock.MinutePulse5, Clock.HourPulse, Clock.DayPulse, Clock.Weekpulse, Clock.MonthPulse, Clock.YearPulse);"));
                }
                
                i++;
            }

            return countersRoutine;
        }
        //======================================================================================================
        public XElement GetRoutineDIAlarm()
        {
            if (_programsInfo.DIAlarmSetups.Count <= 0)
                return new XElement("Empty");

            var DIAlarmRoutine = new XElement("Routine",
                new XAttribute("Name", "A05_DI_Alarm"),
                new XAttribute("Type", "RLL"),
                new XElement("Description", "DI alarm handler (AOI) calls"),
                new XElement("RLLContent")
                );

            var i = 0; //Rung counter

            //Filter DIAlarms form child components -- they are used on MotorDir AOI -- add rest
            foreach (var DIAlarm in _programsInfo.DIAlarmSetups)
            {
                //Skip if DIAlarm is owned by parent component
                if (_programsInfo.Components.Single(c => c.Id == DIAlarm.IdComponent).Dependancy == Dependancy.Parent)
                    continue;

                // Load owner component
                var io = _programsInfo.IOs.Single(c => c.Id == DIAlarm.IdIO);

                //First rung with title
                if (i == 0)
                {
                    DIAlarmRoutine.Element("RLLContent").Add(
                    AddRungWithTitle(i, "DIGITAL ALARM HANDLERS", "Name: " + io.Name + "  -  Description: " + io.Comment,
                    "AlarmDi(AlarmDI." + io.Name + "," +
                    AuxGetIOName(DIAlarm.IdIO, ConnectionType.DI) + "," +
                    "Clock.SecondPulse" + "," +
                    (Convert.ToBoolean(DIAlarm.InputType) ? "AlwaysOFF" : "AlwaysON") + "," +
                    (DIAlarm.TimeDelay.HasValue ? DIAlarm.TimeDelay.ToString() : "0") +
                    ");")
                    );
                }
                else
                {
                    DIAlarmRoutine.Element("RLLContent").Add(
                    AddRung(i, "Name: " + io.Name + "  -  Description: " + io.Comment,
                    "AlarmDi(AlarmDI." + io.Name + "," +
                    AuxGetIOName(DIAlarm.IdIO, ConnectionType.DI) + "," +
                    "Clock.SecondPulse" + "," +
                    (Convert.ToBoolean(DIAlarm.InputType) ? "AlwaysOFF" : "AlwaysON") + "," +
                    (DIAlarm.TimeDelay.HasValue ? DIAlarm.TimeDelay.ToString() : "0") +
                    ");")
                    );
                }

                i++;
            }




            return DIAlarmRoutine;
        }
        //======================================================================================================
        //=======================================--------PROGRAM---------=======================================
        public XElement GetProgramControl()
        {
            var controlProgram = new XElement("Program",
                new XAttribute("Name", "Control"),
                new XAttribute("TestEdits", "false"),
                new XAttribute("MainRoutineName", "A00_MainRoutine"),
                new XAttribute("Disabled", "false"),
                new XAttribute("UseAsFolder", "false"),

                new XElement("Description", "Main control program"),

                    new XElement("Tags"),
                    new XElement("Routines",
                        AddRoutine("A01_Sequence", "Sequenced control logic"),
                        AddRoutine("A02_Matrix", "Outputs from sequence")
                    )
                );
            //Navigate to Routines --> add main routine
            controlProgram.Element("Routines").Add(AddMainRoutine(controlProgram.Element("Routines")));

            return controlProgram;
        }
        //======================================================================================================
        //=======================================--------PROGRAM---------=======================================
        public XElement GetProgramComponent()
        {
            var componentProgram = new XElement("Program",
                new XAttribute("Name", "Component"),
                new XAttribute("TestEdits", "false"),
                new XAttribute("MainRoutineName", "A00_MainRoutine"),
                new XAttribute("Disabled", "false"),
                new XAttribute("UseAsFolder", "false"),

                new XElement("Description", "Component handler (AOI) calls"),

                    new XElement("Tags"),
                    new XElement("Routines")
                );

            int i = 1;                                                                  //Counter for routine name
            var frqconv = GetRoutineFrqConv();
            if (frqconv.Name != "Empty")                                                //If frequency converters exist in project add routine
            {
                frqconv.Attribute("Name").Value = "A0" + i.ToString() + "_FrqConv";     //Rename routine
                componentProgram.Element("Routines").Add(frqconv);                      //Add routine
                i++;
            }

            var motor = GetRoutineMotor();
            if (motor.Name != "Empty")                                                  //If motors exist in project add routine
            {
                motor.Attribute("Name").Value = "A0" + i.ToString() + "_Motor";         //Rename routine
                componentProgram.Element("Routines").Add(motor);                        //Add routine
                i++;    
            }

            var valve = GetRoutineValve();
            if (valve.Name != "Empty")                                                  //If valves exist in project add routine
            {
                valve.Attribute("Name").Value = "A0" + i.ToString() + "_Valve";         //Rename routine
                componentProgram.Element("Routines").Add(valve);                        //Add routine
                i++;
            }


            //Navigate to Routines --> add main routine
            componentProgram.Element("Routines").Add(AddMainRoutine(componentProgram.Element("Routines")));

            return componentProgram;
        }
        //=======================================        ROUTINES        =======================================
        public XElement GetRoutineMotor()
        {
            if (_programsInfo.MDirSetups.Count + _programsInfo.MRevSetups.Count + _programsInfo.MotFrqSetups.Count <= 0)
                return new XElement("Empty");

            var motorRoutine = new XElement("Routine",
                new XAttribute("Name", "Motor"),
                new XAttribute("Type", "RLL"),
                new XElement("Description", "Motor handler (AOI) calls"),
                new XElement("RLLContent")
                );

            var i = 0; //Rung counter

            //Add MotorFrq handler calls
            if (_programsInfo.MotFrqSetups != null)
            {
                foreach (var motor in _programsInfo.MotFrqSetups)
                {
                    // Load owner component
                    var setupOwner = _programsInfo.Components.Single(c => c.Id == motor.IdComponent);
                    // Load owned DI alarms
                    var diAlarms = _programsInfo.DIAlarmSetups.Where(c => c.IdComponent == setupOwner.Id).ToList();

                    //Load External alarms with NO/NC mask
                    string[] ExtAlarms = new string[8];
                    Int32 FaultNegMask = ~0;  
                    for (int j = 0; j < ExtAlarms.Length; j++)
                    {
                        if (diAlarms.Count > j)
                        {
                            ExtAlarms[j] = "IO.DI." + _programsInfo.IOs.Single(c => c.Id == diAlarms[j].IdIO).Name;

                            if (diAlarms[j].InputType == ViewModels.Enums.InputType.NC)
                            {
                                FaultNegMask &= ~(1 << j); //Reset bit on j position
                            }

                        }else
                        {
                            ExtAlarms[j] = "AlwaysOFF";
                        }
                    }

                    //First rung with title
                    if (i == 0)
                    {
                        motorRoutine.Element("RLLContent").Add(
                        AddRungWithTitle(i, "MOTOR HANDLERS", "Name: " + setupOwner.Name + "  -  Description: " + setupOwner.Comment,
                            "MotorDir(Motor." + setupOwner.Name + ",StartUpTimer.DN,StartUpTimer.DN," + ExtAlarms[0] + "," + ExtAlarms[1] + "," + ExtAlarms[2] + "," + ExtAlarms[3] + "," + ExtAlarms[4] + "," + ExtAlarms[5] + "," + ExtAlarms[6] + "," + ExtAlarms[7] + ",_IMotor." + setupOwner.Name + ".StartFW,FRQ." + setupOwner.Name + ".OutRunning,FRQ." + setupOwner.Name + ".STW.9,FRQ." + setupOwner.Name + ".CTW.6,Clock.ScanTimeReal," + FaultNegMask.ToString() + ",2000,StartUpTimer);")
                        );
                    }
                    else
                    {
                        motorRoutine.Element("RLLContent").Add(
                        AddRung(i, "Name: " + setupOwner.Name + "  -  Description: " + setupOwner.Comment,
                            "MotorDir(Motor." + setupOwner.Name + ",StartUpTimer.DN,StartUpTimer.DN," + ExtAlarms[0] + "," + ExtAlarms[1] + "," + ExtAlarms[2] + "," + ExtAlarms[3] + "," + ExtAlarms[4] + "," + ExtAlarms[5] + "," + ExtAlarms[6] + "," + ExtAlarms[7] + ",_IMotor." + setupOwner.Name + ".StartFW,FRQ." + setupOwner.Name + ".OutRunning,FRQ." + setupOwner.Name + ".STW.9,FRQ." + setupOwner.Name + ".CTW.6,Clock.ScanTimeReal," + FaultNegMask.ToString() + ",2000,StartUpTimer);")
                        );
                    }

                    i++;
                }
            }

            //Add MotorDir handler calls
            if (_programsInfo.MDirSetups != null)
            {
                foreach (var motor in _programsInfo.MDirSetups)
                {
                    // Load owner component
                    var setupOwner = _programsInfo.Components.Single(c => c.Id == motor.IdComponent);
                    // Load owned DI alarms
                    var diAlarms = _programsInfo.DIAlarmSetups.Where(c => c.IdComponent == setupOwner.Id).ToList();

                    //Load External alarms with NO/NC mask
                    string[] ExtAlarms = new string[8];
                    Int32 FaultNegMask = ~0;
                    for (int j = 0; j < ExtAlarms.Length; j++)
                    {
                        if (diAlarms.Count > j)
                        {
                            ExtAlarms[j] = "IO.DI." + _programsInfo.IOs.Single(c => c.Id == diAlarms[j].IdIO).Name;

                            if (diAlarms[j].InputType == ViewModels.Enums.InputType.NC)
                            {
                                FaultNegMask &= ~(1 << j); //Reset bit on j position
                            }

                        }
                        else
                        {
                            ExtAlarms[j] = "AlwaysOFF";
                        }
                    }


                    //AOI CALL - first rung with title
                    if (i == 0)
                    {
                        motorRoutine.Element("RLLContent").Add(
                        AddRungWithTitle(i, "MOTOR HANDLERS", "Name: " + setupOwner.Name + "  -  Description: " + setupOwner.Comment,
                            "MotorDir(Motor." + setupOwner.Name + ",StartUpTimer.DN,StartUpTimer.DN," + ExtAlarms[0] + "," + ExtAlarms[1] + "," + ExtAlarms[2] + "," + ExtAlarms[3] + "," + ExtAlarms[4] + "," + ExtAlarms[5] + "," + ExtAlarms[6] + "," + ExtAlarms[7] + ",_IMotor." + setupOwner.Name + ".StartFW," + AuxGetIOName(motor.INRunningFB, ConnectionType.DI) + ",AlwaysON," + AuxGetIOName(motor.OUTStartSignal, ConnectionType.DO) + ",Clock.ScanTimeReal," + FaultNegMask.ToString() + ",2000,StartUpTimer);")
                        );
                    }
                    else
                    {
                        motorRoutine.Element("RLLContent").Add(
                        AddRung(i, "Name: " + setupOwner.Name + "  -  Description: " + setupOwner.Comment,
                            "MotorDir(Motor." + setupOwner.Name + ",StartUpTimer.DN,StartUpTimer.DN," + ExtAlarms[0] + "," + ExtAlarms[1] + "," + ExtAlarms[2] + "," + ExtAlarms[3] + "," + ExtAlarms[4] + "," + ExtAlarms[5] + "," + ExtAlarms[6] + "," + ExtAlarms[7] + ",_IMotor." + setupOwner.Name + ".StartFW," + AuxGetIOName(motor.INRunningFB, ConnectionType.DI) + ",AlwaysON," + AuxGetIOName(motor.OUTStartSignal, ConnectionType.DO) + ",Clock.ScanTimeReal," + FaultNegMask.ToString() + ",2000,StartUpTimer);")
                            );
                    }

                    i++;
                }
            }

            //Add MotorRev handler calls
            if (_programsInfo.MRevSetups != null)
            {
                foreach (var motor in _programsInfo.MRevSetups)
                {
                    // Load owner component
                    var setupOwner = _programsInfo.Components.Single(c => c.Id == motor.IdComponent);
                    // Load owned DI alarms
                    var diAlarms = _programsInfo.DIAlarmSetups.Where(c => c.IdComponent == setupOwner.Id).ToList();

                    //Load External alarms with NO/NC mask
                    string[] ExtAlarms = new string[8];
                    Int32 FaultNegMask = ~0;
                    for (int j = 0; j < ExtAlarms.Length; j++)
                    {
                        if (diAlarms.Count > j)
                        {
                            ExtAlarms[j] = "IO.DI." + _programsInfo.IOs.Single(c => c.Id == diAlarms[j].IdIO).Name;

                            if (diAlarms[j].InputType == ViewModels.Enums.InputType.NC)
                            {
                                FaultNegMask &= ~(1 << j); //Reset bit on j position
                            }

                        }
                        else
                        {
                            ExtAlarms[j] = "AlwaysOFF";
                        }
                    }

                    //AOI CALL - first rung with title
                    if (i == 0)
                    {
                        motorRoutine.Element("RLLContent").Add(
                        AddRungWithTitle(i, "MOTOR HANDLERS", "Name: " + setupOwner.Name + "  -  Description: " + setupOwner.Comment,
                            "MotorRev(Motor." + setupOwner.Name + ",StartUpTimer.DN,StartUpTimer.DN," + ExtAlarms[0] + "," + ExtAlarms[1] + "," + ExtAlarms[2] + "," + ExtAlarms[3] + "," + ExtAlarms[4] + "," + ExtAlarms[5] + "," + ExtAlarms[6] + "," + ExtAlarms[7] + ",_IMotor." + setupOwner.Name + ".StartFW,_IMotor." + setupOwner.Name + ".StartBW," + AuxGetIOName(motor.INRunningFBFW, ConnectionType.DI) + "," + AuxGetIOName(motor.INRunningFBBW, ConnectionType.DI) + ",AlwaysON," + AuxGetIOName(motor.OUTStartSignalFW, ConnectionType.DO) + "," + AuxGetIOName(motor.OUTStartSignalBW, ConnectionType.DO) + ",Clock.ScanTimeReal," + FaultNegMask.ToString() + ",2000,StartUpTimer);")
                            );
                    }
                    else
                    {
                        motorRoutine.Element("RLLContent").Add(
                        AddRung(i, "Name: " + setupOwner.Name + "  -  Description: " + setupOwner.Comment,
                            "MotorRev(Motor." + setupOwner.Name + ",StartUpTimer.DN,StartUpTimer.DN," + ExtAlarms[0] + "," + ExtAlarms[1] + "," + ExtAlarms[2] + "," + ExtAlarms[3] + "," + ExtAlarms[4] + "," + ExtAlarms[5] + "," + ExtAlarms[6] + "," + ExtAlarms[7] + ",_IMotor." + setupOwner.Name + ".StartFW,_IMotor." + setupOwner.Name + ".StartBW," + AuxGetIOName(motor.INRunningFBFW, ConnectionType.DI) + "," + AuxGetIOName(motor.INRunningFBBW, ConnectionType.DI) + ",AlwaysON," + AuxGetIOName(motor.OUTStartSignalFW, ConnectionType.DO) + "," + AuxGetIOName(motor.OUTStartSignalBW, ConnectionType.DO) + ",Clock.ScanTimeReal," + FaultNegMask.ToString() + ",2000,StartUpTimer);")
                        );
                    }

                    i++;
                }
            }

            return motorRoutine;
        }
        //======================================================================================================
        public XElement GetRoutineValve()
        {
            if (_programsInfo.StdVlvSetups.Count <= 0)
                return new XElement("Empty");

            var valveRoutine = new XElement("Routine",
                new XAttribute("Name", "Valve"),
                new XAttribute("Type", "RLL"),
                new XElement("Description", "Valve handler (AOI) calls"),
                new XElement("RLLContent")
                );

            var i = 0; //Rung counter

            //Add Valve Control handler calls
            foreach (var valve in _programsInfo.StdVlvSetups)
            {
                // Load owner component
                var setupOwner = _programsInfo.Components.Single(c => c.Id == valve.IdComponent);
                // Load owned DI alarms
                var diAlarms = _programsInfo.DIAlarmSetups.Where(c => c.IdComponent == setupOwner.Id).ToList();

                //Load External alarms with NO/NC mask
                string[] ExtAlarms = new string[10];
                Int32 FaultNegMask = ~0;
                for (int j = 0; j < ExtAlarms.Length; j++)
                {
                    if (diAlarms.Count > j)
                    {
                        ExtAlarms[j] = "IO.DI." + _programsInfo.IOs.Single(c => c.Id == diAlarms[j].IdIO).Name;

                        if (diAlarms[j].InputType == ViewModels.Enums.InputType.NC)
                        {
                            FaultNegMask &= ~(1 << j); //Reset bit on j position
                        }

                    }
                    else
                    {
                        ExtAlarms[j] = "AlwaysOFF";
                    }
                }


                //First rung with title
                if (i == 0)
                {
                    valveRoutine.Element("RLLContent").Add(
                    AddRungWithTitle(i, "VALVE HANDLERS", "Name: " + setupOwner.Name + "  -  Description: " + setupOwner.Comment,
                        "ValveControl(Valve." + setupOwner.Name + "," +
                        "StartUpTimer.DN" + "," +
                        "StartUpTimer.DN" + "," +
                        "StartUpTimer.DN" + "," +
                        AuxGetIOName(valve.INOpenedFB, ConnectionType.DI) + "," +
                        AuxGetIOName(valve.INOpenedFB, ConnectionType.DI) + "," +
                        ExtAlarms[0] + "," +
                        ExtAlarms[1] + "," +
                        ExtAlarms[2] + "," +
                        ExtAlarms[3] + "," +
                        ExtAlarms[4] + "," +
                        ExtAlarms[5] + "," +
                        ExtAlarms[6] + "," +
                        ExtAlarms[7] + "," +
                        ExtAlarms[8] + "," +
                        ExtAlarms[9] + "," +
                        "_IValve." + setupOwner.Name + ".Open" + "," +
                        "_IValve." + setupOwner.Name + ".Close" + "," +
                        "AlwaysOFF" + "," +
                        "AlwaysOFF" + "," +
                        "AlwaysON" + "," +
                        AuxGetIOName(valve.OUTOpenSignal, ConnectionType.DO) + "," +
                        AuxGetIOName(valve.OUTCloseSignal, ConnectionType.DO) + "," +
                        "Clock.ScanTimeReal" + "," +
                        FaultNegMask.ToString() +
                        ");")
                    );
                }
                else
                {
                    valveRoutine.Element("RLLContent").Add(
                    AddRung(i, "Name: " + setupOwner.Name + "  -  Description: " + setupOwner.Comment,
                        "ValveControl(Valve." + setupOwner.Name + "," +
                        "StartUpTimer.DN" + "," +
                        "StartUpTimer.DN" + "," +
                        "StartUpTimer.DN" + "," +
                        AuxGetIOName(valve.INOpenedFB, ConnectionType.DI) + "," +
                        AuxGetIOName(valve.INOpenedFB, ConnectionType.DI) + "," +
                        ExtAlarms[0] + "," +
                        ExtAlarms[1] + "," +
                        ExtAlarms[2] + "," +
                        ExtAlarms[3] + "," +
                        ExtAlarms[4] + "," +
                        ExtAlarms[5] + "," +
                        ExtAlarms[6] + "," +
                        ExtAlarms[7] + "," +
                        ExtAlarms[8] + "," +
                        ExtAlarms[9] + "," +
                        "_IValve." + setupOwner.Name + ".Open" + "," +
                        "_IValve." + setupOwner.Name + ".Close" + "," +
                        "AlwaysOFF" + "," +
                        "AlwaysOFF" + "," +
                        "AlwaysON" + "," +
                        AuxGetIOName(valve.OUTOpenSignal, ConnectionType.DO) + "," +
                        AuxGetIOName(valve.OUTCloseSignal, ConnectionType.DO) + "," +
                        "Clock.ScanTimeReal" + "," +
                        FaultNegMask.ToString() +
                        ");")
                    );
                }

                i++;
            }


            return valveRoutine;
        }
        //======================================================================================================
        public XElement GetRoutineFrqConv()
        {
            if (_programsInfo.MotFrqSetups.Count <= 0)
                return new XElement("Empty");

            var frqConvRoutine = new XElement("Routine",
                new XAttribute("Name", "FrqConv"),
                new XAttribute("Type", "RLL"),
                new XElement("Description", "Frequencyconverter handler (AOI) calls"),
                new XElement("RLLContent")
                );

            var i = 0; //Rung counter

            //Add Frq handler calls
            if (_programsInfo.MotFrqSetups != null)
            {
                foreach (var motor in _programsInfo.MotFrqSetups)
                {
                    // Load owner component
                    var setupOwner = _programsInfo.Components.Single(c => c.Id == motor.IdComponent);

                    //First rung with title
                    if (i == 0)
                    {
                        frqConvRoutine.Element("RLLContent").Add(
                        AddRungWithTitle(i, "FREQUENCYCONVERTER HANDLERS", "Name: " + setupOwner.Name + "  -  Description: " + setupOwner.Comment,
                            "OTL(Frq." + setupOwner.Name + ".CTW.2)OTL(Frq." + setupOwner.Name + ".CTW.3)OTL(Frq." + setupOwner.Name + ".CTW.4)OTL(Frq." + setupOwner.Name + ".CTW.5)OTL(Frq." + setupOwner.Name + ".CTW.10)DanfossFC_FV2_0(Frq." + setupOwner.Name + "," + setupOwner.Name + ":I1.Data," + setupOwner.Name + ":O1.Data," + setupOwner.Name + ":I1.ConnectionFaulted, _IMotor." + setupOwner.Name + ".Fault_Reset,_IMotor." + setupOwner.Name + ".SpeedSP );")
                        );
                    }
                    else
                    {
                        frqConvRoutine.Element("RLLContent").Add(
                        AddRung(i, "Name: " + setupOwner.Name + "  -  Description: " + setupOwner.Comment,
                            "OTL(Frq." + setupOwner.Name + ".CTW.2)OTL(Frq." + setupOwner.Name + ".CTW.3)OTL(Frq." + setupOwner.Name + ".CTW.4)OTL(Frq." + setupOwner.Name + ".CTW.5)OTL(Frq." + setupOwner.Name + ".CTW.10)DanfossFC_FV2_0(Frq." + setupOwner.Name + "," + setupOwner.Name + ":I1.Data," + setupOwner.Name + ":O1.Data," + setupOwner.Name + ":I1.ConnectionFaulted, _IMotor." + setupOwner.Name + ".Fault_Reset,_IMotor." + setupOwner.Name + ".SpeedSP );")
                        );
                    }

                    i++;
                }
            }
            return frqConvRoutine;
        }

        //======================================================================================================
        //=======================================--------PROGRAM---------=======================================
        public XElement GetProgramOutput()
        {
            var outputProgram = new XElement("Program",
               new XAttribute("Name", "Output"),
                new XAttribute("TestEdits", "false"),
                new XAttribute("MainRoutineName", "A00_MainRoutine"),
                new XAttribute("Disabled", "false"),
                new XAttribute("UseAsFolder", "false"),

                new XElement("Description", "Copy data to physical layer"),

                    new XElement("Tags"),
                    new XElement("Routines",
                        GetRoutineDigitalOutput(),
                        GetRoutineAnalogOutput()
                    )
               );

            //Navigate to Routines --> add main routine
            outputProgram.Element("Routines").Add(AddMainRoutine(outputProgram.Element("Routines")));

            return outputProgram;
        }

        private XElement GetRoutineDigitalOutput()
        {
            var routineOutput = new XElement("Routine",
                new XAttribute("Name", "A01_DigitalOutput"),
                new XElement("Description", "Copy DO data from IO tag to physical layer"),
                new XAttribute("Type", "ST")
            );

            var stContent = new XElement("STContent");

            stContent.Add(AddLine(0, "//             DIGITAL OUTPUT DATA MASKING"));
            stContent.Add(AddLine(1, "//"));
            stContent.Add(AddLine(2, "//"));


            foreach (Module module in _programsInfo.Modules)
            {
                switch (module.IOModulesType)
                {
                    case ViewModels.Enums.IOModulesType.EmbDIx16:
                        continue;//Handled in Program:Input Routine:A01_DigitalInput
                    case ViewModels.Enums.IOModulesType.EmbDOx16:
                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//    Rack:01    Module:0" + module.Address + "    PLC embeded DOx16"));
                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));

                        //Filter IOs from this module - NOTE! same module has DO too!
                        var DOEmbIOs = _programsInfo.IOs.Where(c => (c.IOAddress.Module == module.Address && c.ConnectionType == ViewModels.Enums.ConnectionType.DO));

                        //Copy each IO channel to IO, or copy do Disp.
                        for (int i = 0; i < 16; i++)
                        {
                            var io = DOEmbIOs.SingleOrDefault(c => c.IOAddress.Channel == i);
                            String line = System.String.Empty;
                            String comment = System.String.Empty;
                            if (io == null)
                            {
                                //No IO on this output add Disp.
                                line += "Local:" + module.Address.ToString() + ":O.Data." + i.ToString() +":=Disp.DO" + ";";
                                comment = "Disponsibel IO";
                            }
                            else
                            {
                                line += "Local:" + module.Address.ToString() + ":I.Data." + i.ToString() + ":=IO.DO." + io.Name + ";";
                                comment = io.Comment;
                            }

                            //Fill with spaces until comment-all comments start in same place
                            int count = line.Count();
                            for (int j = count; j < 70; j++)
                            {
                                line += " ";
                            }

                            line += "//    CH:" + i.ToString("D2") + "    " + comment;

                            stContent.Add(AddLine(stContent.Descendants("Line").Count(), line));

                        }

                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));
                        break;
                    case ViewModels.Enums.IOModulesType.DIx4:
                        continue;//Handled in Program:Input Routine:A01_DigitalInput
                    case ViewModels.Enums.IOModulesType.DIx8:
                        continue;//Handled in Program:Input Routine:A01_DigitalInput
                    case ViewModels.Enums.IOModulesType.DOx4:
                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//    Rack:01    Module:0" + module.Address + "    DOx4 module: " + module.Name));
                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));

                        //Filter IOs from this module - NOTE! same module has DO too!
                        var DOx4IOs = _programsInfo.IOs.Where(c => (c.IOAddress.Module == module.Address && c.ConnectionType == ViewModels.Enums.ConnectionType.DO));

                        //Copy each IO channel to IO, or copy do Disp.
                        for (int i = 0; i < 4; i++)
                        {
                            var io = DOx4IOs.SingleOrDefault(c => c.IOAddress.Channel == i);
                            String line = System.String.Empty;
                            String comment = System.String.Empty;
                            if (io == null)
                            {
                                //No IO on this output - add Disp.
                                line += "Local:" + module.Address.ToString() + ":O.Data." + i.ToString() + ":=Disp.DO; ";
                                comment = "Disponsibel IO";
                            }
                            else
                            {
                                line += "Local:" + module.Address.ToString() + ":O.Data." + i.ToString() + ":=IO.DO." + io.Name + ";";
                                comment = io.Comment;
                            }

                            //Fill with spaces until comment - comments start in same line
                            int count = line.Count();
                            for (int j = count; j < 70; j++)
                            {
                                line += " ";
                            }

                            line += "//    CH:" + i.ToString("D2") + "    " + comment;

                            stContent.Add(AddLine(stContent.Descendants("Line").Count(), line));

                        }

                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));
                        break;
                    case ViewModels.Enums.IOModulesType.DOx8:
                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//    Rack:01    Module:0" + module.Address + "    DOx8 module: " + module.Name));
                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));

                        //Filter IOs from this module
                        var DOx8IOs = _programsInfo.IOs.Where(c => c.IOAddress.Module == module.Address);

                        for (int i = 0; i < 8; i++)
                        {
                            var io = DOx8IOs.SingleOrDefault(c => c.IOAddress.Channel == i);
                            String line = System.String.Empty;
                            String comment = System.String.Empty;
                            if (io == null)
                            {
                                //No IO on this input add Disp.
                                line += "Local:" + module.Address.ToString() + ":O.Data." + i.ToString() + ":=Disp.DO" + ";";
                                comment = "Disponsibel IO";
                            }
                            else
                            {
                                line += "Local:" + module.Address.ToString() + ":O.Data." + i.ToString() + ":=IO.DO." + io.Name + ";";
                                comment = io.Comment;
                            }

                            //Fill with spaces until comment
                            int count = line.Count();
                            for (int j = count; j < 70; j++)
                            {
                                line += " ";
                            }

                            line += "//    CH:" + i.ToString("D2") + "    " + comment;

                            stContent.Add(AddLine(stContent.Descendants("Line").Count(), line));

                        }

                        stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));
                        break;
                    case ViewModels.Enums.IOModulesType.AIx4:
                        continue;//Handled in Program:Analog Routine:A01_AnalogInput
                    case ViewModels.Enums.IOModulesType.AIx8:
                        continue;//Handled in Program:Analog Routine:A01_AnalogInput
                    case ViewModels.Enums.IOModulesType.AOx4:
                        continue;//Handled in Program:Output Routine:A02_AnalogOutput
                    default:
                        throw new Exception("New module not added to MainTask/Input/A01_Input routine generator");

                }
            }

            //Add ETH_DO as TODO

            //Filter ETH_DO
            var ETH_DOios = _programsInfo.IOs.Where(c => c.ConnectionType == ConnectionType.DO && c.IOAddress.Type == IOType.IP).ToList();

            if (ETH_DOios.Count != 0)
            {
                stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//    Custom ethernet module I/Os"));
                stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));

                foreach (var io in ETH_DOios)
                {
                    String line = System.String.Empty;

                    line +=  "TODO.BOOL:=IO.DO." + io.Name +  ";";

                    //Fill with spaces until comment
                    int count = line.Count();
                    for (int j = count; j < 70; j++)
                    {
                        line += " ";
                    }

                    line += "//    CH:ETH    " + io.Comment;

                    stContent.Add(AddLine(stContent.Descendants("Line").Count(), line));
                }
            }

            routineOutput.Add(stContent);
            return routineOutput;

        }

        private XElement GetRoutineAnalogOutput()
        {
            var routineAnalogOutput = new XElement("Routine",
                new XAttribute("Name", "A02_AnalogOutput"),
                new XElement("Description", "Copy AO data from IO tag to physical layer"),
                new XAttribute("Type", "ST")
            );

            var stContent = new XElement("STContent");

            stContent.Add(AddLine(0, "//             ANALOG OUTPUT DATA MASKING"));
            stContent.Add(AddLine(1, "//"));
            stContent.Add(AddLine(2, "//"));


            foreach (Module module in _programsInfo.Modules.Where(c => c.IOModulesType == IOModulesType.AOx4))
            {
                stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//    Rack:01    Module:0" + module.Address + "    AOx4 module: " + module.Name));
                stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));

                //Filter IOs from this module
                var AOx4IOs = _programsInfo.IOs.Where(c => c.IOAddress.Module == module.Address);

                for (int i = 0; i < 8; i++)
                {
                    var io = AOx4IOs.SingleOrDefault(c => c.IOAddress.Channel == i);
                    String line = System.String.Empty;
                    String comment = System.String.Empty;
                    if (io == null)
                    {
                        //No IO on this input add Disp.
                        line += "Local:" + io.IOAddress.Module.ToString() + ":O.Ch" + io.IOAddress.Channel.ToString() + "Data" + ":=Disp.AO" + ";";
                        comment = "Disponsibel IO";
                    }
                    else
                    {
                        line += "Local:" + io.IOAddress.Module.ToString() + ":O.Ch" + io.IOAddress.Channel.ToString() + "Data" + ":=IO.AO." + io.Name + ";";
                        comment = io.Comment;
                    }

                    //Fill with spaces until comment
                    int count = line.Count();
                    for (int j = count; j < 70; j++)
                    {
                        line += " ";
                    }

                    line += "//    CH:" + i.ToString("D2") + "    " + comment;

                    stContent.Add(AddLine(stContent.Descendants("Line").Count(), line));

                }

                stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));
            }

            //Add ETH_AO as TODO

            //Filter ETH_AO
            var ETH_AOios = _programsInfo.IOs.Where(c => c.ConnectionType == ConnectionType.AO && c.IOAddress.Type == IOType.IP).ToList();

            if (ETH_AOios.Count != 0)
            {
                stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//    Custom ethernet module I/Os"));
                stContent.Add(AddLine(stContent.Descendants("Line").Count(), "//"));

                foreach (var io in ETH_AOios)
                {
                    String line = System.String.Empty;

                    line += "TODO.INT:=IO.AO." + io.Name + ";";

                    //Fill with spaces until comment
                    int count = line.Count();
                    for (int j = count; j < 70; j++)
                    {
                        line += " ";
                    }

                    line += "//    CH:ETH    " + io.Comment;

                    stContent.Add(AddLine(stContent.Descendants("Line").Count(), line));
                }
            }

            routineAnalogOutput.Add(stContent);
            return routineAnalogOutput;
        }
        //======================================================================================================
        //=======================================--------PROGRAM---------=======================================
        public XElement GetProgramAnalog()
        {
            var analogProgram = new XElement("Program",
               new XAttribute("Name", "Analog"),
                new XAttribute("TestEdits", "false"),
                new XAttribute("MainRoutineName", "A00_MainRoutine"),
                new XAttribute("Disabled", "false"),
                new XAttribute("UseAsFolder", "false"),

                new XElement("Description", "Copy analog data from physical layer - Scaling - Alarming"),

                    new XElement("Tags"),
                    new XElement("Routines")
               );

            var analogRoutine = GetRoutineAnalog();
            //Check if it exist
            if (analogRoutine.Name != "Empty")
            {
                analogProgram.Element("Routines").Add(analogRoutine);
            }
            else
            {
                analogProgram.Element("Routines").Add(AddRoutine("A01_Analog", "Analog input scaling and alarming"));
            }
            
            //Navigate to Routines --> add main routine
            analogProgram.Element("Routines").Add(AddMainRoutine(analogProgram.Element("Routines")));


            return analogProgram;
        }
        //======================================================================================================
        private XElement GetRoutineAnalog()
        {
            if (_programsInfo.AIAlarmSetups.Count <= 0)
                return new XElement("Empty");

            var AIAlarmRoutine = new XElement("Routine",
                new XAttribute("Name", "A01_Analog"),
                new XAttribute("Type", "RLL"),
                new XElement("Description", "Copy AI data from physical layer to IO tag - Scaling - Alarming"),
                new XElement("RLLContent")
                );

            
            var i = 0; //Rung counter

            //Add handler for each AI
            foreach (var AIAlarm in _programsInfo.AIAlarmSetups)
            {
                //Load IO
                var io = _programsInfo.IOs.Single(c => c.Id == AIAlarm.IdIO);
                
                //Check data
                String MinScaleIN;
                String MaxScaleIN;
                String Input;

                if (io.ConnectionType == ConnectionType.AI && io.IOAddress.Type == IOType.IP)
                {
                    MinScaleIN = "TODO.INT";
                    MaxScaleIN = "TODO.INT";
                    Input = "TODO.INT";


                }
                else
                {
                    MinScaleIN = "3227";
                    MaxScaleIN = "16383";
                    Input = "Local:" + io.IOAddress.Module.ToString() + ":I.Ch" + io.IOAddress.Channel.ToString() + "Data";
                }

                //Fill up data for AOI call
                if (i == 0)
                {
                    AIAlarmRoutine.Element("RLLContent").Add(
                    AddRungWithTitle(i, "ANALOG SCALING & ALARM HANDLERS", "Name: " + io.Name + "  -  Description: " + io.Comment,
                    "Analog(AlarmAI." + io.Name + "," +
                    Input + "," +
                    "1" + "," +
                    MinScaleIN + "," +
                    MaxScaleIN + "," +
                    (AIAlarm.ScaleMin.HasValue ? AIAlarm.ScaleMin.ToString() : "TODO.INT") + "," +
                    (AIAlarm.ScaleMax.HasValue ? AIAlarm.ScaleMax.ToString() : "TODO.INT") + "," +
                    (AIAlarm.AlarmLL.HasValue ? "0" : "1") + "," +
                    (AIAlarm.AlarmLL.HasValue ? AIAlarm.AlarmLL.ToString() : "0") + "," +
                    (AIAlarm.AlarmL.HasValue ? "0" : "1") + "," +
                    (AIAlarm.AlarmL.HasValue ? AIAlarm.AlarmL.ToString() : "0") + "," +
                    (AIAlarm.AlarmH.HasValue ? "0" : "1") + "," +
                    (AIAlarm.AlarmH.HasValue ? AIAlarm.AlarmH.ToString() : "0") + "," +
                    (AIAlarm.AlarmHH.HasValue ? "0" : "1") + "," +
                    (AIAlarm.AlarmHH.HasValue ? AIAlarm.AlarmHH.ToString() : "0") + "," +
                    "IO.AI." + io.Name +
                    ");")
                    );
                }
                else
                {
                    AIAlarmRoutine.Element("RLLContent").Add(
                    AddRung(i, "Name: " + io.Name + "  -  Description: " + io.Comment,
                    "Analog(AlarmAI." + io.Name + "," +
                    Input + "," +
                    "1" + "," +
                    MinScaleIN + "," +
                    MaxScaleIN + "," +
                    (AIAlarm.ScaleMin.HasValue ? AIAlarm.ScaleMin.ToString() : "TODO.INT") + "," +
                    (AIAlarm.ScaleMax.HasValue ? AIAlarm.ScaleMax.ToString() : "TODO.INT") + "," +
                    (AIAlarm.AlarmLL.HasValue ? "0" : "1") + "," +
                    (AIAlarm.AlarmLL.HasValue ? AIAlarm.AlarmLL.ToString() : "0") + "," +
                    (AIAlarm.AlarmL.HasValue ? "0" : "1") + "," +
                    (AIAlarm.AlarmL.HasValue ? AIAlarm.AlarmL.ToString() : "0") + "," +
                    (AIAlarm.AlarmH.HasValue ? "0" : "1") + "," +
                    (AIAlarm.AlarmH.HasValue ? AIAlarm.AlarmH.ToString() : "0") + "," +
                    (AIAlarm.AlarmHH.HasValue ? "0" : "1") + "," +
                    (AIAlarm.AlarmHH.HasValue ? AIAlarm.AlarmHH.ToString() : "0") + "," +
                    "IO.AI." + io.Name +
                    ");")
                    );
                    
                }

                i++;


            }

            //Generate rung with Disp. AI inputs
            var modulesAI = _programsInfo.Modules.Where(c => (c.IOModulesType == IOModulesType.AIx4 || c.IOModulesType == IOModulesType.AIx8));
            String code = System.String.Empty;

            if (modulesAI.Count() != 0)
            {
                foreach (var moduleAI in modulesAI)
                {
                    switch (moduleAI.IOModulesType)
                    {
                        case IOModulesType.AIx4:
                            for (int k = 0; k < 4; k++)
                            {
                                //If there is no IO with assigned channel add as disp.
                                if (!_programsInfo.IOs.Any(c => c.IOAddress.Module == moduleAI.Address && c.IOAddress.Channel == k))
                                {
                                    code += "MOV(Local:" + moduleAI.Address.ToString() + ":I.Ch" + k + "Data,Disp.AI)";
                                }
                            }
                            break;
                        case IOModulesType.AIx8:
                            for (int j = 0; j < 8; j++)
                            {
                                //If there is no IO with assigned channel add as disp.
                                if (!_programsInfo.IOs.Any(c => c.IOAddress.Module == moduleAI.Address && c.IOAddress.Channel == j))
                                {
                                    code += "MOV(Local:" + moduleAI.Address.ToString() + ":I.Ch" + j + "Data,Disp.AI)";
                                }
                            }
                            break;

                        default:
                            break;
                    }

                }
            }

            code += ";";
            AIAlarmRoutine.Element("RLLContent").Add(AddRung(i, "Not used analog inputs" , code));


            return AIAlarmRoutine;
        }
        //======================================================================================================
        //======================================================================================================
        //Add LAD rung
        private XElement AddRung(int number, string comment, string code)
        {
            var rung = new XElement("Rung",
                new XAttribute("Number", number),
                new XAttribute("Type", "N"),

                    new XElement("Comment", comment),
                    new XElement("Text", code)
                );

            return rung;
        }
        //Add SL command line
        private XElement AddLine(int number, string code)
        {
            var line = new XElement("Line",
                new XAttribute("Number", number),
                code);

            return line;
        }

        //Create Main routine with routine calls
        private XElement AddMainRoutine(XElement Routines)
        {
            var mainRoutine = new XElement("Routine",
                        new XAttribute("Name", "A00_MainRoutine"),
                        new XAttribute("Type", "RLL")
                );

            var RLLContent = new XElement("RLLContent");

            int i = 0;
            foreach (var routine in Routines.Descendants().Where(c=>c.Name == "Routine"))
            {
                RLLContent.Add(AddRung(i, routine.Attribute("Name").Value + " routine call", "JSR(" + routine.Attribute("Name").Value + ",0);"));
                i++;
            }

            mainRoutine.Add(RLLContent);
            return mainRoutine;
        }

        //Create routine
        private XElement AddRoutine(string name, string description)
        {
            var routine = new XElement("Routine",
                new XAttribute("Name", name),
                new XAttribute("Type", "RLL"),

                new XElement("RLLContent",
                    new XElement("Rung",
                    new XAttribute("Number", "0"),
                    new XAttribute("Type", "N"),
                    new XElement("Comment", HeaderComment(name + "-" + description)),
                    new XElement("Text", "NOP();")

                    )
                )    
            );

            return routine;
        }

        //Header comment template
        private String HeaderComment(string comment)
        {
            var headerComment = System.String.Empty;
            headerComment += "-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-";
            headerComment += System.Environment.NewLine;
            headerComment += comment;
            headerComment += System.Environment.NewLine;
            headerComment += "-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-";

            return headerComment;
        }

        //Add rung with title comment
        private XElement AddRungWithTitle(int rungNumber, string title, string description, string code)
        {
            if (String.IsNullOrEmpty(code) || String.IsNullOrWhiteSpace(code))
                code = "NOP();";

            return new XElement("Rung",
                    new XAttribute("Number", rungNumber.ToString()),
                    new XAttribute("Type", "N"),
                    new XElement("Comment", HeaderComment(title) + System.Environment.NewLine + description),
                    new XElement("Text", code)

                    );
        }

        //Used for AOI call, returns formated IO TAG name or TODO TAG
        private String AuxGetIOName(int? IOID, ConnectionType type)
        {
            var IO = new IO();
            String name;

            if (IOID == null)
            {
                //If does not exist in setup
                switch (type)
                {
                    case ConnectionType.AI:
                        return "TODO.INT";
                    case ConnectionType.AO:
                        return "TODO.INT";
                    case ConnectionType.DI:
                        return "TODO.BOOL";
                    case ConnectionType.DO:
                        return "TODO.BOOL";
                    case ConnectionType.ETH:
                        return "TODO";
                    case ConnectionType.DIO:
                        return "TODO";
                    case ConnectionType.No_Connection_Type:
                        return "TODO";
                    default:
                        return "TODO";
                }


            }
            else
            {
                IO = _programsInfo.IOs.SingleOrDefault(c => c.Id == IOID);
                if (IO == null)
                {
                    switch (type)
                    {
                        case ConnectionType.AI:
                            return "TODO.INT";
                        case ConnectionType.AO:
                            return "TODO.INT";
                        case ConnectionType.DI:
                            return "TODO.BOOL";
                        case ConnectionType.DO:
                            return "TODO.BOOL";
                        case ConnectionType.ETH:
                            return "TODO";
                        case ConnectionType.DIO:
                            return "TODO";
                        case ConnectionType.No_Connection_Type:
                            return "TODO";
                        default:
                            return "TODO";
                    }
                }
                else
                {
                    String IOConnType;

                    switch (IO.ConnectionType)
                    {
                        case ConnectionType.AI:
                            IOConnType = "AI";
                            break;
                        case ConnectionType.AO:
                            IOConnType = "AO";
                            break;
                        case ConnectionType.DI:
                            IOConnType = "DI";
                            break;
                        case ConnectionType.DO:
                            IOConnType = "DO";
                            break;
                        case ConnectionType.ETH:
                            return "TODO";
                        case ConnectionType.DIO:
                            return "TODO";
                        case ConnectionType.No_Connection_Type:
                            return "TODO";
                        default:
                            return "TODO";
                    }
                    name = "IO." + IOConnType + "." + IO.Name;
                }

            }

            return name;
        }
    }
}