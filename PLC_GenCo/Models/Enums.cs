using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace PLC_GenCo.ViewModels
{
    public static class Enums
    {

        public enum StandardComponent
        {
            [Display(Name = "No Match")]
            No_Match,

            [Display(Name = "DI Alarm")]
            DI_Alarm,

            [Display(Name = "DI Poulse")]
            DI_Pulse,

            [Display(Name = "DO Single")]
            DO,

            [Display(Name = "AI Alarm")]
            AI_Alarm,

            //Parent components
            [Display(Name = "Standard Motor DI")]
            P_Std_Motor_Dir,

            [Display(Name = "Standard Motor FRQ")]
            P_Std_Motor_Dir_Frq,

            [Display(Name = "Standard Motor Rev DI")]
            P_Std_Motor_Rev,

            [Display(Name = "Standard Motor Rev FRQ")]
            P_Std_Motor_Rev_Frq,

            [Display(Name = "Standard Valve")]
            P_Std_Valve,

            // Child Components
            [Display(Name = "(C) AI")]
            C_AI,

            [Display(Name = "(C) AO")]
            C_AO,

            [Display(Name = "(C) DO")]
            C_DO,

            [Display(Name = "(C) DI")]
            C_DI,

        }

        public enum ConnectionType
        {
            AI,
            AO,
            DI,
            DO,
            ETH,
            ETH_DI,
            ETH_DO,
            ETH_AI,
            ETH_AO,
            DIO,

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

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>()
                           .Name;
        }

        
    }


}