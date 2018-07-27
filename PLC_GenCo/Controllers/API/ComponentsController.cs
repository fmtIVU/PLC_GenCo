using Microsoft.AspNet.Identity;
using PLC_GenCo.ViewModels;
using PLC_GenCo.XMLDB;
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
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            return xmlDB.Components;
        }

        // GET /api/component
        public IHttpActionResult GetComponent (int id)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            var component = xmlDB.Components.SingleOrDefault(c => c.Id == id);

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
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            xmlDB.Components.Add(component);
            xmlDB.Save();

            return Created(new Uri(Request.RequestUri + "/" + component.Id), component);
        }

        [HttpPut]
        public IHttpActionResult UpdateComponent(int id, Component component)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var componentInDb = xmlDB.Components.SingleOrDefault(c => c.Id ==  id);

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
            componentInDb.Dependancy = component.Dependancy;

            var io = xmlDB.IOs.FirstOrDefault(c => c.Id == componentInDb.IOId);

            if (io != null)
            {
                io.MatchStatus = Enums.MatchStatus.Match;
                io.ComponentId = componentInDb.Id;
            }

            xmlDB.Save();
            return Ok();

        }

        // DELETE /api/components/1
        [HttpDelete]
        public void DeleteComponent(int id)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            var componentInDb = xmlDB.Components.SingleOrDefault(c => c.Id == id);

            if(componentInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            xmlDB.Components.Remove(componentInDb);
            xmlDB.Save();

            return;
        }

    }
}
