
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
    /// سرویس های مرتبط با موجودیت انبار
    /// </summary>
    [RoutePrefix("api/Store")]
    public class StoreController : AdvancedApiController
    {
        /// <summary>
        /// دریافت لیست
        /// depricated. please use the latest version
        /// </summary>
        /// <param name="fm"></param>
        /// <returns></returns>
        [Route("GetList")]
        [Exception]
        [HttpPost]
        [Obsolete("depricated. please use the latest version")]
        public IHttpActionResult GetList([FromBody] publicFilterModel fm)
        {
            using (var db = new Entities())
            {
                db.Configuration.ProxyCreationEnabled = false;

                return Ok(db.Stores.AsNoTracking().Where(c => (string.IsNullOrEmpty(fm.key) || (c.StoreDsc.Contains(fm.key) ||
                    c.StoreNo.Contains(fm.key) || c.StoreLtnDsc.Contains(fm.key)))
                   /* && (fm._from.HasValue ? c.Branch1 >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.Branch1 <= fm._to.Value : 1 == 1)*/).OrderBy(o => o.StoreNo)
                    .Skip(fm._fromIndex).Take(fm._take).ToList().Select(c => new { c.StoreNo, c.StoreDsc, c.StoreLtnDsc, c.Active }));
            }
        }

        /// <summary>
        /// دریافت لیست
        /// </summary>
        /// <param name="fm"></param>
        /// <returns></returns>
        [Route("GetList/v2")]
        [Exception]
        [HttpPost]
        public IHttpActionResult GetList_v2([FromBody] storeGetListFilterModel fm)
        {
            if (!ModelState.IsValid)
                return new BadRequestActionResult(ModelState.Values);
            filterModel = fm;
            using (var repo = new Repo(this, "ray.InvAssistantSp_Store_GetList", "Store_GetList_v2", initAsReader: true))
            {
                repo.cmd.Parameters.AddWithValue("@f_branch", fm.f_branch.getDbValue());
                repo.cmd.Parameters.AddWithValue("@f_storeNo", fm.f_storeNo.getDbValue());
                repo.cmd.Parameters.AddWithValue("@f_userId", fm.f_userId.getDbValue());

                repo.ExecuteAdapter();
                return Ok(
                    repo.ds.Tables[0].AsEnumerable().Select(b => new
                    {
                        StoreNo = b.Field<object>("StoreNo"),
                        StoreDsc = b.Field<object>("StoreDsc"),
                        StoreLtnDsc = b.Field<object>("StoreLtnDsc"),
                        SerialTyp = b.Field<object>("SerialTyp"),
                    })
                );

            }
        }

        [Route("GetList/v3")]
        [HttpPost]
        public IHttpActionResult GetList_v3([FromBody] storeGetListFilterModel fm)
        {
            var getList = new List<storeGetListFilterModel>();

            if (!ModelState.IsValid)
            {
                return new BadRequestActionResult(ModelState.Values);
            }
            else
            {
                using (var repo = new Repo(this, "InvAssistantSp_storeGetList", "Store_GetList_v3", initAsReader: true))
                {
                    try
                    {
                        repo.cmd.Parameters.AddWithValue("@f_branch", fm.f_branch);
                        repo.cmd.Parameters.AddWithValue("@f_storeNo", fm.f_storeNo);
                        repo.cmd.Parameters.AddWithValue("@f_userId", fm.f_userId);
                        var sdr = repo.cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                getList.Add(new storeGetListFilterModel()
                                {
                                    StoreNo = sdr["StoreNo"].ToString(),
                                    StoreDsc = sdr["StoreDsc"].ToString(),
                                    StoreLtnDsc = sdr["StoreLtnDsc"].ToString(),
                                    SerialTyp = sdr["SerialTyp"].ToString(),
                                });
                            }
                            sdr.Close();
                            repo.cmd.ExecuteNonQuery();
                            return Ok(getList);
                        }
                        else
                        {
                            return NotFoundActionResult("کالایی یافت نشد");
                        }
                    }
                    catch (Exception ex)
                    {
                        return Ok(ex.Message);
                    }
                }
            }

        }

        private IHttpActionResult NotFoundActionResult(string v)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// مدل فیلتر دریافت لیست انبار ها
    /// </summary>
    public class storeGetListFilterModel : publicFilterModel
    {
        /// <summary>
        /// فیلتر_شناسه کاربر
        /// </summary>
        public string f_userId { get; set; }
        /// <summary>
        /// فیلتر_شعبه
        /// </summary>
        public int? f_branch { get; set; }
        /// <summary>
        /// فیلتر_شناسه انبار
        /// </summary>
        public string f_storeNo { get; set; }
        public string StoreNo { get; set; }
        public string StoreDsc { get; set; }
        public string StoreLtnDsc { get; set; }
        public string SerialTyp { get; set; }
    }
}
