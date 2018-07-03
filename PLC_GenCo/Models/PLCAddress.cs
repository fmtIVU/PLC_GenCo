using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Models
{
    public class PLCAddress
    {
        public int Rack { get; set; }
        public int Module { get; set; }
        public int Channel { get; set; }

        public PLCAddress()
        {
            Rack = 0;
            Module = 0;
            Channel = 0;    
        }

        public PLCAddress(String PLCAddress)
        {
            if (String.IsNullOrEmpty(PLCAddress) || String.IsNullOrWhiteSpace(PLCAddress))
            {
                Rack = 0;
                Module = 0;
                Channel = 0;
                return;
            }

            String[] separated = PLCAddress.Split(':');


            if (separated.Count() > 2)              //case format with rack RR:MM:CC
            {
                Rack = Convert.ToInt32(separated[0]);
                Module = Convert.ToInt32(separated[1]);
                Channel = Convert.ToInt32(separated[2]);
            }
            else
            {                                       //case format without rack MM:CC
                Rack = 1;
                Module = Convert.ToInt32(separated[0]);
                Channel = Convert.ToInt32(separated[1]);
            }

        }
    }
}