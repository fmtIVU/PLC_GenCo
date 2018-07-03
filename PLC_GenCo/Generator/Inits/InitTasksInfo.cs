using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Generator.Inits
{
    public class InitTasksInfo
    {
        public TasksInfo TasksInfo { get; }

        public InitTasksInfo(TasksInfo tasksInfo)
        {
            TasksInfo = tasksInfo;
        }

        public TasksInfo InitializedData()
        {

            return new TasksInfo();
        }
    }
}