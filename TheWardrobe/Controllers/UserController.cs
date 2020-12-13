////using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using TheWardrobe.Models;

//namespace TheWardrobe.Controllers
//{
//    public class UserController : Controller
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();
//        public ActionResult Index()
//        {
//            var users = from user in db.Users
//                             orderby user.LastName
//                             select user;
//            ViewBag.Users = users;
//            return View();
//        }
//        public ActionResult Show(int id)
//        {
//            User user = db.Users.Find(id);
//            ViewBag.User = user;
//            return View();
//        }

//        public ActionResult New()
//        {
//            return View();
//        }
//        [HttpPost]
//        public ActionResult New(User user)
//        {
//            try
//            {
//                db.Users.Add(user);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            catch (Exception e)
//            {
//                return View();
//            }
//        }

//        public ActionResult Edit(int id)
//        {
//            User user = db.Users.Find(id);
//            return View(user);
//        }

//        [HttpPut]
//        public ActionResult Edit([Bind(Exclude = "UserId")] int id, User requestUser)
//        {
//            try
//            {
//                User user = db.Users.Find(id);
//                if (TryUpdateModel(user))
//                {
//                    user.LastName = requestUser.LastName;
//                    user.FirstName = requestUser.FirstName;
//                    db.SaveChanges();

//                }
//                return RedirectToAction("Index");
//            }
//            catch (Exception e)
//            {
//                return View();
//            }
//        }
//        [HttpDelete]
//        public ActionResult Delete(int id)
//        {
//            User user = db.Users.Find(id);
//            db.Users.Remove(user);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }
//    }
//}