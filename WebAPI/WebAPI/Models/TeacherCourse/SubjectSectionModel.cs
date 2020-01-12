using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.TeacherCourse
{
    public class SubjectSectionModel
    {
        public int SubjectSectionID { get; set; }
        public string Name { get; set; }
        public string image { get; set; }
        public int SubjectCourseID { get; set; }
        public int HierarchyLevel { get; set; }

        public SectionBlockModel[] SectionBlock { get; set; }

        public SubjectSection toDBModel()
        {
            return new SubjectSection { Name = this.Name, HierarchyLevel = HierarchyLevel, image = image, SubjectCourseID = SubjectCourseID, SubjectSectionID = SubjectSectionID };
        }
    }
}