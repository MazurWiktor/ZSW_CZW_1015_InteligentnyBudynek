using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SysWbudProject.Models {
    
    
    public enum DeviceType{
        DIMMER,POWER_SWITCH,THERMOMETER,REED_RELAY,SENSOR
    }
    
    public class Device{

        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^[\w\+\-]+([ ]?[\w\-\+])*$")]
        public string Name { get; set; }

        public int RoomID { get; set; }

        [Required]
        [EnumDataType(typeof(DeviceType))]
        public DeviceType DeviceType { get; set; }

        public int hardwareID { get; set;  }

        public DateTime AddingTime { get; set; }

        public virtual ICollection<Device> Devices { get; set; }

        public virtual Room Room { get; set; }

    }
}