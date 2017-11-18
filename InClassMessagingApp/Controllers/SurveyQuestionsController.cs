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
    public class SurveyQuestionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SurveyQuestions
        public IQueryable<SurveyQuestion> GetSurveyQuestions()
        {
            return db.SurveyQuestions;
        }

        // GET: api/SurveyQuestions/5
        [ResponseType(typeof(SurveyQuestion))]
        public IHttpActionResult GetSurveyQuestion(string id)
        {
            SurveyQuestion surveyQuestion = db.SurveyQuestions.Find(id);
            if (surveyQuestion == null)
            {
                return NotFound();
            }

            return Ok(surveyQuestion);
        }

        // PUT: api/SurveyQuestions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSurveyQuestion(int id, SurveyQuestion surveyQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != surveyQuestion.SurveyQuestionId)
            {
                return BadRequest();
            }

            db.Entry(surveyQuestion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurveyQuestionExists(id))
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

        // POST: api/SurveyQuestions
        [ResponseType(typeof(SurveyQuestion))]
        public IHttpActionResult PostSurveyQuestion(SurveyQuestion surveyQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SurveyQuestions.Add(surveyQuestion);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SurveyQuestionExists(surveyQuestion.SurveyQuestionId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = surveyQuestion.SurveyQuestionId }, surveyQuestion);
        }

        // DELETE: api/SurveyQuestions/5
        [ResponseType(typeof(SurveyQuestion))]
        public IHttpActionResult DeleteSurveyQuestion(string id)
        {
            SurveyQuestion surveyQuestion = db.SurveyQuestions.Find(id);
            if (surveyQuestion == null)
            {
                return NotFound();
            }

            db.SurveyQuestions.Remove(surveyQuestion);
            db.SaveChanges();

            return Ok(surveyQuestion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SurveyQuestionExists(int id)
        {
            return db.SurveyQuestions.Count(e => e.SurveyQuestionId == id) > 0;
        }
    }
}