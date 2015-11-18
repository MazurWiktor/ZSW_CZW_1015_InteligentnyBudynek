using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysWbudProject.Models {
    public class ControlModel {

        public virtual ICollection<Building> Buildings { get; set; }

        public virtual ICollection<Floor> Floors { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }

        public virtual ICollection<Device> Devices { get; set; }

        public int? SelectedBuilding { get; set; }

        public int? SelectedFloor { get; set; }

        public int? SelectedRoom { get; set; }

    }
}