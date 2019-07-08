using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rayvarz.invHServ.Api.Models.Common;
using Rayvarz.invHServ.Api.Models.Ray;
namespace Rayvarz.invHServ.Api.Controllers
{//[Authorize]
    [RoutePrefix("api/InvPrds")]
    public class InvPrdsController : ApiController
    {[HttpPost]
        [Route("GetList")]
        //[ActionName("GetList")]
        public HttpResponseMessage GetList([FromBody] publicFilterModel fm)
        {
            try
            {
                using (var db = new RayvarzEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    return Request.CreateResponse(HttpStatusCode.OK, db.InvPrd.AsNoTracking()/*.Where(c =>
                   (fm._from.HasValue ? c.Center1 >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.Center1 <= fm._to.Value : 1 == 1))*/.OrderBy(o => o.StartFiscalYear)
                    .Skip(fm._fromIndex).Take(fm._take).ToList().Select(c => c));
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
