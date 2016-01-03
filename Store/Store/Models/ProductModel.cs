using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Store.Models;

namespace Store.Models
{
    public class ProductModel
    {
        public List<int> productsIds { get; set; }

        public List<Products> ProductsCollection { get; set; }
    }
}