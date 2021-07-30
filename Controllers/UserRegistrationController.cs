using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WroseModel.Models;
using WroseModel.Repository;
using WroseModel.Helper;
using WroseModel.View_Model;
using System.Web.Security;
using CaptchaMvc.HtmlHelpers;

namespace WroseModel.Controllers
{
    public class UserRegistrationController : Controller
    {
        private IRegistration Iregistration;
        private EncryptDecryptPwd encryptDecryptPwd;
        private EmailHelper emailHelper;
        private GenerateOTP generateOTP;
        public UserRegistrationController()
        {
            this.Iregistration = new RegistrationRepository(new WROSEDBEntities());
            this.encryptDecryptPwd = new EncryptDecryptPwd();
            this.emailHelper = new EmailHelper();
            this.generateOTP = new GenerateOTP();
        }
        public UserRegistrationController(IRegistration registrationRepository)
        {
            Iregistration = registrationRepository;
        }
        // GET: UserRegistration
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserRegistration/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserRegistration/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserRegistration/Create
        [HttpPost]
        public ActionResult Index(FormCollection collection, RegistrationVM objRegistration)
        {
            try
            {
                string useremail = string.Empty;
                int isSaved = 0;
                if (objRegistration.User_Email != null)
                {
                    useremail = this.Iregistration.GetUserByEmail(objRegistration.User_Email);
                }
                //if (ModelState.IsValid)
                //{
                if (!string.IsNullOrEmpty(useremail))
                {
                    ViewBag.DuplicateMessage = "" + useremail + " already exists, please use a different email address.";
                }
                //else if (ModelState.IsValid)
                //{
                else
                {
                    string encryptedPwd = this.encryptDecryptPwd.EncodePasswordToBase64(objRegistration.User_Password);
                    objRegistration.User_Password = encryptedPwd;
                    objRegistration.Active = 1;
                    objRegistration.User_Role_ID = 1;
                    objRegistration.Created_Date = DateTime.Now;
                    isSaved = this.Iregistration.NewRegistration(objRegistration);
                    if (isSaved == 1)
                    {
                        this.emailHelper.SendMail(objRegistration.User_Email, objRegistration.First_Name);
                        ViewBag.SuccessMessage = "You have registered successfully, please check your email " + objRegistration.User_Email;
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.DuplicateMessage = "Registration failed";
                    }
                }
                //}
                //}
                return View();
            }
            catch (Exception ex)
            {
                return View("~/Error/NotFound");
            }
        }
        //[HttpPost]
        //public JsonResult GetUserByEmail(string email)
        //{
        //    var user_Registration = new User_Registration();
        //    try
        //    {
        //        user_Registration = this.Iregistration.GetUserByEmail(email);
        //        if (ModelState.IsValid)
        //        {
        //            if (user_Registration != null)
        //            {
        //                ViewBag.DuplicateMessage= "Email address " + user_Registration.User_Email + " already exists, please use a different email address.";
        //                return Json(user_Registration.User_Email, JsonRequestBehavior.AllowGet);
        //               // message = "Email address " + user_Registration.User_Email + " already exists, please use a different email address.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Json(user_Registration, JsonRequestBehavior.AllowGet);
        //}
        // GET: UserRegistration/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserRegistration/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserRegistration/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserRegistration/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Login()
        {
            try
            {
                HttpCookie cookie = Request.Cookies["Login"];
                if (cookie != null)
                {
                    ViewBag.Email = cookie["Email"].ToString();
                    var password = this.encryptDecryptPwd.DecodeFrom64(cookie["Password"].ToString());
                    ViewBag.Password = password;
                    ViewBag.Remember = true;
                }
                else
                {
                    ViewBag.Remember = false;
                }
                return View();
            }
            catch (Exception ex)
            {
                return View("~/Error/NotFound");
            }

        }
        [HttpPost]
        public ActionResult Login(RegistrationVM registrationVM)
        {
            var user_Registration = new User_Registration();
            HttpCookie cookie = new HttpCookie("Login");
            try
            {
                string email = registrationVM.User_Email;
                var _passWord = this.encryptDecryptPwd.EncodePasswordToBase64(registrationVM.User_Password);
                if (registrationVM.Rememberme)
                {
                    cookie.Values.Add("Email", registrationVM.User_Email);
                    cookie.Values.Add("Password", _passWord);
                    cookie.Expires = DateTime.Now.AddDays(15);
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }

                bool Isvalid = this.Iregistration.GetUserLogin(email, _passWord);
                if (Isvalid)
                {
                    //int timeout = registrationVM.Rememberme ? 60 : 5; // Timeout in minutes, 60 = 1 hour.    
                    //var ticket = new FormsAuthenticationTicket(email, false, timeout);
                    //string encrypted = FormsAuthentication.Encrypt(ticket);
                    //var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    //cookie.Expires = System.DateTime.Now.AddMinutes(timeout);
                    //cookie.HttpOnly = true;
                    //Response.Cookies.Add(cookie);
                    // FormsAuthentication.SetAuthCookie(registrationVM.User_Email, registrationVM.Rememberme);
                    //Session["EmailID"] = registrationVM.User_Email;
                    user_Registration = this.Iregistration.GetUserDetailByEmail(email);
                    Session["email"] = user_Registration.User_Email;
                    Session["username"] = user_Registration.First_Name + " " + user_Registration.Last_Name;
                    return RedirectToAction("DashBoard", "Project");
                }
                else
                {
                    ViewBag.DuplicateMessage = "Invalid Information... Please try again!";
                    ModelState.Clear();
                }
            }
            catch (Exception ex)
            {
                return View("~/Error/NotFound");
            }
            return View();
        }
        public ActionResult LogOut()
        {
            try
            {
                if (Session["email"] != null)
                {
                    Session.Abandon();
                }
                return RedirectToAction("Login", "UserRegistration");
            }
            catch (Exception ex)
            {
                return View("~/Error/NotFound");
            }

        }
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(RegistrationVM registrationVM)
        {
            try
            {
                ViewBag.DuplicateMessage = string.Empty;
                if (!this.IsCaptchaValid("Captcha is not valid"))
                {

                    ViewBag.DuplicateMessage = "captcha is not valid.";
                    return View();
                }
                string useremail = string.Empty;
                if (registrationVM.User_Email != null)
                {
                    useremail = this.Iregistration.GetUserByEmail(registrationVM.User_Email);
                }

                if (string.IsNullOrEmpty(useremail))
                {
                    ViewBag.DuplicateMessage = "This email is not exists... Please try again!";
                    // ModelState.AddModelError("EmailNotExists", "This email is not exists");
                    return View();
                }
                //var objUsr = objCon.UserMs.Where(x => x.Email == pass.EmailId).FirstOrDefault();
                // Genrate OTP     
                //string OTP = this.generateOTP.GeneratePassword();
                var activationCode = Guid.NewGuid();
                //objUsr.ActivetionCode = Guid.NewGuid();
                //objUsr.OTP = OTP;
                //objCon.Entry(objUsr).State = System.Data.Entity.EntityState.Modified;
                //objCon.SaveChanges();

                this.emailHelper.ForgetPasswordEmailToUser(useremail, activationCode.ToString());
                ViewBag.SuccessMessage = "Reset password link has sent to your registered email address " + registrationVM.User_Email;
                ModelState.Clear();
                return View();
            }
            catch (Exception ex)
            {
                return View("~/Error/NotFound");
            }
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(RegistrationVM registrationVM)
        {
            var user_Registration = new User_Registration();
            string useremail = string.Empty;
            int isUpdated = 0;
            try
            {
                useremail = registrationVM.User_Email;
                //var _passWord = this.encryptDecryptPwd.EncodePasswordToBase64(registrationVM.User_Password);
                //bool Isvalid = this.Iregistration.GetUserLogin(useremail, _passWord);
                user_Registration = this.Iregistration.GetUserDetailByEmail(useremail);
                if (!string.IsNullOrEmpty(user_Registration.User_Email))
                {
                    var _NewpassWord = this.encryptDecryptPwd.EncodePasswordToBase64(registrationVM.User_NewPassword);
                    isUpdated = this.Iregistration.UpdatePassword(useremail, _NewpassWord);
                    if (isUpdated == 1)
                    {
                        ViewBag.SuccessMessage = "Password successfully changed";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.DuplicateMessage = "Change password failed! Please try again.";
                    }
                }
                else
                {
                    ViewBag.DuplicateMessage = "This email is not exists... Please try again!";
                }
                return View();
            }
            catch (Exception ex)
            {
                return View("~/Error/NotFound");
            }
        }
    }
}
