using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DicentDraw.Models;
using PagedList;

namespace DicentDraw.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class MessageBoardController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();
        // GET: Admin/MessageBoard
        public ActionResult Index(int page =1)
        {
            var message = db.Message.OrderBy(x => x.MessageID);
            return View(message.ToPagedList(page,5));
        }
    }
}