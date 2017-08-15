using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc;
using DicentDraw.Models;
using DicentDraw.Models.ViewModels;
using CaptchaMvc.Controllers;
using CaptchaMvc.HtmlHelpers;
using System.Web.Security;
using DicentDraw.Code;

namespace DicentDraw.Controllers
{
    public class HomeController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();
        public ActionResult Index()
        {
            //test
            string showImage;
            Random rad = new Random();
            int alldessertCount = db.Dessert.Count(); //取得所有點心數目
            List<string> ImgFileName = new List<string>(); // 暫存點心檔名
            //一次顯示五張圖片
            for (int i = 0; ImgFileName.Count() < 5; i++)
            {
                //隨機取得檔名
                showImage = rad.Next(1, alldessertCount).ToString("000");
                ImgFileName.Add(db.Dessert.Where(x => x.DessertID == "D" + showImage).FirstOrDefault().DessertImage);
                //消去相同檔名
                ImgFileName = ImgFileName.Distinct().ToList();
            }  
            return View(ImgFileName);
        }
        public ActionResult Register()
        {
            //註冊頁面
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(MemberViewModel MemberData)
        {
            var checkAccount = db.member.Where(x => x.Account == MemberData.Account).Count();
            //判斷驗證碼是否正確
            if (this.IsCaptchaValid("驗證碼錯誤"))
            {
                //判斷帳號是否重復
                if (checkAccount < 0)
                {
                    ModelState.AddModelError("Account", "此帳號已經有人使用");
                }
                if (ModelState.IsValid)
                {
                    //新增會員
                    db.member.Add(new member()
                    {
                        Account = MemberData.Account,
                        Email = MemberData.Email,
                        Adress = MemberData.Adress,
                        Name = MemberData.Name,
                        Telphone = MemberData.Telphone,
                        PassWord = MemberData.PassWord,
                        isAdmin = false,
                    });
                    db.SaveChanges();
                    TempData["Success"] = "註冊成功，你現在可以登入了";
                    return RedirectToAction("Index", "MemberPage");
                }
            }

            return View(MemberData);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(MemberLoginViewModel login)
        {
            //判斷驗證碼是否正確
            if (this.IsCaptchaValid("驗證碼錯誤"))
            {
                //判斷輸入是否有誤
                if (ModelState.IsValid)
                {
                    //取得會員帳號
                    var loginMember = db.member.Find(login.Account);
                    //比對是否有此會員以及帳號的正確性
                    if (loginMember != null & loginMember.PassWord == login.PassWord)
                    {
                        //一般會員
                        string roles = "User";
                        //判斷是否為管理者
                        if (loginMember.isAdmin)
                        {
                            roles += ",Admin";
                        }
                        //加入Ticket
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                            loginMember.Account,
                            DateTime.Now,
                            DateTime.Now.AddMinutes(30),
                            true,
                            roles,//角色權限
                            FormsAuthentication.FormsCookiePath);
                        // Encrypt the ticket.
                        string encTicket = FormsAuthentication.Encrypt(ticket);

                        // Create the cookie.
                        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                        TempData["Message"] = "登入成功";
                        
                        if (loginMember.isAdmin)
                        {
                            //若是管理者 移至管理頁面
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                        return RedirectToAction("Index","MemberPage");
                    }
                    else
                    {
                        ModelState.AddModelError("Account", "帳號或密碼錯誤");
                    }
                }
            }
            return View(login);
        }
        public ActionResult Logout()
        {
            //登出
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            //清除Session
            Session.Clear();
            TempData["Message"] = "你已經登出";
            return RedirectToAction("Index");
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