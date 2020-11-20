using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheWardrobe.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Size { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        public string Description { get; set; }
        public DateTime DateAdded { get; set; }

        [Required]
        public int Quantity { get; set; }

        public double Rating { get; set; } // 0.5, 1.0, .., 4.5, 5 => fractional rating

        public string ImageUrl { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual Category Category { get; set; }
    }
}