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
    [RoutePrefix("api/InvDocTypes")]
    public class InvDocTypesController : ApiController
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
                    return Request.CreateResponse(HttpStatusCode.OK, db.InvDocTyp.AsNoTracking().Where(c => (string.IsNullOrEmpty(fm.key) || (c.DocType.ToString().Contains(fm.key) ||
                    c.DocTypeDesc.Contains(fm.key))) && (fm._from.HasValue ? c.DocType >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.DocType <= fm._to.Value : 1 == 1)).OrderBy(o => o.DocType)
                    .Skip(fm._fromIndex).Take(fm._take).ToList().Select(c => new {  c.DocType, c.DocTypeDesc, }));
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
