using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NonProfitCRM.Models
{
    public class NavBarModel
    {
        public class NavBarItem
        {
            public NavBarItem(string icon, string name, string controller, string action)
            {
                Icon = icon;
                Name = name;
                Controller = controller;
                Action = action;
            }
            public NavBarItem(string icon, string name)
            {
                Icon = icon;
                Name = name;
                Controller = "";
                Action = "";
            }
            public string Icon { get; set; }
            public string Name { get; set; }
            public string Controller { get; set; }
            public string Action { get; set; }
            public List<NavBarSubItem> SubItems = new List<NavBarSubItem>();
            private Hashtable subht = new Hashtable();
            public void AddSubitem(NavBarSubItem subitem)
            {
                SubItems.Add(subitem);
                subht.Add(subitem.Controller + "|" + subitem.Action, subitem.Name);
            }
            public bool IsActive(string controller, string action)
            {
                bool ret = subht.ContainsKey(controller + "|" + action);
                if (!ret)
                {
                    ret = subht.ContainsKey(controller + "|" + action.Replace("Detail", "s"));
                    if (!ret)
                    {
                        ret = subht.ContainsKey(controller + "|" + action.Replace("Detail", ""));
                        if (!ret && action == "Detail")
                        {
                            ret = subht.ContainsKey(controller + "|List");
                        }
                    }
                }
                return ret;
            }
            public bool IsActiveSubItem(NavBarSubItem subitem, string controller, string action)
            {
                return (controller == subitem.Controller
                    &&
                    (subitem.Action == action.Replace("Detail", "s")
                        || subitem.Action == action.Replace("Detail", "")
                        || (subitem.Action == "List" && action == "Detail")));
            }
        }
        public class NavBarSubItem
        {
            public NavBarSubItem(string name, string controller, string action)
            {
                Name = name;
                Controller = controller;
                Action = action;
            }
            public string Name { get; set; }
            public string Controller { get; set; }
            public string Action { get; set; }
        }
        public List<NavBarItem> Items = new List<NavBarItem>();
        public void AddItem(NavBarItem item){
            if (item.Controller == "")
            {
                sidebarID++;
                item.Controller = "idx-" + sidebarID.ToString();
            }
            Items.Add(item);
        }
        public string CurrentController { get; set; }
        public string CurrentAction { get; set; }
        private int sidebarID = 0;
    }
}