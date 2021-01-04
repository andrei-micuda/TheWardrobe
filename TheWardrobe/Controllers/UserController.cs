using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheWardrobe.Models;

namespace TheWardrobe.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            var users = from user in db.Users
                        orderby user.UserName
                        select user;

            ViewBag.Users = users;

            return View();
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            ApplicationUser currUser = db.Users.Find(User.Identity.GetUserId());
            ViewBag.currUserId = currUser.Id;

            if (User.IsInRole("Admin") || user.Id == currUser.Id)
            {
                string currRole = user.Roles.FirstOrDefault().RoleId;

                var userRoleName = (from role in db.Roles
                                    where role.Id == currRole
                                    select role.Name).First();
                ViewBag.roleName = userRoleName;
                return View(user);
            }

            TempData["message"] = "You don't have permission to see this user";
            TempData["messageClasses"] = "alert alert-danger";

            return RedirectToAction("Index", "Product");
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            ApplicationUser currUser = db.Users.Find(User.Identity.GetUserId());

            if (User.IsInRole("Admin") || user.Id == currUser.Id)
            {
                user.AllRoles = GetAllRoles();
                string userRole = user.Roles.FirstOrDefault().RoleId.ToString();

                ViewBag.userRole = userRole;
                return View(user);
            }

            TempData["message"] = "You don't have permission to see this user";
            TempData["messageClasses"] = "alert alert-danger";

            return RedirectToAction("Index", "Product");
        }

        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();

            var roles = from role in db.Roles
                        select role;
            foreach(var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpPut]
        public ActionResult Edit(string id, ApplicationUser newUser)
        {
            ApplicationUser user = db.Users.Find(id);
            ApplicationUser currUser = db.Users.Find(User.Identity.GetUserId());

            if (User.IsInRole("Admin") || user.Id == currUser.Id)
            {
                user.AllRoles = GetAllRoles();
                try
                {
                    ApplicationDbContext context = new ApplicationDbContext();
                    var roleManager = new RoleManager<IdentityRole>(new
                    RoleStore<IdentityRole>(context));
                    var UserManager = new UserManager<ApplicationUser>(new
                    UserStore<ApplicationUser>(context));
                    if (TryUpdateModel(user))
                    {
                        user.UserName = newUser.UserName;
                        user.LastName = newUser.LastName;
                        user.FirstName = newUser.FirstName;
                        user.PhoneNumber = newUser.PhoneNumber;
                        user.ImgUrl = newUser.ImgUrl;

                        var roles = from role in db.Roles select role;

                        foreach (var role in roles)
                        {
                            UserManager.RemoveFromRole(id, role.Name);
                        }

                        var selectedRole =
                        db.Roles.Find(HttpContext.Request.Params.Get("newRole"));

                        UserManager.AddToRole(id, selectedRole.Name);
                        db.SaveChanges();

                    }
                    return RedirectToAction("Show", new { id = user.Id});
                }
                catch (Exception e)
                {
                    Response.Write(e.Message);

                    newUser.Id = id;
                    return View(newUser);
                }
            }

            TempData["message"] = "You don't have permission to see this user";
            TempData["messageClasses"] = "alert alert-danger";

            return RedirectToAction("Index", "Product");
        }

        [Authorize(Roles = "User,Editor,Admin")]
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new
            UserStore<ApplicationUser>(context));
            var user = UserManager.Users.FirstOrDefault(u => u.Id == id);
            if (User.IsInRole("Admin") || user.Id == id)
            {
                var categories = db.Categories.Where(a => a.UserId == id);
                foreach (var category in categories)
                {
                    db.Categories.Remove(category);
                }
                var products = db.Products.Where(comm => comm.UserId == id);
                foreach (var product in products)
                {
                    db.Products.Remove(product);
                }
                var ordProducts = db.OrderProducts.Where(comm => comm.UserId == id);
                foreach (var product in ordProducts)
                {
                    db.OrderProducts.Remove(product);
                }
                var reviews = db.Reviews.Where(comm => comm.UserId == id);
                foreach (var review in reviews)
                {
                    db.Reviews.Remove(review);
                }
                db.SaveChanges();
                UserManager.Delete(user);
                db.SaveChanges();
                TempData["message"] = "The account has been deleted";
                TempData["messageClasses"] = "alert alert-info";
            }
            else
            {
                TempData["message"] = "You don't have permission to delete this account";
                TempData["messageClasses"] = "alert alert-danger";
            }
            return RedirectToAction("Index", "Product");

        }
    }
}