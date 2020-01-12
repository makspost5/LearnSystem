using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    //[Authorize(Roles = "admin, teacher")]
    public class AnswerTypeController : ApiController
    {
        private GraduateWorkEntities db = new GraduateWorkEntities();

        // GET: api/AnswerType
        public IQueryable<AnswerType> GetAnswerType()
        {
            return db.AnswerType;
        }
    }
}
