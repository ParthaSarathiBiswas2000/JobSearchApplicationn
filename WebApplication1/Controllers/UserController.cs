using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        private JobHuntDbEntities db = new JobHuntDbEntities();

        // GET: User
        public ActionResult NewUser()
        {
  
            return View(new UserMV());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewUser(UserMV userMV)
        {
            if(ModelState.IsValid) 
            { 
                var checkuser = db.UserTables.Where(u => u.EmailAddress == userMV.EmailAddress).FirstOrDefault();
                if (checkuser != null)
                {
                    ModelState.AddModelError("EmailAddress", "Email is Already Registered !");
                    return View(userMV);
                }
                checkuser = db.UserTables.Where(u => u.UserName == userMV.UserName).FirstOrDefault();
                if (checkuser != null)
                {
                    ModelState.AddModelError("UserName", "Username is Already Registered !");
                    return View(userMV);
                }
                using (var trans = db.Database.BeginTransaction())
                {

                    try
                    {
                        var user = new UserTable();
                        user.UserName = userMV.UserName;
                        user.Password = userMV.Password;
                        user.ContactNo = userMV.ContactNo;
                        user.EmailAddress = userMV.EmailAddress;
                        user.UserTypeID = userMV.AreYouProvider == true ? 2 : 3;
                        db.UserTables.Add(user);
                        db.SaveChanges();

                        if (userMV.AreYouProvider == true)
                        {
                            var company = new CompanyTable();
                            company.UserID = user.UserID;
                            if (string.IsNullOrEmpty(userMV.Company.EmailAddress))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.EmailAddress", "Required");
                                return View(userMV);
                            }

                            if (string.IsNullOrEmpty(userMV.Company.CompanyName))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.CompanyName", "Required");
                                return View(userMV);
                            }

                            if (string.IsNullOrEmpty(userMV.Company.ContactNo))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.ContactNo", "Required");
                                return View(userMV);
                            }

                            if (string.IsNullOrEmpty(userMV.Company.Description))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.Description", "Required");
                                return View(userMV);
                            }

                            company.EmailAddress = userMV.Company.EmailAddress;
                            company.CompanyName = userMV.Company.CompanyName;
                            company.ContactNo = userMV.Company.ContactNo;
                            company.Logo = "~/Content/assets/img/logo/logo.png"; //default logo
                            company.Description = userMV.Company.Description;
                            db.CompanyTables.Add(company);
                            db.SaveChanges();

                        }
                        trans.Commit();
                        return RedirectToAction("Login");
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(string.Empty, "Please Provide Correct Details");
                        trans.Rollback();
                    }
                    
                }
            }
            
            return View(userMV);
        }

        public ActionResult Login()
        {
            return View(new UserLoginMV());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginMV userLoginMV)
        {
            if (ModelState.IsValid)
            {
                var user = db.UserTables.Where(u => u.UserName == userLoginMV.UserName && u.Password == userLoginMV.Password).FirstOrDefault();
                if(user == null)
                {
                    ModelState.AddModelError(string.Empty, "UserName & Password is Incorrect!");
                    return View(userLoginMV);
                }
                Session["UserID"] = user.UserID;
                Session["UserName"] = user.UserName;
                Session["UserTypeID"] = user.UserTypeID;
                if(user.UserTypeID == 2)
                {
                    Session["CompanyID"] = user.CompanyTables.FirstOrDefault().CompanyID;
                }
                return RedirectToAction("Index", "Home");
            }
            return View(userLoginMV);
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
            Session["CompanyID"] = null;
            Session["UserTypeID"] = null;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult AllUser()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            return View(db.UserTables.ToList());
        }

        public ActionResult AllCompany()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            return View(db.CompanyTables.ToList());
        }

        public ActionResult Forget()
        {
            return View(new ForgotPasswordMV());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Forget(ForgotPasswordMV forgotPasswordMV)
        {
            var user = db.UserTables.Where( u => u.EmailAddress == forgotPasswordMV.Email ).FirstOrDefault();
            if(user != null)
            {
                string usernameandpassword = "User Name : " + user.UserName + "\n" + "Password : " + user.Password;

                string body = usernameandpassword;

                bool IsSendEmail = WebApplication1.Forgot.Email.EmailSend(user.EmailAddress,"Your Account Details", body, true);
                if (IsSendEmail)
                {
                    ModelState.AddModelError(string.Empty, "UserName and Password is send ! ");
                }
                else
                {
                    ModelState.AddModelError("Email", "Your Email is Registered but currently Email Sending is not working properly. Please Try Again after some time. ");
                }

                
            }
            else
            {
                ModelState.AddModelError("Email", "Email is not registered ! ");
            }
            return View(forgotPasswordMV);
        }
    }
}