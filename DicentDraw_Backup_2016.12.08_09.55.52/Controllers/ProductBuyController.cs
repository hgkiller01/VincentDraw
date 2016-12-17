using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DicentDraw.Controllers
{
    [Authorize(Roles="User")]
    public class ProductBuyController : Controller
    {
        // GET: ProductBuy
        public ActionResult Index()
        {
            return View();
        }
    }
}