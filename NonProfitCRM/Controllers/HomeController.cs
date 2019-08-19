/*
* MIT License
* 
* Copyright (c) 2015 - present valda-z
* 
* All rights reserved.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using NonProfitCRM.Components;
using NonProfitCRM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace NonProfitCRM.Controllers
{
    [Authorize(Roles = "FRD")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public void SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
                new Microsoft.Owin.Security.AuthenticationProperties { RedirectUri = System.Configuration.ConfigurationManager.AppSettings["RedirectUri"] },
                OpenIdConnectAuthenticationDefaults.AuthenticationType,
                CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult Dashboard(string search)
        {
            if (search == null)
            {
                search = "";
            }
            bool showDeleted = (Request.Cookies["nonprofitorgIsDelOn"]?.Value == "true");
            bool showOnlyMy = !(Request.Cookies["nonprofitorgIsMyTaskOn"]?.Value == "false");

            var cx = new Entities();
            var model = new HomeModel();
            model.Search = search.Trim();
            model.TaskList = cx.ViewTaskList.
                Where(
                    e => e.StatusId < 1000 &&
                    e.Entity != "Event" &&
                    (!showOnlyMy || e.AssignedTo == NonProfitCRM.Components.SystemHelper.GetUserName)
                    ).OrderBy(e=>e.DueDate);

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

            model.StatsEvents = StatisticsHelper.GetEventStatWidget();
            model.StatsEventsPeople = StatisticsHelper.GetEventPeopleStatWidget();
            model.StatsCRM = StatisticsHelper.GetCRMStatWidget();

            model.PieEventType = StatisticsHelper.GetPieEventType();
            model.PieNPOEventType = StatisticsHelper.GetPieNPOEventType();

            return View(model);
        }
    }
}
