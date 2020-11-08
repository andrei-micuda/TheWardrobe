using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Microsoft.AspNetCore.Mvc;

namespace TheWardrobe.Controllers
{
    public class OrderItemController : Controller
    {
        // GET: OrderItem
        public ActionResult Index()
        {
            return View();
        }
    }
}