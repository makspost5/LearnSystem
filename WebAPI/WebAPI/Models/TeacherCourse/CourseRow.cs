using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.TeacherCourse
{
    public class CourseRow
    {
        public int ID;
        public string Name;
        public string Subject;
        public int SectionsCount;
        public bool Available;
    }
}