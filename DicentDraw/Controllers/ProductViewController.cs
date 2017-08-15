using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DicentDraw.Models;
using DicentDraw.Models.ViewModels;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace DicentDraw.Controllers
{
    public class ProductViewController : Controller
    {
        
        protected ShopDBEntities db = new ShopDBEntities();
        // GET: ProductView
        public virtual ActionResult Index(int page = 1)
        {
            return View();
        }
        public ActionResult Cookie_Read([DataSourceRequest] DataSourceRequest request)
        {
            //取得所有販賣中的餅乾
            var Dessert = db.Dessert.Where(x => x.DessertKind == "Cookie" && x.IsOnSale);
            List<AddDessertViewModel> CookieDessert = new List<AddDessertViewModel>();
                foreach (var item in Dessert)
                {
                    //加入ViewModel
                    CookieDessert.Add(AddModel(item));
                }
                 //回傳Kedo內定的JSON
            return Json(CookieDessert.ToDataSourceResult(request));
        }
        public ActionResult Cake_Read([DataSourceRequest] DataSourceRequest request)
        {
            //取得所有販賣中的蛋糕
            var Dessert = db.Dessert.Where(x => x.DessertKind == "Cake" && x.IsOnSale);
            List<AddDessertViewModel> CakeDessert = new List<AddDessertViewModel>();
                foreach (var item in Dessert)
                {
                   //加入ViewModel
                    CakeDessert.Add(AddModel(item));
                }
                //回傳Kedo內定的JSON
            return Json(CakeDessert.ToDataSourceResult(request));
        }
        public ActionResult Pie_Read([DataSourceRequest] DataSourceRequest request)
        {
            //取得所有販賣中的派
            var Dessert = db.Dessert.Where(x => x.DessertKind == "Pie" && x.IsOnSale);
            List<AddDessertViewModel> PieDessert = new List<AddDessertViewModel>();
                foreach (var item in Dessert)
                {
                    //加入ViewModel
                    PieDessert.Add(AddModel(item));
                }
            //回傳Kedo內定的JSON
            return Json(PieDessert.ToDataSourceResult(request));
        }
        public AddDessertViewModel AddModel(Dessert item)
        {
            //目前選擇的點心
            List<AddDessertViewModel> allCout = Session["DessertCount"] as List<AddDessertViewModel>;
            int Amount = 0;
            //判斷是否有選擇過此點心則讀取Session
            if (allCout != null)
            {
                //取出目前要顯示的點心
                var dessertCount = allCout.Where(x => x.DessertID == item.DessertID);
                //若數量>0則顯示數量
                if (dessertCount.Count() > 0)
                {
                    Amount = dessertCount.FirstOrDefault().DessertAmount;
                } 
            }
            //回傳單一點心資料
            return new AddDessertViewModel()
            {
                DessertID = item.DessertID,
                DessertImage = item.DessertImage,
                DessertIntroduction = item.DessertIntroduction,
                DessertKind = item.DessertKind,
                DessertName = item.DessertName,
                DessertPrice = item.DessertPrice,
                DessertAmount = Amount
            };
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