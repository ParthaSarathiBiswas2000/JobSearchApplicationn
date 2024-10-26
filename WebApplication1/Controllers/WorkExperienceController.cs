using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class WorkExperienceController : Controller
    {
        private JobHuntDbEntities db = new JobHuntDbEntities();
        // GET: WorkExperience
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);

            var result = db.WorkExperienceTables.Where(e => e.EmployeeID == id).FirstOrDefault();
            if (result == null)
            {

                var employeedetails = db.EmployeeTables.Where(j => j.EmployeeID == id).FirstOrDefault();
                var work = new WorkExperienceMV();
                work.EmployeeName = employeedetails.EmployeeName;
                work.EmployeeID = (int)id;

                ViewBag.CountryID = new SelectList(db.CountryTables.ToList(), "CountryID", "Country", "0");

                return View(work);
            }
            else
            {


                var employeedetails = db.EmployeeTables.Where(j => j.EmployeeID == id).FirstOrDefault();
                var existingwork = new WorkExperienceMV();
                existingwork.EmployeeName = employeedetails.EmployeeName;
                existingwork.Company = result.Company;
                existingwork.Title = result.Title;
                existingwork.CountryID = result.CountryID;
                existingwork.FromYear = result.FromYear;
                existingwork.ToYear = result.ToYear;
                existingwork.Description = result.Description;
                existingwork.EmployeeID = (int)id;


                ViewBag.CountryID = new SelectList(db.CountryTables.ToList(), "CountryID", "Country", result.CountryID);
                return View(existingwork);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkExperienceMV workexperienceMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);

            if (ModelState.IsValid)
            {

                var result = db.WorkExperienceTables.Where(e => e.EmployeeID == workexperienceMV.EmployeeID).FirstOrDefault();
                if (result == null)
                {
                    var workex = new WorkExperienceTable();
                    workex.Company = workexperienceMV.Company;
                    workex.Title = workexperienceMV.Title;
                    workex.CountryID = workexperienceMV.CountryID;
                    workex.FromYear = workexperienceMV.FromYear;
                    workex.ToYear = workexperienceMV.ToYear;
                    workex.Description = workexperienceMV.Description;
                    workex.EmployeeID = workexperienceMV.EmployeeID;
                    db.WorkExperienceTables.Add(workex);
                    var a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["Successwork"] = "Your work experience is successfully submitted !!";
                        return RedirectToAction("Create");
                    }

                }
                else
                {
                    TempData["Failedwork"] = "Your work experience is already registered!!";
                    return RedirectToAction("Create");
                }
            }
            ViewBag.CountryID = new SelectList(db.CountryTables.ToList(), "CountryID", "Country", "0");
            return View(workexperienceMV);
        }
    }
}