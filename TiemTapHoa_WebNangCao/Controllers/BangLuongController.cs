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
        public ActionResult Index(string searchString, string filter)
        {
            Session["page"] = "BangLuong";
            var bangLuong = from nv in db.NhanViens
                            join bl in db.BangLuongs
                            on nv.MaNV equals bl.MaNV
                            select new BangLuongView
                            {
                                MaBL = bl.MaBL,
                                TenNV = nv.TenNV,
                                Thang = bl.Thang,
                                Nam = bl.Nam,
                                SoNgayNghi = bl.SoNgayNghi,
                                TongSoNgay = bl.TongSoNgay,
                                Luong = bl.Luong,
                            };
            if (!string.IsNullOrEmpty(searchString))
            {
                var nvLst = bangLuong.Where(nv => nv.TenNV.ToLower().Contains(searchString.ToLower()));
                return View(nvLst);
            }
            // if (!string.IsNullOrEmpty(filter))
            //{
            //   ViewBag.cv = filter;
            //   if (filter == "0") return View(nhanVien);
            //   var nvLst = nhanVien.Where(nv => nv.ChucVu.ToString().ToLower().Contains(filter.ToLower()));
            //   return View(nvLst);
            //}
            return View(bangLuong);
        }

        public ActionResult Create()
        {
            BangLuongView bl = new BangLuongView();
            return View(bl);
        }
        [HttpPost]
        public ActionResult Create(BangLuongView bl)
        {
            Session["page"] = "BangLuong";
            var nhanVien = db.NhanViens.Find(bl.NhanVien);
            var chucVu = db.ChucVus.Find(nhanVien.ChucVu);
            double? luong = ((4500000 + chucVu.LuongCV) / bl.TongSoNgay) * (bl.TongSoNgay - bl.SoNgayNghi);
            bl.Luong = luong.HasValue ? Math.Round(luong.Value) : 0;
            BangLuong addBL = new BangLuong(bl.MaBL, bl.NhanVien, bl.Thang, bl.Nam, bl.SoNgayNghi, bl.TongSoNgay, bl.Luong);

            db.BangLuongs.Add(addBL);
            int rowAffected = db.SaveChanges();
            if (rowAffected > 0)
            {
                ViewBag.addSuccessBL = "Thêm thành công";
            }
            else
            {
                ViewBag.addErrorBL = "Thêm thất bại, vui lòng kiểm tra lại thông tin";
            }
            return View(bl);
        }

        public ActionResult Edit(int id)
        {
            var bangLuong = from nv in db.NhanViens
                            join bl in db.BangLuongs
                            on nv.MaNV equals bl.MaNV
                            join cv in db.ChucVus
                            on nv.ChucVu equals cv.MaCV
                            select new BangLuongView
                            {
                                MaBL = bl.MaBL,
                                NhanVien = nv.MaNV,
                                TenNV = nv.TenNV,
                                Thang = bl.Thang,
                                Nam = bl.Nam,
                                SoNgayNghi = bl.SoNgayNghi,
                                TongSoNgay = bl.TongSoNgay,
                                Luong = bl.Luong,
                                ChucVu = cv.MaCV,
                                LuongCV = cv.LuongCV,
                            };
            BangLuongView bl1 = bangLuong.FirstOrDefault(m => m.MaBL.Equals(id));
            return View(bl1);
        }
        [HttpPost]
        public ActionResult Edit(BangLuongView bl)
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




            int rowAffected = db.SaveChanges();
            if (rowAffected > 0)
            {
                ViewBag.editSuccessBL = "Sửa thành công";
            }
            else
            {
                ViewBag.editErrorBL = "Sửa thất bại, vui lòng kiểm tra lại thông tin";
            }
            return View(bl);
        }

        public ActionResult Delete(int id)
        {
            BangLuong BL = db.BangLuongs.Find(id);

            db.BangLuongs.Remove(BL);
            db.SaveChanges();
            return RedirectToAction("Index", "BangLuong");
        }
    }
}
