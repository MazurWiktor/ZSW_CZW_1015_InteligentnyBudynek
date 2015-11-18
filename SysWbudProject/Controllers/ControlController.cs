using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
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

    public class Response
    {

        public Response(string error)
        {
            this.error = error;
        }

        public string error { get; set; }

    }

    public class ControlController : Controller
    {
        private SmartHomeContext db = new SmartHomeContext();

        // GET: Control
        public ActionResult Index()
        {
            ControlModel model = new ControlModel();

            model.Buildings = db.Buildings.ToList();
            model.Floors = db.Floors.ToList();
            model.Rooms = db.Rooms.ToList();
            model.SelectedBuilding = null;
            model.SelectedFloor = null;
            model.SelectedRoom = null;
            model.Devices = new System.Collections.Generic.List<Device>();

            ViewBag.List = model;

            return View();
        }

        [HttpPost]
        public ActionResult FilterByBuilding(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ControlModel model = new ControlModel();
            var buildings = db.Buildings.ToList();
            model.Buildings = buildings;
            model.Floors = db.Floors.Where(floor => floor.BuildingID == id).ToList();
            model.Rooms = db.Rooms.Where(room => room.Floor.BuildingID == id).ToList();
            model.SelectedBuilding = id;
            model.SelectedFloor = null;
            model.SelectedRoom = null;
            model.Devices = new System.Collections.Generic.List<Device>();
            return PartialView("Lists", model);
        }

        public ActionResult FilterByFloor(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ControlModel model = new ControlModel();
            model.Buildings = db.Buildings.ToList();
            Floor filteringFloor = db.Floors.Find(id);
            model.Floors = db.Floors.Where(floor => floor.BuildingID == filteringFloor.BuildingID).ToList();
            model.Rooms = db.Rooms.Where(room => room.FloorID == id).ToList();
            model.SelectedBuilding = filteringFloor.BuildingID;
            model.SelectedFloor = id;
            model.SelectedRoom = null;
            model.Devices = new System.Collections.Generic.List<Device>();
            return PartialView("Lists", model);
        }

        public ActionResult FilterByRoom(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ControlModel model = new ControlModel();
            model.Buildings = db.Buildings.ToList();
            Room filteringRoom = db.Rooms.Find(id);
            model.Floors = db.Floors.Where(floor => floor.BuildingID == filteringRoom.Floor.BuildingID).ToList();
            model.Rooms = db.Rooms.Where(room => room.FloorID == filteringRoom.FloorID).ToList();
            model.SelectedBuilding = filteringRoom.Floor.BuildingID;
            model.SelectedFloor = filteringRoom.FloorID;
            model.SelectedRoom = id;
            model.Devices = db.Devices.Where(device => device.RoomID == id).ToList();
            return PartialView("Lists", model);
        }

        [HttpPost]
        public ActionResult SendToSerialPort(int id, int state)
        {
            Debug.WriteLine("Dostałem: ");
            Debug.WriteLine(id);
            Debug.WriteLine(state);
            try
            {
                SysWbudProject.SerialPorts.SafeSerialPort serialPort = new SysWbudProject.SerialPorts.SafeSerialPort("COM4", 57600, Parity.None, 8);
                serialPort.Open();
                ushort x = (ushort)id;
                ushort y = (ushort)state;
                serialPort.Write(BitConverter.GetBytes(x), 0, 2);
                serialPort.Write(BitConverter.GetBytes(y), 0, 2);
                serialPort.Close();
            }
            catch (Exception)
            {
                return Json(new Response("false"));
            }

            return Json(new Response("true"));
        }
    }
}