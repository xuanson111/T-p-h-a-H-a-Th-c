using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTapHoa.Models;
using TiemTapHoa_WebNangCao.Models;

namespace TiemTapHoa_WebNangCao.Controllers
{
    [Authorize]
    public class BangLuongController : Controller
    {
        DBTiemTapHoaEntities db = new DBTiemTapHoaEntities();
        BangLuongView blv = new BangLuongView();
        public ActionResult Index(string searchString, string filter)
        {
            Session["page"] = "BangLuong";
            var bangLuong = blv.getData();
            return View(bangLuong);
        }

        public ActionResult Search(string searchString)
        {
            var bangLuong = blv.getData();
            if (!string.IsNullOrEmpty(searchString))
            {
                var nvLst = bangLuong.Where(nv => nv.TenNV.ToLower().Contains(searchString.ToLower()));
                return PartialView("lst_BangLuong", nvLst);
            }
            return PartialView("lst_BangLuong", bangLuong);
        }

        public ActionResult Create()
        {
            BangLuongView bl = new BangLuongView();
            return View(bl);
        }
        [HttpPost]
        public ActionResult Create(BangLuongView bl)
        {
            try
            {
                Session["page"] = "BangLuong";
                var nhanVien = db.NhanViens.Find(bl.NhanVien);
                var chucVu = db.ChucVus.Find(nhanVien.ChucVu);
                double? luong = ((4500000 + chucVu.LuongCV) / bl.TongSoNgay) * (bl.TongSoNgay - bl.SoNgayNghi);
                bl.Luong = luong.HasValue ? Math.Round(luong.Value) : 0;
                BangLuong addBL = new BangLuong(bl.MaBL, bl.NhanVien, bl.Thang, bl.Nam, bl.Luong, bl.SoNgayNghi, bl.TongSoNgay);

                db.BangLuongs.Add(addBL);
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
            BangLuongView bl1 = blv.getData().FirstOrDefault(m => m.MaBL.Equals(id));
            return View(bl1);
        }
        [HttpPost]
        public ActionResult Edit(BangLuongView bl)
        {
            try
            {
                var nhanVien = db.NhanViens.Find(bl.NhanVien);
                var chucVu = db.ChucVus.Find(nhanVien.ChucVu);
                double? luong = ((4500000 + chucVu.LuongCV) / bl.TongSoNgay) * (bl.TongSoNgay - bl.SoNgayNghi);
                bl.Luong = luong.HasValue ? Math.Round(luong.Value) : 0;

                BangLuong editbl = db.BangLuongs.Find(bl.MaBL);
                editbl.MaNV = bl.NhanVien;
                editbl.Thang = bl.Thang;
                editbl.Nam = bl.Nam;
                editbl.SoNgayNghi = bl.SoNgayNghi;
                editbl.TongSoNgay = bl.TongSoNgay;

                editbl.Luong = bl.Luong;
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
                BangLuong BL = db.BangLuongs.Find(id);
                db.BangLuongs.Remove(BL);
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
