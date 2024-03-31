using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTapHoa.Models;
using TiemTapHoa_WebNangCao.Models;
using WebGrease.Css.Ast.Selectors;

namespace TiemTapHoa.Controllers
{
    [Authorize]
    public class HangHoaController : Controller
    {
        DBTiemTapHoaEntities db = new DBTiemTapHoaEntities();
        // GET: HangHoa
        public ActionResult Index(string searchString, string filter)
        {
            Session["page"] = "HangHoa";
            return View(db.HangHoas.ToList());
        }

        public ActionResult Search(string searchString, string filter)
        {
            var HangHoa = db.HangHoas.ToList();
            Boolean isSearchStr = !string.IsNullOrEmpty(searchString);
            Boolean isFilter = !string.IsNullOrEmpty(filter);
            if (isSearchStr && isFilter && filter != "0")
            {
                var HangHoaLst = db.HangHoas.Where(nv => nv.TenHH.ToLower().Contains(searchString.ToLower()) && nv.LoaiHangHoa.ToString().ToLower().Contains(filter.ToLower()));
                return PartialView("lst_HangHoa", HangHoaLst);
            }
            if (isSearchStr)
            {
                var hangHoaLst = db.HangHoas.Where(nv => nv.TenHH.ToLower().Contains(searchString.ToLower()));
                return PartialView("lst_HangHoa", hangHoaLst);
            }
            if (isFilter)
            {
                ViewBag.loaiHH = filter;
                if (filter == "0") return PartialView("lst_HangHoa", HangHoa);
                var hangHoaLst = db.HangHoas.Where(nv => nv.LoaiHangHoa.ToString().ToLower().Contains(filter.ToLower()));
                return PartialView("lst_HangHoa", hangHoaLst);
            }
            return PartialView("lst_HangHoa", HangHoa);
        }

        public ActionResult Create()
        {
            HangHoa hh = new HangHoa();
            return View(hh);
        }
        [HttpPost]
        public ActionResult Create(HangHoaView hangHoa)
        {
            try
            {
                HandleImg(hangHoa);
                HangHoa HH = new HangHoa(hangHoa.MaHH, hangHoa.TenHH, hangHoa.DonGia, hangHoa.SoLuong, hangHoa.LoaiHangHoa, hangHoa.UrlHinhAnh, hangHoa.DonVi);
                db.HangHoas.Add(HH);
                int rowAffected = db.SaveChanges();
                if (rowAffected > 0)
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
            HangHoa hh = db.HangHoas.Find(id);
            return View(hh);
        }
        [HttpPost]
        public ActionResult Edit(HangHoaView hh)
        {

            try
            {
                HangHoa editHH = db.HangHoas.Find(hh.MaHH);
                editHH.TenHH = hh.TenHH;
                editHH.DonGia = hh.DonGia;
                editHH.SoLuong = hh.SoLuong;
                editHH.LoaiHangHoa = hh.LoaiHangHoa;
                editHH.DonVi = hh.DonVi;
                // kiểm tra xem người dùng có muốn chỉnh sửa ảnh không
                if (hh.FileHinhAnh != null && hh.FileHinhAnh.ContentLength > 0)
                {
                    if (editHH.HinhAnh != null && editHH.HinhAnh != "")
                    {
                        // xóa hình ảnh cũ
                        string duongDanTepTin = Path.Combine(Server.MapPath("~/assets/img/"), editHH.HinhAnh.Substring(12));
                        if (System.IO.File.Exists(duongDanTepTin))
                        {
                            // Thực hiện xóa tệp tin
                            System.IO.File.Delete(duongDanTepTin);
                        }
                    }
                    // thực hiện lưu hình ảnh mới vào file /assets/Img và lưu đường dẫn vào object nv
                    HandleImg(hh);
                    editHH.HinhAnh = hh.UrlHinhAnh;
                }
                int rowAffected = db.SaveChanges();
                if (rowAffected > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex) { }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                HangHoa HH = db.HangHoas.Find(id);
                if (HH.HinhAnh != null)
                {
                    string duongDanTepTin = Path.Combine(Server.MapPath("~/assets/img/"), HH.HinhAnh.Substring(12));
                    if (System.IO.File.Exists(duongDanTepTin))
                    {
                        // Thực hiện xóa tệp tin
                        System.IO.File.Delete(duongDanTepTin);
                    }

                }
                db.HangHoas.Remove(HH);
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

        public void HandleImg(HangHoaView nv)
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