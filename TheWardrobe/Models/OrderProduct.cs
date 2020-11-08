using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheWardrobe.Models
{
    public class OrderProduct
    {
        [Key]
        public int OrderItemId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int OrderId  { get; set; }
    }
}