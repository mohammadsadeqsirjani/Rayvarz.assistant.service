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
    /// نوع مصارف
    /// </summary>
    [RoutePrefix("api/InvCnsTyp")]
    public class InvCnsTypController : AdvancedApiController
    {
        //        select top 30000 a.ConsType,a.ConsTypeDesc,a.Account,b.AccountDsc,a.AstDoctyp,c.DocTypDsc
        //from ray.InvCnsTyp a
        //left join ray.Account b on b.Account=a.Account
        //left join ray.AstDoctyp c on c.doctyp= a.AstDoctyp
        //ORDER BY a.Account
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
            if (!ModelState.IsValid)
            {
                return new BadRequestActionResult(ModelState.Values);
            }
            using (var db = new Entities())
            {
                db.Configuration.ProxyCreationEnabled = false;

                return Ok(db.InvCnsTyps.AsNoTracking()
                    //.Join(db.Units.AsNoTracking(), oks => oks.UntCode, iks => iks.UntCode, (I, U) => new { I.NationalPartCod, I.TechnicalNo, U.UntName, I.PartNo, I.PartGrp, I.PartNoDsc, I.PartLtnDsc, I.NotActiv, I.UntCode })
                    //.GroupJoin(db.vw_partGrp_fullPath.AsNoTracking(), oks => oks.PartGrp, iks => iks.PartGrp, (i, g) => new { i.NationalPartCod, i.TechnicalNo, i.PartGrp, i.PartNo, i.PartNoDsc, i.PartLtnDsc, i.NotActiv, i.UntCode, i.UntName, grps = g.DefaultIfEmpty() })
                    .Where(c => (string.IsNullOrEmpty(fm.key) || (c.ConsType.ToString().Contains(fm.key) ||
                c.ConsTypeDesc.Contains(fm.key)))
                    && (fm._from.HasValue ? c.ConsType >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.ConsType <= fm._to.Value : 1 == 1)).OrderBy(o => o.ConsType)
                    .Skip(fm._fromIndex).Take(fm._take).ToList().Select(c => new { c.ConsType, c.ConsTypeDesc }));
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
            using (var repo = new Repo(this, "ray.InvAssistantSp_invCnsTyp_GetList", "InvCnsTyp_GetList_v2", initAsReader: true))
            {
                try
                {
                    repo.ExecuteAdapter();
                    return Ok(repo.ds.Tables[0].AsEnumerable().Select(i => new
                    {
                        ConsType = Convert.ToInt32(i.Field<object>("ConsType")),
                        ConsTypeDesc = i.Field<object>("ConsTypeDesc").ToString(),
                    }).ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }

    public class InvCnsTypModel
    {
        public int ConsType { get; set; }
        public string ConsTypeDesc { get; set; }
    }
}
