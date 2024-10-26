using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class SkillsMV
    {
        public SkillsMV()
        {
            Details = new List<SkillTable>();
        }
        public int SkillID { get; set; }
        public string SkillName { get; set; }
        public int EmployeeID { get; set; }

        public List<SkillTable> Details { get; set; }
    }
}