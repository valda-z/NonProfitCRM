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

﻿using NonProfitCRM.Components;
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
        public ActionResult List(string search, string tags)
        {
            if (search == null)
            {
                search = "";
            }

            if (tags == null)
            {
                tags = "";
            }
            tags = tags.Trim();

            bool showDeleted = (Request.Cookies["nonprofitorgIsDelOn"]?.Value == "true");

            bool showActive = (Request.Cookies["eventActiveOn"]?.Value == "true");

            var cx = new Entities();
            var tagsArr = tags.Split(',');

            var inquery = from x in cx.Tag2Event
                          where tags.Contains(x.Tag.Tag1)
                          select x.EventId;

            var model = (from e in cx.ViewEventList
                    where (showDeleted || !e.Deleted) &&
                    (search == "" ||
                    (e.CompanyName.StartsWith(search) ||
                        e.Name.StartsWith(search) ||
                        e.EventTypeName.StartsWith(search) ||
                        e.NonProfitOrgName.StartsWith(search) ||
                        e.ContactNonProfitOrgName.StartsWith(search) ||
                        e.ContactCompanyNote.StartsWith(search)
                    )) &&
                    ((showActive && e.Closed == null) || (!showActive)) &&
                    (tags=="" || inquery.Contains(e.Id))
                    orderby e.DateOfEvent descending
                    select e).
                    Take(Properties.Settings.Default.MAXRECORDS);

            return View(model);
        }

        [HttpGet]
        public ActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var model = new Event();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Event model, string returnUrl, FormCollection m)
        {
            ViewBag.ReturnUrl = returnUrl;

            bool isClosed = false;
            int id = 0;
            int templateId = int.Parse(m["TaskTemplateId"]);

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
                model.Name = DateHelper.FormatDateShort(model.DateOfEvent, User.Identity.Name);

                //update NonProfitOrg
                Event p = model;

                // contact info from company / nonprofitorg
                var comp = cx.Company.Single(e => e.Id == p.CompanyId);
                if (model.ContactCompanyNote == null || model.ContactCompanyNote.Length == 0)
                {
                    p.ContactCompanyNote = comp.Contact1Name == null ? "" : comp.Contact1Name;
                    if (comp.Contact1Email != null && comp.Contact1Email.Length > 0)
                    {
                        p.ContactCompanyNote += ", " + comp.Contact1Email;
                    }
                    if (comp.Contact1Phone != null && comp.Contact1Phone.Length > 0)
                    {
                        p.ContactCompanyNote += ", " + comp.Contact1Phone;
                    }
                }
                p.Insurance = comp.Insurance;
                
                var nonp = cx.NonProfitOrg.Single(e => e.Id == p.NonProfitOrgId);
                p.ContactNonProfitOrgEmail = nonp.Contact1Email;
                p.ContactNonProfitOrgName = nonp.Contact1Name;
                p.ContactNonProfitOrgNote = nonp.Contact1Note;
                p.ContactNonProfitOrgPhone = nonp.Contact1Phone;

                p.Closed = (isClosed ? new DateTime?(DateTime.UtcNow) : null);
                cx.Event.Add(p);
                p.Updated = DateTime.UtcNow;
                p.UpdatedBy = User.Identity.Name;
                Logger.Log(User.Identity.Name, "Create Event " + model.Name + " / " + p.DateOfEvent.ToShortDateString(), null, model,
                    "Event", p.Id);
                cx.SaveChanges();
                foreach (var obj in new
                    TaskTemplate(templateId).GetTasks(User.Identity.Name, p.DateOfEvent, p.Id))
                {
                    cx.Task.Add(obj);
                    cx.SaveChanges();
                }
                id = p.Id;
                scope.Complete();
                StatisticsHelper.InvalidateCacheEvent();
                StatisticsHelper.InvalidateCacheEventPeople();
            }

            // redirect
            //return RedirectToAction("Detail", new { id = id, returnUrl = returnUrl });
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
        public ActionResult Detail(int id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.tags = TagHelper.GetTags(id, "Event");

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
            ViewBag.tags = m["tags"];

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
                        throw new Exception("Unsupported operation!");
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
                        p.Insurance = model.Insurance;
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
                        //p.Name = model.Name;
                        p.Note = model.Note;
                        p.TypeId = model.TypeId;
                        p.Updated = DateTime.UtcNow;
                        p.UpdatedBy = User.Identity.Name;
                        cx.SaveChanges();
                        Logger.Log(User.Identity.Name, "Modify Event " + model.Name + " / " + p.DateOfEvent.ToShortDateString(),
                            _oldObject, Logger.Serialize(p), "Event", p.Id);
                    }

                    TagHelper.SetTags(p.Id, "Event", m["tags"], cx);
                }
                finally
                {

                }

                scope.Complete();
                StatisticsHelper.InvalidateCacheEvent();
                StatisticsHelper.InvalidateCacheEventPeople();
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
            StatisticsHelper.InvalidateCacheEvent();
            StatisticsHelper.InvalidateCacheEventPeople();
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