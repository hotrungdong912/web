using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using G5_Website_giay.Models;
using PagedList;

namespace G5_Website_giay.Controllers
{
    
    public class HomeController : Controller
    {
        public OnlineShopShoesEntities4 db = new OnlineShopShoesEntities4();

        public IEnumerable<SanPham> AllListPaging(int page , int pageSize)
        {
            return db.SanPhams.OrderByDescending(x => x.spNgaySanXuat).ToPagedList(page, pageSize);
        }
        public IEnumerable<SanPham> AllListPagingByType(int page, int pageSize, string typeID)
        {
            return db.SanPhams.OrderByDescending(x => x.spNgaySanXuat).Where(x => x.nhasanxuatID.Equals(typeID)).ToPagedList(page, pageSize);
        }
        public ActionResult Index(string id, int page = 1, int pageSize = 12)
        {
            ViewBag.menu = db.NhaSanXuats.ToList();
            if (id == null)
            {
                var models = AllListPaging(page, pageSize);
                ViewBag.id = null;
                return View(models);
            }
            else
            {
                var models = AllListPagingByType(page, pageSize, id);
                ViewBag.id = id;
                return View(models);
            }            

        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            ViewBag.menu = db.NhaSanXuats.ToList();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ViewBag.menu = db.NhaSanXuats.ToList();
            return View();
        }
        public ActionResult Details(int id)
        {
            ViewBag.menu = db.NhaSanXuats.ToList();
            ViewBag.od = db.SanPhams.Find(id);
            var models = db.SanPhams.Where(p => p.spID == id);
            return View(models);
        }
        public ActionResult Details2(int id)
        {
            ViewBag.menu = db.NhaSanXuats.ToList();
            ViewBag.od = db.Orderproes.Find(id);
            var models = db.chitietDatHangs.Where(p => p.OrderID == id);
            return View(models);
        }
        public ActionResult HistoryOrder()
        {
            ViewBag.menu = db.NhaSanXuats.ToList();
            int cusId = (int)Session["customer"];
            var models = db.Orderproes.Where(p => p.khID == cusId).OrderBy(p => p.OrderDate).ToList();
            return View(models);
        }

    }
}