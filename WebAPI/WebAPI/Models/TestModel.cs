using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class TestModel
    {
        public int TestID { get; set; }
        public string Name { get; set; }
        public string SubjectName { get; set; }
        public string TypeTest { get; set; }
        public int? PassageMinutes { get; set; }
        public string image { get; set; }
    }
}