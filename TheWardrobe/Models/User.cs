using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheWardrobe.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}