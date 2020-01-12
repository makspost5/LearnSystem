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
using WebAPI.Models.TeacherResult;

namespace WebAPI.Controllers
{
    [Authorize]
    public class ResultsController : ApiController
    {
        private GraduateWorkEntities db = new GraduateWorkEntities();

        // GET: api/Results
        public IQueryable<Result> GetResult()
        {
            return db.Result;
        }

        // GET: api/Results/5
        [ResponseType(typeof(Result))]
        public async Task<IHttpActionResult> GetResult(int id)
        {
            Result result = await db.Result.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/TeacherResults/Test
        [Route("api/TeacherResults/Test")]
        public async Task<IHttpActionResult> GetTeacherResultsTest()
        {
            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var teacher = db.Teacher.Where(person => person.PersonID == personId).FirstOrDefault();

            if (teacher == null || teacher.TeacherSubjects.Count == 0)
            {
                return Unauthorized();
            }

            return Ok(teacher.Test.Select(test => new TestResultModel { TestID = test.TestID, Name = test.Name, Subject = test.Subject.Name, TypeTest = test.TestType.Name, GroupResultModels = test.TestAvailable.Select(ta => new GroupResultModel { GroupID = ta.GroupID, Name = ta.Group.Course.Name + "-" + ta.Group.GroupNumber }).ToArray() }));
        }

        // GET: api/TeacherResults/Course
        [Route("api/TeacherResults/Course")]
        public async Task<IHttpActionResult> GetTeacherResultsCourse()
        {
            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var teacher = db.Teacher.Where(person => person.PersonID == personId).FirstOrDefault();

            if (teacher == null || teacher.TeacherSubjects.Count == 0)
            {
                return Unauthorized();
            }
          
            return Ok(teacher.SubjectCourse.Select(course => new CourseResultModel { SubjectCourseID = course.SubjectCourseID, Name = course.Name, Subject = course.Subject.Name, SubjectSectionCount = course.SubjectSection.Count, GroupResultModels = course.SubjectCourseAvailable.Select(ta => new GroupResultModel { GroupID = ta.GroupID, Name = ta.Group.Course.Name + "-" + ta.Group.GroupNumber }).ToArray() }));
        }

        // GET: api/TeacherResults
        [Route("api/TeacherResults/Test/{groupId}/{testId}")]
        public async Task<IHttpActionResult> GetTeacherResultsTestByGroup(int groupId, int testId)
        {
            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);                        //1

            var teacher = db.Teacher.Where(person => person.PersonID == personId).FirstOrDefault();

            if (teacher == null || teacher.TeacherSubjects.Count == 0)                              //2
            {
                return Unauthorized();                                                              //3
            }

            if (!db.Group.Any(gr => gr.GroupID == groupId))                                         //4
            {
                return NotFound();                                                                  //5
            }

            if (!teacher.Test.Any(test => test.TestID == testId))                                   //6
            {
                return NotFound();                                                                  //5
            }

            return Ok(teacher.Test.Where(t => t.TestID == testId)                                   //7
                .FirstOrDefault()
                .Result
                .Where(result => result.Pupil.GroupID == groupId)
                .Select(result => new PersonResultModelTest {
                    Name = result.Pupil.Person.UserName,
                    Grade = result.Grade,
                    Finish = result.FinishTime.ToString()
                }));
        }                                                                                           //8

        // GET: api/TeacherResults
        [Route("api/TeacherResults/Course/{groupId}/{courseId}")]
        public async Task<IHttpActionResult> GetTeacherResultsCourseByGroup(int groupId, int courseId)
        {
            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var teacher = db.Teacher.Where(person => person.PersonID == personId).FirstOrDefault();

            if (teacher == null || teacher.TeacherSubjects.Count == 0)
            {
                return Unauthorized();
            }

            if (!db.Group.Any(gr => gr.GroupID == groupId))
            {
                return NotFound();
            }

            if (!teacher.SubjectCourse.Any(course => course.SubjectCourseID == courseId))
            {
                return NotFound();
            }

            List<PersonResultModelCourse> personResultModelCourses = new List<PersonResultModelCourse>();

            foreach (var subjectSection in teacher.SubjectCourse.Where(t => t.SubjectCourseID == courseId && t.SubjectCourseAvailable.Any(ssA => ssA.GroupID == groupId)).FirstOrDefault().SubjectSection)
            {
                personResultModelCourses.AddRange(subjectSection.SubjectSectionResult.Where(result => result.Pupil.GroupID == groupId).Select(sResult => new PersonResultModelCourse { Name = sResult.Pupil.Person.UserName, LastTime = sResult.FinishTime == null ? sResult.StartTime.ToString() : sResult.FinishTime.ToString(), SectionName = sResult.SubjectSection.Name }).ToList());
            }

            var results = personResultModelCourses.OrderBy(r => r.LastTime).ToList();
            
            return Ok(results);
        }

        // GET: api/PupilResults
        [Route("api/PupilResults")]
        public async Task<IHttpActionResult> GetPupilResults()
        {
            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var pupil = db.Pupil.Where(person => person.PersonID == personId).FirstOrDefault();

            if (pupil == null || !pupil.ConfirmedAccount)
            {
                return Unauthorized();
            }

            return Ok(pupil.Result.Select(result => new PupilResultModelTest { Test = result.Test.Name, Grade = result.Grade, TypeTest = result.Test.TestType.Name, Subject = result.Test.Subject.Name, Finish = result.FinishTime.ToString() }));
        }

        // PUT: api/Results/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutResult(int id, Result result)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != result.ResultID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(result).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ResultExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Results
        [ResponseType(typeof(Result))]
        public async Task<IHttpActionResult> PostResult(Result result)
        {
            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);                                                                //1

            var pupil = db.Pupil.Where(person => person.PersonID == personId).FirstOrDefault();

            if (pupil == null || !pupil.ConfirmedAccount)                                                                                   //2
            {
                return Unauthorized();                                                                                                      //3
            }

            if (!ModelState.IsValid)                                                                                                        //4
            {
                return BadRequest(ModelState);                                                                                              //5
            }

            result.PersonID = pupil.PersonID;
            result.FinishTime = DateTime.Now;
            double grad = 0;
            double allGrad = 0;

            foreach (var qr in result.QuestionResult)                                                                                       //6
            {
                var question = await db.Question.FindAsync(qr.QuestionID);

                bool isRight = false;
                int countRight = 0;

                switch (question.AnswerTypeID)                                                                                              //7 
                {
                    case 1:
                    case 2:
                        for (int i = 0; i < question.Answer.Count; i++)
                        {
                            if (question.Answer.ToArray()[i].IsRight == qr.AnswerResult.ToArray()[i].IsSelected)
                            {
                                countRight++;
                            }
                        }

                        if (countRight == question.Answer.Count)
                        {
                            isRight = true;
                        }

                        break;
                    case 3:
                        for (int i = 0; i < question.AnswerOrder.Count; i++)
                        {
                            if (question.AnswerOrder.ToArray()[i].SerialNumber == qr.AnswerOrderResult.ToArray()[i].SelectedSerialNumber)
                            {
                                countRight++;
                            }
                        }

                        if (countRight == question.AnswerOrder.Count)
                        {
                            isRight = true;
                        }
                        break;
                    case 5:
                        for (int i = 0; i < question.AnswerMatching.Count; i++)
                        {
                            if (question.AnswerMatching.ToArray()[i].RightParth == qr.AnswerMatchingResult.ToArray()[i].SelectedRightParth)
                            {
                                countRight++;
                            }
                        }

                        if (countRight == question.AnswerMatching.Count)
                        {
                            isRight = true;
                        }
                        break;
                }

                allGrad += question.NumberOfPoints.Value;

                if (isRight)
                {
                    grad += question.NumberOfPoints.Value;
                }
            }

            result.Grade = grad/allGrad * 5;

            db.Result.Add(result);
            await db.SaveChangesAsync();

            return Ok("Тест пройден");
        }

        //// DELETE: api/Results/5
        //[ResponseType(typeof(Result))]
        //public async Task<IHttpActionResult> DeleteResult(int id)
        //{
        //    Result result = await db.Result.FindAsync(id);
        //    if (result == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Result.Remove(result);
        //    await db.SaveChangesAsync();

        //    return Ok(result);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ResultExists(int id)
        {
            return db.Result.Count(e => e.ResultID == id) > 0;
        }
    }
}