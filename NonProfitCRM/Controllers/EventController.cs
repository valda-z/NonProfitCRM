using NonProfitCRM.Components;
using NonProfitCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace NonProfitCRM.Controllers
{
    public class EventController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ViewBag.CanEdit = true;
        }

        [HttpGet]
        public ActionResult List(string search)
        {
            if (search == null)
            {
                search = "";
            }

            bool showDeleted = (Request.Cookies["nonprofitorgIsDelOn"]?.Value == "true");

            var model = new Entities().ViewEventList.
                Where(e => (search == "" ||
                    (e.CompanyName.StartsWith(search) ||
                        e.Name.StartsWith(search) ||
                        e.ContactCompanyName.StartsWith(search) ||
                        e.NonProfitOrgName.StartsWith(search) ||
                        e.ContactNonProfitOrgName.StartsWith(search)
                    ))
                    && (showDeleted || !e.Deleted)).
                OrderByDescending(e => e.DateOfEvent).Take(Properties.Settings.Default.MAXRECORDS);

            return View(model);
        }

        [HttpGet]
        public ActionResult Detail(int id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            Event model;
            if (id == 0)
            {
                model = new Event();
            }
            else
            {
                model = new Entities().Event.Single(e => e.Id == id);
            }
            if (model.Deleted)
            {
                ViewBag.CanEdit = false;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Detail(int id, Event model, string returnUrl, FormCollection m)
        {
            ViewBag.ReturnUrl = returnUrl;

            bool isClosed = false;
            if (m["closed"] != null && m["closed"].ToString() == "true")
            {
                isClosed = true;
            }

            bool isokA = false;
            var dtm = DateHelper.ConvertDate(m["DateOfEventRaw"], out isokA);
            if (!isokA || dtm == null)
            {
                ModelState.AddModelError("", "Chybný formát data pro Datum Akce.");
            }
            if (!isokA)
            {
                return View(model);
            }

            using (var scope = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                }))
            {

                Entities cx = new Entities();
                model.DateOfEvent = dtm.Value;

                //update NonProfitOrg
                Event p = null;
                try
                {
                    if (model.Id == 0)
                    {
                        p = model;
                        p.Closed = (isClosed ? new DateTime?(DateTime.UtcNow) : null);
                        cx.Event.Add(p);
                        p.Updated = DateTime.UtcNow;
                        p.UpdatedBy = User.Identity.Name;
                        Logger.Log(User.Identity.Name, "Create Event " + model.Name + " / " + p.DateOfEvent.ToShortDateString(), null, model,
                            "Event", p.Id);
                        cx.SaveChanges();
                        foreach(var obj in new 
                            TaskTemplate().GetTasks(User.Identity.Name, p.DateOfEvent, p.Id))
                        {
                            cx.Task.Add(obj);
                            cx.SaveChanges();
                        }
                    }
                    else
                    {
                        p = cx.Event.Single(e => e.Id == id);
                        string _oldObject = Logger.Serialize(p);
                        if (!(isClosed && p.Closed != null))
                        {
                            p.Closed = (isClosed ? new DateTime?(DateTime.UtcNow) : null);
                        }
                        p.Capacity = model.Capacity;
                        p.CompanyId = model.CompanyId;
                        p.ContactCompanyEmail = model.ContactCompanyEmail;
                        p.ContactCompanyName = model.ContactCompanyName;
                        p.ContactCompanyNote = model.ContactCompanyNote;
                        p.ContactCompanyPhone = model.ContactCompanyPhone;
                        p.ContactNonProfitOrgEmail = model.ContactNonProfitOrgEmail;
                        p.ContactNonProfitOrgName = model.ContactNonProfitOrgName;
                        p.ContactNonProfitOrgNote = model.ContactNonProfitOrgNote;
                        p.ContactNonProfitOrgPhone = model.ContactNonProfitOrgPhone;
                        p.DateOfEvent = model.DateOfEvent;
                        p.NonProfitOrgId = model.NonProfitOrgId;
                        p.Name = model.Name;
                        p.Note = model.Note;
                        p.Updated = DateTime.UtcNow;
                        p.UpdatedBy = User.Identity.Name;
                        cx.SaveChanges();
                        Logger.Log(User.Identity.Name, "Modify Event " + model.Name + " / " + p.DateOfEvent.ToShortDateString(),
                            _oldObject, Logger.Serialize(p), "Event", p.Id);
                    }
                }
                finally
                {

                }

                scope.Complete();
            }

            // redirect
            if (returnUrl != null && returnUrl.Length > 0)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("List", "Event");
            }
        }

        [HttpGet]
        public ActionResult Delete(int id, string returnUrl)
        {
            var cx = new Entities();
            var p = cx.Event.Single(e => e.Id == id);
            {
                string _oldObject = Logger.Serialize(p);
                p.Deleted = true;
                p.Updated = DateTime.UtcNow;
                p.UpdatedBy = User.Identity.Name;
                Logger.Log(User.Identity.Name, "Deleted Event " + p.Name + " / " + p.DateOfEvent.ToShortDateString(),
                    _oldObject, Logger.Serialize(p), "Event", p.Id);
            }
            cx.SaveChanges();
            // redirect
            if (returnUrl != null && returnUrl.Length > 0)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("List", "Event");
            }
        }
    }
}