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
    public class UserController : Controller
    {
        private SmartHomeContext db = new SmartHomeContext();

        // GET: /User/
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: /User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,Name,Password,AccessRight")] User user)
        {
            try
            {
                db.Users.Add(user);
                db.SaveChanges();
                db.Logs.Add(new Log { Category = LogCategory.CREATE, Description = "User \"" + user.Name + "\" (ID =" + user.ID + ")" });
                db.SaveChanges();
            }
            catch (DataException)
            {
                return Json(new Response("true"));
            }
            return Json(new Response("false"));
        }

        // GET: /User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                db.Logs.Add(new Log { Category = LogCategory.EDIT, Description = "User \"" + user.Name + "\" (ID =" + user.ID + ")" });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                User user = db.Users.Find(id);
                db.Users.Remove(user);
                db.Logs.Add(new Log { Category = LogCategory.REMOVE, Description = "User \"" + user.Name + "\" (ID =" + user.ID + ")" });
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