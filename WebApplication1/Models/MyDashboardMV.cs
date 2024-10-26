using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MyDashboardMV
    {
        public MyDashboardMV()
        {
            Educations = new EducationTable();
            WorkExperience = new WorkExperienceTable();
            Skill = new List<SkillTable>();
            User = new UserTable();
            
        }
        public string EmployeeName { get; set; }
        public System.DateTime DOB { get; set; }
        public string CNIC { get; set; }
        public string FNIC { get; set; }
        public string FatherName { get; set; }
        public int CountryID { get; set; }
        public string EmailAddress { get; set; }
        public string Gender { get; set; }
        public string ContactNo { get; set; }
        public string Photo { get; set; }
        public string Qualification { get; set; }
        public string PermanentAddress { get; set; }
        public string InstituteName { get; set; }
        public string TitleOfEducation { get; set; }
        public string Degree { get; set; }
        public System.DateTime FromYear { get; set; }
        public System.DateTime ToYear { get; set; }
        public string City { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public int CountryID1 { get; set; }
        public System.DateTime FromYear1 { get; set; }
        public System.DateTime ToYear1 { get; set; }
        public string SkillName { get; set; }
        public EmployeeTable Employee { get; set; }
        public EducationTable Educations { get; set; }
        public WorkExperienceTable WorkExperience { get; set; }
        public List<SkillTable> Skill { get; set; }
        public UserTable User { get; set; }
        
    }
}