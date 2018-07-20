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
    public class AIAlarmSetupsController : ApiController
    {
        private ApplicationDbContext _context;

        public AIAlarmSetupsController()
        {
            _context = new ApplicationDbContext();
        }
        // GET /api/AIAlarmSetups
        public IEnumerable<AIAlarmSetup> GetAIAlarmSetups()
        {
            return _context.AIAlarms.ToList();
        }

        // GET /api/AIAlarmSetup
        public IHttpActionResult GetAIAlarmSetup(int id)
        {
            var AIAlarmSetup = _context.AIAlarms.SingleOrDefault(c => c.Id == id);

            if (AIAlarmSetup == null)
            {
                return BadRequest();
            }

            return Ok(AIAlarmSetup);
        }

        //POST /api/AIAlarmSetup
        [HttpPost]
        public IHttpActionResult CreateAIAlarmSetup(AIAlarmSetup AIAlarmSetup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //Get io comment from IO list
            AIAlarmSetup.Comment = _context.IOs.First(c => c.Id == AIAlarmSetup.IdIO).Comment;

            _context.AIAlarms.Add(AIAlarmSetup);
            _context.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + AIAlarmSetup.Id), AIAlarmSetup);
        }

        [HttpPut]
        public AIAlarmSetup UpdateAIAlarmSetup(int id, AIAlarmSetup AIAlarmSetup)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var AIAlarmSetupInDb = _context.AIAlarms.SingleOrDefault(c => c.Id == id);

            if (AIAlarmSetupInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            AIAlarmSetupInDb.AICType = AIAlarmSetup.AICType;
            AIAlarmSetupInDb.AlarmHH = AIAlarmSetup.AlarmHH;
            AIAlarmSetupInDb.AlarmH = AIAlarmSetup.AlarmH;
            AIAlarmSetupInDb.AlarmL = AIAlarmSetup.AlarmL;
            AIAlarmSetupInDb.AlarmLL = AIAlarmSetup.AlarmLL;
            AIAlarmSetupInDb.IdComponent = AIAlarmSetup.IdComponent;
            AIAlarmSetupInDb.IdIO = AIAlarmSetup.IdIO;
            AIAlarmSetupInDb.ScaleMax = AIAlarmSetup.ScaleMax;
            AIAlarmSetupInDb.ScaleMin = AIAlarmSetup.ScaleMin;
            AIAlarmSetupInDb.TimeDelay = AIAlarmSetup.TimeDelay;

            _context.SaveChanges();

            return AIAlarmSetup;

        }

        // DELETE /api/AIAlarmSetup/1
        [HttpDelete]
        public void DeleteAIAlarmSetup(int id)
        {
            var AIAlarmSetupInDb = _context.AIAlarms.SingleOrDefault(c => c.Id == id);

            if (AIAlarmSetupInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.AIAlarms.Remove(AIAlarmSetupInDb);
            _context.SaveChanges();
            return;
        }
    }
}