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
    [RoutePrefix("api/Units")]
    public class UnitsController : ApiController
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
                    return Request.CreateResponse(HttpStatusCode.OK, db.Unit.AsNoTracking().Where(c => (string.IsNullOrEmpty(fm.key) || (c.UntCode.Contains(fm.key) ||
                    c.UntLtnName.Contains(fm.key) || c.UntName.Contains(fm.key)))
                    /*&& (fm._from.HasValue ? c.UntCode >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.Center1 <= fm._to.Value : 1 == 1)*/).OrderBy(o => o.UntCode)
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
