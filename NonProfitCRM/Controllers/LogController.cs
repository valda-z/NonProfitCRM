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
using System.Web.Routing;

namespace NonProfitCRM.Controllers
{
    [Authorize(Roles = "FRD_SYSTEM_ADMINISTRATOR")]
    public class LogController : Controller
    {
        public ActionResult SecurityLog(string search)
        {
            if (search == null)
            {
                search = "";
            }

            var model = new Entities().Log.
                Where(e => search == "" || e.UserName.Contains(search) || e.Description.Contains(search) || e.Data.Contains(search)).
                OrderByDescending(e => e.Created).Take(NonProfitCRM.Properties.Settings.Default.MAXRECORDS);

            return View(model);
        }

        public ActionResult SecurityLogDetail(int id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            var model = new Entities().Log.Single(e => e.Id == id);

            return View(model);
        }
    }
}