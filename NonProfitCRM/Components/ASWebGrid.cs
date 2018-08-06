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

    public class ASWebGrid : WebGrid
    {
        private int take;
        private bool noPag = false;
        public ASWebGrid(IEnumerable<Object> source, int rowsPerPage = 10, bool noPaginator = false)
            : base(canSort: false, rowsPerPage: rowsPerPage)
        {
            take = NonProfitCRM.Properties.Settings.Default.MAXRECORDS;
            noPag = noPaginator;
            if (take < rowsPerPage)
            {
                take = rowsPerPage;
            }
            base.Bind(source.Take(take));
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
            string strret = e.Replace(ret.ToString(), sbth.ToString());
            sbth.AppendLine("</tr>");
            sbth.AppendLine("</thead>");

            //replace paginator look and feel
            string _paginator = " " + this.Pager(
                firstText: "<<",
                previousText: "<",
                nextText: ">",
                lastText: ">>",
                mode: WebGridPagerModes.All
                ).ToHtmlString().Replace("<a href=", "<li><a href=").Replace("</a>", "</a></li>") + " ";
            int _idx = this.PageIndex + 1;
            _paginator = _paginator.Replace(" " + _idx.ToString() + " ", "<li class='active'><a>" + _idx.ToString() + "</a></li>");

            _paginator =
                "<nav><ul class='pagination pagination-small'>"
                + _paginator
                + "</ul></nav>";

            if (noPag)
            {
                _paginator = "";
            }

            //return new HTML code decorated by alert and paginator
            return new HtmlString(alert + strret + _paginator);
        }
    }
}