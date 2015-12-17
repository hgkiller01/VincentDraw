using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DicentDraw.Models.ViewModels;
using DicentDraw.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace DicentDraw.Controllers
{
    [Authorize(Roles = "User")]
    public class DessertBuyController : ProductViewController
    {
        //protected ShopDBEntities db = new ShopDBEntities();
        // GET: ProductView
        public override ActionResult Index(int page = 1)
        {
            if (Session["DessertCount"] == null)
            {
                Session["DessertCount"] = new List<AddDessertViewModel>();
            }

            return View();
        }
        public ActionResult Cancel()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ShowSingleDetail()
        {
            ShowTotal();
            return View();
        }
        public void ShowTotal()
        {
            var nowDessert = Session["DessertCount"] as List<AddDessertViewModel>;
            List<AddDessertViewModel> allDessert = new List<AddDessertViewModel>();
            foreach (var item in nowDessert)
            {
                var dessert = db.Dessert.Find(item.DessertID);
                allDessert.Add(AddModel(dessert));
            }
            int total = allDessert.Sum(x => x.DessertPrice * x.DessertAmount);
            ViewBag.Total = total;

        }
        [HttpPost]
        public ActionResult ShowSingleDetail(AddDessertViewModel order)
        {
            var nowDessert = Session["DessertCount"] as List<AddDessertViewModel>;
            if (!(nowDessert.Count() > 0))
            {
                return RedirectToAction("Index");
            }
            Random rad = new Random();
            string OrderID = DateTime.Now.ToString("yyyyMMddHHmmss") + rad.Next(999).ToString("000");
            if (string.IsNullOrEmpty(order.DeliveryAddress))
            {
                ModelState.AddModelError("DeliveryAddress", "請填寫送貨地址");
            }
            if (ModelState.IsValid)
            {
                db.Orders.Add(new Orders()
                {
                    Account = User.Identity.Name,
                    DeliveryAddress = order.DeliveryAddress,
                    Orderstat = 1,
                    OrderID = OrderID,
                    OrderDate = DateTime.Today.Date,
                });
                foreach (var item in nowDessert)
                {
                    string detailID = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var orderDetail = db.OrderDetails.Where(x => x.DetailID.StartsWith(detailID));

                    if (orderDetail.Count() > 0)
                    {
                        detailID = detailID + (Convert.ToInt32(orderDetail.FirstOrDefault().DetailID.Substring(13, 4)) + 1).ToString("0000");
                    }
                    else
                    {
                        detailID = detailID + rad.Next(9999).ToString("0000");
                    }
                    db.OrderDetails.Add(new OrderDetails()
                    {
                        DessertID = item.DessertID,
                        DetailID = detailID,
                        DessertAmount = item.DessertAmount,
                        GiftID = "G999",
                        OrderID = OrderID,
                    });
                    db.SaveChanges();
                }
                TempData["BuySuccess"] = "購買成功";
                Session.Clear();
                return RedirectToAction("Index");
            }
            ShowTotal();
            return View(order);
        }
        public ActionResult Read_SingleDetail([DataSourceRequest]DataSourceRequest Request)
        {
            var nowDessert = Session["DessertCount"] as List<AddDessertViewModel>;
            List<AddDessertViewModel> allDessert = new List<AddDessertViewModel>();
            foreach (var item in nowDessert)
            {
                var dessert = db.Dessert.Find(item.DessertID);
                allDessert.Add(AddModel(dessert));
            }
            int total = allDessert.Sum(x => x.DessertPrice * x.DessertAmount);
            ViewBag.Total = total;
            return Json(allDessert.ToDataSourceResult(Request));
        }
        public ActionResult AddDessertCount(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }
            var NowAmount = Session["DessertCount"] as List<AddDessertViewModel>;
            int Amount = 0;
            if (NowAmount.Where(x => x.DessertID == id).Count() > 0)
            {
                Amount = NowAmount.Where(x => x.DessertID == id).FirstOrDefault().DessertAmount;
            }
            var selectDessert = db.Dessert.Find(id);
            AddDessertViewModel addDessert = AddModel(selectDessert);
            return View(addDessert);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDessertCount(AddDessertViewModel addDessert)
        {
            if (ModelState.IsValid)
            {
                List<AddDessertViewModel> BuyDessert = Session["DessertCount"] as List<AddDessertViewModel>;
                var checkDessert = BuyDessert.Where(x => x.DessertID == addDessert.DessertID);
                if (checkDessert.Count() > 0)
                {
                    checkDessert.FirstOrDefault().DessertAmount = addDessert.DessertAmount;
                }
                else
                {
                    BuyDessert.Add(addDessert);
                }

                return RedirectToAction("Index");
            }
            var selectDessert = db.Dessert.Find(addDessert.DessertID);
            addDessert = AddModel(selectDessert);
            return View(addDessert);
        }
    }
}