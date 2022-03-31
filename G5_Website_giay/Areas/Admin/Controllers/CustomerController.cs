using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using G5_Website_giay.Models;
namespace G5_Website_giay.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        public OnlineShopShoesEntities4 db = new OnlineShopShoesEntities4();
        // GET: Admin/Nation
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            var models = db.khachHangs.ToList();
            return View(models);
        }
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create(khachHang khachHangs)
        {
            if (ModelState.IsValid)
            {
                endCode md5 = new endCode();
                khachHangs.PassWord = endCode.enCodeMD5(khachHangs.PassWord);
                db.khachHangs.Add(khachHangs);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Customer");
        }
        public ActionResult Details(int id)
        {
            var model = db.khachHangs.Find(id);
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var model = db.khachHangs.Find(id);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            var item = db.khachHangs.Find(id);
            db.khachHangs.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index", "Customer");
        }
        public ActionResult Edit(int id)
        {
            var model = db.khachHangs.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(khachHang khachHangs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachHangs).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Customer");
            }
            return View(khachHangs);
        }
    }
}