//using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.Identity;
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
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public ActionResult New(Review reviewToAdd)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    reviewToAdd.DateAdded = DateTime.Now;
                    reviewToAdd.UserId = User.Identity.GetUserId();
                    if (TryUpdateModel(reviewToAdd))
                    {
                        db.Reviews.Add(reviewToAdd);
                        db.SaveChanges();
                        TempData["message"] = "Your review has been succesfully added!";
                        TempData["messageClasses"] = "alert alert-success";
                    }
                }
                TempData["ReviewModelErrors"] = this.ModelState.Values.SelectMany(v => v.Errors);
                return RedirectToAction("UpdateRating", "Product", new { productId = reviewToAdd.ProductId });
            }
            catch (Exception e)
            {
                return RedirectToAction("UpdateRating", "Product", new { productId = reviewToAdd.ProductId });
            }
        }

        [HttpPut]
        public ActionResult Edit(int reviewId, Review reviewUpdated)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dbReview = db.Reviews.Find(reviewId);

                    if (TryUpdateModel(reviewUpdated))
                    {
                        dbReview.ReviewTitle = reviewUpdated.ReviewTitle;
                        dbReview.ReviewBody = reviewUpdated.ReviewBody;
                        dbReview.Rating = reviewUpdated.Rating;
                        dbReview.DateAdded = DateTime.Now;
                        db.SaveChanges();
                        TempData["message"] = "Your review has been succesfully edited!";
                        TempData["messageClasses"] = "alert alert-success";
                    }
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

            TempData["message"] = "Your review has been succesfully deleted!";
            TempData["messageClasses"] = "alert alert-danger";

            return RedirectToAction("Show", "Product", new { productId = reviewToDelete.ProductId });
        }
    }
}