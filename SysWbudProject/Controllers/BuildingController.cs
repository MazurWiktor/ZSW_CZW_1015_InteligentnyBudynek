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
using System.Diagnostics;

namespace SysWbudProject.Controllers
{
    public class BuildingController : Controller
    {
        private SmartHomeContext db = new SmartHomeContext();

        // GET: /Building/
        public ActionResult Index()
        {
            Debug.WriteLine("Building/Index");
            return View(db.Buildings.ToList());
        }


        public ActionResult Create(Building building)
        {

            try
            {
                db.Buildings.Add(building);
                db.SaveChanges();
                db.Logs.Add(new Log { Category = LogCategory.CREATE, Description = "Building \"" + building.Name + "\" (ID =" + building.ID + ")" });
                db.SaveChanges();
            }
            catch (DataException)
            {
                return Json(new Response("true"));
            }
            return Json(new Response("false"));
        }

        // GET: /Building/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }


        // GET: /Building/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // POST: /Building/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Building building)
        {
            if (ModelState.IsValid)
            {
                db.Entry(building).State = EntityState.Modified;
                db.SaveChanges();
                db.Logs.Add(new Log { Category = LogCategory.EDIT, Description = "Building \"" + building.Name + "\" (ID =" + building.ID + ")" });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(building);
        }

        // POST: /Building/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Building building = db.Buildings.Find(id);
                db.Buildings.Remove(building);
                db.Logs.Add(new Log { Category = LogCategory.REMOVE, Description = "Building \"" + building.Name + "\" (ID =" + building.ID + ")" });
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