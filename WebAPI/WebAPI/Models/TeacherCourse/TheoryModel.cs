using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.TeacherCourse
{
    public class TheoryModel
    {
        public int QuestionID { get; set; }
        public string Body { get; set; }
        public int SectionBlockID { get; set; }
        public int Position { get; set; }

        public Theory toDBModel()
        {
            return new Theory { QuestionID = QuestionID, Body = Body, SectionBlockID = SectionBlockID, Position = Position };
        }

        public Theory toDBModel(int QuestionID, int SectionBlockID)
        {
            return new Theory { QuestionID = QuestionID, Body = Body, SectionBlockID = SectionBlockID, Position = Position };
        }
    }
}