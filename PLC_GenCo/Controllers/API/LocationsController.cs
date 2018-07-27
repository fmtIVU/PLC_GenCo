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
    public class LocationsController : ApiController
    {
        private ApplicationDbContext _context;

        public LocationsController()
        {
            _context = new ApplicationDbContext();
        }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }
        // GET /api/locations
        public IEnumerable<ComponentLocation> GetLocations()
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            return xmlDB.Locations;
        }

        // GET /api/location
        public IHttpActionResult GetLocation(int id)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            var location = xmlDB.Locations.SingleOrDefault(c => c.Id == id);

            if (location == null)
            {
                return BadRequest();
            }

            return Ok(location);
        }

        //POST /api/customers
        [HttpPost]
        public IHttpActionResult CreateLocation(ComponentLocation location)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            xmlDB.Locations.Add(location);
            xmlDB.Save();

            return Created(new Uri(Request.RequestUri + "/" + location.Id), location);
        }

        [HttpPut]
        public ComponentLocation UpdateLocation(int id, ComponentLocation location)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var locationInDb = xmlDB.Locations.SingleOrDefault(c => c.Id == id);

            if (locationInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            locationInDb.Prefix = location.Prefix;
            locationInDb.Name = location.Name;

            xmlDB.Save();

            return location;

        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public void Delete(int id)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            var locationInDb = xmlDB.Locations.SingleOrDefault(c => c.Id == id);
            if (locationInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            xmlDB.Locations.Remove(locationInDb);
            xmlDB.Save();

            return;
        }
    }
}