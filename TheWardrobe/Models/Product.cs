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

        [Required(ErrorMessage = "The product name is required!")]
        [StringLength(100, ErrorMessage = "The product name is too long!")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "The price name is required!")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The size is required!")]
        public string Size { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        [StringLength(1000, ErrorMessage = "The product description is too long!")]
        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        [Required(ErrorMessage = "The quantity name is required!")]
        public int Quantity { get; set; }

        [Range(0, 5, ErrorMessage = "The rating must be between 0 - 5!")]
        public double Rating { get; set; } // 0.5, 1.0, .., 4.5, 5 => fractional rating

        [Url(ErrorMessage = "Image link is not a valid URL!")]
        public string ImageUrl { get; set; }
        public int NrOfOrders { get; set; } = 0;

        public string UserId { get; set; }
        public int? CategoryId { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}