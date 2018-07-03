using PLC_GenCo.Models;
using PLC_GenCo.ViewModels;
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
            return _context.Modules.ToList();
        }

        // GET /api/module
        public IHttpActionResult GetModule(int id)
        {
            var module = _context.Modules.SingleOrDefault(c => c.Id == id);

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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.Modules.Add(module);
            _context.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + module.Id), module);
        }

        [HttpPut]
        public Module UpdateModule(int id, Module module)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var moduleInDb = _context.Modules.SingleOrDefault(c => c.Id == id);

            if (moduleInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            moduleInDb.IOModulesType = module.IOModulesType;
            moduleInDb.Name = module.Name;
            moduleInDb.Comments = module.Comments;

            _context.SaveChanges();

            return module;

        }

        // DELETE /api/modules
        [HttpDelete]
        public void Delete(int id)
        {
            var moduleInDb = _context.Modules.SingleOrDefault(c => c.Id == id);

            if (moduleInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Modules.Remove(moduleInDb);
            _context.SaveChanges();

            return;
        }
    }
}