using NonProfitCRM.Components;
using NonProfitCRM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NonProfitCRM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}
