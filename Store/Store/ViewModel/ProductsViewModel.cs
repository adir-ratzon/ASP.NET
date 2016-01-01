using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.ViewModel
{
    public class ProductsViewModel
    {
        public Product product { get; set; }

        public List<Product> products { get; set; }
    }
}