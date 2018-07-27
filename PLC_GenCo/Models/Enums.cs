using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace PLC_GenCo.ViewModels
{
    interface IHeaderInfo
    {
        string PageName { get; set; }
    }

    public static class Enums
    {

        public enum DataType
        {
            BOOL,
            INT,
            REAL,
            Other,
        }

        public enum InOut
        {
            Input,
            Output,
            ImputOutput,
        }

        public enum ParType
        {
            IO,
            TODO,
            Constant,
            Parameter,
            Interface,

        }

        public enum Dependancy
        {
            Single,
            Parent,
            Child,
        }

        public enum FrqType
        {
            [Display(Name = "Danfoss VLT FC 202")]
            FC202,

            [Display(Name = "Danfoss VLT FC 302")]
            FC302,

            [Display(Name = "Danfoss VLT FC 301")]
            FC301,
        }

        public enum ConnectionType
        {
            [Display(Name = "Analog input")]
            AI,
            [Display(Name = "Analog output")]
            AO,
            [Display(Name = "Digital input")]
            DI,
            [Display(Name = "Digital output")]
            DO,
            [Display(Name = "Ethernet")]
            ETH,
            [Display(Name = "Digital IO")]
            DIO,
            [Display(Name = "No connection type")]
            No_Connection_Type,

        }

        public enum ControllerType
        {
            L16ER,
            L18ER
        }

        public enum IOModulesType
        {
            EmbDIx16,
            EmbDOx16,
            DIx4,
            DIx8,
            DOx4,
            DOx8,
            AIx4,
            AIx8,
            AOx4,
            
        }
        public enum MatchStatus
        {
            [Display(Name = "No Match")]
            No_Match,

            [Display(Name = "Check")]
            Check,

            [Display(Name = "Match")]
            Match,
        }

        public enum InputType
        {
            [Display(Name = "Normally closed")]
            NC,

            [Display(Name = "Normally open")]
            NO,
        }

        public enum AICType
        {
            [Display(Name = "AI 4-20mA")]
            AI0420,

            [Display(Name = "AI 0-20mA")]
            AI0020,

            [Display(Name = "Digital (ETH)")]
            DIGITAL,
        }

        public enum IOType
        {
            [Display(Name = "Ethernet module")]
            IP,
            [Display(Name = "PLC IO module")]
            IO,
            [Display(Name = "Modbus module")]
            MB
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>()
                           .Name;
        }


        
    }


}