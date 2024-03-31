using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiemTapHoa_WebNangCao.Models
{
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
    }
}