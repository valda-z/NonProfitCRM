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

﻿using NonProfitCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NonProfitCRM.Components
{
    public class StatisticsHelper
    {
        public class StatDataWidget
        {
            public int Year { get; set; }
            public int CountThisYear { get; set; }
            public int CountLastYear { get; set; }
            public int Q1Count { get; set; }
            public int Q2Count { get; set; }
            public int Q3Count { get; set; }
            public int Q4Count { get; set; }
        }

        public static StatDataWidget GetEventStatWidget()
        {
            var cx = new Entities();

            var ret = new StatDataWidget();

            ret.Year = DateTime.Now.Year;

            var dFrom = new DateTime(DateTime.Now.Year, 1, 1);
            var dTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var dFromLY = dFrom.AddYears(-1);
            var dToLY = dTo.AddYears(-1);

            ret.CountLastYear = (from p in cx.Event
                                 where !p.Deleted && p.DateOfEvent >= dFromLY && p.DateOfEvent < dToLY
                                 select p).Count();

            ret.CountThisYear = (from p in cx.Event
                                 where !p.Deleted && p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                                 select p).Count();


            dFrom = new DateTime(DateTime.Now.Year, 1, 1);
            dTo = dFrom.AddMonths(3);
            ret.Q1Count = (from p in cx.Event
                           where !p.Deleted && p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                           select p).Count();
            dFrom = dFrom.AddMonths(3);
            dTo = dFrom.AddMonths(3);
            ret.Q2Count = (from p in cx.Event
                           where !p.Deleted && p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                           select p).Count();
            dFrom = dFrom.AddMonths(3);
            dTo = dFrom.AddMonths(3);
            ret.Q3Count = (from p in cx.Event
                           where !p.Deleted && p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                           select p).Count();
            dFrom = dFrom.AddMonths(3);
            dTo = dFrom.AddMonths(3);
            ret.Q4Count = (from p in cx.Event
                           where !p.Deleted && p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                           select p).Count();
            return ret;
        }
        public static StatDataWidget GetEventPeopleStatWidget()
        {
            var cx = new Entities();

            var ret = new StatDataWidget();

            ret.Year = DateTime.Now.Year;

            var dFrom = new DateTime(DateTime.Now.Year, 1, 1);
            var dTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var dFromLY = dFrom.AddYears(-1);
            var dToLY = dTo.AddYears(-1);

            ret.CountLastYear = (from p in cx.Event
                                 where !p.Deleted && p.DateOfEvent >= dFromLY && p.DateOfEvent < dToLY
                                 select p.Capacity).Sum();

            ret.CountThisYear = (from p in cx.Event
                                 where !p.Deleted && p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                                 select p.Capacity).Sum();


            dFrom = new DateTime(DateTime.Now.Year, 1, 1);
            dTo = dFrom.AddMonths(3);
            ret.Q1Count = (from p in cx.Event
                           where !p.Deleted && p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                           select p.Capacity).Sum();
            dFrom = dFrom.AddMonths(3);
            dTo = dFrom.AddMonths(3);
            ret.Q2Count = (from p in cx.Event
                           where !p.Deleted && p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                           select p.Capacity).Sum();
            dFrom = dFrom.AddMonths(3);
            dTo = dFrom.AddMonths(3);
            ret.Q3Count = (from p in cx.Event
                           where !p.Deleted && p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                           select p.Capacity).Sum();
            dFrom = dFrom.AddMonths(3);
            dTo = dFrom.AddMonths(3);
            ret.Q4Count = (from p in cx.Event
                           where !p.Deleted && p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                           select p.Capacity).Sum();
            return ret;
        }
    }
}