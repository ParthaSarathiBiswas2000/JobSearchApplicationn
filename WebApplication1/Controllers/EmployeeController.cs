using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EmployeeController : Controller
    {
        private JobHuntDbEntities db = new JobHuntDbEntities();
        // GET: Employee
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

            var result = db.EmployeeTables.Where(e => e.PostJobID == id && e.UserID == userid).FirstOrDefault();
            if (result == null)
            {

                var jobdetails = db.PostJobTables.Where(j => j.PostJobID == id).FirstOrDefault();
                var employee = new EmployeeMV();
                employee.JobCategoryID = jobdetails.JobCategoryID;
                employee.JobCategoryName = jobdetails.JobCategoryTable.JobCategory;
                employee.JobName = jobdetails.JobTitle;
                employee.CompanyName = jobdetails.CompanyTable.CompanyName;
                employee.PostedOn = jobdetails.PostDate;
                employee.LastDate = jobdetails.LastDate;
                employee.UserID = userid;
                employee.PostJobID = (int)id;

                ViewBag.CountryID = new SelectList(db.CountryTables.ToList(), "CountryID", "Country", "0");

                return View(employee);
            }
            else
            {
                

                var jobdetails = db.PostJobTables.Where(j => j.PostJobID == id).FirstOrDefault();
                var existingemployee = new EmployeeMV();
                existingemployee.JobCategoryID = jobdetails.JobCategoryID;
                existingemployee.JobCategoryName = jobdetails.JobCategoryTable.JobCategory;
                existingemployee.JobName = jobdetails.JobTitle;
                existingemployee.CompanyName = jobdetails.CompanyTable.CompanyName;
                existingemployee.PostedOn = jobdetails.PostDate;
                existingemployee.LastDate = jobdetails.LastDate;
                existingemployee.UserID = userid;
                existingemployee.PostJobID = (int)id;
                existingemployee.EmployeeName = result.EmployeeName;
                existingemployee.DOB = result.DOB;
                existingemployee.CNIC = result.CNIC;
                existingemployee.FNIC = result.FNIC;
                existingemployee.FatherName = result.FatherName;
                existingemployee.CountryID = result.CountryID;
                existingemployee.EmailAddress = result.EmailAddress;
                existingemployee.Gender = result.Gender;
                existingemployee.Photo = result.Photo;
                existingemployee.Qualification = result.Qualification;
                existingemployee.PermanentAddress = result.PermanentAddress;
                existingemployee.JobReferences = result.JobReferences;
                existingemployee.Description = result.Description;
                existingemployee.EmployeeID = result.EmployeeID;

                ViewBag.CountryID = new SelectList(db.CountryTables.ToList(), "CountryID", "Country", result.CountryID);
                return View(existingemployee);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeMV employeeMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);

            employeeMV.UserID = userid;

            if(ModelState.IsValid)
            {

                var result = db.EmployeeTables.Where(e => e.PostJobID == employeeMV.PostJobID && e.UserID == userid).FirstOrDefault();
                if (result == null)
                {
                    var emp = new EmployeeTable();
                    emp.UserID = employeeMV.UserID;
                    emp.JobCategoryID = employeeMV.JobCategoryID;
                    emp.PostJobID = employeeMV.PostJobID;
                    emp.EmployeeName = employeeMV.EmployeeName;
                    emp.DOB = employeeMV.DOB;
                    emp.CNIC = employeeMV.CNIC;
                    emp.FNIC = employeeMV.FNIC;
                    emp.FatherName = employeeMV.FatherName;
                    emp.CountryID = employeeMV.CountryID;
                    emp.EmailAddress = employeeMV.EmailAddress;
                    emp.Gender = employeeMV.Gender;
                    emp.Photo = employeeMV.Photo;
                    emp.Qualification = employeeMV.Qualification;
                    emp.PermanentAddress = employeeMV.PermanentAddress;
                    emp.JobReferences = employeeMV.JobReferences;
                    emp.Description = employeeMV.Description;
                    db.EmployeeTables.Add(emp);
                    var a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["Success"] = "Your information is successfully submitted !!";
                        return RedirectToAction("Create");
                    }
                    
                }
                else
                {
                    TempData["Failed"] = "Your Data is already registered!!";
                    return RedirectToAction("Create");
                }

            }
            ViewBag.CountryID = new SelectList(db.CountryTables.ToList(), "CountryID", "Country", "0");
            return View(employeeMV);
        }
       
        
    }
}