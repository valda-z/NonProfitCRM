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
using System.Web;

namespace NonProfitCRM.Components
{
    public class TaskTemplate
    {
        public class TaskTemplateItem
        {
            public int BDays { get; set; }
            public string Description { get; set; }

            public TaskTemplateItem(int days, string text)
            {
                BDays = days;
                Description = text;
            }
        }

        private List<TaskTemplateItem> template;
        public TaskTemplate()
        {
            template = new List<TaskTemplateItem>();
            template.Add(new TaskTemplateItem(-5, "Potvrzení činností"));
            template.Add(new TaskTemplateItem(-3, "Pojišťovací seznamy"));
            template.Add(new TaskTemplateItem(-1, "Pojistit"));
            template.Add(new TaskTemplateItem(-1, "Poslední ověření, obědy, BOZP"));
            template.Add(new TaskTemplateItem(1, "Evaluační dotazníky a zpětná vazba neziskovky"));
            template.Add(new TaskTemplateItem(3, "Follow-up a evaluační dotazníky"));
            template.Add(new TaskTemplateItem(5, "Vyhodnocení evaluačních dotazníků"));
            template.Add(new TaskTemplateItem(7, "Fakturace"));
        }

        public List<Task> GetTasks(string username, DateTime date, int entityId)
        {
            var ret = new List<Task>();
            foreach(var obj in template)
            {
                var task = new Task();
                task.AssignedTo = username;
                task.Description = obj.Description;
                task.DueDate = DateTimeExtensions.GetWorkDays(date, obj.BDays);
                task.EntityId = entityId;
                task.Entity = "Event";
                task.StatusId = 0;
                task.Updated = DateTime.UtcNow;
                task.UpdatedBy = username;
                ret.Add(task);
            }
            return ret;
        }
    }
}