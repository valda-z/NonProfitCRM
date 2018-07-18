using NonProfitCRM.Models;
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
        public ActionResult List(string search)
        {
            if (search == null)
            {
                search = "";
            }

            var model = new Entities().ViewNonProfitOrgList.
                Where(e => search == "" ||
                    (   e.IdentificationNumber.StartsWith(search) ||
                        e.Name.StartsWith(search) ||
                        e.Address.StartsWith(search) ||
                        e.City.StartsWith(search) ||
                        e.Contact1Name.StartsWith(search) ||
                        e.Contact2Name.StartsWith(search) ||
                        e.CountryName.StartsWith(search) ||
                        e.RegionName.StartsWith(search) ||
                        e.Www.StartsWith(search)
                    )).
                OrderBy(e => e.Name).Take(Properties.Settings.Default.MAXRECORDS);

            return View(model);
        }

        [HttpGet]
        public ActionResult Detail(int id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
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
                        p.Www = model.Www;
                        p.Updated = DateTime.UtcNow;
                        p.UpdatedBy = User.Identity.Name;
                        cx.SaveChanges();
                        Logger.Log(User.Identity.Name, "Modify NonProfitOrg " + model.Name + " / " + p.IdentificationNumber,
                            _oldObject, Logger.Serialize(p), "NonProfitOrg", p.Id);
                    }
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