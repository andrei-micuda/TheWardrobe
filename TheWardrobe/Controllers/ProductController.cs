using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheWardrobe.Models;

//using Microsoft.AspNetCore.Mvc;

namespace TheWardrobe.Controllers
{
    public class ProductController : Controller
    {
        private AppDBContext db = new AppDBContext();

        // GET: Product
        public ActionResult Index()
        {
            var products = db.Products.Where(p => p.IsApproved == true).OrderByDescending(p => p.DateAdded).AsEnumerable().ToList();
            ViewBag.Products = products;
            return View();
        }

        public ActionResult ManageProducts()
        {
            //TODO Only retrieve items belonging to the current user
            var pendingProducts = db.Products.Where(p => p.IsApproved == false).OrderByDescending(p => p.DateAdded).AsEnumerable().ToList();
            var approvedProducts = db.Products.Where(p => p.IsApproved == true).OrderByDescending(p => p.DateAdded).AsEnumerable().ToList();
            ViewBag.PendingProducts = pendingProducts;
            ViewBag.ApprovedProducts = approvedProducts;
            return View();
        }

        public ActionResult Approve(int productId)
        {
            var productToUpdate = db.Products.Find(productId);
            productToUpdate.IsApproved = true;
            db.SaveChanges();
            return RedirectToAction("ManageProducts");
        }

        public ActionResult Delete(int productId)
        {
            var productToDelete = db.Products.Find(productId);
            db.Products.Remove(productToDelete);
            db.SaveChanges();
            return RedirectToAction("ManageProducts");
        }

        public ActionResult Show(int productId)
        {
            var product = db.Products.Find(productId);
            ViewBag.Product = product;
            return View();
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetCategorySelectList()
        {
            var selectList = new List<SelectListItem>();
            var categories = from cat in db.Categories
                             select cat;

            foreach (var cat in categories)
            {
                selectList.Add(new SelectListItem
                {
                    Value = cat.CategoryId.ToString(),
                    Text = cat.CategoryName
                });
            }

            return selectList;
        }

        public ActionResult New()
        {
            var product = new Product();
            var categories = from cat in db.Categories
                             select cat;
            ViewBag.Categories = categories;
            return View(product);
        }

        [HttpPost]
        public ActionResult New(Product productToAdd)
        {
            productToAdd.DateAdded = DateTime.Now;
            productToAdd.IsApproved = false;
            db.Products.Add(productToAdd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int productId)
        {
            var product = db.Products.Find(productId);
            var categories = from cat in db.Categories
                             select cat;
            ViewBag.Categories = categories;
            return View(product);
        }

        [HttpPut]
        public ActionResult Edit(int productId, Product productUpdated)
        {
            try
            {
                Product dbProduct = db.Products.Find(productId);

                if (TryUpdateModel(productUpdated))
                {
                    dbProduct.ProductName = productUpdated.ProductName;
                    dbProduct.Description = productUpdated.Description;
                    dbProduct.Price = productUpdated.Price;
                    dbProduct.Size = productUpdated.Size;
                    dbProduct.Quantity = productUpdated.Quantity;
                    dbProduct.CategoryId = productUpdated.CategoryId;
                    dbProduct.ImageUrl = productUpdated.ImageUrl;
                    db.SaveChanges();
                }
                return RedirectToAction("ManageProducts");
            }
            catch (Exception e)
            {
                return View(productUpdated);
            }
        }
    }
}