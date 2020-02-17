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

using CsvHelper;
using NonProfitCRM.Components;
using NonProfitCRM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NonProfitCRM.Controllers
{
    [Authorize(Roles = "FRD")]
    public class ReportController : Controller
    {
        public ActionResult ExportEvent(string date, string company, string nonprofitorg, string type, string btn)
        {
            if (date == null)
            {
                date = "";
            }
            if (company == null)
            {
                company = "";
            }
            if (nonprofitorg == null)
            {
                nonprofitorg = "";
            }
            if (type == null)
            {
                type = "";
            }

            var model = new Entities().Event.
                //Where(e => search == "" || e.UserName.Contains(search) || e.Description.Contains(search) || e.Data.Contains(search)).
                Where(e => !e.Deleted &&
                        (date == "" || e.Name.StartsWith(date)) &&
                        (company == "" || e.Company.Name.Contains(company)) &&
                        (nonprofitorg == "" || e.NonProfitOrg.Name.Contains(nonprofitorg)) &&
                        (type == "" || e.EventType.Name.StartsWith(type))
                    ).
                OrderByDescending(e => e.DateOfEvent);

            if (btn == "export")
            {
                var rmodel = from e in model
                             select new
                             {
                                 datum = e.Name,
                                 ucast = e.Capacity,
                                 firma = e.Company.Name,
                                 neziskovka = e.NonProfitOrg.Name,
                                 organizovano = e.SelfOrganised ? "Vlastní" : "Hestia",
                                 typ = e.EventType.Name
                             };

                var data = rmodel.ToList();

                var stream = new MemoryStream();
                var writeFile = new StreamWriter(stream, Encoding.UTF8);
                var csv = new CsvWriter(writeFile);
                //csv.Configuration.RegisterClassMap<GroupReportCSVMap>();

                csv.Configuration.Delimiter = ";";

                csv.WriteRecords(data);

                writeFile.Flush();

                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/octet-stream", "export-akce.csv");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult QReport(string[] companies, string btn)
        {
            if(btn!=null && btn == "search")
            {
                ViewBag.comp = new string[0];
                if (companies == null)
                {
                    Response.Cookies["qreport-comp"].Value = "";
                }else
                {
                    Response.Cookies["qreport-comp"].Value = string.Join(",", companies);
                    ViewBag.comp = companies;
                }
                Response.Cookies["qreport-comp"].Expires = DateTime.Now.AddYears(10);
            }
            else
            {
                ViewBag.comp = new string[0];
                if (Request.Cookies["qreport-comp"] != null && Request.Cookies["qreport-comp"].Value.Length > 0)
                {
                    ViewBag.comp = Request.Cookies["qreport-comp"].Value.Split(',');
                }
            }

            var model = from p in new Entities().Event select p;

            return View(model);
        }
    }
}