using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Store.Models;

namespace Store.Models
{
    public class ProductModel
    {
        public Product oneProduct { get; set; }

        public List<Product> ProductsCollection { get; set; }
    }
}