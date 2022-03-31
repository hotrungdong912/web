using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using G5_Website_giay.Models;

namespace G5_Website_giay.Controllers
{
    public class LoginController : Controller
    {
        public OnlineShopShoesEntities4 db = new OnlineShopShoesEntities4();

        // GET: Login

        public ActionResult Index()
        {
            ViewBag.menu = db.NhaSanXuats.ToList();
            return View();
        }
        [HttpPost]

        public ActionResult Index(string username, string password)
        {
            ViewBag.menu = db.NhaSanXuats.ToList();
            string pass = endCode.enCodeMD5(password);
            var rs = db.khachHangs.Where(p => p.UserName.Equals(username) && p.PassWord.Equals(pass)).FirstOrDefault();
            if (rs != null)
            {
                Session["customer"] = rs.khID;
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        //PHƯƠNG THỨC GET dữ liệu từ 
        public ActionResult Register()
        {
            ViewBag.menu = db.NhaSanXuats.ToList();
            return View();
        }
        //PHƯƠNG THỨC POST
        [HttpPost]
        public ActionResult Register(khachHang customer)
        {
            if (ModelState.IsValid)
            {
                endCode md5 = new endCode();
                customer.PassWord = endCode.enCodeMD5(customer.PassWord);
                db.khachHangs.Add(customer);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Login");
        }
    }   
}