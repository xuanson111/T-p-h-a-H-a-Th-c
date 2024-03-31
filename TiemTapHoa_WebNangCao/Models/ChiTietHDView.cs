using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace TiemTapHoa_WebNangCao.Models
{
    public class ChiTietHDView
    {
        public ChiTietHDView(int soThuTu, int maHH, string tenHH, float soLuong, double? donGia)
        {
            this.soThuTu = soThuTu;
            this.maHH = maHH;
            this.TenHH = tenHH;
            this.soLuong = soLuong;
            this.donGia = donGia;
        }

        public int soThuTu {  get; set; }
        public int maHH { get; set; }
        public string TenHH { get; set; }
        public float soLuong { get; set; }
        public Nullable<double> donGia { get; set; }

    }
}