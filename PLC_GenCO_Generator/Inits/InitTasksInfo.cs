using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_GenCO_Generator.Inits
{
    class InitTasksInfo
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
