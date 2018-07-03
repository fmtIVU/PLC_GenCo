using PLC_GenCo.Models;
using PLC_GenCo.ViewModels;
using PLC_GenCO_Generator.Generators;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_GenCO_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var _context = new GeneratorDbContext(); //Database


            var controllerinfo = new ControllerInfo();
            
            var datatablesinfo = new DataTablesInfo();

            var globaltagsinfo = new GlobalTagsInfo();
            var modulesinfo = new ModulesInfo();
            var programsinfo = new ProgramsInfo();
            var tasksinfo = new TasksInfo();
            var addondefinitionsinfo = new AddOnDefinitionsInfo();

  

            var generator = new Generator(controllerinfo, datatablesinfo, modulesinfo, addondefinitionsinfo, globaltagsinfo,   programsinfo, tasksinfo);


            //Console.WriteLine(generator.GenerateProject());
            // Create a file to write to.
            string createText = generator.GenerateProject().ToString();
            File.WriteAllText("C:/Users/Ivan/Desktop/OP generator PLC koda/generated_files/generatedfile.xml", createText);

            return;
        }

        public class GeneratorDbContext : DbContext
        {
            public GeneratorDbContext() : base("name=Context")
            {

            }

        }
    }
}
