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
    public class DIAlarmSetupsController : ApiController
    {
        private ApplicationDbContext _context;

        public DIAlarmSetupsController()
        {
            _context = new ApplicationDbContext();
        }
        // GET /api/DIAlarmSetups
        public IEnumerable<DIAlarmSetup> GetDIAlarmSetups()
        {
            return _context.DIAlarms.ToList();
        }

        // GET /api/DIAlarmSetup
        public IHttpActionResult GetDIAlarmSetup(int id)
        {
            var DIAlarmSetup = _context.DIAlarms.SingleOrDefault(c => c.Id == id);

            if (DIAlarmSetup == null)
            {
                return BadRequest();
            }

            return Ok(DIAlarmSetup);
        }

        //POST /api/CreateDIAlarmSetup
        [HttpPost]
        public IHttpActionResult CreateDIAlarmSetup(DIAlarmSetup DIAlarmSetup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            DIAlarmSetup.Comment = _context.IOs.First(c => c.Id == DIAlarmSetup.IdIO).Comment;
            _context.DIAlarms.Add(DIAlarmSetup);
            _context.SaveChanges();
            
            
            return Created(new Uri(Request.RequestUri + "/" + DIAlarmSetup.Id), DIAlarmSetup);
        }

        [HttpPut]
        public DIAlarmSetup UpdateDIAlarmSetup(int id, DIAlarmSetup DIAlarmSetup)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var DIAlarmSetupInDb = _context.DIAlarms.SingleOrDefault(c => c.Id == id);

            if (DIAlarmSetupInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            DIAlarmSetupInDb.InputType = DIAlarmSetup.InputType;
            DIAlarmSetupInDb.TimeDelay = DIAlarmSetup.TimeDelay;
            DIAlarmSetupInDb.IdComponent = DIAlarmSetup.IdComponent;
            
            _context.SaveChanges();

            return DIAlarmSetup;

        }

        // DELETE /api/DIAlarmSetup/1
        [HttpDelete]
        public void DeleteDIAlarmSetup(int id)
        {
            var DIAlarmSetupInDb = _context.DIAlarms.SingleOrDefault(c => c.Id == id);

            if (DIAlarmSetupInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.DIAlarms.Remove(DIAlarmSetupInDb);
            _context.SaveChanges();
            return;
        }
    }
}