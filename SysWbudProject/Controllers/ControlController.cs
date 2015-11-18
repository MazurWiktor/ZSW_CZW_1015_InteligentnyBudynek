using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


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
        // GET: Control
        public ActionResult Index()
        {
            return View();
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