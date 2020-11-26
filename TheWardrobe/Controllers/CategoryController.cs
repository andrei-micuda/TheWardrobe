//using Microsoft.AspNetCore.Mvc;
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
        private AppDBContext db = new AppDBContext();
        public ActionResult Index()
        {
            var categories = from category in db.Categories
                           orderby category.CategoryName
                           select category;
            ViewBag.Categories = categories;
            return View();
        }
        public ActionResult Show(int id)
        {
            Category category = db.Categories.Find(id);
            ViewBag.Category = category;
            return View();
        }

        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        public ActionResult New(Category category)
        {
            try
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        [HttpPut]
        public ActionResult Edit([Bind(Exclude = "UserId")] int id, Category requestCategory)
        {
            try
            {
                Category category = db.Categories.Find(id);
                if (TryUpdateModel(category))
                {
                    category.CategoryName = requestCategory.CategoryName;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}