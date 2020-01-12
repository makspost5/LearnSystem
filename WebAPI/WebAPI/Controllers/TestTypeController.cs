using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "admin, teacher")]
    public class TestTypeController : ApiController
    {
        private GraduateWorkEntities db = new GraduateWorkEntities();

        // GET: api/TestType
        public IQueryable<TestType> GetTestTypes()
        {
            return db.TestType;
        }
    }
}
