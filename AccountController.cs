using System;
using System.Linq;
using System.Web.Mvc;
using Student_manage.Models;
using Student_manage.ViewModels;

namespace Student_manage.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // ADMIN LOGIN

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View("AdminLogin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(Admins model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var admin = db.Admins.FirstOrDefault(a =>
                    a.Username == model.Username &&
                    a.Password == model.Password &&
                    a.IsActive);

                if (admin != null)
                {
                    Session["Username"] = admin.Username;
                    return RedirectToAction("AdminStudentList", "StudentRegister");
                }

                ViewBag.Message = "Invalid username or password";
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error occurred while processing your request." + ex.Message;
                // Log exception (ex)
                return View(model);
            }
        }

        // USER LOGIN

        [HttpGet]
        public ActionResult UserLogin()
        {
            return View("UserLogin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserLogin(Users model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Please fill in all required fields." });
                }

                var user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    Session["UserId"] = user.UserId;
                    Session["Username"] = user.Username;

                    return Json(new
                    {
                        success = true,
                        redirectUrl = Url.Action("SearchStudents", "StudentRegister")
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Invalid username or password."});
                }
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return Json(new { success = false, message = "An unexpected error occurred." + ex.Message});
            }
        }

        // FORGOT PASSWORD

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string username)
        {
            try
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username);

                if (user != null)
                {
                    return RedirectToAction("ResetPassword", "Account", new { username = user.Username });
                }

                ViewBag.Message = "User not found.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error occurred while processing your request." + ex.Message;
                // Log exception (ex)
                return View();
            }
        }

        // RESET PASSWORD (GET)

        [HttpGet]
        public ActionResult ResetPassword(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToAction("ForgotPassword");
                }

                var user = db.Users.FirstOrDefault(u => u.Username == username);
                if (user == null)
                {
                    ViewBag.Message = "User not found.";
                    return RedirectToAction("ForgotPassword");
                }

                var model = new ResetPasswordViewModel
                {
                    UserId = user.UserId
                };

                ViewBag.Username = username;
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error occurred." + ex.Message;
                // Log exception (ex)
                return RedirectToAction("ForgotPassword");
            }
        }

        // RESET PASSWORD (POST)

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = db.Users.FirstOrDefault(u => u.UserId == model.UserId);
                if (user == null)
                {
                    ViewBag.Message = "User not found.";
                    return View(model);
                }

                user.Password = model.NewPassword;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Password updated successfully.";
                return RedirectToAction("UserLogin");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error occurred while updating the password."+ ex.Message;
                // Log exception (ex)
                return View(model);
            }
        }
    }
}
