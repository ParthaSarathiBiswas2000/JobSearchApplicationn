using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PostJobMV
    {
        public int PostJobID { get; set; }
        public int UserID { get; set; }
        public int ComapnyID { get; set; }
        public int JobCategoryID { get; set; }

        [Required(ErrorMessage ="Required")]
        [StringLength(500,ErrorMessage ="Do not enter more than 500 characters")]
        public string JobTitle { get; set; }
        [Required(ErrorMessage = "Required")]
        [StringLength(2000, ErrorMessage = "Do not enter more than 2000 characters")]
        public string JobDescription { get; set; }
        [Required(ErrorMessage = "Required")]
        public int MinSalary { get; set; }
        [Required(ErrorMessage = "Required")]
        public int MaxSalary { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Required")]
        public int Vacancy { get; set; }
        public int JobNatureID { get; set; }
        
        public System.DateTime PostDate { get; set; }

        [Required(ErrorMessage = "Required")]
        public System.DateTime ApplicationLastDate { get; set; } = DateTime.Now.AddDays(30);
        public System.DateTime LastDate { get; set; }
        public int JobStatusID { get; set; }
    }
}