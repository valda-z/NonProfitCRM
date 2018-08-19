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
    public class CalendarController : Controller
    {
        public ActionResult Calendar(int? id)
        {
            bool showTasks = (Request.Cookies["nonprofitorgShowTasks"]?.Value == "true");
            bool showOnlyMy = !(Request.Cookies["nonprofitorgIsMyTaskOn"]?.Value == "false");

            var cx = new Entities();
            var model = new CalendarHomeModel();

            if (id == null)
            {
                id = 0;
            }
            model.Shift = id.Value;

            model.CurrentMonth = DateHelper.GetCurrMonthUTC(id.Value);
            var datF = model.CurrentMonth.AddDays(-15);
            var datT = model.CurrentMonth.AddMonths(1).AddDays(15);

            if (showTasks)
            {
                model.TaskList = cx.ViewTaskList.
                    Where(
                        e => e.StatusId < 1000 &&
                        (!showOnlyMy || e.AssignedTo == User.Identity.Name) &&
                        (e.DueDate > datF && e.DueDate < datT)
                        ).OrderBy(e => e.DueDate);
            }

            model.EventList = cx.ViewEventList.
                Where(
                    e => !e.Deleted &&
                    (e.DateOfEvent > datF && e.DateOfEvent < datT)
                    ).OrderBy(e => e.DateOfEvent);

            return View(model);
        }

    }
}