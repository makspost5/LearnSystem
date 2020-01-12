using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI.Models.Pupil;
using WebAPI.Models.TeacherCourse;
using WebAPI.Models.TeacherTest;

namespace WebAPI.Controllers
{
    public class SubjectCoursesController : ApiController
    {
        private GraduateWorkEntities db = new GraduateWorkEntities();

        // GET: api/Courses/CourseRows
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(CourseRow))]
        [Route("api/Courses/CourseRow")]
        public List<CourseRow> GetTestRows()
        {
            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var teacher = db.Teacher.Where(person => person.PersonID == personId).FirstOrDefault();

            if (teacher == null || teacher.TeacherSubjects.Count == 0)
            {
                return null; //TODO
            }
            List<CourseRow> courseRow = new List<CourseRow>();

            foreach (var subject in teacher.SubjectCourse.Select(ts => ts.Subject))
            {
                courseRow.AddRange(subject.SubjectCourse.Select(course => new CourseRow
                {
                    ID = course.SubjectCourseID,
                    Name = course.Name,
                    Subject = course.Subject.Name,
                    SectionsCount = course.SubjectSection.Count,
                    Available = course.SubjectCourseAvailable.Any(ta => ta.SubjectCourseAvailable1 == true)
                }));
            }

            return courseRow;
        }

        // GET: api/Courses/UpdateData/5
        [Authorize(Roles = "admin, teacher")]
        [Route("api/Courses/UpdateData/{id}")]
        [ResponseType(typeof(TeacherCourseData))]
        public async Task<IHttpActionResult> GetCourseUpdateData(int id)
        {
            TeacherCourseData teacherCourseData = new TeacherCourseData();
            SubjectCourse course = await db.SubjectCourse.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            teacherCourseData.courseParameters = new CourseParameters();

            teacherCourseData.courseParameters.id = course.SubjectCourseID;
            teacherCourseData.courseParameters.name = course.Name;
            teacherCourseData.courseParameters.subjectID = course.SubjectID;
            teacherCourseData.courseParameters.description = course.Description;

            teacherCourseData.groupModels = course.SubjectCourseAvailable.Select(model => new GroupModel { id = model.Group.GroupID, name = model.Group.Course.Name + "-" + model.Group.GroupNumber, subjectId = model.SubjectCourse.SubjectID, isAvailable = model.SubjectCourseAvailable1 }).ToArray();

            return Ok(teacherCourseData);
        }

        // GET: api/SubjectCourses
        [Authorize(Roles = "admin, teacher")]
        public IQueryable<SubjectCourse> GetSubjectCourse()
        {
            return db.SubjectCourse;
        }

        // GET: api/SubjectCourses
        [Route("api/Pupil/SubjectCourses")]
        [Authorize]
        public IQueryable<SubjectCourseModel> GetSubjectCoursePupil()
        {
            return db.SubjectCourse.Select(sc => new SubjectCourseModel { Id = sc.SubjectCourseID, Name = sc.Name, Description = sc.Description, Image = sc.Image, Subject = sc.Subject.Name, Teacher = sc.Teacher.Person.UserName });
        }

        // GET: api/SubjectCourses
        [Route("api/Pupil/Mobile/SubjectCourses")]
        [Authorize]
        public IQueryable<SubjectCourseModel> GetSubjectCoursePupilMobile()
        {
            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var pupil = db.Pupil.Where(person => person.PersonID == personId).FirstOrDefault();

            if (pupil == null || !pupil.ConfirmedAccount)
            {
                return null; //TODO
            }

            return db.SubjectCourse.Where(sub => sub.SubjectCourseAvailable.Any(av => av.GroupID == pupil.GroupID)).Select(sc => new SubjectCourseModel { Id = sc.SubjectCourseID, Name = sc.Name, Description = sc.Description, Image = sc.Image, Subject = sc.Subject.Name, Teacher = sc.Teacher.Person.UserName });
        }

        [Route("api/Pupil/SubjectCourseFinish/{id}")]
        [Authorize]
        public async Task<IHttpActionResult> SubjectCourseFinish(int id)
        {                                                                                                                        //1
            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var pupil = db.Pupil.Where(person => person.PersonID == personId).FirstOrDefault();

            if (pupil == null || !pupil.ConfirmedAccount)                                                                        //2
            {
                return Unauthorized();                                                                                           //3
            }

            var sectionBlockResult = 
                db.SectionBlockResult
                .Where(res => res.SectionBlockID == id && res.SubjectSectionResult.PersonID == pupil.PersonID)
                .FirstOrDefault();

            if (sectionBlockResult != null)                                                                                      //4
            {
                return Ok();                                                                                                     //5
            }

            var sb = await db.SectionBlock.FindAsync(id);
            var ssResult = sb.SubjectSection.SubjectSectionResult.Where(ss => ss.PersonID == pupil.PersonID).FirstOrDefault();

            var positions = sb.SubjectSection.SectionBlock.Select(sbPos => sbPos.Position);
            
            var count = positions.Where(pos => sb.Position >= pos).Count();                                                                                                          //9

            if (count == positions.Count())                                                                                       //6
            {
                ssResult.FinishTime = DateTime.Now;                                                                               //7
                db.Entry(ssResult).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            if (ssResult == null)                                                                                                 //8
            {                                                                                                                     //9
                ssResult = new SubjectSectionResult {                                                                             
                    StartTime = DateTime.Now,
                    PersonID = pupil.PersonID,
                    SubjectSectionID = sb.SubjectSectionID
                };

                db.SubjectSectionResult.Add(ssResult);
                await db.SaveChangesAsync();

                var sbResult = new SectionBlockResult {
                    FinishTime = DateTime.Now,
                    SectionBlockID = sb.SectionBlockID,
                    SubjectSectionResultID = ssResult.SubjectSectionResultID
                };

                db.SectionBlockResult.Add(sbResult);
                await db.SaveChangesAsync();
                return Ok();                                                                                                      //5
            } else
            {                                                                                                                     //10                                 
                var sbResult = new SectionBlockResult {
                    FinishTime = DateTime.Now,
                    SectionBlockID = sb.SectionBlockID,
                    SubjectSectionResultID = ssResult.SubjectSectionResultID
                };

                db.SectionBlockResult.Add(sbResult);
                await db.SaveChangesAsync();
                return Ok();                                                                                                      //5 
            }
        }                                                                                                                         //11                                                                                                        

        // GET: api/SubjectCourses/5
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(SubjectCourse))]
        public async Task<IHttpActionResult> GetSubjectCourse(int id)
        {
            SubjectCourse subjectCourse = await db.SubjectCourse.FindAsync(id);
            if (subjectCourse == null)
            {
                return NotFound();
            }

            return Ok(subjectCourse);
        }

        // PUT: api/SubjectCourses/5
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSubjectCourse(int id, SubjectCourse subjectCourse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subjectCourse.SubjectCourseID)
            {
                return BadRequest();
            }

            db.Entry(subjectCourse).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectCourseExists(id))
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

        // POST: api/SubjectCourses
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(SubjectCourse))]
        public async Task<IHttpActionResult> PostSubjectCourse(TeacherCourseData teacherCourseData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var teacher = db.Teacher.Where(person => person.PersonID == personId).FirstOrDefault();

            var newSubjectCourse = new SubjectCourse
            {
                Name = teacherCourseData.courseParameters.name,
                SubjectID = teacherCourseData.courseParameters.subjectID,
                Description = teacherCourseData.courseParameters.description,
                TeacherID = teacher.PersonID
            };

            db.SubjectCourse.Add(newSubjectCourse);
            await db.SaveChangesAsync();

            var course = db.SubjectCourse.Where(crs => crs.Name == newSubjectCourse.Name && crs.TeacherID == newSubjectCourse.TeacherID && crs.SubjectID == newSubjectCourse.SubjectID).FirstOrDefault();

            foreach (var group in teacherCourseData.groupModels)
            {
                if (course.SubjectID == group.subjectId)
                {
                    db.SubjectCourseAvailable.Add(new SubjectCourseAvailable { GroupID = group.id, SubjectCourseAvailable1 = group.isAvailable, SubjectCourseID = course.SubjectCourseID });
                }
            }


            await db.SaveChangesAsync();

            return Ok(newSubjectCourse.SubjectCourseID);
        }

        // DELETE: api/SubjectCourses/5
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(SubjectCourse))]
        public async Task<IHttpActionResult> DeleteSubjectCourse(int id)
        {
            SubjectCourse subjectCourse = await db.SubjectCourse.FindAsync(id);
            if (subjectCourse == null)
            {
                return NotFound();
            }

            db.SubjectCourse.Remove(subjectCourse);
            await db.SaveChangesAsync();

            return Ok(subjectCourse);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubjectCourseExists(int id)
        {
            return db.SubjectCourse.Count(e => e.SubjectCourseID == id) > 0;
        }
    }
}