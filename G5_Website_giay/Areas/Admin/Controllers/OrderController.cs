using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using G5_Website_giay.Models;
namespace G5_Website_giay.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        // GET: Admin/Order
        public OnlineShopShoesEntities4 db = new OnlineShopShoesEntities4();
        public ActionResult Index()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Index", "AdminLogin");
            var model = db.Orderproes.ToList();
            return View(model);
        }
        public ActionResult Details(int id)
        {
            ViewBag.od = db.Orderproes.Find(id);
            var models = db.chitietDatHangs.Where(p => p.OrderID == id);
            return View(models);
        }
        public ActionResult Edit(int id)
        {
            if (Session["admin"] == null)
                return RedirectToAction("Index", "AdminLogin");
            var model = db.Orderproes.Find(id);
            model.nvID = (int)Session["admin"];
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Orderpro orderpro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderpro).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Order");
        }
    }
}