using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid SKU Number")]
        public int SKU { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)")]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid Price amount")]
        public int Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int Quantity { get; set; }

        public string PicURL { get; set; }

        public bool pExist { get; set; }
    }
}