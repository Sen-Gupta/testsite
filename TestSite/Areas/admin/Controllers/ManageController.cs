using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestSite.Areas.admin.Controllers
{
    [RouteArea("admin")]
    public class ManageController : Controller
    {
        // GET: admin/Manage
        public ActionResult Index()
        {
            return View();
        }
    }
}