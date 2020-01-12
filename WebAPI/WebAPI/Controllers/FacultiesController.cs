using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI;

namespace WebAPI.Controllers
{
    public class FacultiesController : ApiController
    {
        private GraduateWorkEntities db = new GraduateWorkEntities();

        // GET: api/Faculties
        public IQueryable<Faculty> GetFaculty()
        {
            return db.Faculty;
        }
    }
}