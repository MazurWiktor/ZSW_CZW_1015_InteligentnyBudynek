using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO.Ports;
using System.Linq;
using System.Web;

namespace SysWbudProject.Models {

    public enum AccessRight {
        ADMIN,USER
    }
    
    public class User{

        public int ID { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(100)]
        [RegularExpression(@"^[\w\+\-]+([ ]?[\w\-\+])*$")]
        public string Name { get; set; }
        public string Password { get; set; }

        [Required]
        [EnumDataType(typeof(AccessRight))]
        public AccessRight AccessRight { get; set; }

        public virtual ICollection<Phone> Phones { get; set; }
    }
}