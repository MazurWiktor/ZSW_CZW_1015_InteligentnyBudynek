using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SysWbudProject.Models {
    public class Room {

        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^[\w\+\-]+([ ]?[\w\-\+])*$")]
        public string Name { get; set; }

        public int FloorID { get; set; }

        public virtual ICollection<Device> Devices { get; set; }

        public virtual Floor Floor { get; set; }

    }
}