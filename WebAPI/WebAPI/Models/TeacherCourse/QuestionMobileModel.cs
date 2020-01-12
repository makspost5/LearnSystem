using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.TeacherCourse
{
    public class QuestionMobileModel
    {
        public int QuestionID { get; set; }
        public string Body { get; set; }
        public ICollection<Answer> Answer { get; set; }
    }
}