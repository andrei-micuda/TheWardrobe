using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheWardrobe.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public float TotalAmount { get; set; }  
        public int UserId { get; set; }
    }
}