using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    public class Answer
    {
        public int AnswerID { get; set; }
        public string Body { get; set; }
        public bool IsRight { get; set; }
        public int QuestionID { get; set; }
    }
}
