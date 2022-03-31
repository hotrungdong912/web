using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using G5_Website_giay.Models;
namespace G5_Website_giay.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        // GET: Admin/Product
        public OnlineShopShoesEntities4 db = new OnlineShopShoesEntities4();
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            var models = db.SanPhams.ToList();
            return View(models);
        }
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            ViewBag.types = new SelectList(db.LoaiSanPhams.ToList(), "lspID", "lspName");
            ViewBag.manus = new SelectList(db.NhaSanXuats.ToList(), "nhasanxuatID", "nhasanxuatName");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SanPham sanPham, HttpPostedFileBase Images)
        {
            if (Images != null && Images.ContentLength > 0)
            {
                sanPham.Images = Images.FileName;
                string urlImages = Server.MapPath("~/Content/image/" + sanPham.Images);
                Images.SaveAs(urlImages);
            }
            if (ModelState.IsValid)
            {
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listNation = new SelectList(db.LoaiSanPhams, "lspID", "lspName", sanPham.lspID);
            ViewBag.listTypes = new SelectList(db.NhaSanXuats, "nhasanxuatID", "nhasanxuatName", sanPham.nhasanxuatID);

            return View(sanPham);
        }
        public ActionResult Edit(int id)
        {
            if(Session["admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            var models = db.SanPhams.Find(id);
            ViewBag.types = new SelectList(db.LoaiSanPhams.ToList(), "lspID", "lspName", models.lspID);
            ViewBag.manus = new SelectList(db.NhaSanXuats.ToList(), "nhasanxuatID", "nhasanxuatName", models.nhasanxuatID);
            return View(models);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(SanPham sanpham , HttpPostedFileBase newimage)
        {
            if (newimage != null && newimage.ContentLength > 0)
            {
                sanpham.Images = newimage.FileName;
                string urlImage = Server.MapPath("~/Content/image/" + sanpham.Images);
                newimage.SaveAs(urlImage);
            }
            if (ModelState.IsValid)
            {
                db.Entry(sanpham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Product");
            }
            ViewBag.types = new SelectList(db.LoaiSanPhams.ToList(), "lspID", "lspName", sanpham.lspID);
            ViewBag.manus = new SelectList(db.NhaSanXuats.ToList(), "nhasanxuatID", "nhasanxuatName", sanpham.nhasanxuatID);
            return View(sanpham);
        }
        public ActionResult Delete(int id)
        {
            var model = db.SanPhams.Find(id);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult ComfirmDelete(int id)
        {
            var item = db.SanPhams.Find(id);
            var orderDetails1 = db.chitietDatHangs.ToList();
            int check = 0;
            foreach (chitietDatHang i in orderDetails1)
            {
                if (id == i.spID)
                {
                    check = check + 1;
                }
            }
            if (check > 0)
            {
                TempData["check"] = "Sản phẩm này đã tôn tại trong chi tiết hóa đơn ,vui lòng xóa tại chi tiết hóa đơn trước khi xóa sản phẩm.";
                return RedirectToAction("Delete", "Product");
            }
            else
            {
                db.SanPhams.Remove(item);
                db.SaveChanges();
                TempData["check"] = "Bạn đã xóa thành công";
                return RedirectToAction("Index", "Product");
            }
           
        }
        public ActionResult Details(int id)
        {
            var model = db.SanPhams.Find(id);
            return View(model);
        }
    }
}