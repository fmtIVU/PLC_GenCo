using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PLC_GenCo.Controllers.API
{
    public class ComponentsController : ApiController
    {
        private ApplicationDbContext _context;

        public ComponentsController()
        {
            _context = new ApplicationDbContext();
        }
        // GET /api/components
        public IEnumerable<Component> GetComponents()
        {
            return _context.Components.ToList();
        }

        // GET /api/component
        public IHttpActionResult GetComponent (int id)
        {
            var component = _context.Components.SingleOrDefault(c => c.Id == id);

            if (component == null)
            {
                return BadRequest();  
            }

            return Ok(component);
        }

        //POST /api/customers
        [HttpPost]
        public IHttpActionResult CreateComponent (Component component)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.Components.Add(component);
            _context.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + component.Id), component);
        }

        [HttpPut]
        public IHttpActionResult UpdateComponent(int id, Component component)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var componentInDb = _context.Components.SingleOrDefault(c => c.Id ==  id);

            if (componentInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            

            componentInDb.MatchStatus = Enums.MatchStatus.Match;


            componentInDb.Name = component.Name;
            componentInDb.Comment = component.Comment;
            componentInDb.Location = component.Location;
            componentInDb.StandardId = component.StandardId;
            componentInDb.ConnectionType = componentInDb.ConnectionType;
            componentInDb.Depandancy = component.Depandancy;

            var io = _context.IOs.FirstOrDefault(c => c.Id == componentInDb.IOId);

            if (io != null)
            {
                io.MatchStatus = Enums.MatchStatus.Match;
                io.ComponentId = componentInDb.Id;
                io.ParentName = componentInDb.Name;
            }


            _context.SaveChanges();

            return Ok();

        }

        // DELETE /api/components/1
        [HttpDelete]
        public void DeleteComponent(int id)
        {
            var componentInDb = _context.Components.SingleOrDefault(c => c.Id == id);

            if(componentInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Components.Remove(componentInDb);
            _context.SaveChanges();
            return;
        }

    }
}
