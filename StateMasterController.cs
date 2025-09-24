using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Student_manage.Models;
using Student_manage.ViewModels;

namespace Student_manage.Controllers
{
    public class StateMasterController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult MasterView()
        {
            return View();
        }


        public ActionResult Index()
        {
            var states = db.state_master.ToList();
            return View(states);
        }

        public ActionResult Create()
        {
            var model = new StateCityViewModel
            {
                State = new State_Master(),
                Cities = new List<City_Master> { new City_Master() }
            };
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StateCityViewModel ViewModels)
        {
            if (ModelState.IsValid)
            {
                ViewModels.State.Created_by = Session["Username"]?.ToString();
                ViewModels.State.Created_on = DateTime.Now;
                
                if (ViewModels.State.Cities == null)
                {
                    ViewModels.State.Cities = new List<City_Master>();
                }

                
                if (ViewModels.Cities != null)
                {
                    foreach (var city in ViewModels.Cities)
                    {
                        ViewModels.State.Cities.Add(city);
                    }
                }

                db.state_master.Add(ViewModels.State);
                db.SaveChanges();

                TempData["Success"] = "State and Cities saved successfully.";
                return RedirectToAction("Index");
            }

            return View(ViewModels);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var state = db.state_master.Find(id);
            var cities = db.city_master.Where(c => c.State_ID == id).ToList();

            var model = new StateCityViewModel
            {
                State = state,
                Cities = cities
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StateCityViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                db.Entry(model.State).State = EntityState.Modified;              
                var existingCities = db.city_master.Where(c => c.State_ID == model.State.State_ID).ToList();               
                var submittedCityIds = model.Cities.Where(c => c.City_ID != 0).Select(c => c.City_ID).ToList();                
                var citiesToDelete = existingCities
                    .Where(c => !submittedCityIds.Contains(c.City_ID))
                    .ToList();

                foreach (var city in citiesToDelete)
                {
                    // Check if city is used in student_register
                    bool isUsed = db.student_register.Any(s => s.City_ID == city.City_ID);
                    if (!isUsed)
                    {
                        db.city_master.Remove(city); // Safe to delete
                    }
                    
                }

                // 2. ADD or UPDATE submitted cities
                foreach (var city in model.Cities)
                {
                    if (city.City_ID > 0)
                    {
                        // Update existing city
                        var cityInDb = db.city_master.FirstOrDefault(c => c.City_ID == city.City_ID);
                        if (cityInDb != null)
                        {
                            cityInDb.City_Name = city.City_Name;
                        }
                    }
                    else
                    {
                        // Add new city
                        city.State_ID = model.State.State_ID;
                        db.city_master.Add(city);
                    }
                }

                db.SaveChanges();
                TempData["SuccessMessage"] = "State and cities updated successfully.";
                return RedirectToAction("Index");
            }

            return View(model);
        }



        public ActionResult Delete(int id)
        {
            try
            {
                var state = db.state_master.Find(id);

                // Check if any city is using this state
                bool hasCities = db.city_master.Any(c => c.State_ID == id);

                if (hasCities)
                {
                    TempData["ErrorMessage"] = "Cannot delete. This state is used in City Master.";
                    return RedirectToAction("Index");
                }

                db.state_master.Remove(state);
                db.SaveChanges();

                TempData["SuccessMessage"] = "State deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting state: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

    }
}
