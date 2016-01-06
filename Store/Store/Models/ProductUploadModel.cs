using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Store.Models;

namespace Store.Models
{
    public class ProductUploadModel
    {
        public Products pr { get; set; }
        public HttpPostedFileBase  pic { get; set; }

    }
}