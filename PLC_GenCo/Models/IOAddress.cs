using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.Models
{
    public class IOAddress
    {
        public IOType Type {get; set;}
        public string IPorMBAddress { get; set; }
        public int Rack { get; set; }
        public int Module { get; set; }
        public int Channel { get; set; }

        public IOAddress()
        {
            Type = IOType.IO;
            IPorMBAddress = String.Empty;
            Rack = 0;
            Module = 0;
            Channel = 0;    
        }

        public IOAddress(String PLCAddress)
        {
            if (String.IsNullOrEmpty(PLCAddress) || String.IsNullOrWhiteSpace(PLCAddress))
            {
                Type = IOType.IO;
                IPorMBAddress = String.Empty;
                Rack = 0;
                Module = 0;
                Channel = 0;
                return;
            }

            String[] separated = PLCAddress.Split(':');

            switch (separated[0])
            {
                case ("IO"):
                    Type = IOType.IO;

                    if (separated.Count() > 3)              //case format with rack RR:MM:CC
                    {
                        Rack = Convert.ToInt32(separated[1]);
                        Module = Convert.ToInt32(separated[2]);
                        Channel = Convert.ToInt32(separated[3]);
                    }
                    else
                    {                                       //case format without rack MM:CC
                        Rack = 1;
                        Module = Convert.ToInt32(separated[1]);
                        Channel = Convert.ToInt32(separated[2]);
                    }
                    break;
                case ("IP"):
                    Type = IOType.IP;
                    IPorMBAddress = separated[1];
                    break;
                case ("MB"):
                    Type = IOType.MB;
                    IPorMBAddress = separated[1];
                    break;
                default:
                    throw new Exception("PLC address type not recognized - should be IO/IP/MB");
            }


           

        }
    }
}