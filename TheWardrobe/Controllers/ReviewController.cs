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

        [HttpPut]
        public ActionResult Edit(int reviewId, Review reviewUpdated)
        {
            try
            {
                var dbReview = db.Reviews.Find(reviewId);

                if (TryUpdateModel(reviewUpdated))
                {
                    dbReview.ReviewTitle = reviewUpdated.ReviewTitle;
                    dbReview.ReviewBody = reviewUpdated.ReviewBody;
                    dbReview.Rating = reviewUpdated.Rating;
                    dbReview.DateAdded = DateTime.Now;
                    db.SaveChanges();
                }

                return RedirectToAction("Show", "Product", new { productId = reviewUpdated.ProductId });
            }
            catch (Exception e)
            {
                return RedirectToAction("Show", "Product", new { productId = reviewUpdated.ProductId });
            }
        }

        [HttpDelete]
        public ActionResult Delete(int reviewId)
        {
            var reviewToDelete = db.Reviews.Find(reviewId);
            db.Reviews.Remove(reviewToDelete);
            db.SaveChanges();

            return RedirectToAction("Show", "Product", new { productId = reviewToDelete.ProductId });
        }
    }
}