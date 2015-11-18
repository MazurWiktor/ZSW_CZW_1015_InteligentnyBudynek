using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysWbudProject.Models {
    public class Building {

        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^[\w\+\-]+([ ]?[\w\-\+])*$")]
        [Index(IsUnique = true)]
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Floor> Floors { get; set; }

    }
}