﻿﻿using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Kendo.Mvc.Extensions;
using DicentDraw.Models;
using System.Web;
using System.IO;

namespace DicentDraw.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class DessertEditController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();
        public ActionResult Index()
        {
            return View(db.Dessert.ToList());
        }
        public ActionResult Edit(string DessertID)
        {
            //類別下拉選單
            Dictionary<string, string> kind = new Dictionary<string, string>();
            kind.Add("Cookie", "餅乾");
            kind.Add("Cake", "蛋糕");
            kind.Add("Pie", "派");
            var dessert = db.Dessert.Find(DessertID);
            ViewBag.selectKind = new SelectList(kind, "key", "value",dessert.DessertKind);
            return View(dessert);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Dessert dessert, HttpPostedFileBase DessertImage2)
        {
            //判斷是否有圖片上傳
            if (DessertImage2 != null)
            {
                //判斷是否為圖片
                if (!DessertImage2.ContentType.StartsWith("image"))
                {
                    ModelState.AddModelError("DessertImage", "只能上傳圖片類型");
                }
                //判斷圖片大小是否>0
                else if (DessertImage2.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(DessertImage2.FileName);
                    //存檔路徑
                    var path = Path.Combine(Server.MapPath("~/images/"), fileName);
                    //存檔
                    DessertImage2.SaveAs(path);
                    var editPath = Path.Combine(Server.MapPath("~/images/"), dessert.DessertImage);
                    if (System.IO.File.Exists(editPath))
                    {
                        System.IO.File.Delete(editPath);
                    }

                    dessert.DessertImage = fileName;
                }
            }
            //找尋原本資料
            var SearchDessert = db.Dessert.Find(dessert.DessertID);
            if (ModelState.IsValid)
            {
                //修改原本資料
                SearchDessert.DessertImage = dessert.DessertImage;
                SearchDessert.DessertIntroduction = dessert.DessertIntroduction;
                SearchDessert.DessertKind = dessert.DessertKind;
                SearchDessert.DessertName = dessert.DessertName;
                SearchDessert.DessertPrice = dessert.DessertPrice;
                SearchDessert.IsOnSale = dessert.IsOnSale;
                db.Entry(SearchDessert).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
              return  RedirectToAction("Index");
            }
            //下拉類別選單
            Dictionary<string, string> kind = new Dictionary<string, string>();
            kind.Add("Cookie", "餅乾");
            kind.Add("Cake", "蛋糕");
            kind.Add("Pie", "派");
            ViewBag.selectKind = new SelectList(kind, "key", "value", dessert.DessertKind);
            return View(dessert);
        }
        public ActionResult Upload()
        {
            //下拉類別選單
            Dictionary<string, string> kind = new Dictionary<string, string>();
            kind.Add("Cookie", "餅乾");
            kind.Add("Cake", "蛋糕");
            kind.Add("Pie", "派");
            ViewBag.selectKind = new SelectList(kind, "key", "value");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(Dessert dessert, HttpPostedFileBase DessertImage2)
        {
            //判斷是否有圖片上傳
            if (DessertImage2 != null)
            {
                // 判斷是否為圖片
                if (!DessertImage2.ContentType.StartsWith("image"))
                {
                    ModelState.AddModelError("DessertImage", "只能上傳圖片類型");
                }
                else if (DessertImage2.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(DessertImage2.FileName);
                    //存檔路徑
                    var path = Path.Combine(Server.MapPath("~/images/"), fileName);

                    DessertImage2.SaveAs(path);
                    dessert.DessertImage = fileName;
                }
            }
            else
            {
                ModelState.AddModelError("DessertImage", "請選擇圖片上傳");
            }
            //取得最新點心編號
            var searchDessert = db.Dessert.OrderByDescending(x => x.DessertID).FirstOrDefault();
            //產生點心編號
            dessert.DessertID = "D" + (Convert.ToInt32(searchDessert.DessertID.Substring(1, 3)) + 1).ToString("000");
            if (ModelState.IsValid)
            {
                db.Dessert.Add(new Dessert()
                {
                    DessertID = dessert.DessertID,
                    DessertImage = dessert.DessertImage,
                    DessertKind = dessert.DessertKind,
                    DessertIntroduction = dessert.DessertIntroduction,
                    DessertPrice = dessert.DessertPrice,
                    DessertName = dessert.DessertName,
                    IsOnSale = dessert.IsOnSale,
                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //下拉類別選單
            Dictionary<string, string> kind = new Dictionary<string, string>();
            kind.Add("Cookie", "餅乾");
            kind.Add("Cake", "蛋糕");
            kind.Add("Pie", "派");
            ViewBag.selectKind = new SelectList(kind, "key", "value", dessert.DessertKind);
            return View(dessert);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}
