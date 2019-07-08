using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rayvarz.invHServ.Api.Models.Common;
using Rayvarz.invHServ.Api.Models.Ray;
using System.Threading.Tasks;

namespace Rayvarz.invHServ.Api.Controllers
{//[Authorize]
    [RoutePrefix("api/ItemDatas")]
    public class ItemDatasController : ApiController
    {
        [HttpPost]
        [Route("GetList")]
        //[Route("GetList")]
        public HttpResponseMessage GetList([FromBody] itemDataFilterModel fm)
        {
            try
            {
                using (var db = new RayvarzEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    return Request.CreateResponse(HttpStatusCode.OK, db.ItemData.AsNoTracking()
                        .Where(c => (string.IsNullOrEmpty(fm.key) || (c.PartNo.ToString().Contains(fm.key) ||
                    c.PartNoDsc.Contains(fm.key))) /*&& (fm._from.HasValue ? c.PartNo >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.Center1 <= fm._to.Value : 1 == 1)*/
                    && (string.IsNullOrEmpty(fm.barcode) || c.PartNo.Contains(fm.barcode))
                    && ((fm.partNos == null || fm.partNos.Count == 0) || fm.partNos.Contains(c.PartNo))
                    ).OrderBy(o => o.PartNo)
                    .Skip(fm._fromIndex).Take(fm._take).ToList()
                    .Select(c => new { c.PartNo, c.PartNoDsc, c.PartGrp, c.UntCode }));
                }
            }
            catch (Exception ex)
            {
                /*new HttpResponseMessage().Content*/
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        public async Task<IHttpActionResult> getItemHdr([FromBody] itemDataFilterModel fm)
        {
            try
            {
                //return Ok();
                //return NotFound();
                //return Unauthorized();
            }
            catch(Exception ex)
            {
                 
            }
        }
    }
    public class itemDataFilterModel : publicFilterModel
    {
        public string barcode { get; set; }
        public List<string> partNos { get; set; }
    }
}
