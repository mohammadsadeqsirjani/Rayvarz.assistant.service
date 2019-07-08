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
    /// سرویس های مرتبط با شعب
    /// </summary>
    [RoutePrefix("api/Branch")]
    public class BranchController : AdvancedApiController
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

                return Ok(db.Branches.AsNoTracking().Where(c => (string.IsNullOrEmpty(fm.key) || (c.BranchDsc.Contains(fm.key) ||
                    c.Branch1.ToString().Contains(fm.key) || c.BranchLDsc.Contains(fm.key)))
                    && (fm._from.HasValue ? c.Branch1 >= fm._from.Value : true) && (fm._to.HasValue ? c.Branch1 <= fm._to.Value : true)).OrderBy(o => o.Branch1)
                    .Skip(fm._fromIndex).Take(fm._take).ToList().Select(c => new { Branch = c.Branch1, c.BranchDsc, c.BranchLDsc }));
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
            using (var repo = new Repo(this, "ray.InvAssistantSp_branch_GetList", "Branch_GetList_v2", initAsReader: true))
            {
                try
                { 
                    repo.ExecuteAdapter();
                    return Ok(repo.ds.Tables[0].AsEnumerable().Select(i => new
                    {
                        Branch = Convert.ToInt32(i.Field<object>("Branch")),
                        BranchDsc = i.Field<object>("BranchDsc").ToString(),
                        BranchLDsc = i.Field<object>("BranchLDsc").ToString(),
                    })); 
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }

    //public class BranchModel
    //{
    //    public int Branch { get; set; }
    //    public string BranchDsc { get; set; }
    //    public string BranchLDsc { get; set; }
    //}
}
