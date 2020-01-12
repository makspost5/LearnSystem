using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.TeacherResult
{
    public class TestResultModel
    {
        public int TestID { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string TypeTest { get; set; }
        public GroupResultModel[] GroupResultModels { get; set; }
    }

    public class GroupResultModel
    {
        public int GroupID { get; set; }
        public string Name { get; set; }
    }

    public class CourseResultModel
    {
        public int SubjectCourseID { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public int SubjectSectionCount { get; set; }
        public GroupResultModel[] GroupResultModels { get; set; }
    }

    public class PersonResultModelTest
    {
        public string Name { get; set; }
        public double Grade { get; set; }
        public string Finish { get; set; }
    }

    public class PersonResultModelCourse
    {
        public string Name { get; set; }
        public string SectionName { get; set; }
        public string LastTime { get; set; }
    }

    public class PupilResultModelTest
    {
        public double Grade { get; set; }
        public string Finish { get; set; }
        public string Test { get; set; }
        public string Subject { get; set; }
        public string TypeTest { get; set; }
    }
}