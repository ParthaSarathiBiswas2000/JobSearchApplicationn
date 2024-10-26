using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class WorkExperienceMV
    {
        public WorkExperienceMV() 
        {
            Country = new CountryTable();
            Employee = new EmployeeTable();
        }

        public string EmployeeName { get; set; }
        public int WorkExperienceID { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public int CountryID { get; set; }
        public System.DateTime FromYear { get; set; }
        public System.DateTime ToYear { get; set; }
        public string Description { get; set; }
        public int EmployeeID { get; set; }

        public virtual CountryTable Country { get; set; }
        public virtual EmployeeTable Employee { get; set; }
    }
}