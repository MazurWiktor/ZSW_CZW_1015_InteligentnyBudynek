using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SysWbudProject.Models {

    public enum PhoneType {
        ANDROID,WINDOWS_PHONE
    }
    
    public class Phone {

        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^[\w\+\-]+([ ]?[\w\-\+])*$")]
        public string Name { get; set; }

        public string MacAddress { get; set; }

        [Required]
        [EnumDataType(typeof(PhoneType))]
        public PhoneType PhoneType { get; set; }

        public int UserID { get; set; }

        public virtual User User { get; set; }

    }
}