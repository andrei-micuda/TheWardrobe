using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheWardrobe.Models;
using TheWardrobe.ViewModels;

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
            // Retireve action message if available
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
                ViewBag.messageClasses = TempData["messageClasses"].ToString();
            }

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
            TempData["message"] = "The product has been approved!";
            TempData["messageClasses"] = "alert alert-success";
            return RedirectToAction("ManageProducts");
        }

        public ActionResult Delete(int productId)
        {
            var productToDelete = db.Products.Find(productId);
            db.Products.Remove(productToDelete);
            db.SaveChanges();
            TempData["message"] = "The product has been succesfully deleted!";
            TempData["messageClasses"] = "alert alert-danger";
            return RedirectToAction("ManageProducts");
        }

        public ActionResult Show(int? productId)
        {
            // Retireve action message if available
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
                ViewBag.messageClasses = TempData["messageClasses"].ToString();
            }

            // if there are any errors propagated from the ReviewController after model validation, add them to this model
            if (TempData.ContainsKey("ReviewModelErrors"))
            {
                var reviewErrors = (IEnumerable<ModelError>)TempData["ReviewModelErrors"];
                foreach (var err in reviewErrors)
                {
                    ModelState.AddModelError("", err.ErrorMessage);
                }
            }
            if (productId == null)
            {
                return RedirectToAction("Index");
            }

            var model = new ProductShow();

            var product = db.Products.Find(productId);
            model.Product = product;

            var reviews = db.Reviews.Where(r => r.ProductId == product.ProductId).ToList();
            model.Reviews = reviews;
            return View(model);
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
            try
            {
                if (ModelState.IsValid)
                {
                    productToAdd.DateAdded = DateTime.Now;
                    productToAdd.IsApproved = false;

                    if (TryUpdateModel(productToAdd))
                    {
                        db.Products.Add(productToAdd);
                        db.SaveChanges();
                        TempData["message"] = "Your product has been added succesfully, it is awaiting an admin's approval!";
                        TempData["messageClasses"] = "alert alert-success";
                    }
                    else
                    {
                        TempData["message"] = "Something went wrong when adding your product!";
                        TempData["messageClasses"] = "alert alert-danger";
                    }
                    return RedirectToAction("ManageProducts");
                }
                var categories = from cat in db.Categories
                                 select cat;
                ViewBag.Categories = categories;
                return View(productToAdd);
            }
            catch (Exception e)
            {
                var categories = from cat in db.Categories
                                 select cat;
                ViewBag.Categories = categories;
                return View(productToAdd);
            }
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
                if (ModelState.IsValid)
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
                        TempData["message"] = "Your product has been edited succesfully!";
                        TempData["messageClasses"] = "alert alert-success";
                    }
                    else
                    {
                        TempData["message"] = "Something went wrong when editing your product!";
                        TempData["messageClasses"] = "alert alert-danger";
                    }
                    return RedirectToAction("ManageProducts");
                }
                var categories = from cat in db.Categories
                                 select cat;
                ViewBag.Categories = categories;
                return View(productUpdated);
            }
            catch (Exception e)
            {
                var categories = from cat in db.Categories
                                 select cat;
                ViewBag.Categories = categories;
                return View(productUpdated);
            }
        }

        public ActionResult UpdateRating(int productId)
        {
            var product = db.Products.Find(productId);

            product.Rating = db.Reviews.Where(r => r.ProductId == productId).Select(r => r.Rating).DefaultIfEmpty(0).Average();

            return RedirectToAction("Show", new { productId = productId });
        }
    }
}