
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
    /// سرویس های مرتبط با موجودیت علت برگشت به انبار
    /// </summary>
    [RoutePrefix("api/InvRtrnStr")]
    public class InvRtrnStrController : AdvancedApiController
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

                return Ok(db.InvRtrnStrs.AsNoTracking().Where(c => (string.IsNullOrEmpty(fm.key) || (c.RtrnStore.ToString().Contains(fm.key) ||
                    c.RtrnStoreDesc.Contains(fm.key)))
                    /*&& (fm._from.HasValue ? c.UntCode >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.Center1 <= fm._to.Value : 1 == 1)*/).OrderBy(o => o.RtrnStore)
                    .Skip(fm._fromIndex).Take(fm._take).ToList().Select(c => new { c.RtrnStore, c.RtrnStoreDesc }));
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
            using (var repo = new Repo(this, "ray.InvAssistantSp_invRtrnStr_GetList", "InvRtrnStr_GetList_v2", initAsReader: true))
            {
                try
                {
                    repo.ExecuteAdapter();
                    return Ok(repo.ds.Tables[0].AsEnumerable().Select(i => new
                    {
                        RtrnStore = Convert.ToInt16(i.Field<object>("RtrnStore")),
                        RtrnStoreDesc = i.Field<object>("RtrnStoreDesc").dbNullCheckString(),
                    }).ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }

    public class InvRtrnStrModel
    {
        public short RtrnStore { get; set; }
        public string RtrnStoreDesc { get; set; }
    }
}
