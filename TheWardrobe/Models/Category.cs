using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheWardrobe.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public string Photo { get; set; }
        public int? UserId { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}