using NonProfitCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NonProfitCRM.Controllers
{
    [Authorize(Roles = "SYSTEM_ADMINISTRATOR")]
    public class LogController : Controller
    {
        public ActionResult SecurityLog(string search)
        {
            if (search == null)
            {
                search = "";
            }

            var model = new Entities().Log.
                Where(e => search == "" || e.UserName.Contains(search) || e.Description.Contains(search) || e.Data.Contains(search)).
                OrderByDescending(e => e.Created).Take(NonProfitCRM.Properties.Settings.Default.MAXRECORDS);

            return View(model);
        }

        public ActionResult SecurityLogDetail(int id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            var model = new Entities().Log.Single(e => e.Id == id);

            return View(model);
        }
    }
}