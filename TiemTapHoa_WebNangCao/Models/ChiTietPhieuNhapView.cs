using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiemTapHoa_WebNangCao.Models
{
    public class ChiTietPhieuNhapView
    {
        public int MaPhieu { get; set; }
        public Nullable<int> NhaCungCap { get; set; }
        public string TenNCC { get; set; }

        public Nullable<int> HangHoa { get; set; }
        public string TenHH { get; set; }

        public Nullable<double> SoLuong { get; set; }

        public Nullable<double> DonGia { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> NgayTao { get; set; }

        public Nullable<double> ThanhTien { get; set; }
    }
}