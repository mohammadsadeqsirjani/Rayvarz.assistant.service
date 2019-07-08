
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
    /// نوع رسید ها
    /// </summary>
    [RoutePrefix("api/InvRcptTyp")]
    public class InvRcptTypController : AdvancedApiController
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

                return Ok(db.InvRcptTyps.AsNoTracking().Where(c => (string.IsNullOrEmpty(fm.key) || (c.RcptTypeDesc.Contains(fm.key) ||
                    c.RcptType.ToString().Contains(fm.key)))
                   /* && (fm._from.HasValue ? c.Branch1 >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.Branch1 <= fm._to.Value : 1 == 1)*/).OrderBy(o => o.RcptType)
                    .Skip(fm._fromIndex).Take(fm._take).ToList().Select(c => new { c.RcptType, c.RcptTypeDesc }));
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
            using (var repo = new Repo(this, "ray.InvAssistantSp_invRcptTyp_GetList", "InvRcptTyp_GetList_v2", initAsReader: true))
            {
                try
                {
                    repo.ExecuteAdapter();
                    return Ok(repo.ds.Tables[0].AsEnumerable().Select(i => new
                    {
                        RcptType = Convert.ToInt32(i.Field<object>("RcptType")),
                        RcptTypeDesc = i.Field<object>("RcptTypeDesc").ToString(),
                    }).ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }

    public class InvRcptTypModel
    {
        public int RcptType { get; set; }
        public string RcptTypeDesc { get; set; }
    }
}

