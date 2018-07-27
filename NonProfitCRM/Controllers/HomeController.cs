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
        public ActionResult Dashboard(string search)
        {
            if (search == null)
            {
                search = "";
            }
            bool showDeleted = (Request.Cookies["nonprofitorgIsDelOn"]?.Value == "true");
            var cx = new Entities();
            var model = new HomeModel();
            model.Search = search.Trim();
            model.TaskList = cx.ViewTaskList.
                Where(e => e.StatusId < 1000).OrderBy(e=>e.DueDate);

            if (model.Search.Length > 0)
            {
                model.EventList = new Entities().ViewEventList.
                    Where(e => (search == "" ||
                        (e.CompanyName.StartsWith(search) ||
                            e.Name.StartsWith(search) ||
                            e.ContactCompanyName.StartsWith(search) ||
                            e.NonProfitOrgName.StartsWith(search) ||
                            e.ContactNonProfitOrgName.StartsWith(search)
                        ))
                        && (showDeleted || !e.Deleted)).
                    OrderByDescending(e => e.DateOfEvent).Take(Properties.Settings.Default.MAXRECORDS);

                model.CompanyList = new Entities().ViewCompanyList.
                    Where(e => (search == "" ||
                        (e.IdentificationNumber.StartsWith(search) ||
                            e.Name.StartsWith(search) ||
                            e.Address.StartsWith(search) ||
                            e.City.StartsWith(search) ||
                            e.Contact1Name.StartsWith(search) ||
                            e.Contact2Name.StartsWith(search) ||
                            e.CountryName.StartsWith(search) ||
                            e.RegionName.StartsWith(search) ||
                            e.Www.StartsWith(search)
                        )) && (showDeleted || !e.Deleted)).
                    OrderBy(e => e.Name).Take(Properties.Settings.Default.MAXRECORDS);

                model.NonProfitOrgList = new Entities().ViewNonProfitOrgList.
                    Where(e =>
                        (search == "" ||
                            (e.IdentificationNumber.StartsWith(search) ||
                                e.Name.StartsWith(search) ||
                                e.Address.StartsWith(search) ||
                                e.City.StartsWith(search) ||
                                e.Contact1Name.StartsWith(search) ||
                                e.Contact2Name.StartsWith(search) ||
                                e.CountryName.StartsWith(search) ||
                                e.RegionName.StartsWith(search) ||
                                e.Www.StartsWith(search)
                            )
                        ) && (showDeleted || !e.Deleted)).
                    OrderBy(e => e.Name).Take(Properties.Settings.Default.MAXRECORDS);
            }

            return View(model);
        }
    }
}
