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
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NonProfitCRM.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ViewBag.CanEdit = true;
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            SystemHelper.TestIsInRole(SystemHelper.Roles.FRD);
            return base.BeginExecute(requestContext, callback, state);
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

            bool showActive = (Request.Cookies["companyActiveOn"]?.Value == "true");

            var cx = new Entities();
            var tagsArr = tags.Split(',');

            var inquery = from x in cx.Tag2Company
                          where tags.Contains(x.Tag.Tag1)
                          select x.CompanyId;

            var model = (from e in cx.ViewCompanyList
                    where (search == "" ||
                        (e.IdentificationNumber.StartsWith(search) ||
                        e.Name.Contains(search) ||
                        e.Address.Contains(search) ||
                        e.City.Contains(search) ||
                        e.Contact1Name.Contains(search) ||
                        e.Contact2Name.Contains(search) ||
                        e.Contact1Phone.Contains(search) ||
                        e.Contact2Phone.Contains(search) ||
                        e.Contact1Email.Contains(search) ||
                        e.Contact2Email.Contains(search) ||
                        e.CountryName.Contains(search) ||
                        e.RegionName.Contains(search) ||
                        e.Www.Contains(search)
                    )) && (showDeleted || !e.Deleted) &&
                    ((showActive && CompanyStatusHelper.IsCompany.Contains(e.StatusId)) || (!showActive)) &&
                    (tags == "" || inquery.Contains(e.Id))
                    orderby e.Name
                    select e).
                    Take(Properties.Settings.Default.MAXRECORDS);

            return View(model);
        }

        [HttpGet]
        public ActionResult Detail(int id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.tags = TagHelper.GetTags(id, "Company");
            Company model;
            if (id == 0)
            {
                model = new Company();
                model.IdentificationNumber = EntityHelper.Create16DigitString();
            }
            else
            {
                model = new Entities().Company.Single(e => e.Id == id);
            }
            if (model.Deleted)
            {
                ViewBag.CanEdit = false;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Detail(int id, Company model, string returnUrl, FormCollection m)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.tags = m["tags"];
            int myid = id;

            DateTime? dtm = null;
            if (m["FirstContactRaw"].Trim().Length > 0)
            {
                bool isokA = false;
                dtm = DateHelper.ConvertDate(m["FirstContactRaw"], out isokA);
                if (!isokA || dtm == null)
                {
                    ModelState.AddModelError("", "Chybný formát data pro Datum prvního kontaktu.");
                }
                if (!isokA)
                {
                    return View(model);
                }
            }

            using (var scope = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                }))
            {

                Entities cx = new Entities();

                model.FirstContact = dtm;

                //update NonProfitOrg
                Company p = null;
                try
                {
                    if (model.Id == 0)
                    {
                        p = model;
                        cx.Company.Add(p);
                        p.Updated = DateTime.UtcNow;
                        p.UpdatedBy = NonProfitCRM.Components.SystemHelper.GetUserName;
                        Logger.Log(NonProfitCRM.Components.SystemHelper.GetUserName, "Create Company " + model.Name + " / " + p.IdentificationNumber, null, model,
                            "Company", p.Id);
                        cx.SaveChanges();
                        myid = p.Id;
                    }
                    else
                    {
                        p = cx.Company.Single(e => e.Id == id);
                        string _oldObject = Logger.Serialize(p);
                        p.StatusId = model.StatusId;
                        p.Address = model.Address;
                        p.Insurance = model.Insurance;
                        p.City = model.City;
                        p.Contact1Email = model.Contact1Email;
                        p.Contact1Name = model.Contact1Name;
                        p.Contact1Note = model.Contact1Note;
                        p.Contact1Phone = model.Contact1Phone;
                        p.Contact2Email = model.Contact2Email;
                        p.Contact2Name = model.Contact2Name;
                        p.Contact2Note = model.Contact2Note;
                        p.Contact2Phone = model.Contact2Phone;
                        p.CountryId = model.CountryId;
                        p.IdentificationNumber = model.IdentificationNumber;
                        p.Name = model.Name;
                        p.Note = model.Note;
                        p.RegionId = model.RegionId;
                        p.Www = model.Www;
                        p.FirstContact = model.FirstContact;
                        p.ContactPerson = model.ContactPerson;
                        p.Updated = DateTime.UtcNow;
                        p.UpdatedBy = NonProfitCRM.Components.SystemHelper.GetUserName;
                        cx.SaveChanges();
                        Logger.Log(NonProfitCRM.Components.SystemHelper.GetUserName, "Modify Company " + model.Name + " / " + p.IdentificationNumber,
                            _oldObject, Logger.Serialize(p), "Company", p.Id);
                    }

                    TagHelper.SetTags(p.Id, "Company", m["tags"], cx);
                }
                catch (DbUpdateException e)
                {
                    var sqlException = e.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        if (sqlException.Errors.Count > 0)
                        {
                            switch (sqlException.Errors[0].Number)
                            {
                                case 2601: // Foreign Key violation
                                    ModelState.AddModelError("ICOIssue", "Firma nebo Lead se stejným IČ již existuje!");
                                    return View(model);
                                default:
                                    throw;
                            }
                        }
                    }
                    else
                    {
                        throw;
                    }
                }

                scope.Complete();
                StatisticsHelper.InvalidateCacheCRM();
            }

            if (id == 0 && (m["saveandtask"] != null) )
            {
                return RedirectToAction("Detail", new { id = myid, returnUrl = returnUrl });
            }
            else
            {
                // redirect
                if (returnUrl != null && returnUrl.Length > 0)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("List", "Company");
                }
            }
        }

        [HttpGet]
        public ActionResult Delete(int id, string returnUrl)
        {
            var cx = new Entities();
            var p = cx.Company.Single(e => e.Id == id);
            {
                string _oldObject = Logger.Serialize(p);
                p.Deleted = true;
                p.IdentificationNumber = p.Id.ToString() + "|" + p.IdentificationNumber;
                p.Updated = DateTime.UtcNow;
                p.UpdatedBy = NonProfitCRM.Components.SystemHelper.GetUserName;
                Logger.Log(NonProfitCRM.Components.SystemHelper.GetUserName, "Deleted Company " + p.Name + " / " + p.IdentificationNumber,
                    _oldObject, Logger.Serialize(p), "Company", p.Id);
            }
            cx.SaveChanges();
            StatisticsHelper.InvalidateCacheCRM();
            // redirect
            if (returnUrl != null && returnUrl.Length > 0)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("List", "Company");
            }
        }
    }
}