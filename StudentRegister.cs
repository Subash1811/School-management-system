using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Student_manage.Models;
using Student_manage.ViewModels;

namespace Student_manage.Controllers
{
    public class StudentRegisterController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Dashboard()
        {
            try
            {
                var model = new StudentSearchViewModel
                {
                    Results = new List<StudentRegister>()
                };
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while loading the dashboard." + ex.Message;
                return View(new StudentSearchViewModel());
            }
        }

        public ActionResult SearchStudents(StudentSearchViewModel student)
        {
            try
            {
                var query = db.student_register
                              .Include(s => s.City)
                              .Include(s => s.State)
                              .AsQueryable();

                if (student.StudentID.HasValue)
                    query = query.Where(s => s.STU_ID == student.StudentID.Value);

                if (!string.IsNullOrEmpty(student.FIRST_NAME))
                    query = query.Where(s => s.FIRST_NAME.Contains(student.FIRST_NAME));

                if (student.DATE_OF_BIRTH.HasValue)
                    query = query.Where(s => s.DATE_OF_BIRTH == student.DATE_OF_BIRTH.Value);

                if (student.PHONE.HasValue)
                    query = query.Where(s => s.PHONE == student.PHONE.Value);

                student.Results = query.ToList();
                return View(student);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred during the search." + ex.Message;
                student.Results = new List<StudentRegister>();
                return View(student);
            }
        }

        public ActionResult Index()
        {
            try
            {
                var students = db.student_register.Include(s => s.City).Include(s => s.State).ToList();
                return View(students);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Unable to load student list." + ex.Message;
                return View(new List<StudentRegister>());
            }
        }

        public ActionResult Create()
        {
            try
            {
                ViewBag.State_ID = new SelectList(db.state_master.ToList(), "State_ID", "State_Name");
                ViewBag.City_ID = new SelectList(new List<City_Master>(), "City_ID", "City_Name");
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while preparing the create form." + ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentRegister student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    student.Created_by = Session["Username"]?.ToString() ?? "System";
                    student.Created_on = DateTime.Now;
                    student.Active = true;
                    student.InActive = false;

                    db.student_register.Add(student);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Student created successfully.";
                    return RedirectToAction("Create");
                }

                ViewBag.State_ID = new SelectList(db.state_master.ToList(), "State_ID", "State_Name", student.State_ID);
                ViewBag.City_ID = new SelectList(db.city_master.Where(c => c.State_ID == student.State_ID), "City_ID", "City_Name", student.City_ID);
                return View(student);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while creating the student." + ex.Message;
                ViewBag.State_ID = new SelectList(db.state_master.ToList(), "State_ID", "State_Name", student.State_ID);
                ViewBag.City_ID = new SelectList(db.city_master.Where(c => c.State_ID == student.State_ID), "City_ID", "City_Name", student.City_ID);
                return View(student);
            }
        }

        public JsonResult GetCitiesByState(int stateId)
        {
            try
            {
                var cities = db.city_master
                               .Where(c => c.State_ID == stateId && c.Active == null)
                               .Select(c => new { c.City_ID, c.City_Name })
                               .ToList();
                return Json(cities, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Failed to load cities." + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AdminStudentList()
        {
            try
            {
                if (Session["Username"] == null)
                    return RedirectToAction("AdminLogin", "Account");

                var students = db.student_register.Include(s => s.City).Include(s => s.State).ToList();
                return View(students);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while loading student list." + ex.Message;
                return View(new List<StudentRegister>());
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var student = db.student_register
                    .Include(s => s.City)
                    .Include(s => s.State)
                    .FirstOrDefault(s => s.STU_ID == id);

                if (student == null)
                    return HttpNotFound();

                ViewBag.State_ID = new SelectList(db.state_master.ToList(), "State_ID", "State_Name", student.State_ID);
                ViewBag.City_ID = new SelectList(db.city_master.Where(c => c.State_ID == student.State_ID).ToList(), "City_ID", "City_Name", student.City_ID);
                return View(student);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while loading the edit form." + ex.Message;
                return RedirectToAction("AdminStudentList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StudentRegister model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existing = db.student_register.Find(model.STU_ID);
                    if (existing == null)
                        return HttpNotFound();

                    existing.FIRST_NAME = model.FIRST_NAME;
                    existing.LAST_NAME = model.LAST_NAME;
                    existing.DATE_OF_BIRTH = model.DATE_OF_BIRTH;
                    existing.AGE = model.AGE;
                    existing.GENDER = model.GENDER;
                    existing.EMAIL = model.EMAIL;
                    existing.PHONE = model.PHONE;
                    existing.ADDRESS = model.ADDRESS;
                    //existing.State_ID = model.State_ID;
                    //existing.City_ID = model.City_ID;
                    existing.PINCODE = model.PINCODE;
                    existing.PARENTS = model.PARENTS;
                    existing.RELATIONSHIP = model.RELATIONSHIP;
                    existing.Active = model.Active;
                    existing.InActive = !model.Active;

                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Student updated successfully.";
                    return RedirectToAction("SearchStudents");
                }

                ViewBag.State_ID = new SelectList(db.state_master, "State_ID", "State_Name", model.State_ID);
                ViewBag.City_ID = new SelectList(db.city_master.Where(c => c.State_ID == model.State_ID), "City_ID", "City_Name", model.City_ID);
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while updating the student." + ex.Message;
                ViewBag.State_ID = new SelectList(db.state_master, "State_ID", "State_Name", model.State_ID);
                ViewBag.City_ID = new SelectList(db.city_master.Where(c => c.State_ID == model.State_ID), "City_ID", "City_Name", model.City_ID);
                return View(model);
            }
        }

        public ActionResult Delete(int STU_ID)
        {
            try
            {
                var student = db.student_register.Find(STU_ID);
                if (student == null)
                    return HttpNotFound();

                db.student_register.Remove(student);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Student deleted successfully.";
                return RedirectToAction("AdminStudentList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the student." + ex.Message;
                return RedirectToAction("AdminStudentList");
            }
        }
    }
}
