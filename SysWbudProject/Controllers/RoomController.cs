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

namespace SysWbudProject.Controllers
{
    public class RoomController : Controller
    {
        private SmartHomeContext db = new SmartHomeContext();

        // GET: /Room/
        public ActionResult Index()
        {
            var rooms = db.Rooms.Include(r => r.Floor);

            List<SelectListItem> floorsList = new List<SelectListItem>();

            var floors = db.Floors.ToList();
            foreach (var floor in floors)
            {
                floorsList.Add(new SelectListItem { Text = floor.Building.Name + " | " + floor.Name, Value = floor.ID.ToString() });
            }

            ViewBag.floors = floorsList;

            return View(rooms.ToList());
        }

        // GET: /Room/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: /Room/Create
        [HttpPost]
        public ActionResult Create(Room room)
        {
            try
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                db.Logs.Add(new Log { Category = LogCategory.CREATE, Description = "Room \"" + room.Name + "\" (ID =" + room.ID + ")" });
                db.SaveChanges();

            }
            catch (DataException)
            {
                return Json(new Response("true"));
            }
            return Json(new Response("false"));
        }

        // GET: /Room/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.FloorID = new SelectList(db.Floors, "ID", "Name", room.FloorID);
            return View(room);
        }

        // POST: /Room/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,FloorID")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                db.Logs.Add(new Log { Category = LogCategory.EDIT, Description = "Room \"" + room.Name + "\" (ID =" + room.ID + ")" });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FloorID = new SelectList(db.Floors, "ID", "Name", room.FloorID);
            return View(room);
        }

        // POST: /Room/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Room room = db.Rooms.Find(id);
                db.Rooms.Remove(room);
                db.Logs.Add(new Log { Category = LogCategory.REMOVE, Description = "Room \"" + room.Name + "\" (ID =" + room.ID + ")" });
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