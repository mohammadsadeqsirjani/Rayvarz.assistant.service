
using Rayvarz.inv.assistant.service.Models;
using Rayvarz.inv.assistant.service.Models.ray;
using Rayvarz.inv.assistant.service.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rayvarz.inv.assistant.service.Controllers
{
    /// <summary>
    /// سرویس های مرتبط با شماره سفارش
    /// </summary>
    [RoutePrefix("api/InvOrdr")]
    public class InvOrdrController : AdvancedApiController
    {
        /// <summary>
        /// دریافت لیست
        /// </summary>
        /// <param name="fm"></param>
        /// <returns></returns>
        [Route("GetList")]
        [Exception]
        [HttpPost]
        public IHttpActionResult GetList([FromBody] publicFilterModel fm)
        {
            using (var db = new Entities())
            {
                db.Configuration.ProxyCreationEnabled = false;

                return Ok(db.InvOrdrs.AsNoTracking().Where(c => /*c.Active.HasValue && c.Active.Value == 1 &&*/ (string.IsNullOrEmpty(fm.key) || (c.OrdrDsc.Contains(fm.key) ||
                    c.OrdrNO.ToString().Contains(fm.key)))
                    && (fm._from.HasValue ? c.OrdrNO >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.OrdrNO <= fm._to.Value : 1 == 1)).OrderBy(o => o.OrdrNO)
                    .Skip(fm._fromIndex).Take(fm._take).ToList().Select(c => new { c.OrdrNO, c.OrdrDsc, c.Active }));
            }
        }

        [Route("GetList/v2")]
        [Exception]
        [HttpPost]
        public IHttpActionResult GetList_v2([FromBody] publicFilterModel fm)
        {
            if (!ModelState.IsValid)
                return new BadRequestActionResult(ModelState.Values);

            filterModel = fm;
            using (var repo = new Repo(this, "ray.InvAssistantSp_invOrdr_GetList", "InvOrdr_GetList_v2", initAsReader: true))
            {
                try
                {
                    repo.ExecuteAdapter();
                    return Ok(repo.ds.Tables[0].AsEnumerable().Select(i => new
                    {
                        OrdrNO = Convert.ToInt32(i.Field<object>("OrdrNO")),
                        OrdrDsc = i.Field<object>("OrdrDsc").ToString(),
                        Active = Convert.ToInt16(i.Field<object>("Active")),
                    }).ToList());                    
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        private IHttpActionResult NotFoundActionResult(string v)
        {
            throw new NotImplementedException();
        }
    }

    public class InvOrderModel
    {
        public long OrdrNO { get; set; }
        public string OrdrDsc { get; set; }
        public short Active { get; set; }
    }
}
