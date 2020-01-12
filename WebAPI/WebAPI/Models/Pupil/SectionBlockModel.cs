using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.Pupil
{
    public class SectionBlockModel
    {
        public int SectionBlockID { get; set; }
        public string Name { get; set; }
        public int QuestionsCount { get; set; }
        public int Position { get; set; }
        public bool isPassed { get; set; }
    }
}