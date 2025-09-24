using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Student_manage.Models;
using System.Data.Entity;
using System.Collections.Generic;
using Student_manage.ViewModels;

namespace Student_manage.Controllers
{
    public class FeeStructureController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: FeesStructure
        public ActionResult Index()
        {
            try
            {
                var feeStructures = (from fee in db.fees_structure
                                     join cat in db.masterdata
                                         on fee.CategoryId equals cat.Id
                                     select new FeesStructureListViewModel
                                     {
                                         S_No = fee.S_No,
                                         CategoryName = cat.Admission_Category,
                                         Description = fee.Description,
                                         Amount = fee.Amount,
                                         Standard = fee.Standard,
                                         Academic_year = fee.Academic_year,
                                         Term_Fees = fee.Term_Fees,
                                         Payment_Mode = fee.Payment_Mode,
                                         Created_by = fee.Created_by,
                                         Created_on = fee.Created_on,
                                         Fine_Amount = fee.Fine_Amount,
                                         Total_Payable = fee.Total_Payable
                                     }).ToList();
                TempData["SuccessMessage"] = "Fee structure saved successfully!";
                return View(feeStructures);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading fee structures: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: FeesStructure/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.MasterDescriptions = new SelectList(db.masterdata, "Id", "Admission_Category");
                var model = new FeesStructureListViewModel();
                model.FeeStructures.Add(new FeesStructure()); // one default row
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error preparing create form: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: FeesStructure/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FeesStructureListViewModel model)
        {
            try
            {
                if (model.FeeStructures != null && model.FeeStructures.Any())
                {
                    foreach (var feesStructure in model.FeeStructures)
                    {
                        if (ModelState.IsValid)
                        {
                            bool exists = db.masterdata.Any(m => m.Id == feesStructure.CategoryId);
                            if (!exists)
                            {
                                ModelState.AddModelError("", "Invalid category selected.");
                                ViewBag.MasterDescriptions = new SelectList(db.masterdata, "Id", "Admission_Category");
                                return View(model);
                            }

                            feesStructure.Created_on = DateTime.Now;
                            db.fees_structure.Add(feesStructure);
                        }
                    }
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Fee structure saved successfully!";
                    return RedirectToAction("Index");
                }

                ViewBag.MasterDescriptions = new SelectList(db.masterdata, "Id", "Admission_Category");
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error saving fee structure: " + ex.Message;
                ViewBag.MasterDescriptions = new SelectList(db.masterdata, "Id", "Admission_Category");
                return View(model);
            }
        }

        // GET: FeesStructure/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                FeesStructure feesStructure = db.fees_structure.Find(id);
                if (feesStructure == null) return HttpNotFound();

                return View(feesStructure);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading edit form: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: FeesStructure/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FeesStructure feesStructure)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(feesStructure).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Fee structure updated successfully!";
                    return RedirectToAction("Index");
                }
                return View(feesStructure);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating fee structure: " + ex.Message;
                return View(feesStructure);
            }
        }

        // GET: FeesStructure/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                FeesStructure feesStructure = db.fees_structure.Find(id);
                if (feesStructure == null) return HttpNotFound();

                return View(feesStructure);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading delete confirmation: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: FeesStructure/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                FeesStructure feesStructure = db.fees_structure.Find(id);
                if (feesStructure == null) return HttpNotFound();

                db.fees_structure.Remove(feesStructure);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Category deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting fee structure: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
