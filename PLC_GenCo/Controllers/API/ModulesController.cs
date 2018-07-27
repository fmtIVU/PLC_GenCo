using Microsoft.AspNet.Identity;
using PLC_GenCo.Models;
using PLC_GenCo.ViewModels;
using PLC_GenCo.XMLDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PLC_GenCo.Controllers.API
{
    public class ModulesController : ApiController
    {
        private ApplicationDbContext _context;

        public ModulesController()
        {
            _context = new ApplicationDbContext();
        }
        
        // GET /api/modules
        public IEnumerable<Module> GetModules()
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            return xmlDB.Modules;
        }

        // GET /api/module
        public IHttpActionResult GetModule(int id)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            var module = xmlDB.Modules.SingleOrDefault(c => c.Id == id);

            if (module == null)
            {
                return BadRequest();
            }

            return Ok(module);
        }

        //POST /api/modules
        [HttpPost]
        public IHttpActionResult CreateModule(Module module)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            xmlDB.Modules.Add(module);
            xmlDB.Save();

            return Created(new Uri(Request.RequestUri + "/" + module.Id), module);
        }

        [HttpPut]
        public Module UpdateModule(int id, Module module)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var moduleInDb = xmlDB.Modules.SingleOrDefault(c => c.Id == id);

            if (moduleInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            moduleInDb.Name = module.Name;
            moduleInDb.IOModulesType = module.IOModulesType;

            xmlDB.Save();

            return module;

        }

        // DELETE /api/modules
        [HttpDelete]
        public void Delete(int id)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            var moduleInDb = xmlDB.Modules.SingleOrDefault(c => c.Id == id);

            if (moduleInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            xmlDB.Modules.Remove(moduleInDb);
            xmlDB.Save();

            return;
        }
    }
}