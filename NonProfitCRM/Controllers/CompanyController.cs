using NonProfitCRM.Components;
using NonProfitCRM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace NonProfitCRM.Controllers
{
    [Authorize(Roles = "CRM")]
    public class CompanyController : Controller
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

            var model = new Entities().ViewCompanyList.
                Where(e => search == "" ||
                    (e.IdentificationNumber.StartsWith(search) ||
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
            Company model;
            if (id == 0)
            {
                model = new Company();
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

            using (var scope = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                }))
            {

                Entities cx = new Entities();

                //update NonProfitOrg
                Company p = null;
                try
                {
                    if (model.Id == 0)
                    {
                        p = model;
                        cx.Company.Add(p);
                        p.Updated = DateTime.UtcNow;
                        p.UpdatedBy = User.Identity.Name;
                        Logger.Log(User.Identity.Name, "Create Company " + model.Name + " / " + p.IdentificationNumber, null, model,
                            "Company", p.Id);
                        cx.SaveChanges();
                    }
                    else
                    {
                        p = cx.Company.Single(e => e.Id == id);
                        string _oldObject = Logger.Serialize(p);
                        p.Address = model.Address;
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
                        Logger.Log(User.Identity.Name, "Modify Company " + model.Name + " / " + p.IdentificationNumber,
                            _oldObject, Logger.Serialize(p), "Company", p.Id);
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
            }

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
                p.UpdatedBy = User.Identity.Name;
                Logger.Log(User.Identity.Name, "Deleted Company " + p.Name + " / " + p.IdentificationNumber,
                    _oldObject, Logger.Serialize(p), "Company", p.Id);
            }
            cx.SaveChanges();
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