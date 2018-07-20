using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace PLC_GenCo.Generator
{
    public class GenerateTasks
    {
        private TasksInfo _tasksInfo;

        public GenerateTasks(TasksInfo tasksInfo)
        {
            _tasksInfo = tasksInfo;
        }

        public XElement GetTasks()
        {
            var tasks = new XElement("Tasks"
                );

            var mainTask = new XElement("Task",
                new XAttribute("Name", "MainTask"),
                new XAttribute("Type", "CONTINUOUS"),
                new XAttribute("Priority", "10"),
                new XAttribute("Watchdog", "500"),
                new XAttribute("DisableUpdateOutputs", "false"),
                new XAttribute("InhibitTask", "false"),

                new XElement("ScheduledPrograms",
                    new XElement("ScheduledProgram",
                        new XAttribute("Name", "Input")
                        ),
                    new XElement("ScheduledProgram",
                        new XAttribute("Name", "General")
                    ),
                    new XElement("ScheduledProgram",
                        new XAttribute("Name", "Control")
                        ),
                    new XElement("ScheduledProgram",
                        new XAttribute("Name", "Component")
                        ),
                    new XElement("ScheduledProgram",
                        new XAttribute("Name", "Output")
                        )
                    )
                );

            var slowTask = new XElement("Task",
                new XAttribute("Name", "Periodic_1s"),
                new XAttribute("Type", "PERIODIC"),
                new XAttribute("Rate", "1000"),
                new XAttribute("Priority", "10"),
                new XAttribute("Watchdog", "500"),
                new XAttribute("DisableUpdateOutputs", "false"),
                new XAttribute("InhibitTask", "false"),

                new XElement("ScheduledPrograms",
                    new XElement("ScheduledProgram",
                    new XAttribute("Name", "Analog")
                        )
                    )
                );

            tasks.Add(mainTask);
            tasks.Add(slowTask);

            return tasks;
        }
    }
}