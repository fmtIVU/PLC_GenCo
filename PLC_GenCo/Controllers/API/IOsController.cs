using Microsoft.AspNet.Identity;
using PLC_GenCo.Models;
using PLC_GenCo.Models.Setups;
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
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            return xmlDB.IOs;
        }

        // GET /api/IO
        public IHttpActionResult GetIO(int id)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            var IO = xmlDB.IOs.SingleOrDefault(c => c.Id == id);

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
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            xmlDB.IOs.Add(IO);
            xmlDB.Save();

            return Created(new Uri(Request.RequestUri + "/" + IO.Id), IO);
        }

        [HttpPut]
        public IO UpdateIO(int id, IO IO)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var IOInDb = xmlDB.IOs.SingleOrDefault(c => c.Id == id);

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

            xmlDB.Save();

            return IO;

        }

        // DELETE /api/IO/1
        [HttpDelete]
        public void DeleteIO(int id)
        {
            var userName = User.Identity.GetUserName();
            var xmlDB = new XMLDatabase(userName, _context.Users.First(c => c.Name == userName).ActProject);

            var IOInDb = xmlDB.IOs.SingleOrDefault(c => c.Id == id);

            if (IOInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            xmlDB.IOs.Remove(IOInDb);
            xmlDB.Save();
            return;
        }
    }
}