using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI;
using WebAPI.Models.Pupil;

namespace WebAPI.Controllers
{
    public class SectionBlocksController : ApiController
    {
        private GraduateWorkEntities db = new GraduateWorkEntities();

        [Authorize]
        // GET: api/SectionBlocks/5
        [ResponseType(typeof(SectionBlockModel))]
        public async Task<IHttpActionResult> GetSectionBlock(int id)
        {
            SubjectSection sectionBlock = await db.SubjectSection.FindAsync(id);

            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var pupil = db.Pupil.Where(person => person.PersonID == personId).FirstOrDefault();

            if (pupil == null || !pupil.ConfirmedAccount)
            {
                return Unauthorized();
            }

            if (sectionBlock == null)
            {
                return NotFound();
            }

            return Ok(sectionBlock.SectionBlock.Select(sb => new SectionBlockModel { Name = sb.Name, Position = sb.Position, QuestionsCount = sb.Theory.Count, SectionBlockID = sb.SectionBlockID, isPassed = sb.SectionBlockResult.Where(result => result.SubjectSectionResult.PersonID == pupil.PersonID).Any() }));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SectionBlockExists(int id)
        {
            return db.SectionBlock.Count(e => e.SectionBlockID == id) > 0;
        }
    }
}