using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiemTapHoa.Models;

namespace TiemTapHoa_WebNangCao.Models
{
    public partial class HoaDonView
    {
        public int MaHD { get; set; }
        public Nullable<int> MaNV { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public Nullable<double> TongTien { get; set; }
        public string TenNV { get; set; }

        public List<HoaDonView> getData(int amount)
        {
            DBTiemTapHoaEntities db = new DBTiemTapHoaEntities();
            var recentBills = (from hd in db.HoaDons
                               orderby hd.NgayTao descending
                               select new HoaDonView
                               {
                                   MaHD = hd.MaHD,
                                   TongTien = hd.TongTien,
                                   NgayTao = hd.NgayTao,
                               });
            return recentBills.Take(amount).ToList();
        }

        public List<HoaDonView> getData()
        {
            DBTiemTapHoaEntities db = new DBTiemTapHoaEntities();
            var recentBills = (from hd in db.HoaDons
                               orderby hd.NgayTao descending
                               select new HoaDonView
                               {
                                   MaHD = hd.MaHD,
                                   TongTien = hd.TongTien,
                                   NgayTao = hd.NgayTao,
                               });
            return recentBills.ToList();
        }
    }
}