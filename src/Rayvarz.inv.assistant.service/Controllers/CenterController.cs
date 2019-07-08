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
    /// سرویس های مرتبط با مراکز هزینه
    /// </summary>
    [RoutePrefix("api/Center")]
    public class CenterController : AdvancedApiController
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
                var grp = string.IsNullOrEmpty(fm.group) ? 0 : short.Parse(fm.group);
                return Ok(db.Centers.AsNoTracking()
                    .Join(db.CenterGrps.AsNoTracking(),oks=>oks.CenterGrp,iks=>iks.CenterGrp1,(c,g)=> new {c.Center1,c.CenterDsc,g.CenterGrpDsc,c.ActvFlg,c.CenterGrp,c.CenterLtnDsc })
                    .Where(c => (string.IsNullOrEmpty(fm.group) || c.CenterGrp == grp) && c.ActvFlg.HasValue && c.ActvFlg.Value == 1 && (string.IsNullOrEmpty(fm.key) || (c.CenterDsc.Contains(fm.key) ||
                c.CenterLtnDsc.Contains(fm.key) ||
                    c.Center1.ToString().Contains(fm.key)) || c.CenterGrpDsc.Contains(fm.key) || (!c.CenterGrp.HasValue || c.CenterGrp.Value.ToString().Contains(fm.key)))
                    && (fm._from.HasValue ? c.Center1 >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.Center1 <= fm._to.Value : 1 == 1)).OrderBy(o => o.Center1)
                    .Skip(fm._fromIndex).Take(fm._take).ToList().Select(c => new { Center = c.Center1,c.CenterDsc,info = c.CenterGrp + " - " + c.CenterGrpDsc }));
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
            using (var repo = new Repo(this, "ray.InvAssistantSp_center_GetList", "Center_GetList_v2", initAsReader: true))
            {
                try
                {
                    repo.ExecuteAdapter();
                    return Ok(repo.ds.Tables[0].AsEnumerable().Select(i => new
                    {
                        Center = Convert.ToInt32(i.Field<object>("Center")),
                        CenterDsc = i.Field<object>("CenterDsc").ToString(),
                        info = i.Field<object>("info").ToString(),
                    }).ToList());
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
    public class centerModel
    {
        public int Center { get; set; }
        public string CenterDsc { get; set; }
        public string info { get; set; } 
    }
}
