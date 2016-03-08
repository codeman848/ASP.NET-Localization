/*FarmController.cs
 * a controller to handle the views and actions of the farms
 * 
 *      Revision History:
 *          Cody Lefebvre:04.03.2015:Created
 *          Cody Lefebvre:04.06.2015:Added try/catch for exceptions and success messages
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OEC06.Models;

// this controller is to display and maintain all farms that have had or could have
// fertilizer test plots

namespace OEC06.Controllers
{
    public class FarmController : Controller
    {
        private OECContext db = new OECContext();

        // list all farms on file
        public ActionResult Index()
        {
            var farms = db.farms.Include(f => f.province).OrderBy(a=>a.province.name).ThenBy(a=>a.name);
            return View(farms.ToList());
        }

        // display the details of the selected farm
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            farm farm = db.farms.Find(id);
            if (farm == null)
            {
                return HttpNotFound();
            }
            return View(farm);
        }

        // GET: Farm/Create
        public ActionResult Create()
        {
            return View();
        }

        // new farm record filled in ... save to database if it passes edits.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(farm farm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.farms.Add(farm);
                    db.SaveChanges();
                    TempData["Message"] = string.Format(App_GlobalResources.Translations.FarmCreated, farm.name);
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", string.Format(App_GlobalResources.Translations.FarmCreatedEx) + ex.GetBaseException().Message);
            }            

            ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(a => a.name), "provinceCode", "name", farm.provinceCode);
            return View(farm);
        }

        // present the selected farm for updates
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            farm farm = db.farms.Find(id);
            if (farm == null)
            {
                return HttpNotFound();
            }
            ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(a => a.name), "provinceCode", "name", farm.provinceCode);
            return View(farm);
        }

        // farm record updated ... save to database if it passes edits
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(farm farm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(farm).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = string.Format(App_GlobalResources.Translations.FarmUpdated, farm.name);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", string.Format(App_GlobalResources.Translations.FarmUpdatedEx) + ex.GetBaseException().Message);
            }
            ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(a => a.name), "provinceCode", "name", farm.provinceCode);
            return View(farm);
        }

        // display farm record for confirmation that it is to be deleted
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            farm farm = db.farms.Find(id);
            if (farm == null)
            {
                return HttpNotFound();
            }
            return View(farm);
        }

        // delete confirmed ... remove farm from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                farm farm = db.farms.Find(id);
                db.farms.Remove(farm);
                db.SaveChanges();
                TempData["Message"] = string.Format(App_GlobalResources.Translations.FarmDeleted) + farm.name;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["message"] = string.Format(App_GlobalResources.Translations.FarmDeletedEx) + ex.GetBaseException().Message;
            }
            return RedirectToAction("Delete", new { id = id });
        }

        // release memory and connections associated with this page 
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
