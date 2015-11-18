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
    public class LogController : Controller
    {
        private SmartHomeContext db = new SmartHomeContext();

        // GET: Logs
        public ActionResult Index()
        {
            var logs = db.Logs.Include(l => l.Device).Include(l => l.User);
            return View(logs.ToList());
        }

        // GET: Logs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Log log = db.Logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }



        [HttpPost]
        public ActionResult Clear()
        {
            try
            {
                db.Logs.RemoveRange(db.Logs.ToList());
                db.Logs.Add(new Log { Category = LogCategory.REMOVE, Description = "All logs have been removed." });
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