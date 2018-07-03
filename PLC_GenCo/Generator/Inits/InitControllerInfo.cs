using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Generator.Inits
{
    public class InitControllerInfo
    {
        public ControllerInfo ControllerInfo { get; }

        public InitControllerInfo(ControllerInfo controllerInfo)
        {
            ControllerInfo = controllerInfo;
        }

        public ControllerInfo InitializedData()
        {
            var controllerInfoChecked = new ControllerInfo();

            if (String.IsNullOrWhiteSpace(ControllerInfo.name))
            {
                controllerInfoChecked.name = "PLC";
            }
            else
            {
                controllerInfoChecked.name = ControllerInfo.name;
            }

            switch (ControllerInfo.procesorType)
            {
                case ViewModels.Enums.ControllerType.L16ER:
                    controllerInfoChecked.catalogNumber = "1769-L16ER-BB1B";
                    controllerInfoChecked.productCode = "153"; 
                    break;
                case ViewModels.Enums.ControllerType.L18ER:
                    controllerInfoChecked.catalogNumber = "1769-L18ER-BB1B";
                    controllerInfoChecked.productCode = "154";
                    break;
                default:
                    controllerInfoChecked.catalogNumber = "1769-L18ER-BB1B";
                    controllerInfoChecked.productCode = "154";
                    break;
            }

            if (String.IsNullOrWhiteSpace(ControllerInfo.majorRev))
            {
                controllerInfoChecked.majorRev = "24";
            }
            else
            {
                controllerInfoChecked.majorRev = ControllerInfo.majorRev;
            }

            if (String.IsNullOrWhiteSpace(ControllerInfo.minorRev))
            {
                controllerInfoChecked.minorRev = "11";
            }
            else
            {
                controllerInfoChecked.minorRev = ControllerInfo.minorRev;
            }

            if (String.IsNullOrWhiteSpace(ControllerInfo.description))
            {
                controllerInfoChecked.description = controllerInfoChecked.name;
            }
            else
            {
                controllerInfoChecked.description = ControllerInfo.description;
            }

            return controllerInfoChecked;
        }
    }
}