using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NonProfitCRM.Components
{
    public class SystemHelper
    {
        public static string GetAssemblyName()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

    }
}