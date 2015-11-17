using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SysWbudProject.Models;
using SysWbudProject.DAL;

namespace SysWbudProject.Controllers
{
    public class DeviceController : Controller
    {
        private SmartHomeContext db = new SmartHomeContext();

        // GET: /Device/
        public ActionResult Index()
        {
            var devices = db.Devices.Include(d => d.Room);

            List<SelectListItem> roomsList = new List<SelectListItem>();

            var rooms = db.Rooms.ToList();
            foreach (var room in rooms)
            {
                roomsList.Add(new SelectListItem { Text = room.Floor.Building.Name + " | " + room.Floor.Name + " | " + room.Name, Value = room.ID.ToString() });
            }

            ViewBag.rooms = roomsList;

            return View(devices.ToList());
        }

        // GET: /Device/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // POST: /Device/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,Name,RoomID,DeviceType,hardwareID,AddingTime")] Device device)
        {
            try
            {
                db.Devices.Add(device);
                device.AddingTime = DateTime.Now;
                db.SaveChanges();
                db.Logs.Add(new Log { Category = LogCategory.CREATE, Description = "Device \"" + device.Name + "\" (ID =" + device.ID + ")" });
                db.SaveChanges();
            }
            catch (DataException)
            {
                return Json(new Response("true"));
            }
            return Json(new Response("false"));
        }

        // GET: /Device/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomID = new SelectList(db.Rooms, "ID", "Name", device.RoomID);
            return View(device);
        }

        // POST: /Device/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,RoomID,DeviceType,hardwareID,AddingTime")] Device device)
        {
            if (ModelState.IsValid)
            {
                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
                db.Logs.Add(new Log { Category = LogCategory.EDIT, Description = "Device \"" + device.Name + "\" (ID =" + device.ID + ")" });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomID = new SelectList(db.Rooms, "ID", "Name", device.RoomID);
            return View(device);
        }

        // POST: /Device/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Device device = db.Devices.Find(id);
                db.Devices.Remove(device);
                db.SaveChanges();
                db.Logs.Add(new Log { Category = LogCategory.REMOVE, Description = "Device \"" + device.Name + "\" (ID =" + device.ID + ")" });
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