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
    }
}