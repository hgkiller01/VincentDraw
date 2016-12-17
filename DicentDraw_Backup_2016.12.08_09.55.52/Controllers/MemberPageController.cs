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
            var memberAccount = db.member.Where(x => x.Account == memberData.Account).FirstOrDefault();
            if (memberAccount.PassWord != memberData.PassWord)
            {
                ModelState.AddModelError("PassWord", "密碼不正確");
            }
            if (ModelState.IsValid)
            {
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
            var result = db.Orders.Where(x => x.Account == User.Identity.Name).OrderBy(x => x.OrderID);
           return View(result.ToPagedList(page,5));
        }
        public ActionResult ListDetail(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
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