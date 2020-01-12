using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WebAPI.Models.TeacherTest;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "admin, teacher")]
    public class SubjectController : ApiController
    {
        private GraduateWorkEntities db = new GraduateWorkEntities();

        // GET: api/Subject
        public ICollection<Subject> GetSubjects()
        {
            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var teacher = db.Teacher.Where(person => person.PersonID == personId).FirstOrDefault();

            if (teacher == null || teacher.TeacherSubjects.Count == 0)
            {
                return null; //TODO
            }

            return teacher.TeacherSubjects.Select(ts => ts.Subject).ToArray();
        }

        // GET: api/SubjectGroups
        [Route("api/SubjectGroups")]
        public IEnumerable<GroupModel> GetSubjectGroups()
        {
            var personId = int.Parse(ClaimsPrincipal.Current.Identity.Name);

            var teacher = db.Teacher.Where(person => person.PersonID == personId).FirstOrDefault();

            if (teacher == null || teacher.TeacherSubjects.Count == 0)
            {
                return null; //TODO
            }

            List<GroupModel> groupModels = new List<GroupModel>();

            foreach (var subject in teacher.TeacherSubjects.Select(ts => ts.Subject).ToArray())
            {
                foreach (var group in subject.GroupSubject.Select(gs => gs.Group))
                {
                    groupModels.Add(new GroupModel { id = group.GroupID, name = group.Course.Name + "-" + group.GroupNumber, subjectId = subject.SubjectID, isAvailable = false });
                }

            }

            return groupModels;
        }
    }
}
