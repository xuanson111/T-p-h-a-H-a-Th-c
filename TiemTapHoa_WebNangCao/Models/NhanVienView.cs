using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTapHoa_WebNangCao.Models;

namespace TiemTapHoa.Models
{
    public partial class NhanVienView
    {
        public int MaNV { get; set; }
        public string TenNV { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string SoTaiKhoan { get; set; }
        public Nullable<int> ChucVu { get; set; }
        public string TenChucVu { get; set; }
        public Nullable<double> LuongCV { get; set; }
        public string GioiTinh { get; set; }
        public string UrlHinhAnh { get; set; }
        public HttpPostedFileBase FileHinhAnh { get; set; }

        public List<NhanVienView> toList()
        {
            DBTiemTapHoaEntities db = new DBTiemTapHoaEntities();
            var nhanVien = from chucVu in db.ChucVus
                           join nv in db.NhanViens
                           on chucVu.MaCV equals nv.ChucVu
                           select new NhanVienView
                           {
                               MaNV = nv.MaNV,
                               TenNV = nv.TenNV,
                               NgaySinh = nv.NgaySinh,
                               Email = nv.Email,
                               SDT = nv.SDT,
                               DiaChi = nv.DiaChi,
                               SoTaiKhoan = nv.SoTaiKhoan,
                               ChucVu = chucVu.MaCV,
                               TenChucVu = chucVu.TenCV,
                               GioiTinh = nv.GioiTinh,
                               UrlHinhAnh = nv.HinhAnh,
                           };
            return nhanVien.ToList();
        }
    }
}