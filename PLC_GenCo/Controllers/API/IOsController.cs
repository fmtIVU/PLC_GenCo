using PLC_GenCo.Models;
using PLC_GenCo.Models.Setups;
using PLC_GenCo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PLC_GenCo.Controllers.API
{
    public class IOsController : ApiController
    {
        private ApplicationDbContext _context;

        public IOsController()
        {
            _context = new ApplicationDbContext();
        }
        // GET /api/IOs
        public IEnumerable<IO> GetIOs()
        {
            return _context.IOs.ToList();
        }

        // GET /api/IO
        public IHttpActionResult GetIO(int id)
        {
            var IO = _context.IOs.SingleOrDefault(c => c.Id == id);

            if (IO == null)
            {
                return BadRequest();
            }

            return Ok(IO);
        }

        //POST /api/IO
        [HttpPost]
        public IHttpActionResult CreateIO(IO IO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.IOs.Add(IO);
            _context.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + IO.Id), IO);
        }

        [HttpPut]
        public IO UpdateIO(int id, IO IO)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var IOInDb = _context.IOs.SingleOrDefault(c => c.Id == id);

            if (IOInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            IOInDb.Comment = IO.Comment;
            IOInDb.ComponentId = IO.ComponentId;
            IOInDb.ConnectionType = IO.ConnectionType;
            IOInDb.Location = IO.Location;
            IOInDb.MatchStatus = IO.MatchStatus;
            IOInDb.Name = IO.Name;
            IOInDb.ParentName = IO.ParentName;
            IOInDb.IOAddress = IO.IOAddress;

            _context.SaveChanges();

            return IO;

        }

        // DELETE /api/IO/1
        [HttpDelete]
        public void DeleteIO(int id)
        {
            var IOInDb = _context.IOs.SingleOrDefault(c => c.Id == id);

            if (IOInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.IOs.Remove(IOInDb);
            _context.SaveChanges();
            return;
        }
    }
}