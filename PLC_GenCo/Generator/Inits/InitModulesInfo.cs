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
            var modulesInfoChecked = new ModulesInfo {
            modules = ModulesInfo.modules,
            IOs = ModulesInfo.IOs,
            Components = ModulesInfo.Components,
            MotFrqSetups = ModulesInfo.MotFrqSetups
            };


            var initControllerInfo = new InitControllerInfo(ModulesInfo.controller);

            modulesInfoChecked.controller = initControllerInfo.InitializedData();   //init controller, already made in InitControllerInfo


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
                //Allocate comment array
                module.Comments = new string[length];

                //Embeded module has both DI and DO --- filter Di comments for DI nad same for DO
                var connectionType = new ViewModels.Enums.ConnectionType();
                var modulesIO = new List<IO>();
                if (module.IOModulesType == ViewModels.Enums.IOModulesType.EmbDIx16 || module.IOModulesType == ViewModels.Enums.IOModulesType.EmbDOx16)
                {
                    switch (module.IOModulesType)
                    {
                        case ViewModels.Enums.IOModulesType.EmbDIx16:
                            connectionType = ViewModels.Enums.ConnectionType.DI;
                            break;
                        case ViewModels.Enums.IOModulesType.EmbDOx16:
                            connectionType = ViewModels.Enums.ConnectionType.DO;
                            break;
                        default:
                            break;
                    }

                    modulesIO = ModulesInfo.IOs.Where(m => m.IOAddress.Module == module.Address && m.ConnectionType == connectionType).ToList();
                }
                else
                {
                    modulesIO = ModulesInfo.IOs.Where(m => m.IOAddress.Module == module.Address).ToList();
                }

                

                //Skip comment init if there is no IOs from the module
                if (modulesIO == null)
                    continue;

                for (int i = 0; i < length; i++)
                {
                    //Check channel IO comment exists
                    var io = modulesIO.SingleOrDefault(m => m.IOAddress.Channel == i);

                    //Skip if not
                    if (io == null)
                    {
                        module.Comments[i] = "Disp.";
                        continue;
                    }

                    //IF it is null empty or whitespace -> Disp.
                    if (String.IsNullOrEmpty(io.Comment) || String.IsNullOrWhiteSpace(io.Comment))
                    {
                        module.Comments[i] = "Disp.";
                    }
                    else
                    {
                        module.Comments[i] = io.Comment;
                    }
                    
                }
            }

            

            return modulesInfoChecked;
        }
    }
}