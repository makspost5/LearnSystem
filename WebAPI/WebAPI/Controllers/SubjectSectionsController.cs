using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI;
using WebAPI.Models.TeacherCourse;

namespace WebAPI.Controllers
{
    public class SubjectSectionsController : ApiController
    {
        private GraduateWorkEntities db = new GraduateWorkEntities();

        // GET: api/SubjectSections
        [Authorize]
        public IQueryable<SubjectSection> GetSubjectSection()
        {
            return db.SubjectSection;
        }

        // GET: api/SubjectSectionsByCourse/5
        [Authorize]
        [ResponseType(typeof(SubjectSection))]
        [Route("api/SubjectSectionsByCourse/{id}")]
        public async Task<IHttpActionResult> GetSubjectSectionsByCourse(int id)
        {
            SubjectCourse subjectCourse = await db.SubjectCourse.FindAsync(id);
            if (subjectCourse == null)
            {
                return NotFound();
            }

            return Ok(subjectCourse.SubjectSection);
        }

        // GET: api/SubjectSections/5
        [Authorize]
        [ResponseType(typeof(SubjectSection))]
        public async Task<IHttpActionResult> GetSubjectSection(int id)
        {
            SubjectSection subjectSection = await db.SubjectSection.FindAsync(id);
            if (subjectSection == null)
            {
                return NotFound();
            }

            return Ok(subjectSection);
        }

        // PUT: api/SubjectSections/5
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSubjectSection(int id, SubjectSection subjectSection)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subjectSection.SubjectSectionID)
            {
                return BadRequest();
            }

            db.Entry(subjectSection).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectSectionExists(id))
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

        // POST: api/SubjectSections
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(SubjectSection))]
        public async Task<IHttpActionResult> PostSubjectSection(SubjectSectionModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var subjectSection = data.toDBModel();

            db.SubjectSection.Add(subjectSection);
            await db.SaveChangesAsync();

            foreach (var block in data.SectionBlock)
            {
                var sectionBlock = block.toDBModel(subjectSection.SubjectSectionID);

                db.SectionBlock.Add(sectionBlock);
                await db.SaveChangesAsync();

                foreach (var questionBlock in block.Question)
                {
                    var question = questionBlock.toDBModel();

                    db.Question.Add(question);
                    await db.SaveChangesAsync();

                    foreach (var a in questionBlock.Answer)
                    {
                        a.QuestionID = question.QuestionID;
                        db.Answer.Add(a);
                    }

                    foreach (var a in questionBlock.AnswerMatching)
                    {
                        a.QuestionID = question.QuestionID;
                        db.AnswerMatching.Add(a);
                    }

                    foreach (var a in questionBlock.AnswerOrder)
                    {
                        a.QuestionID = question.QuestionID;
                        db.AnswerOrder.Add(a);
                    }

                    var theoryName = Guid.NewGuid().ToString();
                    var path = @"D:\Учеба\Диплом\GraduateWork\WebAPI\WebAPI\Files\" + personId;

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    path = path +@"\" + theoryName + ".txt";

                    using (StreamWriter outputFile = new StreamWriter(path))
                    {
                        await outputFile.WriteAsync(questionBlock.Theory.Body);
                    }

                    questionBlock.Theory.Body = path;

                    db.Theory.Add(questionBlock.Theory.toDBModel(question.QuestionID, sectionBlock.SectionBlockID));

                    await db.SaveChangesAsync();
                }
            }

            //db.SubjectSection.Add(subjectSection);
            //await db.SaveChangesAsync();

            return Ok(data);// CreatedAtRoute("DefaultApi", new { id = subjectSection.SubjectSectionID }, subjectSection);
        }

        // DELETE: api/SubjectSections/5
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(SubjectSection))]
        public async Task<IHttpActionResult> DeleteSubjectSection(int id)
        {
            SubjectSection subjectSection = await db.SubjectSection.FindAsync(id);
            if (subjectSection == null)
            {
                return NotFound();
            }

            db.SubjectSection.Remove(subjectSection);
            await db.SaveChangesAsync();

            return Ok(subjectSection);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubjectSectionExists(int id)
        {
            return db.SubjectSection.Count(e => e.SubjectSectionID == id) > 0;
        }
    }
}