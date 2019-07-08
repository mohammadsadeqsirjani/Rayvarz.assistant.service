using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rayvarz.invHServ.Api.Models.Common;
using Rayvarz.invHServ.Api.Models.Ray;
namespace Rayvarz.invHServ.Api.Controllers
{
    //[Authorize]
    [RoutePrefix("api/InvOrdrs")]
    public class InvOrdrsController : ApiController
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
                    return Request.CreateResponse(HttpStatusCode.OK, db.InvOrdr.AsNoTracking().Where(c => (string.IsNullOrEmpty(fm.key) || (c.Center1.ToString().Contains(fm.key) ||
                    c.OrdrDsc.Contains(fm.key))) && (fm._from.HasValue ? c.OrdrNO >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.OrdrNO <= fm._to.Value : 1 == 1)).OrderBy(o => o.Center1)
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
    public class invOrdersFilterModel : publicFilterModel
    {
        public string Cstmr { get; set; }

    }
}
