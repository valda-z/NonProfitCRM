using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NonProfitCRM.Models
{
    public class HomeModel
    {
        public IEnumerable<ViewTaskList> TaskList { get; set; }
        public IEnumerable<ViewEventList> EventList { get; set; }
        public IEnumerable<ViewCompanyList> CompanyList { get; set; }
        public IEnumerable<ViewNonProfitOrgList> NonProfitOrgList { get; set; }
        public string Search { get; set; }

    }
}