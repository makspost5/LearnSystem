using System.Linq;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class CoursesController : ApiController
    {
        private GraduateWorkEntities db = new GraduateWorkEntities();

        // GET: api/Courses
        public IQueryable<Course> GetFaculty()
        {
            return db.Course;
        }
    }
}
