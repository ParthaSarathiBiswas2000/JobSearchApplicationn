using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class SkillController : Controller
    {
        private JobHuntDbEntities db = new JobHuntDbEntities();
        // GET: Skill
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddSkills(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var details = db.SkillTables.Where(j => j.EmployeeID == id).ToList();
            if (details.Count > 0)
            {
                details = details.OrderBy(r => r.SkillID).ToList();
            }
            var requirements = new SkillsMV();
            requirements.Details = details;
            requirements.EmployeeID = (int)id;

            
            return View(requirements);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSkills(SkillsMV skillsMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            try
            {
                var requirements = new SkillTable();
                
                requirements.SkillName = skillsMV.SkillName;
                requirements.EmployeeID = skillsMV.EmployeeID;
                db.SkillTables.Add(requirements);
                db.SaveChanges();
                return RedirectToAction("AddSkills", new { id = requirements.EmployeeID });
            }
            catch (Exception ex)
            {
                var details = db.SkillTables.Where(j => j.EmployeeID == skillsMV.EmployeeID).ToList();
                if (details.Count() > 0)
                {
                    details = details.OrderBy(r => r.SkillID).ToList();
                }
                skillsMV.Details = details;
                ModelState.AddModelError("SkillName", "Required*");
            }


            return View(skillsMV);
        }

        public ActionResult DeleteSkills(int? id) //Delete method for the job requirements for a particular job
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var employeeid = db.SkillTables.Find(id).EmployeeID;
            var requirements = db.SkillTables.Find(id);
            db.Entry(requirements).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("AddSkills", new { id = employeeid });
        }
    }
}