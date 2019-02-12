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

﻿using NonProfitCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NonProfitCRM.Components;
using System.Transactions;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace NonProfitCRM.Controllers
{
    [Authorize(Roles = "CRM")]
    public class NonProfitOrgController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ViewBag.CanEdit = true;
        }

        [HttpGet]
        public ActionResult List(string search, string tags, 
            string srchCapacity, string srchType, string srchDistrict, string srchCity)
        {
            int srchCapacityInt = 0;
            if (search == null)
            {
                search = "";
            }
            if (srchCapacity == null)
            {
                srchCapacity = "";
            }
            if (srchCity == null)
            {
                srchCity = "";
            }
            if (srchDistrict == null)
            {
                srchDistrict = "";
            }
            if (srchType == null)
            {
                srchType = "";
            }
            if (srchCapacity != "")
            {
                int.TryParse(srchCapacity, out srchCapacityInt);
            }
            if (tags == null)
            {
                tags = "";
            }
            tags = tags.Trim();

            bool showDeleted = (Request.Cookies["nonprofitorgIsDelOn"]?.Value == "true");

            var cx = new Entities();
            var tagsArr = tags.Split(',');

            var inquery = from x in cx.Tag2NonProfitOrg
                          where tags.Contains(x.Tag.Tag1)
                          select x.NonProfitOrgId;

            var model = (from e in cx.ViewNonProfitOrgList
                    where 
                        (search == "" ||
                        (e.IdentificationNumber.StartsWith(search) ||
                            e.Name.StartsWith(search) ||
                            e.NonProfitOrgTypeName.StartsWith(search) ||
                            e.Address.StartsWith(search) ||
                            e.City.StartsWith(search) ||
                            e.Contact1Name.StartsWith(search) ||
                            e.Contact2Name.StartsWith(search) ||
                            e.Contact1Phone.StartsWith(search) ||
                            e.Contact2Phone.StartsWith(search) ||
                            e.Contact1Email.StartsWith(search) ||
                            e.Contact2Email.StartsWith(search) ||
                            e.CountryName.StartsWith(search) ||
                            e.RegionName.StartsWith(search) ||
                            e.Www.StartsWith(search)
                        )
                    ) && (showDeleted || !e.Deleted) &&
                    (
                        (srchCapacity == "" || e.Capacity >= srchCapacityInt) &&
                        (srchCity == "" || e.City.StartsWith(srchCity)) &&
                        (srchDistrict == "" || e.RegionName.StartsWith(srchDistrict)) &&
                        (srchType == "" || e.NonProfitOrgTypeName.StartsWith(srchType)) 
                    ) &&
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
            ViewBag.tags = TagHelper.GetTags(id, "NonProfitOrg");
            NonProfitOrg model;
            if (id == 0)
            {
                model = new NonProfitOrg();
            }
            else
            {
                model = new Entities().NonProfitOrg.Single(e => e.Id == id);
            }
            if (model.Deleted)
            {
                ViewBag.CanEdit = false;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Detail(int id, NonProfitOrg model, string returnUrl, FormCollection m)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.tags = m["tags"];

            using (var scope = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                }))
            {

                Entities cx = new Entities();

                //update NonProfitOrg
                NonProfitOrg p = null;
                try
                {
                    if (model.Id == 0)
                    {
                        p = model;
                        cx.NonProfitOrg.Add(p);
                        p.Updated = DateTime.UtcNow;
                        p.UpdatedBy = User.Identity.Name;
                        Logger.Log(User.Identity.Name, "Create NonProfitOrg " + model.Name + " / " + p.IdentificationNumber, null, model, "NonProfitOrg", p.Id);
                        cx.SaveChanges();
                    }
                    else
                    {
                        p = cx.NonProfitOrg.Single(e => e.Id==id);
                        string _oldObject = Logger.Serialize(p);
                        p.Address = model.Address;
                        p.Capacity = model.Capacity;
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
                        p.TypeId = model.TypeId;
                        p.Www = model.Www;
                        p.Updated = DateTime.UtcNow;
                        p.UpdatedBy = User.Identity.Name;
                        cx.SaveChanges();
                        Logger.Log(User.Identity.Name, "Modify NonProfitOrg " + model.Name + " / " + p.IdentificationNumber,
                            _oldObject, Logger.Serialize(p), "NonProfitOrg", p.Id);
                    }
                    TagHelper.SetTags(p.Id, "NonProfitOrg", m["tags"], cx);
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
                                    ModelState.AddModelError("ICOIssue", "Neziskovka se stejným IČ již existuje!");
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

            // redirect
            if (returnUrl != null && returnUrl.Length > 0)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("List", "NonProfitOrg");
            }
        }

        [HttpGet]
        public ActionResult Delete(int id, string returnUrl)
        {
            var cx = new Entities();
            var p = cx.NonProfitOrg.Single(e => e.Id == id);
            {
                string _oldObject = Logger.Serialize(p);
                p.Deleted = true;
                p.IdentificationNumber = p.Id.ToString() + "|" + p.IdentificationNumber;
                p.Updated = DateTime.UtcNow;
                p.UpdatedBy = User.Identity.Name;
                Logger.Log(User.Identity.Name, "Deleted NonProfitOrg " + p.Name + " / " + p.IdentificationNumber,
                    _oldObject, Logger.Serialize(p), "NonProfitOrg", p.Id);
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
                return RedirectToAction("List", "NonProfitOrg");
            }
        }

    }
}