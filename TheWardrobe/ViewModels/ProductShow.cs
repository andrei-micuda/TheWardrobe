using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheWardrobe.Models;

namespace TheWardrobe.ViewModels
{
    public class ProductShow
    {
        public Product Product { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}