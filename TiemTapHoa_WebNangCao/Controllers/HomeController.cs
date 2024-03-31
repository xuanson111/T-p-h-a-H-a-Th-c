using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTapHoa.Models;
using TiemTapHoa_WebNangCao.Models;

namespace TiemTapHoa_WebNangCao.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        DBTiemTapHoaEntities db = new DBTiemTapHoaEntities();
        public ActionResult Index()
        {
            HoaDonView hd = new HoaDonView();
            var recentBills = hd.getData(5);
            Session["page"] = "Home";
            return View(recentBills);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}