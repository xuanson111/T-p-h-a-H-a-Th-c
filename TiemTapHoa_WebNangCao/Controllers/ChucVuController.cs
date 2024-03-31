using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiemTapHoa.Models;
using TiemTapHoa_WebNangCao.Models;

namespace TiemTapHoa.Controllers
{
    [Authorize]
    public class ChucVuController : Controller
    {
        DBTiemTapHoaEntities db = new DBTiemTapHoaEntities();
        // GET: ChucVu
        public ActionResult Index()
        {
            Session["page"] = "ChucVu";
            return View(db.ChucVus.ToList());
        }

        public ActionResult Search(string searchString)
        {
            var dsCV = db.ChucVus.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                dsCV = db.ChucVus.Where(ncc => ncc.TenCV.ToLower().Contains(searchString.ToLower())).ToList();
            }
            return PartialView("lst_ChucVu", dsCV);
        }

        public ActionResult Create()
        {
            ChucVu cv = new ChucVu();
            return View(cv);
        }
        [HttpPost]
        public ActionResult Create(ChucVu cv)
        {
            try
            {
                db.ChucVus.Add(cv);
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

        public ActionResult Edit(int id)
        {
            ChucVu editCV = db.ChucVus.Find(id);
            return View(editCV);
        }

        [HttpPost]
        public ActionResult Edit(ChucVu chucvu)
        {
            try
            {
                ChucVu editCV = db.ChucVus.Find(chucvu.MaCV);
                editCV.TenCV = chucvu.TenCV;
                editCV.LuongCV = chucvu.LuongCV;
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

        public ActionResult Delete(int id)
        {
            try
            {
                ChucVu chucvu = db.ChucVus.Find(id);
                db.ChucVus.Remove(chucvu);
                int rowsAffected = db.SaveChanges();
                if (rowsAffected > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            } catch (Exception ex)
            {
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
    }
}