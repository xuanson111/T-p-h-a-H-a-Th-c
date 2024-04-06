using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTapHoa_WebNangCao.Models;
using System.Web.Security;
using System.Web.SessionState;

namespace TiemTapHoa_WebNangCao.Controllers
{
    [Authorize]
    public class HoaDonController : Controller
    { 
        DBTiemTapHoaEntities db = new DBTiemTapHoaEntities();
        // GET: HoaDon
        
        public ActionResult Index(string searchString, string filter)
        {
            var dsHangHoa = db.HangHoas.ToList();
            Session["page"] = "HoaDon";
            return View(dsHangHoa);
        }

        public ActionResult PrintPage(int? isPay)
        {
            if (isPay > 0)
            {
                saveBill();
            }
            return View((List<ChiTietHDView>)Session["bill"]);
        }

        public ActionResult Search(string searchString, bool isFilter)
        {
            var dsHangHoa = db.HangHoas.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                if (isFilter)
                {
                    if (searchString != "0")
                        dsHangHoa = db.HangHoas.Where(hh => hh.LoaiHangHoa.ToLower().Contains(searchString.ToLower())).ToList();
                }
                else
                {
                    dsHangHoa = db.HangHoas.Where(hh => hh.TenHH.ToLower().Contains(searchString.ToLower())).ToList();
                }
            }
            return PartialView("_HoaDon_SearchProduct", dsHangHoa);
        }

        public ActionResult handleLstAddOrder(int id, int quantity)
        {
            if (Session["bill"] == null)
            {
                HangHoa newHH = db.HangHoas.FirstOrDefault(hh => hh.MaHH.Equals(id));
                List<ChiTietHDView> lstOrder = new List<ChiTietHDView>();
                lstOrder.Add(new ChiTietHDView(1, id, newHH.TenHH, 1, newHH.DonGia));
                Session["bill"] = lstOrder;
                Session["countBill"] = 1;
            }
            else
            {
                List<ChiTietHDView> lstOrder = (List<ChiTietHDView>)Session["bill"];
                int index = isExist(id);
                if (index != -1)
                {
                    lstOrder[index].soLuong += quantity;
                }
                else
                {
                    int countBill = (int)Session["countBill"];
                    HangHoa newHH = db.HangHoas.FirstOrDefault(hh => hh.MaHH.Equals(id));
                    lstOrder.Add(new ChiTietHDView(countBill, id, newHH.TenHH, 1, newHH.DonGia));
                    Session["countBill"] = countBill + 1;
                }
                Session["bill"] = lstOrder;
            }
            return PartialView("_HoaDon_LstOrderItem", (List<ChiTietHDView>)Session["bill"]);
        }

        private int isExist(int id)
        {
            List<ChiTietHDView> lstOrder = (List<ChiTietHDView>)Session["bill"];
            for (int i = 0; i < lstOrder.Count; i++)
            {
                if (lstOrder[i].maHH == id)
                {
                    return i;
                }
            }
            return -1;
        }

        public ActionResult clearOrder()
        {
            Session["bill"] = null;
            return PartialView("_HoaDon_LstOrderItem", (List<ChiTietHDView>)Session["bill"]);
        }

        public ActionResult handleLstUpdateOrder(int id, string quantity)
        {
            if (Session["bill"] != null)
            {
                List<ChiTietHDView> lstOrder = (List<ChiTietHDView>)Session["bill"];
                int index = isExist(id);
                if (index != -1)
                {
                    lstOrder[index].soLuong = float.Parse(quantity);
                }
                Session["bill"] = lstOrder;
            }
            return PartialView("_HoaDon_LstOrderItem", (List<ChiTietHDView>)Session["bill"]);
        }

        public ActionResult deleteItem(int id)
        {
            if (Session["bill"] != null)
            {
                List<ChiTietHDView> lstOrder = (List<ChiTietHDView>)Session["bill"];
                int index = isExist(id);
                lstOrder.Remove(lstOrder[index]);
                Session["bill"] = lstOrder;
            }
            return PartialView("_HoaDon_LstOrderItem", (List<ChiTietHDView>)Session["bill"]);
        }

        public bool saveBill()
        {
            int rowAffected = -1;
            if (Session["bill"] != null)
            {
                List<ChiTietHDView> lstOrder = (List<ChiTietHDView>)Session["bill"];
                double total = 0;
                for (int i = 0; i <  lstOrder.Count; i++)
                {
                    total += double.Parse((lstOrder[i].soLuong * lstOrder[i].donGia).ToString());
                }
                db.HoaDons.Add(new HoaDon(0 , null, DateTime.Now, total));
                rowAffected = db.SaveChanges();
                var lstHD = db.HoaDons.ToList();
                int maxMaHD = lstHD[lstHD.Count - 1].MaHD;
                for (int i = 0; i < lstOrder.Count;i++)
                {
                    db.ChiTietHDs.Add(new ChiTietHD(maxMaHD, lstOrder[i].maHH, lstOrder[i].soLuong));
                    db.SaveChanges();
                }
            }
            if (rowAffected > 0) return true;
            else return false;
        }

        public ActionResult HoaDonView()
        {
            HoaDonView hd = new HoaDonView();
            var Bills = hd.getData();
            Session["page"] = "HoaDon";
            return View(Bills);
        }

    }
}