using Rayvarz.invHServ.Api.Models.Common;
using Rayvarz.invHServ.Api.Models.Ray;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rayvarz.invHServ.Api.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Centers")]
    public class CentersController : ApiController
    {
        [HttpPost]
        [Route("GetList")]
        //[Route("GetList")]
        public HttpResponseMessage GetList([FromBody] publicFilterModel fm)
        {
            try
            {
                using (var db = new RayvarzEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    return Request.CreateResponse(HttpStatusCode.OK, db.Center.AsNoTracking().Where(c => (string.IsNullOrEmpty(fm.key) || (c.Center1.ToString().Contains(fm.key) ||
                    c.CenterDsc.Contains(fm.key))) && (fm._from.HasValue ? c.Center1 >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.Center1 <= fm._to.Value : 1 == 1)).OrderBy(o => o.Center1)
                    .Skip(fm._fromIndex).Take(fm._take).ToList().Select(c => new { Center = c.Center1, c.CenterDsc, c.CenterGrp }));
                }
            }
            catch (Exception ex)
            {
                /*new HttpResponseMessage().Content*/
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }
        

    }
}
