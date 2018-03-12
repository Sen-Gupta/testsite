using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestSite.Areas.admin.Controllers
{
    public class BikesController : Controller
    {
        // GET: Bikes
        public ActionResult Index()
        {
            return View("~/Views/admin/Bikes/Index.cshtml");
        }
    }
}