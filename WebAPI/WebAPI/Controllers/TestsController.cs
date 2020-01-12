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
using WebAPI.Models;
using WebAPI.Models.TeacherTest;

namespace WebAPI.Controllers
{
    public class TestsController : ApiController
    {
        private GraduateWorkEntities db = new GraduateWorkEntities();

        // GET: api/Tests
        [Authorize(Roles = "admin, teacher")]
        public List<TestModel> GetTest()
        {
            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var teacher = db.Teacher.Where(person => person.PersonID == personId).FirstOrDefault();

            if (teacher == null || teacher.TeacherSubjects.Count == 0)
            {
                return null; //TODO
            }

            List<TestModel> tests = new List<TestModel>();

            foreach (var subject in teacher.TeacherSubjects.Select(ts => ts.Subject))
            {
                tests.AddRange(subject.Test.Select(test => new TestModel { TestID = test.TestID, Name = test.Name, image = test.image, PassageMinutes = test.PassageMinutes, SubjectName = test.Subject.Name, TypeTest = test.TestType.Name }));
            }

            return tests;
        }

        // GET: api/Tests/5
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(Test))]
        public async Task<IHttpActionResult> GetTest(int id)
        {
            Test test = await db.Test.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }

            return Ok(test);
        }

        // GET: api/Tests/UpdateData/5
        [Authorize(Roles = "admin, teacher")]
        [Route("api/Tests/UpdateData/{id}")]
        [ResponseType(typeof(TeacherTestData))]
        public async Task<IHttpActionResult> GetTestUpdateData(int id)
        {
            TeacherTestData teacherTestData = new TeacherTestData();
            Test test = await db.Test.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }

            teacherTestData.testParameters = new TestParameters();

            teacherTestData.testParameters.id = test.TestID;
            teacherTestData.testParameters.name = test.Name;
            teacherTestData.testParameters.subjectID = test.SubjectID;
            teacherTestData.testParameters.typeTestID = test.TypeTestID;

            var questions = new List<Models.TeacherTest.Question>();

            foreach (var question in test.Question)
            {
                questions.Add(new Models.TeacherTest.Question
                {
                    id = question.QuestionID,
                    body = question.Body,
                    selectedTypeValue = question.AnswerTypeID,
                    numberOfPoints = question.NumberOfPoints.Value
                });

                switch (question.AnswerTypeID)
                {
                    case 1:
                    case 2:
                        var standartAnswers = new List<Models.TeacherTest.Answer>();

                        foreach (var answer in question.Answer)
                        {
                            standartAnswers.Add(new Models.TeacherTest.Answer
                            {
                                id = answer.AnswerID,
                                body = answer.Body,
                                isRight = answer.IsRight
                            });
                        }

                        questions[questions.Count - 1].answers = standartAnswers.ToArray();

                        break;
                    case 3:
                        var orderAnswers = new List<Models.TeacherTest.AnswerOrder>();
                        foreach (var answerOrder in question.AnswerOrder)
                        {
                            orderAnswers.Add(new Models.TeacherTest.AnswerOrder
                            {
                                id = answerOrder.AnswerOrderID,
                                body = answerOrder.Body,
                                number = answerOrder.SerialNumber
                            });
                        }
                        questions[questions.Count - 1].answerOrder = orderAnswers.ToArray();
                        break;
                    case 4:
                        //TODO
                        //db.Answer.Add(new Answer
                        //{
                        //    Body = question.answers[0].body,
                        //    IsRight = question.answers[0].isRight,
                        //    QuestionID = addedQuestion.QuestionID
                        //});
                        break;
                    case 5:
                        var answerMatching = new List<Models.TeacherTest.AnswerMatching>();
                        foreach (var answerMatch in question.AnswerMatching)
                        {
                            answerMatching.Add(new Models.TeacherTest.AnswerMatching
                            {
                                id = answerMatch.AnswerMatchingID,
                                LeftParth = answerMatch.LeftParth,
                                RightParth = answerMatch.RightParth
                            });
                        }
                        questions[questions.Count - 1].answerMatching = answerMatching.ToArray();
                        break;
                    case 6:
                        break;
                }
            }

            teacherTestData.questions = questions.ToArray();

            teacherTestData.groupModels = test.TestAvailable.Select(model => new GroupModel { id = model.Group.GroupID, name = model.Group.Course.Name + "-" + model.Group.GroupNumber, subjectId = model.Test.SubjectID, isAvailable = model.IsTestAvailable }).ToArray();

            return Ok(teacherTestData);
        }

        // GET: api/Test/TestRows
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(TestRow))]
        [Route("api/Test/TestRows")]
        public List<TestRow> GetTestRows()
        {
            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var teacher = db.Teacher.Where(person => person.PersonID == personId).FirstOrDefault();

            if (teacher == null || teacher.TeacherSubjects.Count == 0)
            {
                return null; //TODO
            }
            List<TestRow> testRows = new List<TestRow>();

            foreach (var subject in teacher.TeacherSubjects.Select(ts => ts.Subject))
            {
                testRows.AddRange(subject.Test.Select(test => new TestRow
                {
                    ID = test.TestID,
                    Name = test.Name,
                    Subject = test.Subject.Name,
                    TestType = test.TestType.Name,
                    Available = test.TestAvailable.Any(ta => ta.IsTestAvailable == true)
                }));
            }

            return testRows;
        }

        // GET: api/Pupil/Test/TestRows
        [Authorize]
        [ResponseType(typeof(Models.Pupil.Test.TestRow))]
        [Route("api/Pupil/Test/TestRows")]
        public List<Models.Pupil.Test.TestRow> GetPupilTestRows()
        {
            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var pupil = db.Pupil.Where(person => person.PersonID == personId).FirstOrDefault();

            if (pupil == null || !pupil.ConfirmedAccount)
            {
                return null; //TODO
            }
            List<Models.Pupil.Test.TestRow> testRows = new List<Models.Pupil.Test.TestRow>();

            foreach (var subject in pupil.Group.GroupSubject.Select(ts => ts.Subject))
            {
                testRows.AddRange(subject.Test.Where(test => test.TestAvailable.Where(av => av.GroupID == pupil.GroupID).FirstOrDefault().IsTestAvailable).Select(test => new Models.Pupil.Test.TestRow
                {
                    ID = test.TestID,
                    Name = test.Name,
                    Subject = test.Subject.Name,
                    TestType = test.TestType.Name,
                    TeacherName = test.Teacher.Person.UserName,
                    Mark = pupil.Result.Where(r => r.TestID == test.TestID).FirstOrDefault() == null ? -1 : pupil.Result.Where(r => r.TestID == test.TestID).FirstOrDefault().Grade
                }));
            }

            return testRows;
        }

        // PUT: api/Tests/5
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTest(int id, Test test)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != test.TestID)
            {
                return BadRequest();
            }

            db.Entry(test).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestExists(id))
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

        // POST: api/Tests
        //[ResponseType(typeof(Test))]
        [Authorize(Roles = "admin, teacher")]
        public async Task<IHttpActionResult> PostTest(TeacherTestData teacherTestData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var teacher = db.Teacher.Where(person => person.PersonID == personId).FirstOrDefault();

            if (teacherTestData.mode == "create")
                db.Test.Add(new Test
                {
                    TeacherID = teacher.PersonID,
                    Name = teacherTestData.testParameters.name,
                    SubjectID = teacherTestData.testParameters.subjectID,
                    TypeTestID = teacherTestData.testParameters.typeTestID,
                });
            else if (teacherTestData.mode == "update")
            {
                var newTest = db.Test.Where(testDb => testDb.TestID == teacherTestData.testParameters.id).FirstOrDefault();

                newTest.TeacherID = teacher.PersonID;
                newTest.Name = teacherTestData.testParameters.name;
                newTest.SubjectID = teacherTestData.testParameters.subjectID;
                newTest.TypeTestID = teacherTestData.testParameters.typeTestID;

                db.Entry(newTest).State = EntityState.Modified;
                db.SaveChanges();
            }

            await db.SaveChangesAsync();

            var test = db.Test.Where(testDB => testDB.Name == teacherTestData.testParameters.name && testDB.TeacherID == teacher.PersonID).FirstOrDefault();

            foreach (var question in teacherTestData.questions)
            {
                if (teacherTestData.mode == "create" || question.id == 0)
                    db.Question.Add(new Question
                    {
                        TestID = test.TestID,
                        Body = question.body,
                        NumberOfPoints = question.numberOfPoints,
                        AnswerTypeID = question.selectedTypeValue
                    });
                else if (teacherTestData.mode == "update")
                {
                    var newQuestion = db.Question.Where(questionDB => questionDB.QuestionID == question.id).FirstOrDefault();
                    newQuestion.TestID = test.TestID;
                    newQuestion.Body = question.body;
                    newQuestion.NumberOfPoints = question.numberOfPoints;
                    newQuestion.AnswerTypeID = question.selectedTypeValue;

                    db.Entry(newQuestion).State = EntityState.Modified;
                    db.SaveChanges();
                }

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                var addedQuestion = db.Question.Where(questionDB => questionDB.Body == question.body && questionDB.TestID == test.TestID).AsEnumerable().LastOrDefault();

                switch (question.selectedTypeValue)
                {
                    case 1:
                    case 2:
                        foreach (var answer in question.answers)
                        {
                            if (teacherTestData.mode == "create" || answer.id == 0)
                                db.Answer.Add(new Answer
                                {
                                    Body = answer.body,
                                    IsRight = answer.isRight,
                                    QuestionID = addedQuestion.QuestionID
                                });
                            else if (teacherTestData.mode == "update")
                            {
                                var newAnswer = db.Answer.Where(answerDB => answerDB.AnswerID == answer.id).FirstOrDefault();

                                newAnswer.Body = answer.body;
                                newAnswer.IsRight = answer.isRight;
                                newAnswer.QuestionID = addedQuestion.QuestionID;

                                db.Entry(newAnswer).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }

                        break;
                    case 3:
                        foreach (var answerOrder in question.answerOrder)
                        {
                            if (teacherTestData.mode == "create" || answerOrder.id == 0)
                                db.AnswerOrder.Add(new AnswerOrder
                                {
                                    QuestionID = addedQuestion.QuestionID,
                                    Body = answerOrder.body,
                                    SerialNumber = answerOrder.number
                                });
                            else if (teacherTestData.mode == "update")
                            {
                                var newAnswerOrder = db.AnswerOrder.Where(answerDB => answerDB.AnswerOrderID == answerOrder.id).FirstOrDefault();

                                newAnswerOrder.QuestionID = addedQuestion.QuestionID;
                                newAnswerOrder.Body = answerOrder.body;
                                newAnswerOrder.SerialNumber = answerOrder.number;

                                db.Entry(newAnswerOrder).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        break;
                    case 4:
                        //TODO
                        //db.Answer.Add(new Answer
                        //{
                        //    Body = question.answers[0].body,
                        //    IsRight = question.answers[0].isRight,
                        //    QuestionID = addedQuestion.QuestionID
                        //});
                        break;
                    case 5:
                        foreach (var answerMatching in question.answerMatching)
                        {
                            if (teacherTestData.mode == "create" || answerMatching.id == 0)
                                db.AnswerMatching.Add(new AnswerMatching
                                {
                                    QuestionID = addedQuestion.QuestionID,
                                    LeftParth = answerMatching.LeftParth,
                                    RightParth = answerMatching.RightParth
                                });
                            else if (teacherTestData.mode == "update")
                            {
                                var newAnswerMatching = db.AnswerMatching.Where(answerDB => answerDB.AnswerMatchingID == answerMatching.id).FirstOrDefault();

                                newAnswerMatching.QuestionID = addedQuestion.QuestionID;
                                newAnswerMatching.LeftParth = answerMatching.LeftParth;
                                newAnswerMatching.RightParth = answerMatching.RightParth;

                                db.Entry(newAnswerMatching).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        break;
                    case 6:
                        break;

                }
            }

            foreach (var group in teacherTestData.groupModels)
            {
                if (teacherTestData.mode == "create")
                {
                    if (test.SubjectID == group.subjectId)
                    {
                        db.TestAvailable.Add(new TestAvailable { GroupID = group.id, IsTestAvailable = group.isAvailable, TestID = test.TestID });
                    }
                }
                else
                {
                    var newTestAvailable = db.TestAvailable.Where(testA => testA.TestID == test.TestID && testA.GroupID == group.id).FirstOrDefault();
                    newTestAvailable.IsTestAvailable = group.isAvailable;
                    db.Entry(newTestAvailable).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        // DELETE: api/Tests/5
        [Authorize(Roles = "admin, teacher")]
        [ResponseType(typeof(Test))]
        public async Task<IHttpActionResult> DeleteTest(int id)
        {
            Test test = await db.Test.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }

            db.Test.Remove(test);
            await db.SaveChangesAsync();

            return Ok(test);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TestExists(int id)
        {
            return db.Test.Count(e => e.TestID == id) > 0;
        }
    }
}