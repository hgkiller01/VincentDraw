using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DicentDraw.Models;

namespace DicentDraw.Controllers
{
    public class MessageBoardController : Controller
    {
       private ShopDBEntities db = new ShopDBEntities();
        // GET: MessageBoard
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Message message)
        {
            if (ModelState.IsValid)
            {
                db.Message.Add(message);
                db.SaveChanges();
                 TempData["Message"] = "謝謝您提供寶貴的意見";
                return RedirectToAction("Index", "Home");
            }
            return View(message);
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