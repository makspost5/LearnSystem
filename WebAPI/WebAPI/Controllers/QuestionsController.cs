using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI.Models.TeacherCourse;

namespace WebAPI.Controllers
{
    public class QuestionsController : ApiController
    {
        private GraduateWorkEntities db = new GraduateWorkEntities();

        // GET: api/Questions
        [Authorize(Roles = "admin, teacher")]
        public IQueryable<Question> GetQuestion()
        {
            return db.Question;
        }

        // GET: api/Questions/5
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(Question))]
        public async Task<IHttpActionResult> GetQuestion(int id)
        {
            Question question = await db.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        // GET: api/QuestionsByTestId/5
        [Route("api/QuestionsByTestId/{testId}")]
        [Authorize]
        [ResponseType(typeof(Question))]
        public IQueryable<Question> GetQuestionByTestId(int testId)
        {
            return db.Question.Where(q => q.TestID == testId);
        }

        // GET: api/QuestionsBySectionBlockId/5
        [Route("api/QuestionsBySectionBlockId/{sectionBlockId}")]
        [Authorize]
        [ResponseType(typeof(Question))]
        public IQueryable<Question> GetQuestionSectionBlockId(int sectionBlockId)
        {
            return db.Question.Where(q => q.Theory.SectionBlockID == sectionBlockId);
        }

        // GET: api/QuestionsBySectionBlockId/Mobile/5
        [Route("api/QuestionsBySectionBlockId/Mobile/{sectionBlockId}")]
        [Authorize]
        [ResponseType(typeof(QuestionMobileModel))]
        public IQueryable<QuestionMobileModel> GetQuestionSectionBlockIdMobile(int sectionBlockId)
        {
            return db.Question.Where(q => q.Theory.SectionBlockID == sectionBlockId).Select(quest => new QuestionMobileModel {QuestionID = quest.QuestionID, Answer = quest.Answer, Body = quest.Body});
        }

        // GET: api/TheoryBody/5
        [Route("api/TheoryBody/{questionId}")]
        [Authorize]
        [ResponseType(typeof(Question))]
        public async Task<IHttpActionResult> GetTheoryBody(int questionId)
        {
            string body = string.Empty;
            var question = db.Question.FindAsync(questionId);

            if (question == null || question.Result == null)
            {
                return NotFound();
            }

            try
            {
                using (StreamReader reader = new StreamReader(question.Result.Theory.Body))
                {
                    body = await reader.ReadToEndAsync();
                }

                JObject jObject = new JObject();
                jObject["body"] = body;
                jObject["id"] = question.Result.QuestionID;

                return Ok(jObject);
                }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // PUT: api/Questions/5
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutQuestion(int id, Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != question.QuestionID)
            {
                return BadRequest();
            }

            db.Entry(question).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
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

        // POST: api/Questions
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(Question))]
        public async Task<IHttpActionResult> PostQuestion(Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Question.Add(question);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = question.QuestionID }, question);
        }

        // DELETE: api/Questions/5
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(Question))]
        public async Task<IHttpActionResult> DeleteQuestion(int id)
        {
            Question question = await db.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            db.Question.Remove(question);
            await db.SaveChangesAsync();

            return Ok(question);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionExists(int id)
        {
            return db.Question.Count(e => e.QuestionID == id) > 0;
        }
    }
}