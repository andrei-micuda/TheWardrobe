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
    }
}