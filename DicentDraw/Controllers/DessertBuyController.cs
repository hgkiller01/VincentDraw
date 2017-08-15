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
            //若沒有選擇禮盒 則加入
            if (Session["DessertCount"] == null)
            {
                Session["DessertCount"] = new List<AddDessertViewModel>();
            }

            return View();
        }
        public ActionResult Cancel()
        {
            //取消訂購清除SESSION
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
            //取得所有選擇點心
            var nowDessert = Session["DessertCount"] as List<AddDessertViewModel>;
            List<AddDessertViewModel> allDessert = new List<AddDessertViewModel>();
            foreach (var item in nowDessert)
            {
                var dessert = db.Dessert.Find(item.DessertID);
                //將資料轉換成AddDessertViewModel
                allDessert.Add(AddModel(dessert));
            }
            //顯示總金額
            int total = allDessert.Sum(x => x.DessertPrice * x.DessertAmount);
            ViewBag.Total = total;

        }
        [HttpPost]
        public ActionResult ShowSingleDetail(AddDessertViewModel order)
        {
            //取得所有已購買的點心
            var nowDessert = Session["DessertCount"] as List<AddDessertViewModel>;
            //判斷是否有購買任何點心
            if (!(nowDessert.Count() > 0))
            {
                return RedirectToAction("Index");
            }
            //產生訂單編號
            Random rad = new Random();
            string OrderID = DateTime.Now.ToString("yyyyMMddHHmmss") + rad.Next(999).ToString("000");
            //判斷是否有填地址
            if (string.IsNullOrEmpty(order.DeliveryAddress))
            {
                ModelState.AddModelError("DeliveryAddress", "請填寫送貨地址");
            }
            if (ModelState.IsValid)
            {
                //加入訂單主檔
                db.Orders.Add(new Orders()
                {
                    Account = User.Identity.Name,
                    DeliveryAddress = order.DeliveryAddress,
                    Orderstat = 1, // 處理中
                    OrderID = OrderID,
                    OrderDate = DateTime.Today.Date,
                });
                //加入訂單明細
                foreach (var item in nowDessert)
                {
                    //產生明細編號
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
                        GiftID = "G999",//G999為無禮盒
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
                //轉換成AddDessertViewModel
                allDessert.Add(AddModel(dessert));
            }
            //計算總價
            int total = allDessert.Sum(x => x.DessertPrice * x.DessertAmount);
            //顯示總價
            ViewBag.Total = total;
            return Json(allDessert.ToDataSourceResult(Request));
        }
        public ActionResult AddDessertCount(string id)
        {
            //判斷是否有ID輸入
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }
            //取得現在數量
            var NowAmount = Session["DessertCount"] as List<AddDessertViewModel>;
            int Amount = 0;
            //判斷此點心是否已經有選擇過了
            if (NowAmount.Where(x => x.DessertID == id).Count() > 0)
            {
                //取得目前點心的數量
                Amount = NowAmount.Where(x => x.DessertID == id).FirstOrDefault().DessertAmount;
            }
            //取得點心 回傳至頁面
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
                    //若此點心已經選擇過了 則改成修改數量
                    checkDessert.FirstOrDefault().DessertAmount = addDessert.DessertAmount;
                }
                else
                {
                    //此點心沒被選過 直接加入
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