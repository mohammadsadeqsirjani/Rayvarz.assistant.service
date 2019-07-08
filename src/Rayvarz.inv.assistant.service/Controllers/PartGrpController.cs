
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
    /// سرویس های مرتبط با گروه کالایی
    /// </summary>
    [RoutePrefix("api/PartGrp")]
    public class PartGrpController : AdvancedApiController
    {
        /// <summary>
        /// دریافت لیست گروه هایی کالایی
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

                return Ok(db.InvAssistantVw_getPartGrpFullPath.Where(pg => string.IsNullOrEmpty(fm.key) || pg.PartGrpDsc.Contains(fm.key) || pg.PartGrp.Contains(fm.key))
                .OrderBy(o => o.PartGrp).Skip(fm._fromIndex).Take(fm._take).ToList().Select(c => c).Select(s => new { s.PartGrp, PartGrpDsc = s.PartGrpDsc.Trim(), FullPath = s.FullPath.Trim() }));

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
            using (var repo = new Repo(this, "ray.InvAssistantSp_partGrp_GetList", "PartGrp_GetList_v2", initAsReader: true))
            {
                try
                {
                    repo.ExecuteAdapter();
                    return Ok(repo.ds.Tables[0].AsEnumerable().Select(i => new
                    {
                        PartGrp = i.Field<object>("PartGrp").dbNullCheckString(),
                        PartGrpDsc = i.Field<object>("PartGrpDsc").dbNullCheckString(),
                        FullPath = i.Field<object>("FullPath").dbNullCheckString(),
                    }).ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        public class PartGrpModel
        {
            public string PartGrp { get; set; }
            public string PartGrpDsc { get; set; }
            public string FullPath { get; set; }
        }
    }
}

