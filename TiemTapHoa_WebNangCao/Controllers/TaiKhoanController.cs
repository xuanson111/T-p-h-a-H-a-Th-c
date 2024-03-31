using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TiemTapHoa.Models;
using TiemTapHoa_WebNangCao.Models;

namespace TiemTapHoa.Controllers
{
    [AllowAnonymous]
    public class TaiKhoanController : Controller
    {
        DBTiemTapHoaEntities db = new DBTiemTapHoaEntities();
        // GET: TaiKhoan

        public ActionResult DangXuat()
        {
            Session["User"] = null;
            return RedirectToAction("Login", "TaiKhoan");
        }

        public ActionResult Login()
        {
            FormsAuthentication.SignOut();
            TaiKhoan tk = new TaiKhoan();
            return View(tk);
        }

        [HttpPost]
        public ActionResult Login(TaiKhoan tk)
        {
            var user = db.TaiKhoans.FirstOrDefault(m => m.TaiKhoan1.Equals(tk.TaiKhoan1) && m.MatKhau.Equals(tk.MatKhau));
            if (user == null)
            {
                ViewBag.isLoginError = "Đăng nhập thất bại, tài khoản hoặc mật khẩu không đúng";
            }
            else
            {
                Session["User"] = user;
                Session["page"] = "Home";
                FormsAuthentication.SetAuthCookie(user.TaiKhoan1, false);
                return RedirectToAction("Index", "Home");
            }
            return View(tk);
        }
    }
}