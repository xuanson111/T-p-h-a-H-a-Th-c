using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiemTapHoa_WebNangCao.Models
{
    public partial class HangHoaView
    {
        public int MaHH { get; set; }
        public string TenHH { get; set; }
        public Nullable<double> DonGia { get; set; }
        public Nullable<double> SoLuong { get; set; }
        public string LoaiHangHoa { get; set; }
        public string DonVi { get; set; }
        public string UrlHinhAnh { get; set; }
        public HttpPostedFileBase FileHinhAnh { get; set; }

    }
}