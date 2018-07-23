using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NonProfitCRM.Components
{
    public class CompanyStatusHelper
    {
        public enum Status
        {
            COMPANY = 0,
            LEAD = 1,
            LEADDEAD = -1
        }

        public static int[] IsCompany
        {
            get
            {
                return new int[] { (int)Status.COMPANY };
            }
        }
        public static int[] IsLead
        {
            get
            {
                return new int[] { (int)Status.LEAD, (int)Status.LEADDEAD };
            }
        }
    }
}