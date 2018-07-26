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
            var cx = new Entities();
            var model = new HomeModel();
            model.TaskList = cx.ViewTaskList.
                Where(e => e.StatusId < 1000).OrderBy(e=>e.DueDate);
            return View(model);
        }
    }
}
