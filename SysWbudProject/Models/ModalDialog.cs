using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SysWbudProject.Models {
    public class ModalDialog {
        
        public string ID { get; set; }

        public string Title { get; set; }

        public string SubmitFunction { get; set; }

        public string SubmitText { get; set; }

        public Func<object, IHtmlString> Content { get; set; }

    }
}