
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
    /// سرویس های مرتبط با موجودیت واحد شمارش
    /// </summary>
    [RoutePrefix("api/Unit")]
    public class UnitController : AdvancedApiController
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

                return Ok(db.Units.AsNoTracking().Where(c => (string.IsNullOrEmpty(fm.key) || (c.UntCode.Contains(fm.key) ||
                    c.UntLtnName.Contains(fm.key) || c.UntName.Contains(fm.key)))
                    /*&& (fm._from.HasValue ? c.UntCode >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.Center1 <= fm._to.Value : 1 == 1)*/).OrderBy(o => o.UntCode)
                    .Skip(fm._fromIndex).Take(fm._take).ToList().Select(c => new { c.UntCode, c.UntName }));
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
            using (var repo = new Repo(this, "ray.InvAssistantSp_unit_GetList", "Unit_GetList_v2", initAsReader: true))
            {
                try
                {
                    repo.ExecuteAdapter();
                    return Ok(repo.ds.Tables[0].AsEnumerable().Select(i => new
                    {
                        UntCode = i.Field<object>("UntCode").dbNullCheckString(),
                        UntName = i.Field<object>("UntName").dbNullCheckString(),
                    }));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
    public class UnitModel
    {
        public string UntCode { get; set; }
        public string UntName { get; set; }
    }
}
