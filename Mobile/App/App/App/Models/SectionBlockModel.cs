using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    public class SectionBlockModel
    {
        public int SectionBlockID { get; set; }
        public string Name { get; set; }
        public int QuestionsCount { get; set; }
        public int Position { get; set; }
        public bool isPassed { get; set; }
        public string Color { get; set; }
    }
}
