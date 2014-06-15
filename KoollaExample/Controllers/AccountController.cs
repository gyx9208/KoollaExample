using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KoollaExample.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Response.Cookies["UserId"].Expires = System.DateTime.Now.AddDays(-1);
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Signup()
        {
            return View();
        }

        public ActionResult Account()
        {
            return View();
        }

        public ActionResult Forgetpassword()
        {
            return View();
        }

        public ActionResult Userlicence()
        {
            return View();
        }

        public JsonResult Varifyaccount()
        {
            JsonResult res = new JsonResult();
            string email = Request.Form["account"];
            string password = Request.Form["password"];
            using (var db = new koollaexampleEntities())
            {
                try
                {
                    var ac = db.users.Where(u => u.email == email && u.password == password).Single();
                    HttpCookie cookie = new HttpCookie("UserId");
                    cookie.Value = ac.uid.ToString();
                    cookie.Expires = System.DateTime.Now.AddDays(1);
                    Response.Cookies.Add(cookie);
                    HttpCookie cookie2 = new HttpCookie("User");
                    cookie2.Value = ac.email;
                    cookie2.Expires = System.DateTime.Now.AddDays(1);
                    Response.Cookies.Add(cookie2);
                    res.Data = new { success = true };
                }
                catch (InvalidOperationException ex)
                {
                    //账号不存在
                    res.Data = new {success=false, error="您输入的账号或密码错误" };
                }
            }
            return res;
        }

        public JsonResult Signaccount()
        {
            JsonResult res = new JsonResult();
            string email = Request.Form["account"];
            string password = Request.Form["password"];
            using (var db = new koollaexampleEntities())
            {
                try
                {
                    var ac = db.users.Where(u => u.email == email).Count();
                    if (ac == 0)
                    {
                        users u = new users()
                        {
                            email=email,
                            password=password
                        };
                        db.users.Add(u);
                        db.SaveChanges();

                        HttpCookie cookie = new HttpCookie("UserId");
                        cookie.Value = u.uid.ToString();
                        cookie.Expires = System.DateTime.Now.AddDays(1);
                        Response.Cookies.Add(cookie);
                        HttpCookie cookie2 = new HttpCookie("User");
                        cookie2.Value = u.email;
                        cookie2.Expires = System.DateTime.Now.AddDays(1);
                        Response.Cookies.Add(cookie2);
                        res.Data = new { success = true };
                    }
                    else
                    {
                        res.Data = new { success = false, error = "您输入的账号已存在" };
                    }
                    
                }
                catch (InvalidOperationException ex)
                {
                    res.Data = new { success = false, error = "您输入的账号已存在" };
                }
            }

            return res;
        }
    }
}
