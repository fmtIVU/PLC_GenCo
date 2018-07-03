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
            return _context.ComponentLocations.ToList();
        }

        // GET /api/location
        public IHttpActionResult GetLocation(int id)
        {
            var location = _context.ComponentLocations.SingleOrDefault(c => c.Id == id);

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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.ComponentLocations.Add(location);
            _context.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + location.Id), location);
        }

        [HttpPut]
        public ComponentLocation UpdateLocation(int id, ComponentLocation location)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var locationInDb = _context.ComponentLocations.SingleOrDefault(c => c.Id == id);

            if (locationInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            locationInDb.Prefix = location.Prefix;
            locationInDb.Name = location.Name;

            _context.SaveChanges();

            return location;

        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public void Delete(int id)
        {
            var locationInDb = _context.ComponentLocations.SingleOrDefault(c => c.Id == id);

            if (locationInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.ComponentLocations.Remove(locationInDb);
            _context.SaveChanges();

            return;
        }
    }
}