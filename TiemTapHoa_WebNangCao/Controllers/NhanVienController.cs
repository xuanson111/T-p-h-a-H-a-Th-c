using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using TiemTapHoa.Models;
using TiemTapHoa_WebNangCao.Models;

namespace TiemTapHoa.Controllers
{
    [Authorize]
    public class NhanVienController : Controller
    {
        DBTiemTapHoaEntities db = new DBTiemTapHoaEntities();
        NhanVienView NV = new NhanVienView();

        // GET: NhanVien
        public ActionResult Index(string searchString, string filter)
        {
            Session["page"] = "NhanVien";
            var nhanVien = NV.toList();
            return View(nhanVien);
        }

        public ActionResult Search(string searchString, string filter)
        {
            var nhanVien = NV.toList();
            Boolean isSearchStr = !string.IsNullOrEmpty(searchString);
            Boolean isFilter = !string.IsNullOrEmpty(filter);
            if (isSearchStr && isFilter && filter != "0")
            {
                var nvLst = nhanVien.Where(nv => nv.TenNV.ToLower().Contains(searchString.ToLower()) && nv.ChucVu.ToString().ToLower().Contains(filter.ToLower()));
                return PartialView("lst_NhanVien", nvLst);
            }
            if (isSearchStr)
            {
                var nvLst = nhanVien.Where(nv => nv.TenNV.ToLower().Contains(searchString.ToLower()));
                return PartialView("lst_NhanVien", nvLst);
            }
            if (isFilter)
            {
                ViewBag.cv = filter;
                if (filter == "0") return PartialView("lst_NhanVien", nhanVien);
                var nvLst = nhanVien.Where(nv => nv.ChucVu.ToString().ToLower().Contains(filter.ToLower()));
                return PartialView("lst_NhanVien", nvLst);
            }
            return PartialView("lst_NhanVien", nhanVien);
        }

        public ActionResult Create()
        {
            NhanVienView nv = new NhanVienView();
            return View(nv);
        }
        [HttpPost]
        public ActionResult Create(NhanVienView nv)
        {
            try
            {
                HandleImg(nv);
                // đẩy thông tin vào biến addNV và thêm vào db
                NhanVien addNV = new NhanVien(nv.MaNV, nv.TenNV, nv.NgaySinh, nv.Email, nv.SDT, nv.DiaChi, nv.SoTaiKhoan, nv.ChucVu, nv.GioiTinh, nv.UrlHinhAnh);
                db.NhanViens.Add(addNV);
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
            var nhanVien = NV.toList();
            NhanVienView nv1 = nhanVien.FirstOrDefault(m => m.MaNV.Equals(id));
            return View(nv1);
        }
        [HttpPost]
        public ActionResult Edit(NhanVienView nv)
        {
            try
            {
                NhanVien editnv = db.NhanViens.Find(nv.MaNV);
                editnv.TenNV = nv.TenNV;
                editnv.SDT = nv.SDT;
                editnv.ChucVu = nv.ChucVu;
                editnv.DiaChi = nv.DiaChi;
                editnv.NgaySinh = nv.NgaySinh;
                editnv.Email = nv.Email;
                editnv.SoTaiKhoan = nv.SoTaiKhoan;
                editnv.GioiTinh = nv.GioiTinh;
                // kiểm tra xem người dùng có muốn chỉnh sửa ảnh không (có khi nv.FileHinhAnh không rỗng)
                if (nv.FileHinhAnh != null && nv.FileHinhAnh.ContentLength > 0)
                {
                    if (editnv.HinhAnh != null && editnv.HinhAnh != "")
                    {
                        // xóa hình ảnh cũ
                        string duongDanTepTin = Path.Combine(Server.MapPath("~/assets/img/"), editnv.HinhAnh.Substring(12));
                        if (System.IO.File.Exists(duongDanTepTin))
                        {
                            // Thực hiện xóa tệp tin
                            System.IO.File.Delete(duongDanTepTin);
                        }
                    }
                    // thực hiện lưu hình ảnh mới vào file /assets/Img và lưu đường dẫn vào object nv
                    HandleImg(nv);
                    editnv.HinhAnh = nv.UrlHinhAnh;
                }
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
                NhanVien NV = db.NhanViens.Find(id);
                if (NV.HinhAnh != null)
                {
                    string duongDanTepTin = Path.Combine(Server.MapPath("~/assets/img/"), NV.HinhAnh.Substring(12));
                    if (System.IO.File.Exists(duongDanTepTin))
                    {
                        // Thực hiện xóa tệp tin
                        System.IO.File.Delete(duongDanTepTin);
                    }

                }
                db.NhanViens.Remove(NV);
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

        public void HandleImg(NhanVienView nv)
        {
            // Xử lý hình ảnh
            if (nv.FileHinhAnh != null && nv.FileHinhAnh.ContentLength > 0)
            {
                // Xử lý tệp tin được tải lên, ví dụ lưu vào thư mục và cập nhật đường dẫn trong model
                string folderPath = Server.MapPath("~/assets/Img");
                // Lấy tên tập tin (không bao gồm .jpg  .png)
                string fileName = Path.GetFileNameWithoutExtension(nv.FileHinhAnh.FileName);
                // lấy đuôi của tập tin (vd: .jpg)
                string extension = Path.GetExtension(nv.FileHinhAnh.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string fullPath = Path.Combine(folderPath, fileName);
                nv.FileHinhAnh.SaveAs(fullPath);

                // Lưu đường dẫn vào model, ví dụ:
                nv.UrlHinhAnh = "/assets/img/" + fileName;
            }
        }

    }
}