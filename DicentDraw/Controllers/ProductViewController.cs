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
            var Dessert = db.Dessert.Where(x => x.DessertKind == "Cookie" && x.IsOnSale);
            List<AddDessertViewModel> CookieDessert = new List<AddDessertViewModel>();
                foreach (var item in Dessert)
                {
                    CookieDessert.Add(AddModel(item));
                }

            return Json(CookieDessert.ToDataSourceResult(request));
        }
        public ActionResult Cake_Read([DataSourceRequest] DataSourceRequest request)
        {
            var Dessert = db.Dessert.Where(x => x.DessertKind == "Cake" && x.IsOnSale);
            List<AddDessertViewModel> CakeDessert = new List<AddDessertViewModel>();
                foreach (var item in Dessert)
                {
                    CakeDessert.Add(AddModel(item));
                }

            return Json(CakeDessert.ToDataSourceResult(request));
        }
        public ActionResult Pie_Read([DataSourceRequest] DataSourceRequest request)
        {
            var Dessert = db.Dessert.Where(x => x.DessertKind == "Pie" && x.IsOnSale);
            List<AddDessertViewModel> PieDessert = new List<AddDessertViewModel>();
                foreach (var item in Dessert)
                {
                    PieDessert.Add(AddModel(item));
                }

            return Json(PieDessert.ToDataSourceResult(request));
        }
        public AddDessertViewModel AddModel(Dessert item)
        {
            List<AddDessertViewModel> allCout = Session["DessertCount"] as List<AddDessertViewModel>;
            int Amount = 0;
            if (allCout != null)
            {
                var dessertCount = allCout.Where(x => x.DessertID == item.DessertID);
                if (dessertCount.Count() > 0)
                {
                    Amount = dessertCount.FirstOrDefault().DessertAmount;
                } 
            }
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