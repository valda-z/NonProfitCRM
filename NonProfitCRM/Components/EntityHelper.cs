using NonProfitCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NonProfitCRM.Components
{
    public class EntityHelper
    {
        public static string GetEntityName(string entity, int id)
        {
            string ret = "";

            var cx = new Entities();
            switch (entity)
            {
                case "Event":
                    ret = cx.Event.Single(e => e.Id == id).Name;
                    break;
                case "Company":
                    ret = cx.Company.Single(e => e.Id == id).Name;
                    break;
                case "NonProfitOrg":
                    ret = cx.NonProfitOrg.Single(e => e.Id == id).Name;
                    break;
            }

            return ret;
        }
    }
}