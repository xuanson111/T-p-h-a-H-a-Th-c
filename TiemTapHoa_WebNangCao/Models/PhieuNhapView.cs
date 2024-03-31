using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTapHoa_WebNangCao.Models;

namespace TiemTapHoa_WebNangCao.Models
{
	public partial class PhieuNhapView
	{
        public int MaPhieu { get; set; }
        public Nullable<int> MaNCC { get; set; }
        public string TenNCC { get; set; }

        public Nullable<int> HangHoa { get; set; }
        public string TenHH { get; set; }

        public Nullable<double> SoLuong { get; set; }

        public Nullable<double> DonGia { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> NgayTao { get; set; }

        public Nullable<double> ThanhTien { get; set; }

        public List<PhieuNhapView> getData()
        {
            DBTiemTapHoaEntities db = new DBTiemTapHoaEntities();
            var phieuNhap = from ctpn in db.ChiTietPhieuNhaps
                              join pn in db.PhieuNhaps
                              on ctpn.MaPhieu equals pn.MaPhieu
                              join hh in db.HangHoas
                              on ctpn.MaHH equals hh.MaHH
                              join ncc in db.NhaCungCaps
                              on pn.MaNCC equals ncc.MaNCC
                              select new PhieuNhapView
                              {
                                  MaPhieu = pn.MaPhieu,
                                  HangHoa = hh.MaHH,
                                  TenHH = hh.TenHH,
                                  MaNCC = ncc.MaNCC,
                                  TenNCC = ncc.TenNCC,
                                  NgayTao = pn.NgayTao,
                                  SoLuong = ctpn.SoLuong,
                                  DonGia = ctpn.DonGia,
                                  ThanhTien = pn.ThanhTien,
                              };
            /*var phieuNhap = from NCC in db.NhaCungCaps
                            join pn in db.PhieuNhaps
                            on NCC.MaNCC equals pn.MaNCC
                            select new PhieuNhapView
                            {
                                MaPhieu = pn.MaPhieu,
                                MaNCC = NCC.MaNCC,
                                TenNCC = NCC.TenNCC,
                                NgayTao = pn.NgayTao,
                                ThanhTien = pn.ThanhTien,
                            };*/
            return phieuNhap.ToList();
        }
    }
}