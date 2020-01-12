using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    public class SubjectSection
    {
        public int SubjectSectionID { get; set; }
        public string Name { get; set; }
        public string image { get; set; }
        public int SubjectCourseID { get; set; }
        public int HierarchyLevel { get; set; }
    }
}
