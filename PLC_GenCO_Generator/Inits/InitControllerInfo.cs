using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_GenCO_Generator.Inits
{
    
    class InitControllerInfo
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

            if (String.IsNullOrWhiteSpace(ControllerInfo.procesorType))
            {
                controllerInfoChecked.procesorType = "1769-L16ER-BB1B";
            }
            else
            {
                controllerInfoChecked.procesorType = ControllerInfo.procesorType;
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
