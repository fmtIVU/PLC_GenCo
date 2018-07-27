using PLC_GenCo.Models;
using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using static PLC_GenCo.ViewModels.Enums;

namespace PLC_GenCo.XMLDB
{
    public class XMLDatabase
    {
        private string _location;
        private string _user;
        private string _project;
        private XElement _DB;

        private int _ComponentId;
        private int _StandardId;
        private int _LocationId;
        private int _ModuleId;
        private int _IOId;

        public List<Component> Components { get; set; }
        public List<Standard> Standards { get; set; }
        public List<ComponentLocation> Locations { get; set; }
        public List<Module> Modules { get; set; }
        public PLC PLC { get; set; }
        public List<IO> IOs { get; set; }

        const string location = @"C:\Users\Ivan\Desktop\OP generator PLC koda\Profiles\";


        public XMLDatabase(string user, string project)
        {
            _location = location;
            _user = user;
            _project = project;
            _DB = XElement.Load(_location + _user + @"\" + _project + @"\" + _project + ".xml");

            Components = GetComponents();
            PLC = GetPLC();
            Modules = GetModules();
            IOs = GetIOs();
            Standards = GetStandards();
            Locations = GetLocations();

            _ComponentId = Convert.ToInt32(_DB.Attribute("ComponentId").Value);
            _StandardId = Convert.ToInt32(_DB.Attribute("StandardId").Value);
            _LocationId = Convert.ToInt32(_DB.Attribute("LocationId").Value);
            _ModuleId = Convert.ToInt32(_DB.Attribute("ModuleId").Value);
            _IOId = Convert.ToInt32(_DB.Attribute("IOId").Value);
        }
    

        public XElement GetProject()
        {
            return _DB;
        }

        public void Save()
        {
            //Clear XML list
            //Add new from propreties
            _DB.Element("Components").RemoveNodes();
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i] = UpdateComponent(Components[i]);
            }

            _DB.Element("Standards").RemoveNodes();
            for (int i = 0; i < Standards.Count; i++)
            {
                Standards[i] = UpdateStandard(Standards[i]);
            }

            _DB.Element("IOs").RemoveNodes();
            for (int i = 0; i < IOs.Count; i++)
            {
                IOs[i] = UpdateIO(IOs[i]);
            }

            _DB.Element("Locations").RemoveNodes();
            for (int i = 0; i < Locations.Count; i++)
            {
                Locations[i] = UpdateLocation(Locations[i]);
            }

            _DB.Element("Modules").RemoveNodes();
            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i] = UpdateModule(Modules[i]);
            }

            UpdatePLC(PLC);

            //Save to file
            _DB.Save(_location + _user + @"\" + _project + @"\" + _project + ".xml");

            //Update properties
            Components = GetComponents();
            PLC = GetPLC();
            Modules = GetModules();
            IOs = GetIOs();
            Standards = GetStandards();
            Locations = GetLocations();
        }

        private PLC GetPLC()
        {
            var XMLPLC = _DB.Element("PLC");

            if (!XMLPLC.HasAttributes)
            {
                return new PLC();
            }
            return new PLC {
                Id = Convert.ToInt32(XMLPLC.Attribute("Id").Value),
                Name = XMLPLC.Attribute("Name").Value,
                Description = XMLPLC.Attribute("Description").Value,
                ProductType = (ControllerType)Convert.ToInt32(XMLPLC.Attribute("ProductType").Value)
            };
        }

        private List<Module> GetModules()
        {
            var XMLmodules = _DB.Element("Modules").Elements().ToList();
            var modules = new List<Module>();

            //Return null if empty
            if (XMLmodules.Count == 0)
            {
                return modules;
            }
             
            foreach (var module in XMLmodules)
            {
                modules.Add(new Module {
                    Id = Convert.ToInt32(module.Attribute("Id").Value),
                    Name = module.Attribute("Name").Value,
                    Address = Convert.ToInt32(module.Attribute("Address").Value),
                    IOModulesType = (IOModulesType)Convert.ToInt32(module.Attribute("IOModulesType").Value)
                });
            }

            return modules;
        }

        private List<IO> GetIOs()
        {
            var XMLIOs = _DB.Element("IOs").Elements().ToList();
            var IOs = new List<IO>();

            //Return null if empty
            if (XMLIOs.Count == 0)
            {
                return IOs;
            }

            foreach (var IO in XMLIOs)
            {
                var newIO = new IO
                {
                    Id = Convert.ToInt32(IO.Attribute("Id").Value),
                    Name = IO.Attribute("Name").Value,
                    Comment = IO.Attribute("Comment").Value,
                    Location = IO.Attribute("Location").Value,
                    ParentName = IO.Attribute("ParentName").Value,
                    ComponentId = Convert.ToInt32(IO.Attribute("ComponentId").Value),
                    ConnectionType = (ConnectionType)Convert.ToInt32(IO.Attribute("ConnectionType").Value),
                    MatchStatus = (MatchStatus)Convert.ToInt32(IO.Attribute("MatchStatus").Value)
                };

                newIO.IOAddress = new IOAddress();
                newIO.IOAddress.Type = (IOType)(Convert.ToInt32(IO.Attribute("IOAddressType").Value));

                switch (newIO.IOAddress.Type)
                {
                    case (IOType.IO):
                        newIO.IOAddress.IPorMBAddress = " ";
                        newIO.IOAddress.Rack = Convert.ToInt32(IO.Attribute("IOAddressRack").Value);
                        newIO.IOAddress.Module = Convert.ToInt32(IO.Attribute("IOAddressModule").Value);
                        newIO.IOAddress.Channel = Convert.ToInt32(IO.Attribute("IOAddressChannel").Value);
                        
                        break;
                    case (IOType.IP):
                        newIO.IOAddress.Rack = 0;
                        newIO.IOAddress.Module = 0;
                        newIO.IOAddress.Channel = 0;
                        newIO.IOAddress.IPorMBAddress = IO.Attribute("IOAddressIPorMBAddress").Value;
                        break;
                    case (IOType.MB):
                        newIO.IOAddress.Rack = 0;
                        newIO.IOAddress.Module = 0;
                        newIO.IOAddress.Channel = 0;
                        newIO.IOAddress.IPorMBAddress = IO.Attribute("IOAddressIPorMBAddress").Value;
                        break;
                    default:
                        throw new Exception("PLC address type not recognized - should be IO/IP/MB");
                }

                IOs.Add(newIO);
            }

            return IOs;
        }

        private List<Component> GetComponents()
        {
            var XMLcomponents = _DB.Element("Components").Elements().ToList();
            var components = new List<Component>();

            //return null if empty
            if (XMLcomponents.Count == 0)
            {
                return components;
            }

            foreach (var component in XMLcomponents)
            {
                var newComponent = new Component
                {
                    Id = Convert.ToInt32(component.Attribute("Id").Value),
                    Name = component.Attribute("Name").Value,
                    Comment = component.Attribute("Comment").Value,
                    Location = component.Attribute("Location").Value,
                    ConnectionType = (ConnectionType)Convert.ToInt32(component.Attribute("ConnectionType").Value),
                    Dependancy = (Dependancy)Convert.ToInt32(component.Attribute("Dependancy").Value),
                    MatchStatus = (MatchStatus)Convert.ToInt32(component.Attribute("MatchStatus").Value)
                };

                if (String.IsNullOrEmpty(component.Attribute("IOId").Value) || Convert.ToInt32(component.Attribute("IOId").Value) == 0)
                {
                    newComponent.IOId = null;
                }else
                {
                    newComponent.IOId = Convert.ToInt32(component.Attribute("IOId").Value);
                }

                if (String.IsNullOrEmpty(component.Attribute("StandardId").Value) || Convert.ToInt32(component.Attribute("StandardId").Value) == 0)
                {
                    newComponent.StandardId = null;
                }
                else
                {
                    newComponent.StandardId = Convert.ToInt32(component.Attribute("StandardId").Value);
                }

                //Init Setup if exists
                if (component.Element("Configuration").Attribute("AOIName") != null)
                {
                    newComponent.Setup = new Setup
                    {
                        Parameters = new List<Parameter>(),
                        AOIName = component.Element("Configuration").Attribute("AOIName").Value
                    };


                    foreach (var parameter in component.Element("Configuration").Elements().ToList())
                    {

                        newComponent.Setup.Parameters.Add(new Parameter
                        {
                            Name = parameter.Attribute("Name").Value,
                            ShortName = parameter.Attribute("ShortName").Value,
                            DataType = (DataType)Enum.Parse(typeof(DataType), parameter.Attribute("DataType").Value),
                            Type = (ParType)Enum.Parse(typeof(ParType), parameter.Attribute("Type").Value),
                            Usage = (InOut)Enum.Parse(typeof(InOut), parameter.Attribute("Usage").Value),
                            DefaultValue = parameter.Attribute("DefaultValue").Value,
                            Value = parameter.Attribute("Value").Value
                        });

                    }
                }

                components.Add(newComponent);

            }

            return components;
        }

        private List<ComponentLocation> GetLocations()
        {
            var XMLlocations = _DB.Element("Locations").Elements().ToList();
            var locations = new List<ComponentLocation>();

            //return null if empty
            if (XMLlocations.Count == 0)
            {
                return locations;
            }

            foreach (var location in XMLlocations)
            {
                locations.Add(new ComponentLocation
                {
                    Id = Convert.ToInt32(location.Attribute("Id").Value),
                    Name = location.Attribute("Name").Value,
                    Prefix = location.Attribute("Prefix").Value
                });
            }

            return locations;
        }

        private List<Standard> GetStandards()
        {
            var XMLStandards = _DB.Element("Standards").Elements().ToList();
            var standards = new List<Standard>();

            //return null if empty
            if (XMLStandards.Count == 0)
            {
                return standards;
            }

            foreach (var standard in XMLStandards)
            {
                standards.Add(new Standard
                {
                    Id = Convert.ToInt32(standard.Attribute("Id").Value),
                    AOIName = standard.Attribute("AOIName").Value,
                    Description = standard.Attribute("Description").Value,
                    ConnectionType = (ConnectionType)(Convert.ToInt32(standard.Attribute("ConnectionType").Value)),
                    Group = standard.Attribute("Group").Value
                });
            }

            return standards;
        }

        private List<Component> GetGroup(string groupName)
        {
            var standardGroup = new List<Standard>();
            var componentGroup = new List<Component>();

            //Filter standards with same group
            foreach (var standard in Standards)
            {
                if (standard.Group == groupName)
                {
                    standardGroup.Add(standard);
                }
            } 

            return componentGroup;
        }

        private PLC UpdatePLC(PLC plc)
        {
            //If there is no name PLC is not set up yet
            if (plc.Name == null)
            {
                return new PLC();
            }

            var XMLPLC = _DB.Element("PLC");
            //Check if exist - Creat new or update
            if (XMLPLC.Attribute("Id") != null)
            {
                //Update
                XMLPLC.Attribute("Id").Value = plc.Id.ToString();
                XMLPLC.Attribute("Name").Value = plc.Name;
                XMLPLC.Attribute("Description").Value = plc.Description;
                XMLPLC.Attribute("ProductType").Value = Convert.ToInt32(plc.ProductType).ToString();

            }
            else
            {
                //Add new
                XMLPLC.Add(
                    new XAttribute("Id", "1"),
                    new XAttribute("Name", plc.Name),
                    new XAttribute("Description", plc.Name),
                    new XAttribute("ProductType", Convert.ToInt32(plc.ProductType).ToString())
                    );
            }
            plc.Id = 1;

            return plc;

        }

        private Module UpdateModule(Module module)
        {

            //Check if exist - Creat new or update
            if (module.Id != 0)
            {
                //Add existing
                _DB.Element("Modules").Add(
                    new XElement("Module",
                        new XAttribute("Id", module.Id.ToString()),
                        new XAttribute("Name", module.Name),
                        new XAttribute("Address", module.Address.ToString()),
                        new XAttribute("IOModulesType", Convert.ToInt32(module.IOModulesType).ToString())
                        )
                    );
            }
            else
            {
                //Add new
                _DB.Element("Modules").Add( 
                    new XElement("Module",
                        new XAttribute("Id", _ModuleId.ToString()),
                        new XAttribute("Name", module.Name),
                        new XAttribute("Address", module.Address),
                        new XAttribute("IOModulesType", Convert.ToInt32(module.IOModulesType).ToString())
                        )
                    );
                module.Id = _ModuleId;
                _ModuleId++;
                _DB.Attribute("ModuleId").Value = _ModuleId.ToString();
               
            }

            return module;

        }

        private IO UpdateIO(IO io)
        {
            var XMLIOs = _DB.Element("IOs").Elements().ToList();

            //Check if exist - Create new or update
            if (io.Id != 0)
            {
                //Update existing
                _DB.Element("IOs").Add(
                    new XElement("IO",
                        new XAttribute("Id", io.Id.ToString()),
                        new XAttribute("Name", io.Name),
                        new XAttribute("Comment", io.Comment),
                        new XAttribute("Location", io.Location),
                        new XAttribute("ComponentId",  io.ComponentId.ToString()),
                        new XAttribute("ParentName", io.ParentName),
                        new XAttribute("ConnectionType", (Convert.ToInt32(io.ConnectionType)).ToString()),
                        new XAttribute("MatchStatus", (Convert.ToInt32(io.MatchStatus)).ToString()),
                        new XAttribute("IOAddressType", (Convert.ToInt32(io.IOAddress.Type)).ToString()),
                        new XAttribute("IOAddressIPorMBAddress", io.IOAddress.IPorMBAddress),
                        new XAttribute("IOAddressRack", io.IOAddress.Rack.ToString()),
                        new XAttribute("IOAddressModule", io.IOAddress.Module.ToString()),
                        new XAttribute("IOAddressChannel", io.IOAddress.Channel.ToString())
                        )
                    );
            }
            else
            {

                //Add new
                _DB.Element("IOs").Add(
                    new XElement("IO",
                        new XAttribute("Id", _IOId.ToString()),
                        new XAttribute("Name", io.Name),
                        new XAttribute("Comment", String.IsNullOrEmpty(io.Comment) ? " " : io.Comment),
                        new XAttribute("Location", io.Location),
                        new XAttribute("ComponentId", io.ComponentId.HasValue ? io.ComponentId.ToString() : "0"),
                        new XAttribute("ParentName", String.IsNullOrEmpty(io.ParentName) ? " " : io.ParentName),
                        new XAttribute("ConnectionType", (Convert.ToInt32(io.ConnectionType)).ToString()),
                        new XAttribute("MatchStatus", (Convert.ToInt32(io.MatchStatus)).ToString()),
                        new XAttribute("IOAddressType", (Convert.ToInt32(io.IOAddress.Type)).ToString()),
                        new XAttribute("IOAddressIPorMBAddress", String.IsNullOrEmpty(io.IOAddress.IPorMBAddress) ? " " : io.IOAddress.IPorMBAddress),
                        new XAttribute("IOAddressRack", io.IOAddress.Rack),
                        new XAttribute("IOAddressModule", io.IOAddress.Module),
                        new XAttribute("IOAddressChannel", io.IOAddress.Channel)
                        )
                    );
                io.Id = _IOId;
                _IOId++;
                _DB.Attribute("IOId").Value = _IOId.ToString();
            }

            return io;
        }

        private Component UpdateComponent(Component component)
        {
            //Load Standard name
            //check if it has standard
            if (component.StandardId != 0 && component.StandardId != null)
            {
                var standardName = GetStandards().First(c => c.Id == component.StandardId).AOIName;

                //Case standard changed load new setup OR no standard at all
                if (component.Setup == null || standardName != component.Setup.AOIName)
                {
                    component.Setup = new Setup {
                        AOIName = standardName,
                        Parameters = new List<Parameter>()
                    };

                    //Load configuration first time
                    var conf = XElement.Load(_location + _user + @"\" + _project + @"\Standards\" + standardName + ".xml");

                    foreach (var parameter in conf.Elements().ToList())
                    {

                        component.Setup.Parameters.Add(new Parameter {
                            Name = parameter.Attribute("Name").Value,
                            ShortName = parameter.Attribute("ShortName").Value,
                            DefaultValue = parameter.Attribute("DefaultValue").Value,
                            Value = parameter.Attribute("Value").Value,
                            DataType = (DataType)Enum.Parse(typeof(DataType), parameter.Attribute("DataType").Value),
                            Usage = (InOut)Enum.Parse(typeof(InOut), parameter.Attribute("Usage").Value),
                            Type = (ParType)Enum.Parse(typeof(ParType), parameter.Attribute("Type").Value),
                        });
                    }
                    
                    
                }


            }

            //If Id=0 component is new and new ID must me assigned
            if (component.Id == 0)
            {
                component.Id = _ComponentId;
                _ComponentId++;
                _DB.Attribute("ComponentId").Value = _ComponentId.ToString();
            }

            //Create new
            var comp = new XElement("Component",
                    new XAttribute("Id", component.Id.ToString()),
                    new XAttribute("Name", component.Name),
                    new XAttribute("Comment", component.Comment),
                    new XAttribute("Location", component.Location),
                    new XAttribute("ConnectionType", (Convert.ToInt32(component.ConnectionType)).ToString()),
                    new XAttribute("MatchStatus", (Convert.ToInt32(component.MatchStatus)).ToString()),
                    new XAttribute("Dependancy", (Convert.ToInt32(component.Dependancy)).ToString()),
                    new XAttribute("IOId", component.IOId.HasValue ? component.IOId.ToString() : "0"),
                    new XAttribute("StandardId", component.StandardId.HasValue ? component.StandardId.ToString() : "0"),

                    new XElement("Configuration")

                    );

            if (component.StandardId != null && component.StandardId != 0)
            {
                //Update parameters
                foreach (var parameter in component.Setup.Parameters.ToList())
                {

                    comp.Element("Configuration").Add(new XElement("Parameter",
                            new XAttribute("Name", parameter.Name),
                            new XAttribute("ShortName", parameter.ShortName),
                            new XAttribute("DataType", Convert.ToInt32(parameter.DataType).ToString()),
                            new XAttribute("Type", Convert.ToInt32(parameter.Type).ToString()),
                            new XAttribute("Usage", Convert.ToInt32(parameter.Usage).ToString()),
                            new XAttribute("DefaultValue", parameter.DefaultValue),
                            new XAttribute("Value", parameter.Value)
                            ));
                }

                comp.Element("Configuration").Add(new XAttribute("AOIName", component.Setup.AOIName));
            }

            

            _DB.Element("Components").Add(comp);

            return component;

        }

        private ComponentLocation UpdateLocation(ComponentLocation location)
        {

            //Check if exist - Creat new or update
            if (location.Id != 0)
            {
                //Add existing
                _DB.Element("Locations").Add(
                    new XElement("Location",
                        new XAttribute("Id", location.Id.ToString()),
                        new XAttribute("Name", location.Name),
                        new XAttribute("Prefix", location.Prefix)
                        )
                    );
            }
            else
            {
                //Add new
                _DB.Element("Locations").Add(
                    new XElement("Location",
                        new XAttribute("Id", _LocationId.ToString()),
                        new XAttribute("Name", location.Name),
                        new XAttribute("Prefix", String.IsNullOrEmpty(location.Prefix) ? " " : location.Prefix)
                        )
                    );
                location.Id = _LocationId;
                _LocationId++;
                _DB.Attribute("LocationId").Value = _LocationId.ToString();
            }

            return location;
        }

        private Standard UpdateStandard(Standard standard)
        {

            //Check if exist - Creat new or update
            if (standard.Id != 0)
            {
                //Add existing
                _DB.Element("Standards").Add(
                    new XElement("Standard",
                        new XAttribute("Id", standard.Id.ToString()),
                        new XAttribute("AOIName", standard.AOIName),
                        new XAttribute("Description", standard.Description),
                        new XAttribute("ConnectionType", Convert.ToInt32(standard.ConnectionType).ToString()),
                        new XAttribute("Group", standard.Group)
                        )
                    );
            }
            else
            {
                //Add new
                _DB.Element("Standards").Add(
                    new XElement("Standard",
                        new XAttribute("Id", _StandardId.ToString()),
                        new XAttribute("AOIName", standard.AOIName),
                        new XAttribute("Description", String.IsNullOrEmpty(standard.Description) ? "No description" : standard.Description),
                        new XAttribute("ConnectionType", Convert.ToInt32(standard.ConnectionType).ToString()),
                        new XAttribute("Group", String.IsNullOrEmpty(standard.Group) ? "No group" : standard.Group)
                        )
                    );
                standard.Id = _StandardId;
                _StandardId++;
                _DB.Attribute("StandardId").Value = _StandardId.ToString();
            }

            return standard;
        }

        public void AddConfigFile(string name, XElement config)
        {
            config.Save(_location + _user + @"\" + _project + @"\Standards\" + name + ".xml");
        }
    }
}