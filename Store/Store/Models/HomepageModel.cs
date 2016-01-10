using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class HomepageModel
    {
        public List<Products> ProductsCollection { get; set; }
        public Products SingleProduct { get; set; }
        public CustomerEntity CustomerEntity { get; set; }
        public FilterByPrice Pricing { get; set; }
    }
}