using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace PLC_GenCo.Generator
{
    public class UploadRoutine
    {
        public string DirectoryPath { get; set; }
        public string Name { get; set; }
        public XElement Routine { get; set; }
        public XElement Tags { get; set; }


        public UploadRoutine(string directoryPath, string name)
        {
            DirectoryPath = directoryPath;
            Name = name;


        }

        public bool Read()
        {
            Uri uri;

            try
            {
                uri = new System.Uri(DirectoryPath + Name + ".L5X");
            }
            catch (Exception)
            {

                return false;
            }
            

            var l5kFile = XElement.Load(uri.ToString());


            Routine = l5kFile.Element("Controller").Element("Programs").Element("Program").Element("Routines").Element("Routine");

            Tags = l5kFile.Element("Controller").Element("Tags");


            return true;
        }
    }
}