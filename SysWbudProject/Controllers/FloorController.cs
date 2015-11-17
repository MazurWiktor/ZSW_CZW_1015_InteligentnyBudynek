using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SysWbudProject.Models;
using SmartHome.DAL;
using System.Web.Script.Serialization;

namespace SysWbudProject.Controllers
{
    public class FloorController : Controller
    {
        private SmartHomeContext db = new SmartHomeContext();

        // GET: /Floor/
        public ActionResult Index()
        {
            var floors = db.Floors.Include(f => f.Building);

            List<SelectListItem> buildingsList = new List<SelectListItem>();

            var buildings = db.Buildings.ToList();
            foreach (var building in buildings)
            {
                buildingsList.Add(new SelectListItem { Text = building.Name, Value = building.ID.ToString() });
            }

            ViewBag.buildings = buildingsList;

            return View(floors.ToList());
        }

        [HttpPost]
        public ActionResult Create(Floor floor)
        {

            try
            {
                db.Floors.Add(floor);
                db.SaveChanges();
                db.Logs.Add(new Log { Category = LogCategory.CREATE, Description = "Floor \"" + floor.Name + "\" (ID =" + floor.ID + ")" });
                db.SaveChanges();
            }
            catch (DataException)
            {
                return Json(new Response("true"));
            }
            return Json(new Response("false"));
        }

        // GET: /Floor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Floor floor = db.Floors.Find(id);
            if (floor == null)
            {
                return HttpNotFound();
            }
            return View(floor);
        }




        // GET: /Floor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Floor floor = db.Floors.Find(id);
            if (floor == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuildingID = new SelectList(db.Buildings, "ID", "Name", floor.BuildingID);
            return View(floor);
        }

        // POST: /Floor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,BuildingID")] Floor floor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(floor).State = EntityState.Modified;
                db.SaveChanges();
                db.Logs.Add(new Log { Category = LogCategory.EDIT, Description = "Floor \"" + floor.Name + "\" (ID =" + floor.ID + ")" });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuildingID = new SelectList(db.Buildings, "ID", "Name", floor.BuildingID);
            return View(floor);
        }

        // POST: /Floor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Floor floor = db.Floors.Find(id);
                db.Floors.Remove(floor);
                db.Logs.Add(new Log { Category = LogCategory.REMOVE, Description = "Floor \"" + floor.Name + "\" (ID =" + floor.ID + ")" });
                db.SaveChanges();
            }
            catch (DataException)
            {
                return Json(new Response("true"));
            }
            catch (ArgumentNullException)
            {
                return Json(new Response("true"));
            }
            return Json(new Response("false"));
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