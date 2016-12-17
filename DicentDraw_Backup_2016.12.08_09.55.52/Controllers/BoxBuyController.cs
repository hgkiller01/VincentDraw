using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using DicentDraw.Models;
using DicentDraw.Models.ViewModels;
using PagedList;

namespace DicentDraw.Controllers
{
    [Authorize(Roles="User")]
    public class BoxBuyController : ProductViewController
    {
        // GET: BoxBuy
        public override ActionResult Index(int page =1)
        {
            if (Session["DessertCount"] == null)
            {
                Session["DessertCount"] = new List<AddDessertViewModel>();
            }
            IEnumerable<Gift> gift = db.Gift.Where(x => x.GiftID != "G999" && x.IsOnSales).OrderBy(x => x.GiftID);
            
            return View(gift.ToPagedList(page, 5));
        }
        public ActionResult ShowDessert()
        {
            if (Session["Gift"] == null)
            {
                return RedirectToAction("Index");
            }
            AddDessertViewModel GiftModel = Session["Gift"] as AddDessertViewModel;
            return View(GiftModel);
        }
        [HttpPost]
        public ActionResult ShowDessert(string GiftID)
        {
            var gift = db.Gift.Find(GiftID);
            var dessertCount = Session["DessertCount"] as List<AddDessertViewModel>;
            if (Session["Gift"] != null)
            {
                Session["Gift"] = null;
                dessertCount.Clear();
            }
            if (Session["Gift"] == null)
            {
                Session["Gift"] = new AddDessertViewModel()
                {
                    GiftID = gift.GiftID,
                    PieCount = gift.PieCount,
                    CookieCount = gift.CookieCount,
                    CakeCount = gift.CakeCount
                };
            }
            AddDessertViewModel GiftModel = Session["Gift"] as AddDessertViewModel;
            return View(GiftModel);
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
            AddDessertViewModel GiftModel = Session["Gift"] as AddDessertViewModel;
            var dessert = db.Dessert.Find(id);
            AddDessertViewModel showDessert = AddModel(dessert);
            showDessert.GiftID = GiftModel.GiftID;
            showDessert.PieCount = GiftModel.PieCount;
            showDessert.CookieCount = GiftModel.CookieCount;
            showDessert.CakeCount = GiftModel.CakeCount;
            return View(showDessert);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDessertCount(AddDessertViewModel addDessert)
        {
            List<AddDessertViewModel> BuyDessert = Session["DessertCount"] as List<AddDessertViewModel>;
            int totalCookie = BuyDessert.Where(x => x.DessertKind == addDessert.DessertKind).Sum(x => x.DessertAmount);
                if (addDessert.DessertKind == "Cookie")
                {
                    if (addDessert.DessertAmount + totalCookie > addDessert.CookieCount)
                    {
                        if (addDessert.CookieCount == 0)
                        {
                            ModelState.AddModelError("DessertAmount", "此禮盒不能選餅乾");
                        }
                        else
                        {
                            ModelState.AddModelError("DessertAmount", "餅乾的數量超過" + addDessert.CookieCount + "個");
                        }
                        
                    }
                }
                if (addDessert.DessertKind == "Pie")
                {
                    if (addDessert.DessertAmount + totalCookie > addDessert.PieCount)
                    {
                        if (addDessert.PieCount == 0)
                        {
                            ModelState.AddModelError("DessertAmount", "此禮盒不能選派");
                        }
                        else
                        {
                            ModelState.AddModelError("DessertAmount", "派的數量超過" + addDessert.PieCount + "個");
                        }
                       
                    }
                }
                if (addDessert.DessertKind == "Cake")
                {
                    if (addDessert.DessertAmount + totalCookie > addDessert.CakeCount)
                    {
                        if (addDessert.CakeCount == 0)
                        {
                            ModelState.AddModelError("DessertAmount", "此禮盒不能選蛋糕");
                        }
                        else
                        {
                            ModelState.AddModelError("DessertAmount", "蛋糕的數量超過" + addDessert.CakeCount + "個");
                        }
                        
                    }
                }
            if (ModelState.IsValid)
            {
                var checkDessert = BuyDessert.Where(x => x.DessertID == addDessert.DessertID);
                if (checkDessert.Count() > 0)
                {
                    checkDessert.FirstOrDefault().DessertAmount = addDessert.DessertAmount;
                }
                else
                {
                    BuyDessert.Add(addDessert);
                }

                return RedirectToAction("ShowDessert");
            }
            var selectDessert = db.Dessert.Find(addDessert.DessertID);
            addDessert = AddModel(selectDessert);
            AddDessertViewModel GiftModel = Session["Gift"] as AddDessertViewModel;
            addDessert.GiftID = GiftModel.GiftID;
            addDessert.PieCount = GiftModel.PieCount;
            addDessert.CookieCount = GiftModel.CookieCount;
            addDessert.CakeCount = GiftModel.CakeCount;
            return View(addDessert);
        }
        public ActionResult ShowBoxDetail()
        {
            ShowTotal();
            return View();
        }
        public void ShowTotal()
        {
            var nowDessert = Session["DessertCount"] as List<AddDessertViewModel>;
            var nowGift = Session["Gift"] as AddDessertViewModel;
            List<AddDessertViewModel> allDessert = new List<AddDessertViewModel>();
            foreach (var item in nowDessert)
            {
                var dessert = db.Dessert.Find(item.DessertID);
                allDessert.Add(AddModel(dessert));
            }
            int total = allDessert.Sum(x => x.DessertPrice * x.DessertAmount);
            nowGift.GiftName = db.Gift.Find(nowGift.GiftID).GiftName;
            nowGift.GiftPrice = db.Gift.Find(nowGift.GiftID).GiftPrice;
            ViewBag.Box = nowGift.GiftName;
            ViewBag.Total = total + nowGift.GiftPrice;

        }
        [HttpPost]
        public ActionResult ShowBoxDetail(AddDessertViewModel order)
        {
            var nowDessert = Session["DessertCount"] as List<AddDessertViewModel>;
            var nowGift = Session["Gift"] as AddDessertViewModel;
            Random rad = new Random();
            string OrderID = DateTime.Now.ToString("yyyyMMddHHmmss") + rad.Next(999).ToString("000");
            if (string.IsNullOrEmpty(order.DeliveryAddress))
            {
                ModelState.AddModelError("DeliveryAddress", "請填寫送貨地址");
            }
            #region
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
                    do
                    {
                        detailID = detailID + rad.Next(9999).ToString("0000");
                    } while (db.OrderDetails.Where(x => x.DetailID == detailID).Count() > 0);
                    
                    db.OrderDetails.Add(new OrderDetails()
                    {
                        DessertID = item.DessertID,
                        DetailID = detailID,
                        DessertAmount = item.DessertAmount,
                        GiftID = nowGift.GiftID,
                        OrderID = OrderID,
                    });
                    db.SaveChanges();
                }
                TempData["BuySuccess"] = "購買成功";
                Session.Clear();
                return RedirectToAction("Index");
            }
            #endregion
            ShowTotal();
            return View(order);
        }
        public ActionResult Read_BoxDetail([DataSourceRequest]DataSourceRequest Request)
        {
            var nowDessert = Session["DessertCount"] as List<AddDessertViewModel>;
            var nowGift = Session["Gift"] as AddDessertViewModel;
            List<AddDessertViewModel> allDessert = new List<AddDessertViewModel>();
            foreach (var item in nowDessert)
            {
                var dessert = db.Dessert.Find(item.DessertID);
                allDessert.Add(AddModel(dessert));
            }
            int total = allDessert.Sum(x => x.DessertPrice * x.DessertAmount);
            nowGift.GiftName = db.Gift.Find(nowGift.GiftID).GiftName;
            ViewBag.Box = nowGift.GiftName;
            ViewBag.Total = total + nowGift.GiftPrice;
            return Json(allDessert.ToDataSourceResult(Request));
        }
    }
}