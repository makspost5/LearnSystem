using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    public class SubjectCourseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Teacher { get; set; }
        public string Subject { get; set; }
    }
}
