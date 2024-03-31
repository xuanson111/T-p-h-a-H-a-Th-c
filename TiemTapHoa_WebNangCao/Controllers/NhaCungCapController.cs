using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTapHoa.Models;
using TiemTapHoa_WebNangCao.Models;

namespace TiemTapHoa.Controllers
{
    [Authorize]
    public class NhaCungCapController : Controller
    {
        DBTiemTapHoaEntities db = new DBTiemTapHoaEntities();

        // GET: NhaCungCap
        public ActionResult Index()
        {
            var dsNCC = db.NhaCungCaps.ToList();
            Session["page"] = "NhaCungCap";
            return View(dsNCC);
        }

        public ActionResult Search(string searchString)
        {
            var dsNCC = db.NhaCungCaps.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                dsNCC = db.NhaCungCaps.Where(ncc => ncc.TenNCC.ToLower().Contains(searchString.ToLower())).ToList();
            }
            return PartialView("lst_NhaCungCap", dsNCC);
        }

        public ActionResult Create()
        {
            NhaCungCap ncc = new NhaCungCap();
            return View(ncc);
        }

        [HttpPost]
        public ActionResult Create(NhaCungCap ncc)
        {
            try
            {
                db.NhaCungCaps.Add(ncc);
                int rowsAffected = db.SaveChanges();
                if (rowsAffected > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            NhaCungCap ncc = db.NhaCungCaps.Find(id);
            return View(ncc);
        }

        [HttpPost]
        public ActionResult Edit(NhaCungCap ncc)
        {
            try
            {
                NhaCungCap editNCC = db.NhaCungCaps.Find(ncc.MaNCC);
                editNCC.MaNCC = ncc.MaNCC;
                editNCC.TenNCC = ncc.TenNCC;
                editNCC.SDT = ncc.SDT;
                editNCC.SoTaiKhoan = ncc.SoTaiKhoan;
                editNCC.Email = ncc.Email;
                editNCC.DiaChi = ncc.DiaChi;
                int rowsAffected = db.SaveChanges();
                if (rowsAffected > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                NhaCungCap ncc = db.NhaCungCaps.Find(id);
                db.NhaCungCaps.Remove(ncc);
                int rowsAffected = db.SaveChanges();
                if (rowsAffected > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
    }
}