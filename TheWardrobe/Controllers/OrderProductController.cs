using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheWardrobe.Models;
//using Microsoft.AspNetCore.Mvc;

namespace TheWardrobe.Controllers
{
    public class OrderProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "User, Editor, Admin")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var products = (from product in db.OrderProducts
                           where product.UserId == userId
                           select product).AsEnumerable().ToList();

            ViewBag.OrderProducts = products;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User, Editor, Admin")]
        public ActionResult New([Bind(Include = "Quantity, Size, ProductId")] OrderProduct OrderProduct)
        {
            var selectedSize = HttpContext.Request.Params.Get("select-size");
            OrderProduct.Size = selectedSize;
            var userId = User.Identity.GetUserId();
            var ExistingProduct = db.OrderProducts.Where(product => product.UserId == userId && product.Size == OrderProduct.Size && product.ProductId == OrderProduct.ProductId).AsEnumerable().ToList();

            var quantity = OrderProduct.Quantity;
            if(ExistingProduct.Count() == 0)
            {
                OrderProduct.UserId = User.Identity.GetUserId();
                db.OrderProducts.Add(OrderProduct);
                db.SaveChanges();
                TempData["message"] = "The product has been succesfully added to your cart!";
                TempData["messageClasses"] = "alert alert-primary";
            }
            else
            {
                var product = db.Products.Find(OrderProduct.ProductId);
                if( product.Quantity >= quantity + ExistingProduct.First().Quantity)
                {
                    ExistingProduct.First().Quantity += quantity;
                    db.SaveChanges();
                    TempData["message"] = "The product has been succesfully updated to your cart!";
                    TempData["messageClasses"] = "alert alert-primary";
                }
                else
                {
                    TempData["message"] = "You have to select a smaller quantity!";
                    TempData["messageClasses"] = "alert alert-danger";
                }
                
            }

            return RedirectToAction("Show", "Product", new { productId = OrderProduct.ProductId });
        }



        [HttpDelete]
        [Authorize(Roles = "User, Editor, Admin")]
        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            if(id != -1 && id != -2) // delete request de la user/editor/admin
            {
                var productToDelete = db.OrderProducts.Find(id);
                if (userId == productToDelete.UserId || User.IsInRole("Admin"))
                {
                    db.OrderProducts.Remove(productToDelete);
                    db.SaveChanges();
                    TempData["message"] = "The product has been succesfully deleted from your cart!";
                    TempData["messageClasses"] = "alert alert-primary";
                }
                else
                {
                    TempData["message"] = "You don't have the right to delete that product!";
                    TempData["messageClasses"] = "alert alert-warning";
                }
                return RedirectToAction("Index");
            }
            else
            {
                var productsToDelete = (from product in db.OrderProducts
                                        where product.UserId == userId
                                        select product).AsEnumerable().ToList();

                foreach (var ordProduct in productsToDelete){
                   
                    if(id == -2)  // send order
                    {
                        var product = db.Products.Find(ordProduct.ProductId);
                        
                        var remainingQuantity = product.Quantity - ordProduct.Quantity;

                        TempData["message"] = "Your order was successfully placed!";
                        TempData["messageClasses"] = "alert alert-primary";
                        if (remainingQuantity == 0)
                        {
                            
                            var otherUsersProductsDel = db.OrderProducts.Where(op => op.ProductId == product.ProductId && op.Size == ordProduct.Size).AsEnumerable().ToList();
                            
                            foreach (var otherprod in otherUsersProductsDel)
                            {
                                db.OrderProducts.Remove(otherprod);
                                db.SaveChanges();
                            }
                            product.Quantity = remainingQuantity;
                            product.NrOfOrders += ordProduct.ProductId;
                            db.SaveChanges();

                        }
                        else   // empty cart
                        {
                            var otherUsersProductsEdit = db.OrderProducts.Where(op => op.ProductId == product.ProductId && op.UserId != ordProduct.UserId && op.Size == ordProduct.Size && op.Quantity > remainingQuantity).AsEnumerable().ToList();
                            foreach (var otherprod in otherUsersProductsEdit)
                            {
                                otherprod.Quantity = remainingQuantity;
                                db.SaveChanges();
                            }

                            db.OrderProducts.Remove(ordProduct);
                            product.Quantity = remainingQuantity;
                            db.SaveChanges();
                        }

                    }
                    else
                    {
                        db.OrderProducts.Remove(ordProduct);
                        db.SaveChanges();
                        TempData["message"] = "Your cart has been emptied!";
                        TempData["messageClasses"] = "alert alert-primary";
                    }
                    
                }              

                return RedirectToAction("Index", "Product");
            }

        }

    }
}