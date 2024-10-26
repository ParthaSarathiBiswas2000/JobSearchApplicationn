using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class JobController : Controller
    {
        private JobHuntDbEntities db = new JobHuntDbEntities();
        // GET: Job
        public ActionResult PostJob()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var job = new PostJobMV();
            ViewBag.JobCategoryID = new SelectList(db.JobCategoryTables.ToList(), "JobCategoryID", "JobCategory", "0");
            ViewBag.JobNatureID = new SelectList(db.JobNatureTables.ToList(), "JobNatureID", "JobNature", "0");
            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostJob(PostJobMV postJobMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);

            postJobMV.UserID = userid;
            postJobMV.ComapnyID = companyid;

            if(ModelState.IsValid)
            {
                var post = new PostJobTable(); 
                    post.UserID = postJobMV.UserID;
                    post.ComapnyID =postJobMV.ComapnyID;
                    post.JobCategoryID = postJobMV.JobCategoryID;
                    post.JobTitle = postJobMV.JobTitle;
                    post.JobDescription = postJobMV.JobDescription;
                    post.MinSalary = postJobMV.MinSalary;
                    post.MaxSalary = postJobMV.MaxSalary;
                    post.Location = postJobMV.Location;
                    post.Vacancy = postJobMV.Vacancy;
                    post.JobNatureID = postJobMV.JobNatureID;
                    post.PostDate = DateTime.Now;
                    post.ApplicationLastDate = postJobMV.ApplicationLastDate;
                    post.LastDate = postJobMV.ApplicationLastDate;
                    post.JobStatusID = 1;

                db.PostJobTables.Add(post);
                db.SaveChanges();
                return RedirectToAction("CompanyJobList");
            }



            ViewBag.JobCategoryID = new SelectList(db.JobCategoryTables.ToList(), "JobCategoryID", "JobCategory", "0");
            ViewBag.JobNatureID = new SelectList(db.JobNatureTables.ToList(), "JobNatureID", "JobNature", "0");
            return View(postJobMV);
        }

        public ActionResult CompanyJobList()
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);
            var data = db.PostJobTables.Where(c => c.ComapnyID == companyid && c.UserID == userid).ToList();

            return View(data);
        }

        public ActionResult AddJobRequirements(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var details = db.JobRequirementDetailTables.Where(j => j.PostJobID == id).ToList();
            if(details.Count > 0)
            {
                details = details.OrderBy(r => r.JobRequirementID).ToList();
            }
            var requirements = new JobRequirementsMV();
            requirements.Details = details;
            requirements.PostJobID = (int)id;

            ViewBag.JobRequirementID = new SelectList(db.JobRequirementsTables.ToList(), "JobRequirementID", "JobRequirementTitle","0");
            return View(requirements);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddJobRequirements(JobRequirementsMV jobRequirementsMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            try
            {
                var requirements = new JobRequirementDetailTable();
                requirements.JobRequirementID = jobRequirementsMV.JobRequirementID;
                requirements.JobRequirementDetails = jobRequirementsMV.JobRequirementDetails;
                requirements.PostJobID = jobRequirementsMV.PostJobID;
                db.JobRequirementDetailTables.Add(requirements);
                db.SaveChanges();
                return RedirectToAction("AddJobRequirements", new { id = requirements.PostJobID });
            }
            catch (Exception ex)
            {
                var details = db.JobRequirementDetailTables.Where(j => j.PostJobID == jobRequirementsMV.PostJobID).ToList();
                if (details.Count() > 0)
                {
                    details = details.OrderBy(r => r.JobRequirementID).ToList();
                }
                jobRequirementsMV.Details = details;
                ModelState.AddModelError("JobRequirementID", "Required*");
            }

            ViewBag.JobRequirementID = new SelectList(db.JobRequirementsTables.ToList(), "JobRequirementID", "JobRequirementTitle",jobRequirementsMV.JobRequirementID);

            return View(jobRequirementsMV);
        }

        public ActionResult DeleteRequirements(int? id) //Delete method for the job requirements for a particular job
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpostid = db.JobRequirementDetailTables.Find(id).PostJobID;
            var requirements = db.JobRequirementDetailTables.Find(id);
            db.Entry(requirements).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("AddJobRequirements", new { id = jobpostid });
        }

        public ActionResult DeleteJobPost(int? id) //Delete method for a posted job
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpost = db.PostJobTables.Find(id);
            db.Entry(jobpost).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("CompanyJobList");
        }

        public ActionResult JobDetails(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var postjob = db.PostJobTables.Find(id);
            return View(postjob);
        }

        public ActionResult AllCompanyPendingJobs()
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);
            var data = db.PostJobTables.ToList();
            if(data.Count() > 0)
            {
                data = data.OrderByDescending( o => o.PostJobID ).ToList();
            }

            return View(data);
        }

        public ActionResult ApprovedPost(int? id) //Approve method for a posted job by admin
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpost = db.PostJobTables.Find(id);
            jobpost.JobStatusID = 2;
            db.Entry(jobpost).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AllCompanyPendingJobs");
        }

        public ActionResult CancelledPost(int? id) //Cancel method for a posted job by admin
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpost = db.PostJobTables.Find(id);
            jobpost.JobStatusID = 3;
            db.Entry(jobpost).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AllCompanyPendingJobs");
        }

        public ActionResult FilterJob()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var obj = new FilterJobMV();
            var date = DateTime.Now.Date;
            var result = db.PostJobTables.Where(r => r.ApplicationLastDate >= date &&  r.JobStatusID == 2 ).ToList();
            obj.Result = result;

            ViewBag.JobCategoryID = new SelectList(db.JobCategoryTables.ToList(), "JobCategoryID", "JobCategory", "0");
            ViewBag.JobNatureID = new SelectList(db.JobNatureTables.ToList(), "JobNatureID", "JobNature", "0");

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FilterJob(FilterJobMV filterJobMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var date = DateTime.Now.Date;
            var result = db.PostJobTables.Where(r => r.ApplicationLastDate >= date && r.JobStatusID == 2 && (r.JobCategoryID == filterJobMV.JobCategoryID && r.JobNatureID == filterJobMV.JobNatureID)).ToList();
            filterJobMV.Result = result;

            ViewBag.JobCategoryID = new SelectList(db.JobCategoryTables.ToList(), "JobCategoryID", "JobCategory", filterJobMV.JobCategoryID);
            ViewBag.JobNatureID = new SelectList(db.JobNatureTables.ToList(), "JobNatureID", "JobNature", filterJobMV.JobNatureID);

            return View(filterJobMV);
        }
    }
}