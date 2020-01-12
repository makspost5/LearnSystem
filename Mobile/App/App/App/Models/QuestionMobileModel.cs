using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    public class QuestionMobileModel
    {
        public int QuestionID { get; set; }
        public string Body { get; set; }
        public Answer[] Answer { get; set; }
        public string TheoryBody { get; set; }
    }
}
