using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DicentDraw.Models;
using DicentDraw.Models.ViewModels;
using PagedList;
using System.Data.Entity;

namespace DicentDraw.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class OrderListController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();
        // GET: Admin/OrderList
        public ActionResult Index(int page=1)
        {
           var result = db.Orders.OrderBy(x => x.OrderDate);
            return View(result.ToPagedList(page,5));
        }
        public ActionResult Index2(int page=1)
        {
            var result = db.Orders.OrderBy(x => x.OrderDate);
            return PartialView("_Table", result.ToPagedList(page, 5));
        }
        public ActionResult Edit(string id)
        {
            var order = db.Orders.Find(id);
            var Status = new Dictionary<int, string>();
            Status.Add(1, "處理中");
            Status.Add(2, "已送貨");
            Status.Add(3, "取消");
            ViewBag.Status = new SelectList(Status, "key", "value",order.Orderstat);
            return PartialView(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2(string OrderID , string Select)
        {
            var selectOrder = db.Orders.Where(x => x.OrderID == OrderID).FirstOrDefault();
            selectOrder.Orderstat = Convert.ToInt32(Select);
            db.Entry(selectOrder).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ListDetail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }
            var detail = db.OrderDetails.Where(x => x.OrderID == id);
            return PartialView(detail);
        }
    }
}