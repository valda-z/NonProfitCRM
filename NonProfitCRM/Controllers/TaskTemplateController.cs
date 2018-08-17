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

using NonProfitCRM.Components;
using NonProfitCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NonProfitCRM.Controllers
{
    [Authorize(Roles = "SYSTEM_ADMINISTRATOR")]
    public class TaskTemplateController : Controller
    {
        public ActionResult List(string search)
        {
            if (search == null)
            {
                search = "";
            }

            var model = new Entities().EventTaskTemplate.
                Where(e => search == "" || e.Name.Contains(search)).
                OrderBy(e => e.Name);

            return View(model);
        }
        public ActionResult Detail(int id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            EventTaskTemplate p;
            if (id == 0)
            {
                p = new EventTaskTemplate();
            }
            else
            {
                p = new Entities().EventTaskTemplate.Where(e => e.Id == id).Single();
            }
            return View(p);
        }
        [HttpPost]
        public ActionResult Detail(int id, EventTaskTemplate model, string returnUrl, FormCollection m)
        {
            EventTaskTemplate p;
            Entities cx = new Entities();
            model.Data = TaskTemplate.ArrSerialize(TaskTemplate.ArrDeserialize(model.Data));
            if (model.Id == 0)
            {
                p = model;
                cx.EventTaskTemplate.Add(p);
                cx.SaveChanges();
            }
            else
            {
                p = cx.EventTaskTemplate.Single(e => e.Id == id);
                p.Name = model.Name;
                p.Data = model.Data;
                cx.SaveChanges();
            }

            // redirect
            if (returnUrl != null && returnUrl.Length > 0)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("TaskTemplate", "List");
            }
        }
    }
}