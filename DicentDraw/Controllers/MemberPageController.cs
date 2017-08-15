using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DicentDraw.Models.ViewModels;
using DicentDraw.Models;
using System.Data.Entity;
using PagedList;

namespace DicentDraw.Controllers
{
    [Authorize(Roles="User")]
    public class MemberPageController : Controller
    {
        // GET: MemberPage
        private ShopDBEntities db = new ShopDBEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChangeData()
        {
            //取得會員資料
           var memberAccount = db.member.Where(x => x.Account == User.Identity.Name).FirstOrDefault();
           MemberDataViewModel memberData = new MemberDataViewModel()
           {
               Account = memberAccount.Account,
               Adress = memberAccount.Adress,
               Email = memberAccount.Email,
               Name = memberAccount.Name,
               Telphone = memberAccount.Telphone,
           };
            
            return View(memberData);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeData(MemberDataViewModel memberData)
        {
            //取得會員資料
            var memberAccount = db.member.Where(x => x.Account == memberData.Account).FirstOrDefault();
            //判斷密碼是否正確
            if (memberAccount.PassWord != memberData.PassWord)
            {
                ModelState.AddModelError("PassWord", "密碼不正確");
            }
            if (ModelState.IsValid)
            {
                //更改會員資料
                memberAccount.Adress = memberData.Adress;
                memberAccount.Email = memberData.Email;
                memberAccount.Name = memberData.Name;
                memberAccount.Telphone = memberData.Telphone;
                memberAccount.PassWord = memberData.PassWord2;
                db.Entry(memberAccount).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Change"] = "更改成功";
                memberData.PassWord = "";
                memberData.PassWord2 = "";
                return View(memberData);
            }
            return View(memberData);
        }
        public ActionResult OrderList(int page = 1)
        {
            //取得所有訂單資料
            var result = db.Orders.Where(x => x.Account == User.Identity.Name).OrderBy(x => x.OrderID);
            var result2 = result.ToList();
           return View(result.ToList().ToPagedList(page,5));
        }
        public ActionResult ListDetail(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            //取得訂單明細
            var orderdetail = db.OrderDetails.Where(x => x.OrderID == id);
            return PartialView(orderdetail.ToList());
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