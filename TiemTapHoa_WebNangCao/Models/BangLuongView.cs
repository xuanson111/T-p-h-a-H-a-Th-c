using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiemTapHoa_WebNangCao.Models
{
    [Authorize]
    public class BangLuongView
    {
        public int MaBL { get; set; }
        public Nullable<int> NhanVien { get; set; }
        public string TenNV { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int SoNgayNghi { get; set; }

        public int TongSoNgay { get; set; }

        public Nullable<double> Luong { get; set; }

        public Nullable<int> ChucVu { get; set; }

        public Nullable<double> LuongCV { get; set; }

        public List<BangLuongView> getData()
        {
            DBTiemTapHoaEntities db = new DBTiemTapHoaEntities();
            var bangLuong = from nv in db.NhanViens
                            join bl in db.BangLuongs
                            on nv.MaNV equals bl.MaNV
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
                            };
            return bangLuong.ToList();
        }
    }
}