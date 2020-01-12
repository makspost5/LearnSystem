using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.TeacherTest
{
    public class Question
    {
        public int id;
        public string body;
        public double numberOfPoints;
        public int selectedTypeValue;
        public Answer[] answers;
        public AnswerOrder[] answerOrder;
        public AnswerMatching[] answerMatching;
    }
}