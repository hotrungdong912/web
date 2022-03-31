using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using G5_Website_giay.Models;
using G5_Website_giay.Controllers;
using System.Web.Helpers;
namespace G5_Website_giay.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public OnlineShopShoesEntities4 db = new OnlineShopShoesEntities4();
      
        public ActionResult Index()
        {
            ViewBag.menu = db.NhaSanXuats.ToList();
            List<cartItem> list = (List<cartItem>)Session["cartSession"];
            return View(list);
        }
        public ActionResult AddItem(int id)
        {
            ViewBag.menu = db.NhaSanXuats.ToList();
            var cart = Session["cartSession"];
            List<cartItem> list = new List<cartItem>();
            //cart is null
            if (cart == null)
            {
                SanPham sanpham = db.SanPhams.Where(x => x.spID == id).SingleOrDefault();
                cartItem item = new cartItem();
                item.SanPham = sanpham;
                item.Quantity = 1;
                list.Add(item);
                Session["cartSession"] = list;
            }
            else //cart not null
            {
                list = (List<cartItem>)Session["cartSession"];
                if (list.Exists(x => x.SanPham.spID == id))
                {
                    foreach (var a in list)
                    {
                        if (a.SanPham.spID == id)
                        {
                            a.Quantity = a.Quantity + 1;
                        }
                    }
                    Session["cartSession"] = list;
                }
                else // not exist
                {
                    SanPham sanpham = db.SanPhams.Where(x => x.spID == id).SingleOrDefault();
                    cartItem item = new cartItem();
                    item.SanPham = sanpham;
                    item.Quantity = 1;
                    list.Add(item);
                    Session["cartSession"] = list;
                }
            }
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult Delete(int id)
        {
            List<cartItem> list = (List<cartItem>)Session["cartSession"];
            cartItem item = list.Where(p => p.SanPham.spID.Equals(id)).FirstOrDefault();
            list.Remove(item);
            Session["cartSession"] = list;
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult sendEmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult sendEmail(string useremail , string body)
        {
            string subject = "Bạn đã mua hàng ở cửa hàng Cửa hàng giày G5 của chúng tôi đây là thông tin mua hàng của bạn";
            WebMail.Send(useremail, subject, body, null, null, null, true, null, null, null, null, null, null);
            return View();
        }
        public ActionResult Order()
        {
            if (Session["customer"] == null)
                return RedirectToAction("Index", "Login");
            else
            {
                // Orproduct 
                Orderpro od = new Orderpro();
                od.OrderID = db.Orderproes.OrderByDescending(p => p.OrderID).First().OrderID + 1;
                od.OrderDate = DateTime.Now;
                od.khID = (int)Session["customer"];
                od.Status = 0;
                db.Orderproes.Add(od);
                db.SaveChanges();
                List<cartItem> list = (List<cartItem>)Session["cartSession"];
                string message = "";
                float sum = 0;
                string body = "";
                foreach (cartItem item in list)
                {
                    chitietDatHang chitiet = new chitietDatHang();
                    chitiet.OrderID = od.OrderID;
                    chitiet.spID = item.SanPham.spID;
                    chitiet.soluong = item.Quantity;
                    db.chitietDatHangs.Add(chitiet);
                    db.SaveChanges();
                    message += "<br/> ProductID: " + item.SanPham.spID;
                    message += "<br/> ProductName: " + item.SanPham.spName;
                    message += "<br/> Quantity: " + item.Quantity;
                    message += "<br/> Price: " + item.SanPham.gia;
                    message +=  String.Format("{0:0,0 VND}", item.Quantity * item.SanPham.gia);
                    sum += (float)(item.Quantity * item.SanPham.gia);
                    body += "<br/> ProductID: " + item.SanPham.spID;
                    body += "<br/> ProductName: " + item.SanPham.spName;
                    body += "<br/> Quantity: " + item.Quantity;
                }
                message += "<br> Total Money: " + String.Format("{0:0,0 VND}", sum);
                body += "<br> -------------------------------------";
                body += "<br> Total Money: " + String.Format("{0:0,0 VND}", sum);               
                // Send to customer Customer 
                khachHang ct = db.khachHangs.Find(od.khID);
                sendEmail(ct.Email,body);
                Session["cartSession"] = null;
                return RedirectToAction("Index", "Home");
            }
        }

    }
}