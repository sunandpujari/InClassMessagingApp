using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using InClassMessagingApp.Models;

namespace InClassMessagingApp.Controllers
{
    public class SurveyUsersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SurveyUsers
        public IQueryable<SurveyUser> GetSurveyUsers()
        {
            return db.SurveyUsers;
        }

        // GET: api/SurveyUsers/5
        [ResponseType(typeof(SurveyUser))]
        public IHttpActionResult GetSurveyUser(string id)
        {
            SurveyUser surveyUser = db.SurveyUsers.Find(id);
            if (surveyUser == null)
            {
                return NotFound();
            }

            return Ok(surveyUser);
        }

        // PUT: api/SurveyUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSurveyUser(string id, SurveyUser surveyUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != surveyUser.SurveyUserId)
            {
                return BadRequest();
            }

            db.Entry(surveyUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurveyUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SurveyUsers
        [ResponseType(typeof(SurveyUser))]
        public IHttpActionResult PostSurveyUser(SurveyUser surveyUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SurveyUsers.Add(surveyUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SurveyUserExists(surveyUser.SurveyUserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = surveyUser.SurveyUserId }, surveyUser);
        }

        // DELETE: api/SurveyUsers/5
        [ResponseType(typeof(SurveyUser))]
        public IHttpActionResult DeleteSurveyUser(string id)
        {
            SurveyUser surveyUser = db.SurveyUsers.Find(id);
            if (surveyUser == null)
            {
                return NotFound();
            }

            db.SurveyUsers.Remove(surveyUser);
            db.SaveChanges();

            return Ok(surveyUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SurveyUserExists(string id)
        {
            return db.SurveyUsers.Count(e => e.SurveyUserId == id) > 0;
        }
    }
}