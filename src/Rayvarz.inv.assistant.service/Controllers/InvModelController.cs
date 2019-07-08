using Rayvarz.inv.assistant.service.Models;
using Rayvarz.inv.assistant.service.Models.ray;
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
    /// سرویس های مرتبط با جدول مدل
    /// </summary>
    [RoutePrefix("api/InvModel")]
    public class InvModelController : AdvancedApiController
    {
        /// <summary>
        /// دریافت لیست
        /// </summary>
        /// <param name="fm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        //[Route("GetList")]
        public HttpResponseMessage GetList([FromBody] publicFilterModel fm)
        {
            try
            {
                using (var db = new Entities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    return Request.CreateResponse(HttpStatusCode.OK, db.InvModels.AsNoTracking()
                        .Where(c => (string.IsNullOrEmpty(fm.key) || c.Model.Contains(fm.key) ||  c.ModelDsc.Contains(fm.key) )
                     /*&& (fm._from.HasValue ? c.Center1 >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.Center1 <= fm._to.Value : 1 == 1)*/).OrderBy(o => o.Model)
                    .Skip(fm._fromIndex).Take(fm._take).ToList().
                    Select(c => new { c.Model,c.ModelDsc }));
                }
            }
            catch (Exception ex)
            {
                /*new HttpResponseMessage().Content*/
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
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
            using (var repo = new Repo(this, "ray.InvAssistantSp_invModel_GetList", "InvModel_GetList_v2", initAsReader: true))
            {
                try
                {
                    repo.ExecuteAdapter();
                    return Ok(repo.ds.Tables[0].AsEnumerable().Select(i => new
                    {
                        Model = i.Field<object>("Model").ToString(),
                        ModelDsc = i.Field<object>("ModelDsc").ToString(),
                    }).ToList());
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
    public class InvModelModel
    {
        public string Model { get; set; }
        public string ModelDsc { get; set; }
    }
}
