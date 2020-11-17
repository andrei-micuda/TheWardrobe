//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheWardrobe.Models;

namespace TheWardrobe.Controllers
{
    public class ReviewController : Controller
    {
        private AppDBContext db = new AppDBContext();

        [HttpPost]
        public ActionResult New(Review reviewToAdd)
        {
            reviewToAdd.DateAdded = DateTime.Now;
            db.Reviews.Add(reviewToAdd);
            db.SaveChanges();
            return RedirectToAction("UpdateRating", "Product", new { productId = reviewToAdd.ProductId });
        }
    }
}