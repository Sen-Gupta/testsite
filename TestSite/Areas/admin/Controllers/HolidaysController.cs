using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestSite.Areas.admin.Controllers
{
    public class HolidaysController : Controller
    {
        // GET: Holidays
        public ActionResult Index()
        {
            return View("~/Views/admin/Holidays/Index.cshtml");
        }
    }
}