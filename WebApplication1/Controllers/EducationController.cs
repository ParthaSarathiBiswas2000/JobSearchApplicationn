using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EducationController : Controller
    {
        // GET: Education
        private JobHuntDbEntities db = new JobHuntDbEntities();

        public ActionResult CreateEducation(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);

            var result = db.EducationTables.Where(e =>e.EmployeeID == id).FirstOrDefault();
            if (result == null)
            {
                var educationDetails = db.EmployeeTables.Where(e => e.EmployeeID == id).FirstOrDefault();
                if (educationDetails != null)
                {
                    var details = new EducationMV();
                    details.EmployeeName = educationDetails.EmployeeName;
                    details.JobName = educationDetails.PostJobTable.JobTitle;
                    details.CompanyName = educationDetails.PostJobTable.CompanyTable.CompanyName;
                    details.EmployeeID = (int)id;

                    ViewBag.CountryID = new SelectList(db.CountryTables.ToList(), "CountryID", "Country", "0");
                    return View(details);
                }
                else
                {
                    TempData["Failure"] = "First Enter your Personal Details !!";
                    return RedirectToAction("FilterJob", "Job");
                }
            }
            else
            {
                var educationDetails = db.EmployeeTables.Where(e => e.EmployeeID == id).FirstOrDefault();
                var existingdetails = new EducationMV();
                existingdetails.EmployeeName = educationDetails.EmployeeName;
                existingdetails.JobName = educationDetails.PostJobTable.JobTitle;
                existingdetails.CompanyName = educationDetails.PostJobTable.CompanyTable.CompanyName;
                existingdetails.EmployeeID = (int)id;
                existingdetails.InstituteName = result.InstituteName;
                existingdetails.TitleOfEducation = result.TitleOfEducation;
                existingdetails.Degree = result.Degree;
                existingdetails.FromYear = result.FromYear;
                existingdetails.ToYear = result.ToYear;
                existingdetails.City = result.City;
                existingdetails.CountryID = result.CountryID;

                ViewBag.CountryID = new SelectList(db.CountryTables.ToList(), "CountryID", "Country", result.CountryID);
                return View(existingdetails);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEducation(EducationMV educationMV)
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

                var result = db.EducationTables.Where(e => e.EmployeeID == educationMV.EmployeeID).FirstOrDefault();
                if (result == null)
                {
                    var edu = new EducationTable();
                    edu.InstituteName = educationMV.InstituteName;
                    edu.TitleOfEducation = educationMV.TitleOfEducation;
                    edu.Degree = educationMV.Degree;
                    edu.FromYear = educationMV.FromYear;
                    edu.ToYear = educationMV.ToYear;
                    edu.City = educationMV.City;
                    edu.CountryID = educationMV.CountryID;
                    edu.EmployeeID = educationMV.EmployeeID;
                    db.EducationTables.Add(edu);
                    var a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["Successedu"] = "Your educational information is successfully submitted !!";
                        return RedirectToAction("CreateEducation");
                    }
                   
                }
                else
                {
                    TempData["Failededu"] = "Your educational information is already registered!!";
                    return RedirectToAction("CreateEducation");
                }
            }
            ViewBag.CountryID = new SelectList(db.CountryTables.ToList(), "CountryID", "Country", "0");
            return View(educationMV);
        }
    }
}