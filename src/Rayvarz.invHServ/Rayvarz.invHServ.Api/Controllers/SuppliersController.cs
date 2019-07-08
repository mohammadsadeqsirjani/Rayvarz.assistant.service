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
    [RoutePrefix("api/Suppliers")]
    public class SuppliersController : ApiController
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
                    return Request.CreateResponse(HttpStatusCode.OK, db.Supplier.AsNoTracking().Where(c => (string.IsNullOrEmpty(fm.key) || (c.Supplier1.Contains(fm.key) ||
                     c.SupFirstName.Contains(fm.key) || c.SupLastName.Contains(fm.key) || c.SupLtnName.Contains(fm.key) || c.SupName.Contains(fm.key)))
                     /*&& (fm._from.HasValue ? c.Center1 >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.Center1 <= fm._to.Value : 1 == 1)*/).OrderBy(o => o.Supplier1)
                    .Skip(fm._fromIndex).Take(fm._take).ToList().Select(c => new { Supplier = c.Supplier1, c.SupName, c.SupFirstName, c.SupLastName, c.SupLtnName }));
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
