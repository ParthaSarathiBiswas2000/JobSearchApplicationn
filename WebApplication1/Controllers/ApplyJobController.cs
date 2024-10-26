using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ApplyJobController : Controller
    {
        private JobHuntDbEntities db = new JobHuntDbEntities();
        // GET: ApplyJob
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ApplyJob(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);

            var result = db.JobApplyTables.Where(e => e.EmployeeID == id).FirstOrDefault();
            if (result == null)
            {
                var educationDetails = db.EmployeeTables.Where(e => e.EmployeeID == id).FirstOrDefault();
                if (educationDetails != null)
                {
                    var details = new ApplyJobMV();
                    details.EmployeeName = educationDetails.EmployeeName;
                    details.JobCategoryName = educationDetails.JobCategoryTable.JobCategory;
                    details.JobName = educationDetails.PostJobTable.JobTitle;
                    details.CompanyName = educationDetails.PostJobTable.CompanyTable.CompanyName;
                    details.EmployeeID = (int)id;
                    details.PostJobID = educationDetails.PostJobID;

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
                var existingdetails = new ApplyJobMV();
                existingdetails.EmployeeName = educationDetails.EmployeeName;
                existingdetails.JobName = educationDetails.PostJobTable.JobTitle;
                existingdetails.JobCategoryName = educationDetails.JobCategoryTable.JobCategory;
                existingdetails.CompanyName = educationDetails.PostJobTable.CompanyTable.CompanyName;
                existingdetails.EmployeeID = (int)id;
                existingdetails.JobApplyDateTime = result.JobApplyDateTime;
                existingdetails.JobApplyStatusID = result.JobApplyStatusID;
                existingdetails.JobApplyStatusUpdateDateTime = result.JobApplyStatusUpdateDateTime;
                existingdetails.JobApplyStatusUpdateReason = result.JobApplyStatusUpdateReason;
                existingdetails.PostJobID = result.PostJobID;

                return View(existingdetails);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyJob(ApplyJobMV applyJobMV)
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

                var result = db.JobApplyTables.Where(e => e.EmployeeID == applyJobMV.EmployeeID).FirstOrDefault();
                if (result == null)
                {
                    var apply = new JobApplyTable();
                    apply.EmployeeID = applyJobMV.EmployeeID;
                    apply.JobApplyDateTime = DateTime.Now;
                    apply.JobApplyStatusID = 1;
                    apply.JobApplyStatusUpdateDateTime = DateTime.Now;
                    apply.JobApplyStatusUpdateReason = "Your Application is under consideration !!!";
                    apply.PostJobID = applyJobMV.PostJobID;
                    db.JobApplyTables.Add(apply);
                    var a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["Successapply"] = "Your Application is successfully submitted !!";
                        return RedirectToAction("FilterJob","Job");
                    }

                }
                else
                {
                    TempData["Failedapply"] = "You have already applied for this job!!";
                    return RedirectToAction("FilterJob","Job");
                }
            }
            
            return View(applyJobMV);
        }

        public ActionResult MyJobs()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);
            var data = db.JobApplyTables.Where(c => c.EmployeeTable.UserID == userid).ToList();

            return View(data);
        }

        public ActionResult DeleteMyJob(int? id) //Delete method for a applied job by candidate
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpost = db.JobApplyTables.Find(id);
            db.Entry(jobpost).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            TempData["Cancel"] = "Your Job Application is cancelled successfully !!!";
            return RedirectToAction("MyJobs");
        }

        public ActionResult CompanyAllAppliedJobs()
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);
            var data = db.JobApplyTables.Where(c => c.PostJobTable.ComapnyID == companyid).ToList();
            if (data.Count() > 0)
            {
                data = data.OrderByDescending(o => o.JobApplyID).ToList();
            }

            return View(data);
        }

        public ActionResult SelectCandidate(int? id) 
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpost = db.JobApplyTables.Find(id);
            jobpost.JobApplyStatusID = 4;
            jobpost.JobApplyStatusUpdateDateTime = DateTime.Now;
            jobpost.JobApplyStatusUpdateReason = "Congratulations ! You Are Selected for this job !!";
            db.Entry(jobpost).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("CompanyAllAppliedJobs");
        }

        public ActionResult CancelCandidate(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpost = db.JobApplyTables.Find(id);
            jobpost.JobApplyStatusID = 3;
            jobpost.JobApplyStatusUpdateDateTime = DateTime.Now;
            jobpost.JobApplyStatusUpdateReason = "Sorry ! You have not matched the eligibility criteria, Better luck next time !!";
            db.Entry(jobpost).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("CompanyAllAppliedJobs");
        }

        public ActionResult ApplicationForm()
        {
            return View();
        }
    }
}