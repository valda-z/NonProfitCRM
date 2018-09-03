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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;

namespace NonProfitCRM.Components
{

    // motto: https://msdn.microsoft.com/en-us/magazine/hh288075.aspx

    /*
	IEnumerable<Object> source,
	IEnumerable<string> columnNames,
	string defaultSort,
	int rowsPerPage,
	bool canPage,
	bool canSort,
	string ajaxUpdateContainerId,
	string ajaxUpdateCallback,
	string fieldNamePrefix,
	string pageFieldName,
	string selectionFieldName,
	string sortFieldName,
	string sortDirectionFieldName     
     */

    public class ASWebGridABC : WebGrid
    {
        private int take;
        private bool noPag = false;
        List<PagNavItem> pageNavItems = new List<PagNavItem>();
        int CurrPageId = 1;

        private string getFirstLeter(string text)
        {
            string ret = "#";
            string tmp = StringHelper.ReplaceDiacritics(text.ToLower());
            if (tmp.Length>=1 &&
                        tmp[0] >= 'a' && tmp[0]<='z')
            {
                ret = tmp.Substring(0, 1).ToUpper();
            }
            return ret;
        }

        public class PagItem
        {
            public int Id { get; set; }
            public string Key { get; set; }
            public int PageId { get; set; }
        }

        public class PagNavItem
        {
            public PagNavItem(int pg)
            {
                PageId = pg;
                KeyMin = "";
                KeyMax = "";
            }
            public string KeyMin { get; set; }
            public string KeyMax { get; set; }
            public int PageId { get; set; }

            public string ToMyString()
            {
                if (KeyMin == KeyMax)
                {
                    return KeyMin;
                }
                else
                {
                    return KeyMin + "-" + KeyMax;
                }
            }
        }

        public IEnumerable<Object> prepareSource(IEnumerable<Object> source, int rowsPerPage)
        {
            var pageItems = new List<PagItem>();

            IEnumerable<Object> ret = null;

            int i = 0;
            var currItem = new PagNavItem(1);

            if (source is IQueryable<NonProfitCRM.Models.ViewNonProfitOrgList>)
            {
                var dts = ((IQueryable<NonProfitCRM.Models.ViewNonProfitOrgList>)source).ToList();
                foreach (var obj in dts)
                {
                    var p = new PagItem();
                    p.Id = obj.Id;
                    p.Key = getFirstLeter(obj.Name);
                    // for first one
                    if (currItem.KeyMin == "")
                    {
                        currItem.KeyMin = p.Key;
                        currItem.KeyMax = p.Key;
                    }
                    // is there key change?
                    if (p.Key != currItem.KeyMax)
                    {
                        //there is enough space for next key
                        if (i <= rowsPerPage)
                        {
                            currItem.KeyMax = p.Key;
                        }
                        else
                        {
                            i = 0;
                            pageNavItems.Add(currItem);
                            currItem = new PagNavItem(currItem.PageId + 1);
                            currItem.KeyMin = p.Key;
                            currItem.KeyMax = p.Key;
                        }
                    }
                    p.PageId = currItem.PageId;
                    pageItems.Add(p);
                    i++;
                }
                pageNavItems.Add(currItem);

                ret = from e in dts
                      where (from pi in pageItems where pi.PageId == CurrPageId select pi.Id).Contains(e.Id)
                      select e;
            }
            else if (source is IQueryable<NonProfitCRM.Models.ViewCompanyList>)
            {
                var dts = ((IQueryable<NonProfitCRM.Models.ViewCompanyList>)source).ToList();
                foreach (var obj in dts)
                {
                    var p = new PagItem();
                    p.Id = obj.Id;
                    p.Key = getFirstLeter(obj.Name);
                    // for first one
                    if (currItem.KeyMin == "")
                    {
                        currItem.KeyMin = p.Key;
                        currItem.KeyMax = p.Key;
                    }
                    // is there key change?
                    if (p.Key != currItem.KeyMax)
                    {
                        //there is enough space for next key
                        if (i <= rowsPerPage)
                        {
                            currItem.KeyMax = p.Key;
                        }
                        else
                        {
                            i = 0;
                            pageNavItems.Add(currItem);
                            currItem = new PagNavItem(currItem.PageId + 1);
                            currItem.KeyMin = p.Key;
                            currItem.KeyMax = p.Key;
                        }
                    }
                    p.PageId = currItem.PageId;
                    pageItems.Add(p);
                    i++;
                }
                pageNavItems.Add(currItem);

                ret = from e in dts
                      where (from pi in pageItems where pi.PageId == CurrPageId select pi.Id).Contains(e.Id)
                      select e;
            }

            return ret;
        }

        public ASWebGridABC(IEnumerable<Object> source, bool noPaginator = false)
            : base(canSort: false, rowsPerPage: NonProfitCRM.Properties.Settings.Default.MAXRECORDS)
        {
            take = NonProfitCRM.Properties.Settings.Default.MAXRECORDS;
            if(!int.TryParse(HttpContext.Current.Request.QueryString["page"], out CurrPageId))
            {
                CurrPageId = 1;
            }
            var src = prepareSource(source, 25);
            base.Bind(src);
        }

        public IHtmlString Draw(IEnumerable<WebGridColumn> columns)
        {
            // prepare data table HTML
            IHtmlString ret = this.Table(columns: columns, 
                tableStyle: "table table-hover");
            string alert = "";
            if (this.Rows.Count == 0)
            {
                alert = "<div class='alert alert-danger alert-sm'>No record found...</div>";
            }else if(this.TotalRowCount == take)
            {
                alert = "<div class='alert alert-warning alert-sm'>There are more than "+
                    take.ToString()+" records, use search filter to limit returned records.</div>";
            }

            Regex e = new Regex("<thead>.*<\\/thead>", RegexOptions.Singleline);
            StringBuilder sbth = new StringBuilder();
            sbth.AppendLine("<thead>");
            sbth.AppendLine("<tr>");
            foreach (var i in columns)
            {
                sbth.AppendFormat("<td scope=\"col\" class=\"{1}\">{0}</td>",
                    HttpUtility.HtmlEncode(i.Header),
                    i.Style);
            }
            sbth.AppendLine("</tr>");
            sbth.AppendLine("</thead>");
            string strret = e.Replace(ret.ToString(), sbth.ToString());

            //replace paginator look and feel
            var _sbPag = new StringBuilder();
            foreach(var p in pageNavItems)
            {
                if (p.PageId == CurrPageId)
                {
                    _sbPag.AppendFormat("<li class='active'><a>{0}</a></li>", p.ToMyString());
                }
                else
                {
                    _sbPag.AppendFormat("<li><a href='{1}{2}'>{0}</a></li>",
                        p.ToMyString(), this.GetPageUrl(0).Replace("page=1","page="), p.PageId);
                }
            }
            string _paginator =
                "<nav><ul class='pagination pagination-small'>"
                + _sbPag.ToString()
                + "</ul></nav>";

            //return new HTML code decorated by alert and paginator
            return new HtmlString(alert + strret + _paginator);
        }
    }
}