using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TiemTapHoa.Models;
using TiemTapHoa_WebNangCao.Models;

namespace TiemTapHoa_WebNangCao.Controllers
{
    [Authorize]
    public class PhieuNhapController : Controller
    {
        // GET: PhieuNhap

        DBTiemTapHoaEntities db = new DBTiemTapHoaEntities();
        PhieuNhapView ph = new PhieuNhapView();
        public ActionResult Index()
        {
            Session["page"] = "PhieuNhap";
            var phieuNhap = ph.getData();
            return View(phieuNhap);
        }

        public ActionResult Search(string filter)
        {
            var phieuNhap = ph.getData();
            if (!string.IsNullOrEmpty(filter))
            {
                ViewBag.ncc = filter;
                if (filter == "0") { return PartialView("lst_PhieuNhap", phieuNhap); }
                var pnLst = phieuNhap.Where(pn => pn.MaNCC.ToString().ToLower().Contains(filter.ToLower()));
                return PartialView("lst_PhieuNhap", pnLst);
            }
            return PartialView("lst_PhieuNhap", phieuNhap);
        }

        public ActionResult Details(int id)
        {
            var ctphieuNhap = from ctpn in db.ChiTietPhieuNhaps
                              join pn in db.PhieuNhaps
                              on ctpn.MaPhieu equals pn.MaPhieu
                              join hh in db.HangHoas
                              on ctpn.MaHH equals hh.MaHH
                              join ncc in db.NhaCungCaps
                              on pn.MaNCC equals ncc.MaNCC
                              select new ChiTietPhieuNhapView
                              {
                                  MaPhieu = pn.MaPhieu,
                                  HangHoa = hh.MaHH,
                                  TenHH = hh.TenHH,
                                  NhaCungCap = ncc.MaNCC,
                                  TenNCC = ncc.TenNCC,
                                  NgayTao = pn.NgayTao,
                                  SoLuong = ctpn.SoLuong,
                                  DonGia = ctpn.DonGia,
                                  ThanhTien = pn.ThanhTien,
                              };
            ChiTietPhieuNhapView ctpn1 = ctphieuNhap.FirstOrDefault(m => m.MaPhieu.Equals(id));
            //var result = ctphieuNhap.ToList();
            return View(ctpn1);
        }

        public ActionResult Create()
        {
            ChiTietPhieuNhapView ctpn = new ChiTietPhieuNhapView();
            return View(ctpn);
        }

        [HttpPost]
        public ActionResult Create(PhieuNhapView pn)
        {
            try
            {
                ChiTietPhieuNhap addCTPN = new ChiTietPhieuNhap(pn.MaPhieu, pn.HangHoa, pn.SoLuong, pn.DonGia);
                db.ChiTietPhieuNhaps.Add(addCTPN);
                pn.ThanhTien = pn.SoLuong * pn.DonGia;
                PhieuNhap addPN = new PhieuNhap(pn.MaPhieu, pn.MaNCC, DateTime.Now, pn.ThanhTien);
                db.PhieuNhaps.Add(addPN);
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
    }
}