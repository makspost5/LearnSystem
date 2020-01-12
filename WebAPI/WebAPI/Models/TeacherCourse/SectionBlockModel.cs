using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.TeacherCourse
{
    public class SectionBlockModel
    {
        public int SectionBlockID { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int SubjectSectionID { get; set; }

        public QuestionModel[] Question { get; set; }

        public SectionBlock toDBModel()
        {
            return new SectionBlock { Name = Name, Position = Position, SectionBlockID = SectionBlockID, SubjectSectionID = SubjectSectionID };
        }

        public SectionBlock toDBModel(int SubjectSectionID)
        {
            return new SectionBlock { Name = this.Name, Position = Position, SectionBlockID = SectionBlockID, SubjectSectionID = SubjectSectionID };
        }
    }
}