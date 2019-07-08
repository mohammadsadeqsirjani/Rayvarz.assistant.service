
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
    /// سرویس های مرتبط با نوع درخواست
    /// </summary>
    [RoutePrefix("api/InvReqTyp")]
    public class InvReqTypController : AdvancedApiController
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

                return Ok(db.InvReqTyps.AsNoTracking().Where(c => (string.IsNullOrEmpty(fm.key) || (c.ReqType.ToString().Contains(fm.key) ||
                    c.ReqTypeDesc.Contains(fm.key) || c.ReqTypeLtnDesc.Contains(fm.key)))
                   /* && (fm._from.HasValue ? c.Branch1 >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.Branch1 <= fm._to.Value : 1 == 1)*/).OrderBy(o => o.ReqType)
                    .Skip(fm._fromIndex).Take(fm._take).ToList().Select(c => new { c.ReqType,c.ReqTypeDesc,c.ReqTypeLtnDesc }));
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
            using (var repo = new Repo(this, "ray.InvAssistantSp_invReqTyp_GetList", "InvReqTyp_GetList_v2", initAsReader: true))
            {
                try
                {
                    repo.ExecuteAdapter();
                    return Ok(repo.ds.Tables[0].AsEnumerable().Select(i => new
                    {
                        ReqType = Convert.ToInt16(i.Field<object>("ReqType")),
                        ReqTypeDesc = i.Field<object>("ReqTypeDesc").dbNullCheckString(),
                        ReqTypeLtnDesc = i.Field<object>("ReqTypeLtnDesc").dbNullCheckString()
                    }).ToList()); ;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        } 
    }

    public class InvReqTypModel
    {
        public short ReqType { get; set; }
        public string ReqTypeDesc { get; set; }
        public string ReqTypeLtnDesc { get; set; }
    }
}
