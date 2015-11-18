using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysWbudProject.Models {

    public enum LogCategory{
        LOGIN,LOGOUT,CREATE,EDIT,REMOVE,USER_CHANGE,CHANGE
    }

    public class Log {

        public Log() {
            Time = DateTime.Now;
            UserID = null;
            DeviceID = null;
            MacAddress = "";
        }

        public int ID { get; set; }

        public DateTime Time { get; set; }

        public int? UserID { get; set; }

        public string MacAddress { get; set; }

        public int? DeviceID { get; set; }

        public LogCategory Category { get; set; }

        public string Description { get; set; }

        public virtual User User { get; set; }

        public virtual Device Device { get; set; }

    }
}