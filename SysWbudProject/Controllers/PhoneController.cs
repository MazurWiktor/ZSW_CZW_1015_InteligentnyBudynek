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
    public class PhoneController : Controller
    {
        private SmartHomeContext db = new SmartHomeContext();

        // GET: /Phone/
        public ActionResult Index()
        {
            var phones = db.Phones.Include(p => p.User);

            List<SelectListItem> usersList = new List<SelectListItem>();

            var users = db.Users.ToList();
            foreach (var user in users)
            {
                usersList.Add(new SelectListItem { Text = user.Name, Value = user.ID.ToString() });
            }

            ViewBag.users = usersList;

            return View(phones.ToList());
        }

        // GET: /Phone/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phone phone = db.Phones.Find(id);
            if (phone == null)
            {
                return HttpNotFound();
            }
            return View(phone);
        }

        // POST: /Phone/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,Name,MacAddress,PhoneType,UserID")] Phone phone)
        {
            try
            {
                db.Phones.Add(phone);
                db.SaveChanges();
                db.Logs.Add(new Log { Category = LogCategory.CREATE, Description = "Phone \"" + phone.Name + "\" (ID =" + phone.ID + ")" });
                db.SaveChanges();
            }
            catch (DataException)
            {
                return Json(new Response("true"));
            }
            return Json(new Response("false"));
        }

        // GET: /Phone/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phone phone = db.Phones.Find(id);
            if (phone == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "ID", "Name", phone.UserID);
            return View(phone);
        }

        // POST: /Phone/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,MacAddress,UserID")] Phone phone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phone).State = EntityState.Modified;
                db.SaveChanges();
                db.Logs.Add(new Log { Category = LogCategory.EDIT, Description = "Phone \"" + phone.Name + "\" (ID =" + phone.ID + ")" });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "ID", "Name", phone.UserID);
            return View(phone);
        }

        // POST: /Phone/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Phone phone = db.Phones.Find(id);
                db.Phones.Remove(phone);
                db.Logs.Add(new Log { Category = LogCategory.REMOVE, Description = "Phone \"" + phone.Name + "\" (ID =" + phone.ID + ")" });
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