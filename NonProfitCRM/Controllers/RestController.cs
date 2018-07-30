using NonProfitCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NonProfitCRM.Controllers
{
    public class RestController : ApiController
    {
        // GET: api/Rest/Tag
        [HttpGet]
        public IEnumerable<string> Tag()
        {
            var model = new Entities().Tag.OrderBy(e => e.Tag1);
            var ret = new List<string>();
            foreach(var item in model)
            {
                ret.Add(item.Tag1);
            }

            return ret;
        }
    }
}
