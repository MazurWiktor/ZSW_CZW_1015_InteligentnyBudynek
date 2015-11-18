using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeWP.Models {

    public enum DeviceType {
        DIMMER, POWER_SWITCH, THERMOMETER, REED_RELAY, SENSOR
    }

    public class Device {

        public int ID { get; set; }

        public string Name { get; set; }

        public int RoomID { get; set; }

        public DeviceType DeviceType { get; set; }

        public int hardwareID { get; set; }

        public DateTime AddingTime { get; set; }

        public Room room { get; set; }

    }
}
