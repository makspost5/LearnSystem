using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PersonController : ApiController
    {
        private GraduateWorkEntities db = new GraduateWorkEntities();

        // GET: api/Person
        [Authorize]
        public IQueryable<Person> GetPerson()
        {
            return db.Person;
        }

        // GET: api/Person/5
        [Authorize]
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> GetPerson(int id)
        {
            Person person = await db.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // PUT: api/Person/5
        [Authorize]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPerson(int id, Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != person.PersonID)
            {
                return BadRequest();
            }

            db.Entry(person).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        // POST: api/Person/Registration
        [Route("Registration")]
        [ResponseType(typeof(Pupil))]
        public async Task<IHttpActionResult> PostPerson(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(user.Person.Password));
            user.Person.Password = Convert.ToBase64String(hash);
            
            if (db.Person.Any(person => person.Email == user.Person.Email))
            {
                return BadRequest("Пользователь уже существует");
            }

            db.Person.Add(user.Person);
            db.SaveChanges();

            var personId = db.Person.Where(person =>
                user.Person.UserName == person.UserName &&
                user.Person.Email == person.Email).Select(person => person.PersonID).FirstOrDefault();
            
            var pupil = db.Pupil.Add(new Pupil { PersonID = personId, GroupID = user.GroupId });

            await db.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Person
        [ResponseType(typeof(Person))]
        [Route("AddTeacher")]
        [Authorize]
        public async Task<IHttpActionResult> PostPerson(Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Person.Add(person);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = person.PersonID }, person);
        }

        // DELETE: api/Person/5
        [Authorize]
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> DeletePerson(int id)
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            var Name = ClaimsPrincipal.Current.Identity.Name;
            var Name1 = User.Identity.Name;

            Person person = await db.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            db.Person.Remove(person);
            await db.SaveChangesAsync();

            return Ok(person);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonExists(int id)
        {
            return db.Person.Count(e => e.PersonID == id) > 0;
        }
    }
}