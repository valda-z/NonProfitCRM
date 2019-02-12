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
        private static string keyCRM = "statistics-data-crm";
        private static string keyPieEventType = "statistics-data-pieeventtype";
        private static string keyPieNPOEventType = "statistics-data-pienpoeventtype";
        private static string keyEvent = "statistics-data-event";
        private static string keyEventPeople = "statistics-data-eventpeople";
        public static void InvalidateCacheCRM()
        {
            HttpContext.Current.Cache.Remove(keyCRM);
            HttpContext.Current.Cache.Remove(keyPieNPOEventType);
        }
        public static void InvalidateCacheEvent()
        {
            HttpContext.Current.Cache.Remove(keyEvent);
            HttpContext.Current.Cache.Remove(keyPieEventType);
            HttpContext.Current.Cache.Remove(keyPieNPOEventType);
        }
        public static void InvalidateCacheEventPeople()
        {
            HttpContext.Current.Cache.Remove(keyEventPeople);
        }

        public class StatDataPieEventTypeItem
        {
            public string Tag { get; set; }
            public int Count { get; set; }
        }

        public static List<StatDataPieEventTypeItem> GetPieEventType()
        {
            var cx = new Entities();

            List<StatDataPieEventTypeItem> ret = 
                (List<StatDataPieEventTypeItem>)HttpContext.Current.Cache[keyPieEventType];
            if (ret == null)
            {

                ret = new List<StatDataPieEventTypeItem>();

                var dFrom = new DateTime(DateTime.Now.Year, 1, 1);
                var dTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                var query = from p in cx.ViewStatEventType
                            where p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                            group p by p.Tag into g
                            select new
                            {
                                tag = g.Key,
                                count = g.Count()
                            };
                foreach(var x in query)
                {
                    var i = new StatDataPieEventTypeItem();
                    i.Tag = x.tag.Trim();
                    i.Count = x.count;
                    ret.Add(i);
                }

                HttpContext.Current.Cache.Insert(keyPieEventType, ret,
                                   null, DateTime.Now.AddMinutes(10d),
                                   System.Web.Caching.Cache.NoSlidingExpiration);

            }
            return ret;
        }

        public static List<StatDataPieEventTypeItem> GetPieNPOEventType()
        {
            var cx = new Entities();

            List<StatDataPieEventTypeItem> ret =
                (List<StatDataPieEventTypeItem>)HttpContext.Current.Cache[keyPieNPOEventType];
            if (ret == null)
            {

                ret = new List<StatDataPieEventTypeItem>();

                var dFrom = new DateTime(DateTime.Now.Year, 1, 1);
                var dTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                var query = from p in cx.ViewStatEventNonProfitOrgType
                            where p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                            group p by p.Tag into g
                            select new
                            {
                                tag = g.Key,
                                count = g.Count()
                            };
                foreach (var x in query)
                {
                    var i = new StatDataPieEventTypeItem();
                    i.Tag = x.tag.Trim();
                    i.Count = x.count;
                    ret.Add(i);
                }

                HttpContext.Current.Cache.Insert(keyPieNPOEventType, ret,
                                   null, DateTime.Now.AddMinutes(10d),
                                   System.Web.Caching.Cache.NoSlidingExpiration);

            }
            return ret;
        }

        public class StatDataCRM
        {
            public int CompanyCount { get; set; }
            public int LeadCount { get; set; }
            public int InactiveCount { get; set; }
        }

        public static StatDataCRM GetCRMStatWidget()
        {
            var cx = new Entities();

            StatDataCRM ret = (StatDataCRM)HttpContext.Current.Cache[keyCRM];
            if (ret == null)
            {

                ret = new StatDataCRM();

                ret.CompanyCount = (from p in cx.Company
                                    where !p.Deleted && p.StatusId == ((int)CompanyStatusHelper.Status.COMPANY_APPL)
                                    select p).Count();

                ret.LeadCount = (from p in cx.Company
                                 where !p.Deleted && p.StatusId == ((int)CompanyStatusHelper.Status.LEAD)
                                 select p).Count();

                ret.InactiveCount = (from p in cx.Company
                                     where !p.Deleted && p.StatusId == ((int)CompanyStatusHelper.Status.LEADDEAD)
                                     select p).Count();

                HttpContext.Current.Cache.Insert(keyCRM, ret,
                                   null, DateTime.Now.AddMinutes(10d),
                                   System.Web.Caching.Cache.NoSlidingExpiration);

            }
            return ret;
        }

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

            StatDataWidget ret = (StatDataWidget)HttpContext.Current.Cache[keyEvent];
            if (ret == null)
            {
                ret = new StatDataWidget();

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

                HttpContext.Current.Cache.Insert(keyEvent, ret,
                                   null, DateTime.Now.AddMinutes(10d),
                                   System.Web.Caching.Cache.NoSlidingExpiration);
            }
            return ret;
        }
        public static StatDataWidget GetEventPeopleStatWidget()
        {
            var cx = new Entities();

            StatDataWidget ret = (StatDataWidget)HttpContext.Current.Cache[keyEventPeople];
            if (ret == null)
            {
                ret = new StatDataWidget();

                ret.Year = DateTime.Now.Year;

                var dFrom = new DateTime(DateTime.Now.Year, 1, 1);
                var dTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var dFromLY = dFrom.AddYears(-1);
                var dToLY = dTo.AddYears(-1);

                ret.CountLastYear = (from p in cx.Event
                                     where !p.Deleted && p.DateOfEvent >= dFromLY && p.DateOfEvent < dToLY
                                     select p.Capacity).DefaultIfEmpty(0).Sum();

                ret.CountThisYear = (from p in cx.Event
                                     where !p.Deleted && p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                                     select p.Capacity).DefaultIfEmpty(0).Sum();


                dFrom = new DateTime(DateTime.Now.Year, 1, 1);
                dTo = dFrom.AddMonths(3);
                ret.Q1Count = (from p in cx.Event
                               where !p.Deleted && p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                               select p.Capacity).DefaultIfEmpty(0).Sum();
                dFrom = dFrom.AddMonths(3);
                dTo = dFrom.AddMonths(3);
                ret.Q2Count = (from p in cx.Event
                               where !p.Deleted && p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                               select p.Capacity).DefaultIfEmpty(0).Sum();
                dFrom = dFrom.AddMonths(3);
                dTo = dFrom.AddMonths(3);
                ret.Q3Count = (from p in cx.Event
                               where !p.Deleted && p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                               select p.Capacity).DefaultIfEmpty(0).Sum();
                dFrom = dFrom.AddMonths(3);
                dTo = dFrom.AddMonths(3);
                ret.Q4Count = (from p in cx.Event
                               where !p.Deleted && p.DateOfEvent >= dFrom && p.DateOfEvent < dTo
                               select p.Capacity).DefaultIfEmpty(0).Sum();

                HttpContext.Current.Cache.Insert(keyEventPeople, ret,
                                   null, DateTime.Now.AddMinutes(10d),
                                   System.Web.Caching.Cache.NoSlidingExpiration);
            }
            return ret;
        }
    }
}