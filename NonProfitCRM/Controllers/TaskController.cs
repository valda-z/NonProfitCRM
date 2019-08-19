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
using System.Web.Routing;

namespace NonProfitCRM.Controllers
{
    [Authorize]
    public class TaskController : Controller
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
        public ActionResult Detail(int id, string entityid, string entity, string returnUrl, string cmd)
        {
            if (cmd != null && cmd == "DONE" && id != 0)
            {
                using (var scope = new TransactionScope(
                    TransactionScopeOption.RequiresNew,
                    new TransactionOptions()
                    {
                        IsolationLevel = IsolationLevel.ReadCommitted
                    }))
                {

                    var cx = new Entities();
                    Task p = null;
                    try
                    {
                        p = cx.Task.Single(e => e.Id == id);
                        string _oldObject = Logger.Serialize(p);
                        p.StatusId = 1000;
                        p.Updated = DateTime.UtcNow;
                        p.UpdatedBy = NonProfitCRM.Components.SystemHelper.GetUserName;
                        cx.SaveChanges();
                        Logger.Log(NonProfitCRM.Components.SystemHelper.GetUserName, "Modify Task - DONE " + p.Description + " / " + p.Entity + " / " + p.EntityId,
                            _oldObject, Logger.Serialize(p), "Task", p.Id);
                    }
                    finally
                    {

                    }

                    scope.Complete();
                }
                if (returnUrl != null && returnUrl.Length > 0)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("List", "Task");
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            Task model;
            if (id == 0)
            {
                model = new Task();
                model.Entity = entity;
                model.EntityId = int.Parse(entityid);
                model.AssignedTo = NonProfitCRM.Components.SystemHelper.GetUserName;
            }
            else
            {
                model = new Entities().Task.Single(e => e.Id == id);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Detail(int id, Task model, string returnUrl, FormCollection m)
        {
            ViewBag.ReturnUrl = returnUrl;

            bool isokA = false;
            var dtm = DateHelper.ConvertDate(m["DueDateRaw"], out isokA);
            if (!isokA || dtm == null)
            {
                ModelState.AddModelError("", "Chybný formát data pro pole Datum.");
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

                var cx = new Entities();
                model.DueDate = dtm.Value;
                Task p = null;
                try
                {
                    if (model.Id == 0)
                    {
                        p = model;
                        cx.Task.Add(p);
                        p.Updated = DateTime.UtcNow;
                        p.UpdatedBy = NonProfitCRM.Components.SystemHelper.GetUserName;
                        Logger.Log(NonProfitCRM.Components.SystemHelper.GetUserName, "Create Task " + model.Description + " / " + p.Entity + " / " + p.EntityId, null, model,
                            "Task", p.Id);
                        cx.SaveChanges();
                    }
                    else
                    {
                        p = cx.Task.Single(e => e.Id == id);
                        string _oldObject = Logger.Serialize(p);
                        p.AssignedTo = model.AssignedTo;
                        p.Description = model.Description;
                        p.DueDate = model.DueDate;
                        p.Note = model.Note;
                        p.StatusId = model.StatusId;
                        p.Updated = DateTime.UtcNow;
                        p.UpdatedBy = NonProfitCRM.Components.SystemHelper.GetUserName;
                        cx.SaveChanges();
                        Logger.Log(NonProfitCRM.Components.SystemHelper.GetUserName, "Modify Task " + model.Description + " / " + p.Entity + " / " + p.EntityId,
                            _oldObject, Logger.Serialize(p), "Task", p.Id);
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
                return RedirectToAction("List", "Task");
            }
        }

        [HttpGet]
        public ActionResult Delete(int id, string returnUrl)
        {
            var cx = new Entities();
            var p = cx.Task.Single(e => e.Id == id);
            {
                string _oldObject = Logger.Serialize(p);
                p.Updated = DateTime.UtcNow;
                p.UpdatedBy = NonProfitCRM.Components.SystemHelper.GetUserName;
                Logger.Log(NonProfitCRM.Components.SystemHelper.GetUserName, "Deleted Task " + p.Description + " / " + p.Entity + " / " + p.EntityId,
                    _oldObject, Logger.Serialize(p), "Task", p.Id);
            }
            cx.Task.Remove(p);
            cx.SaveChanges();
            // redirect
            if (returnUrl != null && returnUrl.Length > 0)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("List", "Task");
            }
        }
    }
}