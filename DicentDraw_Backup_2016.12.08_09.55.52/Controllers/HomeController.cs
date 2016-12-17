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
            int alldessertCount = db.Dessert.Count();
            List<string> ImgFileName = new List<string>();
            for (int i = 0; ImgFileName.Count() < 5; i++)
            {
                showImage = rad.Next(1, alldessertCount).ToString("000");
                ImgFileName.Add(db.Dessert.Where(x => x.DessertID == "D" + showImage).FirstOrDefault().DessertImage);
                ImgFileName = ImgFileName.Distinct().ToList();
            }  
            return View(ImgFileName);
        }
        public ActionResult Register()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(MemberViewModel MemberData)
        {
            var checkAccount = db.member.Where(x => x.Account == MemberData.Account).Count();
            if (this.IsCaptchaValid("驗證碼錯誤"))
            {
                if (checkAccount < 0)
                {
                    ModelState.AddModelError("Account", "此帳號已經有人使用");
                }
                if (ModelState.IsValid)
                {
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
            if (this.IsCaptchaValid("驗證碼錯誤"))
            {
                if (ModelState.IsValid)
                {
                    var loginMember = db.member.Find(login.Account);
                    if (loginMember != null & loginMember.PassWord == login.PassWord)
                    {
                        string roles = "User";
                        if (loginMember.isAdmin)
                        {
                            roles += ",Admin";
                        }
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                            loginMember.Account,
                            DateTime.Now,
                            DateTime.Now.AddMinutes(30),
                            true,
                            roles,
                            FormsAuthentication.FormsCookiePath);
                        // Encrypt the ticket.
                        string encTicket = FormsAuthentication.Encrypt(ticket);

                        // Create the cookie.
                        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                        TempData["LoginSucess"] = "登入成功";
                        if (loginMember.isAdmin)
                        {
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
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            Session.Clear();
            ViewData["Logout"] = "你已經登出";
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