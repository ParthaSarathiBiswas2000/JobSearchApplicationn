using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class MyDashboardController : Controller
    {
        private JobHuntDbEntities db = new JobHuntDbEntities();
        // GET: MyDashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyAppliedJobs() 
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);

            var employee = db.EmployeeTables.Where(e=>e.UserID == userid).FirstOrDefault();
            if (employee == null)
            {
                TempData["Error1"] = "Your personal information is not registered";
                return RedirectToAction("Index", "Home");
            }

            var education = db.EducationTables.Where(e => e.EmployeeTable.UserID == userid).FirstOrDefault();
            if (education == null)
            {
                TempData["Error2"] = "Your education details is not registered";
                return RedirectToAction("Index", "Home");
            }

            var workex = db.WorkExperienceTables.Where(e => e.EmployeeTable.UserID == userid).FirstOrDefault();
            if (workex == null)
            {
                TempData["Error3"] = "Your work experience details is not registered";
                return RedirectToAction("Index", "Home");
            }
            var viewModel = new MyDashboardMV
            {
                Employee = employee,
                Educations = education,
                WorkExperience = workex,
            };


            return View(viewModel); 
        }

        public ActionResult MyApplicationForm(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);

            var employee = db.EmployeeTables.Where(e => e.EmployeeID == id).FirstOrDefault();

            var education = db.EducationTables.Where(e => e.EmployeeID == id).FirstOrDefault();
            
            var workex = db.WorkExperienceTables.Where(e => e.EmployeeID == id).FirstOrDefault();

            var skill = db.SkillTables.Where(e => e.EmployeeID == id).ToList();

            var user = db.UserTables.Where(e=>e.UserID == userid).FirstOrDefault();
            
            
            var viewModel = new MyDashboardMV
            {
                Employee = employee,
                Educations = education,
                WorkExperience = workex,
                Skill = skill,
                User = user,
                
            };


            return View(viewModel);
        }
    }
}