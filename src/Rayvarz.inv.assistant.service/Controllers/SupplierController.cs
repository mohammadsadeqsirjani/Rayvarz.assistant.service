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
    /// سرویس های مرتبط با جدول تامین کنندگان
    /// </summary>
    [RoutePrefix("api/Supplier")]
    public class SupplierController : AdvancedApiController
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
                    return Request.CreateResponse(HttpStatusCode.OK, db.Suppliers.AsNoTracking().Where(c => (string.IsNullOrEmpty(fm.key) || (c.Supplier1.Contains(fm.key) ||
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

        [Route("GetList/v2")]
        [Exception]
        [HttpPost]
        public IHttpActionResult GetList_v2([FromBody] publicFilterModel fm)
        {
            if (!ModelState.IsValid)
                return new BadRequestActionResult(ModelState.Values);

            filterModel = fm;
            using (var repo = new Repo(this, "ray.InvAssistantSp_supplier_GetList", "Supplier_GetList_v2", initAsReader: true))
            {
                try
                {
                    repo.ExecuteAdapter();
                    return Ok(repo.ds.Tables[0].AsEnumerable().Select(i => new
                    {
                        Supplier = i.Field<object>("Supplier").dbNullCheckString(),
                        SupName = i.Field<object>("SupName").dbNullCheckString(),
                        SupFirstName = i.Field<object>("SupFirstName").dbNullCheckString(),
                        SupLastName = i.Field<object>("SupLastName").dbNullCheckString(),
                        SupLtnName = i.Field<object>("SupLtnName").dbNullCheckString(),
                    }).ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }

    public class SupplierModel
    {
        public string Supplier { get; set; }
        public string SupName { get; set; }
        public string SupFirstName { get; set; }
        public string SupLastName { get; set; }
        public string SupLtnName { get; set; }
    }
}
