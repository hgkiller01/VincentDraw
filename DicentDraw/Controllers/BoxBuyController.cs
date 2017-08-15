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
    [Authorize(Roles="User")]//判斷是否登入
    public class BoxBuyController : ProductViewController
    {
        // GET: BoxBuy
        public override ActionResult Index(int page =1)
        {
            //判斷點心數量Session是否建立 
            if (Session["DessertCount"] == null)
            {
                Session["DessertCount"] = new List<AddDessertViewModel>();
            }
            //取出所有禮盒 除了G999無禮盒除外
            IEnumerable<Gift> gift = db.Gift.Where(x => x.GiftID != "G999" && x.IsOnSales).OrderBy(x => x.GiftID);
            
            return View(gift.ToPagedList(page, 5));
        }
        public ActionResult ShowDessert()
        {
            //若沒選擇禮盒 則回到選擇畫面
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
            //取得禮盒
            var gift = db.Gift.Find(GiftID);
            var dessertCount = Session["DessertCount"] as List<AddDessertViewModel>;
            //若已經有禮盒 則清空
            if (Session["Gift"] != null)
            {
                Session["Gift"] = null;
                dessertCount.Clear();
            }
            //若禮盒已經清空 則重新輸入禮盒
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
            //判斷是否傳入ID
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }
            //取出所有選擇的點心
            var NowAmount = Session["DessertCount"] as List<AddDessertViewModel>;
            int Amount = 0;
            //如果有選擇過則修改數量
            if (NowAmount.Where(x => x.DessertID == id).Count() > 0)
            {
                Amount = NowAmount.Where(x => x.DessertID == id).FirstOrDefault().DessertAmount;
            }
            //取得目前禮盒
            AddDessertViewModel GiftModel = Session["Gift"] as AddDessertViewModel;
            var dessert = db.Dessert.Find(id);
            AddDessertViewModel showDessert = AddModel(dessert);
            //將此禮盒各種類點心的數量傳至頁面
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
            //取得目前所有點心
            List<AddDessertViewModel> BuyDessert = Session["DessertCount"] as List<AddDessertViewModel>;
            //取得目前點心類別的數量
            int totalDessert = BuyDessert.Where(x => x.DessertKind == addDessert.DessertKind).Sum(x => x.DessertAmount);
            //目前類別為餅乾
            if (addDessert.DessertKind == "Cookie")
                {
                    if (addDessert.DessertAmount + totalDessert > addDessert.CookieCount)
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
            //目前類別為派
            if (addDessert.DessertKind == "Pie")
                {
                    if (addDessert.DessertAmount + totalDessert > addDessert.PieCount)
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
            //目前類別為蛋糕
            if (addDessert.DessertKind == "Cake")
                {
                    if (addDessert.DessertAmount + totalDessert > addDessert.CakeCount)
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
            //判斷Model輸入準確性
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
            //取得目前點心資料
            var selectDessert = db.Dessert.Find(addDessert.DessertID);
            //轉為AddDessertViewModel
            addDessert = AddModel(selectDessert);
            AddDessertViewModel GiftModel = Session["Gift"] as AddDessertViewModel;
            //將此禮盒各種類點心的數量傳至頁面
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
            //目前所選取點心
            var nowDessert = Session["DessertCount"] as List<AddDessertViewModel>;
            //目前所選取禮盒
            var nowGift = Session["Gift"] as AddDessertViewModel;
            List<AddDessertViewModel> allDessert = new List<AddDessertViewModel>();
            //將所有點心資料轉為AddDessertViewModel
            foreach (var item in nowDessert)
            {
                var dessert = db.Dessert.Find(item.DessertID);
                allDessert.Add(AddModel(dessert));
            }
            //計算所有價格
            int total = allDessert.Sum(x => x.DessertPrice * x.DessertAmount);
            //禮盒名稱以及價格
            nowGift.GiftName = db.Gift.Find(nowGift.GiftID).GiftName;
            nowGift.GiftPrice = db.Gift.Find(nowGift.GiftID).GiftPrice;
            ViewBag.Box = nowGift.GiftName;
            ViewBag.Total = total + nowGift.GiftPrice;

        }
        public ActionResult Cancel()
        {
            Session.Clear();
            return RedirectToAction("index");
        }
        [HttpPost]
        public ActionResult ShowBoxDetail(AddDessertViewModel order)
        {
            //取出選擇的點心及禮盒
            var nowDessert = Session["DessertCount"] as List<AddDessertViewModel>;
            var nowGift = Session["Gift"] as AddDessertViewModel;
            //產生訂單編號
            Random rad = new Random();
            string OrderID = DateTime.Now.ToString("yyyyMMddHHmmss") + rad.Next(999).ToString("000");
            //判斷是否有地址
            if (string.IsNullOrEmpty(order.DeliveryAddress))
            {
                ModelState.AddModelError("DeliveryAddress", "請填寫送貨地址");
            }
            #region
            if (ModelState.IsValid)
            {
                //加入訂單主檔
                db.Orders.Add(new Orders()
                {
                    Account = User.Identity.Name,
                    DeliveryAddress = order.DeliveryAddress,
                    Orderstat = 1,//處理中
                    OrderID = OrderID,
                    OrderDate = DateTime.Today.Date,
                });
                //加入訂單明細
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
            //取出選擇的點心及禮盒
            var nowDessert = Session["DessertCount"] as List<AddDessertViewModel>;
            var nowGift = Session["Gift"] as AddDessertViewModel;
            List<AddDessertViewModel> allDessert = new List<AddDessertViewModel>();
            //點心轉為AddDessertViewModel
            foreach (var item in nowDessert)
            {
                var dessert = db.Dessert.Find(item.DessertID);
                allDessert.Add(AddModel(dessert));
            }
            //計算總價
            int total = allDessert.Sum(x => x.DessertPrice * x.DessertAmount);
            nowGift.GiftName = db.Gift.Find(nowGift.GiftID).GiftName;
            //顯示禮盒與總價
            ViewBag.Box = nowGift.GiftName;
            ViewBag.Total = total + nowGift.GiftPrice;
            return Json(allDessert.ToDataSourceResult(Request));
        }
    }
}