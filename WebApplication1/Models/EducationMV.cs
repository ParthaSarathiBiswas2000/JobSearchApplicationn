using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class EducationMV
    {
        public EducationMV()
        {
            Employee = new EmployeeTable();
            Country = new CountryTable();
        }

        public string EmployeeName { get; set; }
        public string JobName { get; set; }
        public string CompanyName { get; set; }
        public int EducationID { get; set; }

        [Required(ErrorMessage = "Institute Name is Required")]
        public string InstituteName { get; set; }

        [Required(ErrorMessage = "Stream is Required")]
        public string TitleOfEducation { get; set; }

        [Required(ErrorMessage = "Degree is Required")]
        public string Degree { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        public System.DateTime FromYear { get; set; }

        [Required(ErrorMessage = "This field is Required")]
        public System.DateTime ToYear { get; set; }

        [Required(ErrorMessage = "City is Required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is Required")]
        public int CountryID { get; set; }
        public int EmployeeID { get; set; }

        public virtual EmployeeTable Employee {  get; set; }
        public virtual CountryTable Country { get; set; }
    }
}