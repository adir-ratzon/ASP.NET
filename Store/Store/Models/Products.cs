using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SKU { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string PicURL { get; set; }

        public bool pExist { get; set; }
    }
}