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
    public class TagHelper
    {
        public static string GetTags(int entityid, string entity)
        {
            var sb = new List<string>();

            var cx = new Entities();
            switch (entity)
            {
                case "Event":
                    foreach (var obj in cx.Tag2Event.Where(e => e.EventId == entityid))
                    {
                        sb.Add(obj.Tag.Tag1);
                    }
                    break;
                case "Company":
                    foreach (var obj in cx.Tag2Company.Where(e => e.CompanyId == entityid))
                    {
                        sb.Add(obj.Tag.Tag1);
                    }
                    break;
                case "NonProfitOrg":
                    foreach (var obj in cx.Tag2NonProfitOrg.Where(e => e.NonProfitOrgId == entityid))
                    {
                        sb.Add(obj.Tag.Tag1);
                    }
                    break;
            }

            return string.Join(",", sb.ToArray());
        }
        public static void SetTags(int entityid, string entity, string tags, Entities cx)
        {
            //collect tagid
            var tagids = new List<int>();
            var tagidsexist = new List<int>();
            string[] tagsarr = tags.Split(',');
            var newtags = new List<string>();
            var currenttags = new List<string>();
            foreach (var obj in cx.Tag.Where(e => tagsarr.Contains(e.Tag1)))
            {
                tagids.Add(obj.Id);
                currenttags.Add(obj.Tag1.ToLower());
            }
            //create new tags if needed and add to collection of tagsid
            foreach(var i in tagsarr)
            {
                if(!currenttags.Contains(i.ToLower()))
                {
                    var nt = new Tag();
                    nt.Tag1 = i;
                    cx.Tag.Add(nt);
                    cx.SaveChanges();
                    tagids.Add(nt.Id);
                }
            }

            switch (entity)
            {
                case "Event":
                    {
                        //delete tags
                        foreach (var obj in cx.Tag2Event.Where(e => e.EventId == entityid))
                        {
                            if (!tagids.Contains(obj.TagId))
                            {
                                cx.Tag2Event.Remove(obj);
                            }
                            else
                            {
                                tagidsexist.Add(obj.TagId);
                            }
                        }
                        cx.SaveChanges();
                        foreach (var obj in tagids)
                        {
                            if (!tagidsexist.Contains(obj))
                            {
                                var i = new Tag2Event();
                                i.EventId = entityid;
                                i.TagId = obj;
                                cx.Tag2Event.Add(i);
                                cx.SaveChanges();
                            }
                        }
                    }
                    break;
                case "Company":
                    {
                        //delete tags
                        foreach (var obj in cx.Tag2Company.Where(e => e.CompanyId == entityid))
                        {
                            if (!tagids.Contains(obj.TagId))
                            {
                                cx.Tag2Company.Remove(obj);
                            }
                            else
                            {
                                tagidsexist.Add(obj.TagId);
                            }
                        }
                        cx.SaveChanges();
                        foreach (var obj in tagids)
                        {
                            if (!tagidsexist.Contains(obj))
                            {
                                var i = new Tag2Company();
                                i.CompanyId = entityid;
                                i.TagId = obj;
                                cx.Tag2Company.Add(i);
                                cx.SaveChanges();
                            }
                        }
                    }
                    break;
                case "NonProfitOrg":
                    {
                        //delete tags
                        foreach (var obj in cx.Tag2NonProfitOrg.Where(e => e.NonProfitOrgId == entityid))
                        {
                            if (!tagids.Contains(obj.TagId))
                            {
                                cx.Tag2NonProfitOrg.Remove(obj);
                            }
                            else
                            {
                                tagidsexist.Add(obj.TagId);
                            }
                        }
                        cx.SaveChanges();
                        foreach (var obj in tagids)
                        {
                            if (!tagidsexist.Contains(obj))
                            {
                                var i = new Tag2NonProfitOrg();
                                i.NonProfitOrgId = entityid;
                                i.TagId = obj;
                                cx.Tag2NonProfitOrg.Add(i);
                                cx.SaveChanges();
                            }
                        }
                    }
                    break;
            }

            // try to delete not used tags
            var inqC = from e in cx.Tag2Company
                       select e.TagId;
            var inqN = from e in cx.Tag2NonProfitOrg
                       select e.TagId;
            var inqE = from e in cx.Tag2Event
                       select e.TagId;

            var tagToDel = from e in cx.Tag
                           where
                                !inqC.Contains(e.Id) &&
                                !inqN.Contains(e.Id) &&
                                !inqE.Contains(e.Id)
                           select e;

            cx.Tag.RemoveRange(tagToDel);
            cx.SaveChanges();
        }
    }
}