using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.TeacherTest
{
    public class TeacherTestData
    {
        public TestParameters testParameters;
        public Question[] questions;
        public GroupModel[] groupModels;
        public String mode = "create";
    }
}