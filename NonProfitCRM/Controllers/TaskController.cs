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
    public class TaskController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ViewBag.CanEdit = true;
        }

        [HttpGet]
        public ActionResult Detail(int id, string entityid, string entity, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            Task model;
            if (id == 0)
            {
                model = new Task();
                model.Entity = entity;
                model.EntityId = int.Parse(entityid);
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
                        p.UpdatedBy = User.Identity.Name;
                        Logger.Log(User.Identity.Name, "Create Task " + model.Description + " / " + p.Entity + " / " + p.EntityId, null, model,
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
                        p.UpdatedBy = User.Identity.Name;
                        cx.SaveChanges();
                        Logger.Log(User.Identity.Name, "Modify Task " + model.Description + " / " + p.Entity + " / " + p.EntityId,
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
                p.UpdatedBy = User.Identity.Name;
                Logger.Log(User.Identity.Name, "Deleted Task " + p.Description + " / " + p.Entity + " / " + p.EntityId,
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