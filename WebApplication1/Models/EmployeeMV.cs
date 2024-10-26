using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Web;
using static System.Collections.Specialized.BitVector32;

namespace WebApplication1.Models
{
    public class EmployeeMV
    {
        private JobHuntDbEntities db = new JobHuntDbEntities();
        public EmployeeMV()
        {
            User = new UserMV();
            JobCategory = new JobCategoryTable();
            EducationMV = new List<EducationTable>();

            Employee = new List<EmployeeTable>();
        }
        public int EmployeeID { get; set; }
        public int UserID { get; set; }
        public int JobCategoryID { get; set; }
        public int PostJobID { get; set; }
        public string JobCategoryName { get; set; }
        public string JobName { get; set; }
        public string CompanyName { get; set; }
        public DateTime PostedOn { get; set; }
        public DateTime LastDate {  get; set; }

        [Required(ErrorMessage = "Employee name is Required")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Date of Birth is Required")]
        public System.DateTime DOB { get; set; }

        [Required(ErrorMessage = "CNIC is Required")]
        public string CNIC { get; set; }

        [Required(ErrorMessage = "FNIC is Required")]
        public string FNIC { get; set; }

        [Required(ErrorMessage = "Father name is Required")]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "Country is Required")]
        public int CountryID { get; set; }

        [Required(ErrorMessage = "Email Address is Required")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Gender is Required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Photo is Required")]
        public string Photo { get; set; }

        [Required(ErrorMessage = "Qualification is Required")]
        public string Qualification { get; set; }

        [Required(ErrorMessage = "Parmanent Address is Required")]
        public string PermanentAddress { get; set; }

        [Required(ErrorMessage = "Job Reference is Required")]
        public string JobReferences { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }

        public string ContactNo { get; set; }

        public virtual JobCategoryTable JobCategory { get; set; }
        public virtual UserMV User { get; set; }
        public virtual ICollection<EducationTable> Education { get; set; }
        public virtual List<EducationTable> EducationMV { get; set;}
        public virtual List<EmployeeTable> Employee { get; set; }

        

        
    }
}