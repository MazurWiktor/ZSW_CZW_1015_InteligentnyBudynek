using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SysWbudProject.Models {
    public class Floor {

        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^[\w\+\-]+([ ]?[\w\-\+])*$")]
        public string Name { get; set; }
        public int BuildingID { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }

        public virtual Building Building { get; set; }

    }
}