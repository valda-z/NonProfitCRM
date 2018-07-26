using NonProfitCRM.Models;
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