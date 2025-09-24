using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Student_manage.Models;

namespace Student_manage.Controllers
{
    public class AdmissionCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdmissionCategory
        public ActionResult Index()
        {
            try
            {
                var categories = db.masterdata.ToList();
                return View(categories);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading categories: " + ex.Message;
                return View(Enumerable.Empty<MasterData>());
            }
        }

        // GET: AdmissionCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdmissionCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MasterData masterData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.masterdata.Add(masterData);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Admission Category created successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error creating category: " + ex.Message;
                }
            }
            return View(masterData);
        }
        // GET: AdmissionCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var masterData = db.masterdata.Find(id);
                if (masterData == null)
                    return HttpNotFound();

                return View(masterData);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading category for edit: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: AdmissionCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MasterData masterData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(masterData).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Admission Category updated successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error updating category: " + ex.Message;
                }
            }
            return View(masterData);
        }

        // GET: AdmissionCategory/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var category = db.masterdata.Find(id);
                if (category == null)
                    return HttpNotFound();

                return View(category);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading category for deletion: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var category = db.masterdata.Find(id);
                if (category == null)
                    return HttpNotFound();
                db.masterdata.Remove(category);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Category deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting category: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}