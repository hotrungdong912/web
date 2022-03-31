using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using G5_Website_giay.Models;
namespace G5_Website_giay.Areas.Admin.Controllers
{
    public class NationController : Controller
    {
        public OnlineShopShoesEntities4 db = new OnlineShopShoesEntities4();
        // GET: Admin/Nation
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            var models = db.QuocGias.ToList();
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
        public ActionResult Create(QuocGia quocGias)
        {
            if (ModelState.IsValid)
            {
                db.QuocGias.Add(quocGias);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Nation");
        }
        public ActionResult Details(string id)
        {
            var model = db.QuocGias.Find(id);
            return View(model);
        }
        public ActionResult Delete(string id)
        {
            var model = db.QuocGias.Find(id);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            var item = db.QuocGias.Find(id);
            db.QuocGias.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index", "Nation");
        }
        public ActionResult Edit(string id)
        {
            var model = db.QuocGias.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(QuocGia quocGias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quocGias).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Nation");
            }
            return View(quocGias);
        }
    }
}