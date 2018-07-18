using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace NonProfitCRM.Components.Http
{
    public static class ConnectionHelper
    {
        public static string GetUserIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                var ctx = request.Properties["MS_HttpContext"] as HttpContextBase;
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            return null;
        }
    }
}