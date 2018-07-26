using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NonProfitCRM.Models
{
    public class HomeModel
    {
        public IEnumerable<ViewTaskList> TaskList { get; set; }
    }
}