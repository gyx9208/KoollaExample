using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KoollaExample.Models;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace KoollaExample.Controllers
{
    public class SellMoreController : Controller
    {
        //
        // GET: /SellMore/

        public ActionResult Index()
        {
            if (Request.Cookies["UserId"] == null)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult Customer()
        {
            if (Request.Cookies["UserId"] == null)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult GroupMail()
        {
            if (Request.Cookies["UserId"] == null)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public JsonResult CustomerRead()
        {
            JsonResult res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (Request.Cookies["UserId"] == null)
                return res;
            int id = int.Parse(Request.Cookies["UserId"].Value);
            List<MyCustomer> list = new List<MyCustomer>();
            using (var db = new koollaexampleEntities())
            {
                var c = db.customer.Where(p => p.uid == id).ToList();
                foreach (customer cus in c)
                {
                    var d = cus.deal.OrderByDescending(p => p.status).First();
                    MyCustomer mc = new MyCustomer(cus, d);
                    list.Add(mc);
                }
            }
            res.Data = list;
            return res;
        }

        public JsonResult CustomerUpdate()
        {
            JsonResult res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (Request.Cookies["UserId"] == null)
                return res;
            var models = Request.QueryString["models"];
            models = models.Substring(1, models.Length - 2);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MyCustomer));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(models));
            var model = (MyCustomer)serializer.ReadObject(ms);



            using (var db = new koollaexampleEntities())
            {
                var oldC = db.customer.Where(p => p.cid == model.cid).Single();
                var oldD = oldC.deal.OrderByDescending(p => p.status).First();
                if (model.status.StatusId > oldD.status)
                {//success
                    var dbC = model.BuildDBCustomer(int.Parse(Request.Cookies["UserId"].Value));
                    var dbD = model.BuildDBDeal();
                    oldC.channel = dbC.channel;
                    oldC.company = dbC.company;
                    oldC.email = dbC.email;
                    oldC.money = dbC.money;
                    oldC.name = dbC.name;
                    oldC.phone = dbC.phone;
                    oldC.product = dbC.product;
                    oldC.deal.Add(dbD);
                }
                else if (model.status.StatusId == oldD.status)
                {
                    var dbC = model.BuildDBCustomer(int.Parse(Request.Cookies["UserId"].Value));
                    var dbD = model.BuildDBDeal();
                    oldC.channel = dbC.channel;
                    oldC.company = dbC.company;
                    oldC.email = dbC.email;
                    oldC.money = dbC.money;
                    oldC.name = dbC.name;
                    oldC.phone = dbC.phone;
                    oldC.product = dbC.product;
                }
                else
                {
                    res.Data = new { error = "不能将进展改变为之前的状态。" };
                }
                db.SaveChanges();
            }

            return res;
        }

        public JsonResult CustomerDestroy()
        {
            JsonResult res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (Request.Cookies["UserId"] == null)
                return res;
            var models = Request.QueryString["models"];
            models = models.Substring(1, models.Length - 2);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MyCustomer));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(models));
            var model = (MyCustomer)serializer.ReadObject(ms);

            using (var db = new koollaexampleEntities())
            {
                var delete = db.customer.Where(p => p.cid == model.cid).Single();
                db.customer.Remove(delete);
                db.SaveChanges();
            }

            return res;
        }

        public JsonResult CustomerCreate()
        {
            JsonResult res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (Request.Cookies["UserId"] == null)
                return res;

            var models = Request.QueryString["models"];
            models = models.Substring(1, models.Length - 2);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MyCustomer));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(models));
            var model = (MyCustomer)serializer.ReadObject(ms);

            var dbC = model.BuildDBCustomer(int.Parse(Request.Cookies["UserId"].Value));
            var dbD = model.BuildDBDeal();
            dbC.deal.Add(dbD);
            using (var db = new koollaexampleEntities())
            {
                db.customer.Add(dbC);
                db.SaveChanges();
            }
            var c = new MyCustomer(dbC, dbD);
            res.Data = c;
            return res;
        }

        public ActionResult CustomerNote(string id)
        {
            int cid = 0;
            try
            {
                cid = int.Parse(id);
            }
            catch (Exception)
            {
                ViewBag.Name = "您没有访问此客户的权限";
                return View();
            }
            if (Request.Cookies["UserId"] == null)
            {
                ViewBag.Name = "您没有访问此客户的权限";
                return View();
            }
            using (var db = new koollaexampleEntities())
            {
                var c = db.customer.Where(p => p.cid == cid).Single();
                if (c.uid != int.Parse(Request.Cookies["UserId"].Value))
                {
                    ViewBag.Name = "您没有访问此客户的权限";
                    return View();
                }
                else
                {
                    var d = c.deal.OrderByDescending(p => p.status).First();
                    var dbC = new MyCustomer(c, d);
                    ViewBag.Name = dbC.name;
                    ViewBag.StatusName = dbC.status.StatusName;
                    ViewBag.ProductName = dbC.product.ProductName;
                    ViewBag.ChannelName = dbC.channel.ChannelName;
                    ViewBag.Money = dbC.money;
                    ViewBag.Phone = dbC.phone;
                    ViewBag.Email = dbC.email;
                    ViewBag.Cid = dbC.cid;
                }
            }
            return View();
        }


        public ActionResult Dashboard()
        {
            if (Request.Cookies["UserId"] == null)
                return RedirectToAction("Login", "Account");
            int uid = int.Parse(Request.Cookies["UserId"].Value);
            int monthIn = 0;
            int monthOpen = 0;
            int monthInP = 0;
            int monthOpenP = 0;
            using (var db = new koollaexampleEntities())
            {
                var customer = db.users.Where(u => u.uid == uid).Single().customer.ToList();
                foreach (customer cus in customer)
                {
                    var lastdeal = cus.deal.OrderByDescending(p => p.status).First();
                    DateTime now = DateTime.Now;
                    DateTime month = new DateTime(now.Year, now.Month, 1);
                    DateTime week = now.Date.AddDays(0 - (int)now.DayOfWeek);
                    if (lastdeal.date > month && (lastdeal.status == 5 || lastdeal.status == 6))
                    {//本月入账
                        monthIn += lastdeal.customer.money;
                        monthInP += 1;
                    }
                    var opendeal = cus.deal.Where(p => p.status == 4).SingleOrDefault();
                    if (opendeal != null && opendeal.date > month)
                    {//本月开户
                        monthOpen += opendeal.customer.money;
                        monthOpenP += 1;
                    }
                }
            }
            if (monthOpen == 0)
                ViewBag.rate = 0;
            else
                ViewBag.rate = ((double)monthIn / monthOpen * 100).ToString("F1");
            if (monthOpenP == 0)
                ViewBag.rate2 = 0;
            else
                ViewBag.rate2 = ((double)monthInP / monthOpenP * 100).ToString("F1");
            if (monthInP == 0)
                ViewBag.average = 0;
            else
                ViewBag.average = ((double)monthIn / monthInP).ToString("F1");
            ViewBag.monthIn = monthIn;
            ViewBag.monthInP = monthInP;
            ViewBag.monthOpen = monthOpen;
            ViewBag.monthOpenP = monthOpenP;
            return View();
        }

        public JsonResult weeksell()
        {
            JsonResult res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (Request.Cookies["UserId"] == null)
                return res;
            int uid = int.Parse(Request.Cookies["UserId"].Value);

            int[] count = new int[9];
            int[] money = new int[9];
            using (var db = new koollaexampleEntities())
            {
                var customer = db.users.Where(u => u.uid == uid).Single().customer.ToList();
                foreach (customer cus in customer)
                {
                    DateTime now = DateTime.Now;
                    DateTime week = now.Date.AddDays(0 - (int)now.DayOfWeek);
                    var deals = cus.deal.Where(p => p.date > week).ToList();
                    foreach (deal d in deals)
                    {
                        count[d.status] += 1;
                        money[d.status] += cus.money;
                    }
                }
            }
            var data = new List<List<object>>();
            for (int i = 0; i < 2;i++ )
            {
                var l = new List<object>();
                var status = new MyStatus(8 - i);
                l.Add(status.StatusName);
                l.Add(count[8 - i]);
                l.Add(money[8 - i]);
                data.Add(l);
            }
            for (int i = 7; i >= 2; i--)
            {
                var l = new List<object>();
                var status = new MyStatus(8 - i);
                l.Add(status.StatusName);
                l.Add(count[8 - i]);
                l.Add(money[8 - i]);
                data.Add(l);
            }
            res.Data = data;
            return res;
        }


        public JsonResult monthmoney()
        {
            JsonResult res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (Request.Cookies["UserId"] == null)
                return res;

            int uid = int.Parse(Request.Cookies["UserId"].Value);

            var target = new List<List<object>>();
            var actual = new List<List<object>>();

            DateTime now = DateTime.Now;
            DateTime monthDate = new DateTime(now.Year, now.Month, 1);
            
            var deallist=new List<deal>();
            using (var db = new koollaexampleEntities())
            {
                var customer = db.users.Where(u => u.uid == uid).Single().customer.ToList();
                foreach (customer cus in customer)
                {
                    var deal = cus.deal.Where(p => p.date > monthDate && (p.status==5 || p.status==6)).OrderByDescending(p => p.status).FirstOrDefault();
                    if (deal != null)
                        deallist.Add(deal);
                }
                deallist = deallist.OrderBy(p => p.date).ToList();
                int index = 0;
                int month = DateTime.DaysInMonth(now.Year, now.Month);
                int money = 0;
                for (int i = 1; i <= month; i++)
                {
                    List<object> l = new List<object>();
                    l.Add(i);
                    l.Add(i * 20000);
                    List<object> ll = new List<object>();
                    ll.Add(i);

                    DateTime d = (new DateTime(now.Year, now.Month, i)).AddDays(1);
                    while (index < deallist.Count && deallist[index].date < d )
                    {
                        money += deallist[index].customer.money;
                        index++;
                    }
                    ll.Add(money);
                    target.Add(l);
                    actual.Add(ll);
                }
            }
            
            res.Data = new {target=target,actual=actual};

            return res;
        }


        public JsonResult monthcount()
        {
            JsonResult res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (Request.Cookies["UserId"] == null)
                return res;

            int uid = int.Parse(Request.Cookies["UserId"].Value);

            var target = new List<List<object>>();
            var actual = new List<List<object>>();

            DateTime now = DateTime.Now;
            DateTime monthDate = new DateTime(now.Year, now.Month, 1);

            var deallist = new List<deal>();
            using (var db = new koollaexampleEntities())
            {
                var customer = db.users.Where(u => u.uid == uid).Single().customer.ToList();
                foreach (customer cus in customer)
                {
                    var deal = cus.deal.Where(p => p.date > monthDate && (p.status == 5 || p.status == 6)).OrderByDescending(p => p.status).FirstOrDefault();
                    if (deal != null)
                        deallist.Add(deal);
                }
                deallist = deallist.OrderBy(p => p.date).ToList();
                int index = 0;
                int month = DateTime.DaysInMonth(now.Year, now.Month);
                int count = 0;
                for (int i = 1; i <= month; i++)
                {
                    List<object> l = new List<object>();
                    l.Add(i);
                    l.Add(i * 1);
                    List<object> ll = new List<object>();
                    ll.Add(i);

                    DateTime d = (new DateTime(now.Year, now.Month, i)).AddDays(1);
                    while (index < deallist.Count && deallist[index].date < d)
                    {
                        count ++;
                        index++;
                    }
                    ll.Add(count);
                    target.Add(l);
                    actual.Add(ll);
                }
            }

            res.Data = new { target = target, actual = actual };

            return res;
        }


        public JsonResult sellfilter()
        {
            JsonResult res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (Request.Cookies["UserId"] == null)
                return res;
            int uid = int.Parse(Request.Cookies["UserId"].Value);

            int[] count = new int[9];
            int[] money = new int[9];
            using (var db = new koollaexampleEntities())
            {
                var customer = db.users.Where(u => u.uid == uid).Single().customer.ToList();
                foreach (customer cus in customer)
                {
                    DateTime now = DateTime.Now;
                    DateTime week = now.Date.AddDays(0 - (int)now.DayOfWeek);
                    var deals = cus.deal.ToList();
                    foreach (deal d in deals)
                    {
                        count[d.status] += 1;
                        money[d.status] += cus.money;
                    }
                }
            }
            var data = new List<List<object>>();
            for (int i = 7; i >= 2; i--)
            {
                var l = new List<object>();
                var status = new MyStatus(8 - i);
                l.Add(status.StatusName);
                l.Add(count[8 - i]);
                l.Add(money[8 - i]);
                data.Add(l);
            }
            res.Data = data;

            return res;
        }


        public JsonResult routeanalyse()
        {
            JsonResult res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (Request.Cookies["UserId"] == null)
                return res;
            int uid = int.Parse(Request.Cookies["UserId"].Value);

            var result = new List<List<object>>();
            using (var db = new koollaexampleEntities())
            {
                var c = db.users.Where(p => p.uid == uid).Single().customer;
                for (int i = 1; i <= 6; i++)
                {
                    var channel= new MyChannel(i);
                    var l = new List<object>();
                    l.Add(channel.ChannelName);
                    var count = c.Where(p => p.channel == i).Count();
                    l.Add(count);
                    result.Add(l);
                }
                
            }
            res.Data = result;
            return res;
        }
    }
}
