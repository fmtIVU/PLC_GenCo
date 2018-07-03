using PLC_GenCo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLC_GenCo.Generator.Inits
{
    public class InitModulesInfo
    {
        public ModulesInfo ModulesInfo { get; }

        public InitModulesInfo(ModulesInfo modulesInfo)
        {
            ModulesInfo = modulesInfo;
        }

        public ModulesInfo InitializedData()
        {
            var modulesInfoChecked = new ModulesInfo { modules = new List<Module>(), controller = new ControllerInfo() };

            var initControllerInfo = new InitControllerInfo(ModulesInfo.controller);

            modulesInfoChecked.controller = initControllerInfo.InitializedData();   //init controller, already made in InitControllerInfo




            //init modules TODO
            foreach (Module module in ModulesInfo.modules)
            {
                modulesInfoChecked.modules.Add(module);
            }

            foreach (Module module in modulesInfoChecked.modules)                    //init module tags comment
            {
                var length = 0;

                switch (module.IOModulesType)
                {
                    case ViewModels.Enums.IOModulesType.EmbDIx16:
                        length = 16;
                        break;
                    case ViewModels.Enums.IOModulesType.EmbDOx16:
                        length = 16;
                        break;
                    case ViewModels.Enums.IOModulesType.DIx4:
                        length = 4;
                        break;
                    case ViewModels.Enums.IOModulesType.DIx8:
                        length = 8;
                        break;
                    case ViewModels.Enums.IOModulesType.DOx4:
                        length = 4;
                        break;
                    case ViewModels.Enums.IOModulesType.DOx8:
                        length = 8;
                        break;
                    case ViewModels.Enums.IOModulesType.AIx4:
                        length = 4;
                        break;
                    case ViewModels.Enums.IOModulesType.AIx8:
                        length = 8;
                        break;
                    case ViewModels.Enums.IOModulesType.AOx4:
                        length = 4;
                        break;
                    default:
                        throw new Exception("Module does not exist in Module init. switch case");
                }

                for (int i = 0; i < length; i++)
                {
                    //Check if comment exists
                    var comment = ModulesInfo.modules.SingleOrDefault(m => m.Id == module.Id).Comments[i];
                    if (!String.IsNullOrEmpty(comment))
                    {
                        module.Comments[i] = comment;
                    }
                    else
                    {
                        module.Comments[i] = "Disp.";
                    }
                    
                }
            }

            

            return modulesInfoChecked;
        }
    }
}