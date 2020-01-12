using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.TeacherCourse
{
    public class QuestionModel
    {
        public int QuestionID { get; set; }
        public string Body { get; set; }
        public int AnswerTypeID { get; set; }

        public TheoryModel Theory { get; set; }

        public Answer[] Answer { get; set; }
        public AnswerMatching[] AnswerMatching { get; set; }
        public AnswerOrder[] AnswerOrder { get; set; }

        public Question toDBModel()
        {
            return new Question { QuestionID = this.QuestionID, Body = Body, AnswerTypeID = AnswerTypeID };
        }
    }
}