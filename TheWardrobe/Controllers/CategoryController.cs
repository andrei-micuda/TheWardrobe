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
    public class CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var categories = (from category in db.Categories
                             orderby category.CategoryName
                             select category).AsEnumerable().ToList();

            ViewBag.Categories = categories;
            return View();

            
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(int id)
        {
            Category category = db.Categories.Find(id);
            ViewBag.Category = category;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult New(Category category)
        {
            try
            {
                category.UserId = User.Identity.GetUserId();
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            ViewBag.Photo = category.Photo;
            return View(category);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                Category category = db.Categories.Find(id);
                if (TryUpdateModel(category))
                {
                    category.CategoryName = requestCategory.CategoryName;
                    category.Photo = requestCategory.Photo;

                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}