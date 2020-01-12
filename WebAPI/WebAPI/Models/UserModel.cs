using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class UserModel
    {
        public Person Person { get; set; }
        public int GroupId { get; set; }
    }
}