
using Rayvarz.inv.assistant.service.Models;
using Rayvarz.inv.assistant.service.Models.ray;
using Rayvarz.inv.assistant.service.Tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rayvarz.inv.assistant.service.Controllers
{
    /// <summary>
    /// سرویس های مربوط به شناسنامه کالا
    /// </summary>
    [RoutePrefix("api/itemData")]
    public class ItemDataController : AdvancedApiController
    {
        private string storeNo
        {
            get
            {
                return Request.Headers.FirstOrDefault(h => h.Key.Equals("StoreNo")).Value.FirstOrDefault();
            }
        }
        private string userId
        {
            get
            {//get from Request.Headers.Authorization.Scheme; , singleThone
                return "1";
            }
        }
        /// <summary>
        /// ثبت شناسنامه کالا
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("insertItemData")]
        [Exception]
        [HttpPost]
        public IHttpActionResult insertItemData([FromBody]itemDataBindingModel item)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestActionResult(ModelState.Values);
            }


            if (ConfigurationManager.AppSettings["api"].ToLower() == "official")
            {
                using (var conn = new SqlConnection(new Entities().Database.Connection.ConnectionString))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ray.InvSp_InsertItemData";
                        cmd.Parameters.AddWithValue("@PartNo", item.PartNo.getDbValue());
                        cmd.Parameters.AddWithValue("@PartNoDsc", item.PartNoDsc.getDbValue());
                        cmd.Parameters.AddWithValue("@PartLtnDsc", item.PartLtnDsc.getDbValue());
                        cmd.Parameters.AddWithValue("@PartGrp", item.PartGrp.getDbValue());
                        cmd.Parameters.AddWithValue("@UntCode", item.UntCode.getDbValue());
                        var par_msg = cmd.Parameters.Add("@msg", SqlDbType.VarChar, 200);
                        par_msg.Direction = ParameterDirection.Output;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        string msg = par_msg.Value == null || par_msg.Value is DBNull ? "" : par_msg.Value.ToString();
                        if (string.IsNullOrEmpty(msg))
                            return Ok();
                        else
                            return new NotFoundActionResult(msg);
                    }
                }
            }

            using (var db = new Entities())
            {
                if (db.ItemDatas.AsNoTracking().Any(i => i.PartNo == item.PartNo))
                    return new NotFoundActionResult("کد کالا تکراری است.");

                db.ItemDatas.Add(new ItemData()
                {
                    PartNo = item.PartNo,
                    PartNoDsc = item.PartNoDsc,
                    NotActiv = item.NotActiv,
                    UntCode = item.UntCode,
                    PartGrp = item.PartGrp,
                    TechnicalNo = item.TechnicalNo,
                    NationalPartCod = item.NationalPartCod,
                    Salable = item.Salable,
                    IsMchnPart = item.IsMakePart,
                    IsTools = item.IsTools,
                    IsAsset = item.IsAsset,
                    IsMold = item.IsMold,
                    IsMtrl = item.IsMtrl,
                    ISOfsTool = item.ISOfsTool,
                    IsConsTool = item.IsConsTool,
                    IsMakePart = item.IsMakePart,
                    IsShop = item.IsShop,
                    IsPack = item.IsPack,
                    IsLabTools = item.IsLabTools,
                    IsExpired = item.IsExpired,
                    IsPrdPart = item.IsPrdPart,
                    RowGuid = Guid.NewGuid(),
                    CreateDate = dateTimeService.crntPersianDate
                });
                db.SaveChanges();
            }
            return Ok(/* new { msg = "اطلاعات با موفقیت ذخیره شد.",message = "اطلاعات با موفقیت ذخیره شد." }*/);
        }

        [Route("insertItemData/v2")]
        [HttpPost]
        public IHttpActionResult insertItemData([FromBody] itemDataModel item)
        {
            if (!ModelState.IsValid)
                return new BadRequestActionResult(ModelState.Values);
            

            filterModel = item;
            using (var repo = new Repo(this, "ray.InvAssistantSp_insert_ItemData", "itemData_insertItemData_v2", initAsReader: true))
            {
                try
                {
                    repo.cmd.Parameters.AddWithValue("@PartNo", item.PartNo.getDbValue());
                    repo.cmd.Parameters.AddWithValue("@PartNoDsc", item.PartNoDsc);
                    repo.cmd.Parameters.AddWithValue("@NotActiv", item.NotActiv);
                    repo.cmd.Parameters.AddWithValue("@UntCode", item.UntCode);
                    repo.cmd.Parameters.AddWithValue("@PartGrp", item.PartGrp);
                    repo.cmd.Parameters.AddWithValue("@TechnicalNo", item.TechnicalNo);
                    repo.cmd.Parameters.AddWithValue("@NationalPartCod", item.NationalPartCod);
                    repo.cmd.Parameters.AddWithValue("@Salable", item.Salable);
                    repo.cmd.Parameters.AddWithValue("@IsMchnPart", item.IsMchnPart);
                    repo.cmd.Parameters.AddWithValue("@IsTools", item.IsTools);
                    repo.cmd.Parameters.AddWithValue("@IsAsset", item.IsAsset);
                    repo.cmd.Parameters.AddWithValue("@IsMold", item.IsMold);
                    repo.cmd.Parameters.AddWithValue("@IsMtrl", item.IsMtrl);
                    repo.cmd.Parameters.AddWithValue("@ISOfsTool", item.ISOfsTool);
                    repo.cmd.Parameters.AddWithValue("@IsConsTool", item.IsConsTool);
                    repo.cmd.Parameters.AddWithValue("@IsMakePart", item.IsMakePart);
                    repo.cmd.Parameters.AddWithValue("@IsShop", item.IsShop);
                    repo.cmd.Parameters.AddWithValue("@IsPack", item.IsPack);
                    repo.cmd.Parameters.AddWithValue("@IsLabTools", item.IsLabTools);
                    repo.cmd.Parameters.AddWithValue("@IsExpired", item.IsExpired);
                    repo.cmd.Parameters.AddWithValue("@IsPrdPart", item.IsPrdPart);

                    //repo.cmd.Parameters.AddWithValue("@PartLtnDsc", item.PartLtnDsc);

                    var rMsg = repo.cmd.Parameters.Add("@rMsg", SqlDbType.VarChar, -1);
                    rMsg.Direction = ParameterDirection.Output;
                    var rCode = repo.cmd.Parameters.Add("@rCode", SqlDbType.Int);
                    rCode.Direction = ParameterDirection.Output;

                    repo.cmd.ExecuteNonQuery();

                    return Ok(new { msg = rMsg.Value, code = rCode.Value });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
        }

        /// <summary>
        /// ویرایش شناسنامه کالا
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("editItemData")]
        [Exception]
        [HttpPost]
        public IHttpActionResult editItemData([FromBody] itemDataBindingModel item)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestActionResult(ModelState.Values);
            }
            using (var db = new Entities())
            {
                var it = db.ItemDatas.Where(i => i.PartNo == item.PartNo).FirstOrDefault();
                if (it == null)
                    return new NotFoundActionResult("کالا یافت نشد");

                if (it.InvDtlDatas.Count() > 0)
                {
                    it.NotActiv = item.NotActiv;
                }
                //return new NotFoundActionResult("کالا در سیستم مورد استفاده قرار گرفته است. امکان ویرایش وجود ندارد.");
                else
                {
                    it.PartNoDsc = item.PartNoDsc;
                    it.NotActiv = item.NotActiv;
                    it.UntCode = item.UntCode;
                    it.PartGrp = item.PartGrp;
                    it.TechnicalNo = item.TechnicalNo;
                    it.NationalPartCod = item.NationalPartCod;
                    it.Salable = item.Salable;
                    it.IsMchnPart = item.IsMakePart;
                    it.IsTools = item.IsTools;
                    it.IsAsset = item.IsAsset;
                    it.IsMold = item.IsMold;
                    it.IsMtrl = item.IsMtrl;
                    it.ISOfsTool = item.ISOfsTool;
                    it.IsConsTool = item.IsConsTool;
                    it.IsMakePart = item.IsMakePart;
                    it.IsShop = item.IsShop;
                    it.IsPack = item.IsPack;
                    it.IsLabTools = item.IsLabTools;
                    it.IsExpired = item.IsExpired;
                    it.IsPrdPart = item.IsPrdPart;
                    it.LastChngDate = dateTimeService.crntPersianDate;
                    //db.ItemDatas.Add(new ItemData()
                    //{
                    //    PartNo = item.PartNo,
                    //    PartNoDsc = item.PartNoDsc,
                    //    NotActiv = item.NotActiv,
                    //    UntCode = item.UntCode,
                    //    PartGrp = item.PartGrp,
                    //    it.TechnicalNo = item.TechnicalNo,
                    //    it.NationalPartCod = item.NationalPartCod,
                    //    it.Salable = item.Salable,
                    //    it.IsMchnPart = item.IsMakePart,
                    //    it.IsTools = item.IsTools,
                    //    it.IsAsset = item.IsAsset,
                    //    it.IsMold = item.IsMold,
                    //    it.IsMtrl = item.IsMtrl,
                    //    it.ISOfsTool = item.ISOfsTool,
                    //    it.IsConsTool = item.IsConsTool,
                    //    it.IsMakePart = item.IsMakePart,
                    //    it.IsShop = item.IsShop,
                    //    it.IsPack = item.IsPack,
                    //    it.IsLabTools = item.IsLabTools,
                    //    it.IsExpired = item.IsExpired,
                    //    it.IsPrdPart = item.IsPrdPart,
                    //    RowGuid = Guid.NewGuid()
                    //});
                }
                db.SaveChanges();
            }
            return Ok();
        }

        [Route("editItemData/v2")]
        [Exception]
        [HttpPost]
        public IHttpActionResult editItemData_v2([FromBody] itemDataModel item)
        {
            //if (!ModelState.IsValid)
            //    return new BadRequestActionResult(ModelState.Values);

            filterModel = item;
            using (var repo = new Repo(this, "ray.InvAssistantSp_edit_ItemData", "itemData_editItemData_v2", initAsReader: true))
            {
                try
                {
                    repo.cmd.Parameters.AddWithValue("@PartNo", item.PartNo);
                    repo.cmd.Parameters.AddWithValue("@PartNoDsc", item.PartNoDsc);
                    repo.cmd.Parameters.AddWithValue("@NotActiv", item.NotActiv);
                    repo.cmd.Parameters.AddWithValue("@UntCode", item.UntCode);
                    repo.cmd.Parameters.AddWithValue("@PartGrp", item.PartGrp);
                    repo.cmd.Parameters.AddWithValue("@TechnicalNo", item.TechnicalNo);
                    repo.cmd.Parameters.AddWithValue("@NationalPartCod", item.NationalPartCod);
                    repo.cmd.Parameters.AddWithValue("@Salable", item.Salable);
                    repo.cmd.Parameters.AddWithValue("@IsMchnPart", item.IsMchnPart);
                    repo.cmd.Parameters.AddWithValue("@IsTools", item.IsTools);
                    repo.cmd.Parameters.AddWithValue("@IsAsset", item.IsAsset);
                    repo.cmd.Parameters.AddWithValue("@IsMold", item.IsMold);
                    repo.cmd.Parameters.AddWithValue("@IsMtrl", item.IsMtrl);
                    repo.cmd.Parameters.AddWithValue("@ISOfsTool", item.ISOfsTool);
                    repo.cmd.Parameters.AddWithValue("@IsConsTool", item.IsConsTool);
                    repo.cmd.Parameters.AddWithValue("@IsMakePart", item.IsMakePart);
                    repo.cmd.Parameters.AddWithValue("@IsShop", item.IsShop);
                    repo.cmd.Parameters.AddWithValue("@IsPack", item.IsPack);
                    repo.cmd.Parameters.AddWithValue("@IsLabTools", item.IsLabTools);
                    repo.cmd.Parameters.AddWithValue("@IsExpired", item.IsExpired);
                    repo.cmd.Parameters.AddWithValue("@IsPrdPart", item.IsPrdPart);

                    //repo.cmd.Parameters.AddWithValue("@PartLtnDsc", item.PartLtnDsc);

                    var rMsg = repo.cmd.Parameters.Add("@rMsg", SqlDbType.VarChar, -1);
                    rMsg.Direction = ParameterDirection.Output;
                    var rCode = repo.cmd.Parameters.Add("@rCode", SqlDbType.Int);
                    rCode.Direction = ParameterDirection.Output;

                    repo.cmd.ExecuteNonQuery();

                    return Ok(new { msg = rMsg.Value, code = rCode.Value });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        /// <summary>
        /// دریافت لیست کالا ها
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



                //Func<User, bool> e = u => u.token == MD5Builder.GenerateKey(token) && ((string.IsNullOrEmpty(roleType) ? 1 == 1 : u.fk_RoleType == roleType));

                var cond = db.Stores.Where(s => s.StoreNo == storeNo).Select(s => new { s.Salable, s.IsMchnPart, s.IsTools, s.IsMold, s.IsMtrl, s.ISOfsTool, s.IsConsTool, s.IsBisector, s.IsAsset, s.IsPack }).Single();


                return Ok(db.ItemDatas.AsNoTracking()
                    .Where(i =>
                    (cond.Salable != new byte?(1) || new byte?(1) == i.Salable.Value)
                    &&
                    (cond.IsMchnPart != new byte?(1) || new byte?(1) == i.IsMchnPart.Value)
                    &&
                    (cond.IsTools != new byte?(1) || new byte?(1) == i.IsTools.Value)
                    &&
                    (cond.IsMold != new byte?(1) || new byte?(1) == i.IsMold.Value)
                    &&
                    (cond.IsMtrl != new byte?(1) || new byte?(1) == i.IsMtrl.Value)
                    &&
                    (cond.ISOfsTool != new byte?(1) || new byte?(1) == i.ISOfsTool.Value)
                    &&
                    (cond.IsConsTool != new byte?(1) || new byte?(1) == i.IsConsTool.Value)
                    &&
                    (cond.IsBisector != new byte?(1) || new byte?(1) == i.IsMakePart.Value)
                    &&
                    (cond.IsAsset != new byte?(1) || new byte?(1) == i.IsAsset.Value)
                    &&
                    (cond.IsPack != new byte?(1) || new byte?(1) == i.IsPack.Value)

                    ).Join(db.Units.AsNoTracking(), oks => oks.UntCode, iks => iks.UntCode, (I, U) => new { I.NationalPartCod, I.TechnicalNo, U.UntName, I.PartNo, I.PartGrp, I.PartNoDsc, I.PartLtnDsc, I.NotActiv, I.UntCode })

                    .GroupJoin(db.InvAssistantVw_getPartGrpFullPath.AsNoTracking(), oks => oks.PartGrp, iks => iks.PartGrp, (i, g) => new { i.NationalPartCod, i.TechnicalNo, i.PartGrp, i.PartNo, i.PartNoDsc, i.PartLtnDsc, i.NotActiv, i.UntCode, i.UntName, grps = g.DefaultIfEmpty() })

                    .Where(c => (string.IsNullOrEmpty(fm.group) || c.PartGrp == fm.group) &&  /*c.n.HasValue && c.ActvFlg.Value == 1 &&*/


                    (string.IsNullOrEmpty(fm.key) ||
                    (c.PartNo.Contains(fm.key) ||
                    c.PartNoDsc.Contains(fm.key) ||
                    c.PartLtnDsc.Contains(fm.key)) ||
                    c.NationalPartCod.Contains(fm.key) ||
                    c.TechnicalNo.Contains(fm.key))

                    /*&& (fm._from.HasValue ? c.PartNo >= fm._from.Value.ToString() : 1 == 1) && (fm._to.HasValue ? Convert.ToInt64(c.PartNo) <= fm._to.Value : 1 == 1)*/).OrderBy(o => o.PartNo)
                    .Skip(fm._fromIndex).Take(fm._take).ToList().Select(c => new { c.PartNo, c.TechnicalNo, c.PartNoDsc, c.PartLtnDsc, c.UntCode, c.UntName, NotActiv = (c.NotActiv.HasValue ? c.NotActiv.Value : 0), c.NationalPartCod, c.PartGrp, fullpath = (c.grps != null && c.grps.FirstOrDefault() != null) ? c.grps.FirstOrDefault().FullPath : "" }));
            }
        }

        [Route("GetList/v2")]
        [Exception]
        [HttpPost]
        public IHttpActionResult GetList_v2([FromBody] publicFilterModel fm)
        {
            if (!ModelState.IsValid)
                return new BadRequestActionResult(ModelState.Values);

            try
            {
                using (var repo = new Repo(this, "ray.InvAssistantSp_itemData_GetList", "itemData_GetList_v2", initAsReader: true))
                {
                    repo.ExecuteAdapter();
                    return Ok(repo.ds.Tables[0].AsEnumerable().Select(i => new
                    {
                        PartNo = i.Field<object>("PartNo").ToString(),
                        TechnicalNo = i.Field<object>("TechnicalNo").dbNullCheckString(),
                        PartNoDsc = i.Field<object>("PartNoDsc").dbNullCheckString(),
                        PartLtnDsc = i.Field<object>("PartLtnDsc").dbNullCheckString(),
                        UntCode = i.Field<object>("UntCode").dbNullCheckString(),
                        UntName = i.Field<object>("UntName").dbNullCheckString(),
                        NotActiv = i.Field<object>("NotActiv").dbNullCheckInt(),
                        NationalPartCod = i.Field<object>("NationalPartCod").dbNullCheckInt(),
                        PartGrp = i.Field<object>("PartGrp").dbNullCheckString(),
                        fullpath = i.Field<object>("fullpath").dbNullCheckString(),
                    }).ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        


        /// <summary>
        /// دریافت آخرین کد قابل استفاده در یک گروه کالا
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("getLastAvailablePartNoOfGrp")]
        [Exception]
        [HttpPost]
        public IHttpActionResult getLastAvailablePartNoOfGrp(getLastAvailablePartNoOfGrpBindingModel input)
        {
            using (var db = new Entities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                var maxval = db.ItemDatas.Where(it => it.PartGrp == input.PartGrp).Max(m => m.PartNo);

                return Ok(string.IsNullOrEmpty(maxval) ? long.Parse(input.PartGrp + "1") : long.Parse(maxval) + 1);
            }
        }

        [Route("getLastAvailablePartNoOfGrp/v2")]
        [Exception]
        [HttpPost]
        public IHttpActionResult getLastAvailablePartNoOfGrp_v2(getLastAvailablePartNoOfGrpBindingModel input)
        {
            using (var repo = new Repo(this, "ray.InvAssistantSp_GetLastAvailableParNoOfGrp", "itemData_getLastAvailablePartNoOfGrp_v2", initAsReader: true))
            {
                try
                {
                    repo.cmd.Parameters.AddWithValue("@PartGrp", input.PartGrp);
                    var maxval = repo.cmd.Parameters.Add("@maxval", SqlDbType.VarChar, 20);
                    maxval.Direction = ParameterDirection.Output;
                    return Ok(new { maxval = maxval.Value });
                }
                catch(Exception ex)
                {
                    return Ok(ex.Message);
                }
            }
        }
        /// <summary>
        /// دریافت جزئیات کالا - به منظور ویرایش اطلاعات
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("getItemInfo")]
        [Exception]
        [HttpPost]
        public IHttpActionResult getItemInfo(getItemInfoBindingModel input)
        {//use "ray.InvSp_FindPropertyFromItemData" instead!!!
            if (!ModelState.IsValid)
            {
                return new BadRequestActionResult(ModelState.Values);
            }
            //var crntFiscalYear = invTools.CurrentFiscalYear;
            //var token = Request.Headers.Authorization.Scheme;
            //var StoreNo = Request.Headers.FirstOrDefault(h => h.Key.Equals("StoreNo")).Value.FirstOrDefault();
            using (var db = new Entities())
            {
                //var soh = db.InvArcSohs.Where(a => a.PartNo == input.partCode && a.StoreNo == StoreNo && a.FiscalYear == 1396).ToList().OrderByDescending(o => int.Parse(o.DocDate)).FirstOrDefault();
                //var qty = soh == null ? 0 : soh.Qty;
                // var soh = invTools.GetItemDataSoh(db, input.partCode, storeNo, crntFiscalYear.FiscalYear);
                var itemdata =
                db.ItemDatas.AsNoTracking()
                    .Join(db.Units.AsNoTracking(), oks => oks.UntCode, iks => iks.UntCode, (I, U) => new { I, U })
                    .GroupJoin(db.PartGrps.AsNoTracking(), oks => oks.I.PartGrp, iks => iks.PartGrp1, (IU, G) => new { IU, G })
                    .SelectMany(x => x.G.DefaultIfEmpty(), (x, y) => new { x.IU, G = y })
                    //new { I.NationalPartCod, I.TechnicalNo, U.UntName, I.PartNo, I.PartGrp, I.PartNoDsc, I.PartLtnDsc, I.NotActiv, I.UntCode })
                    .Where(c => c.IU.I.PartNo == input.partCode).FirstOrDefault();
                //.Select(c => new { c.PartNo, c.TechnicalNo, c.PartNoDsc, c.NotActiv, c.PartLtnDsc, c.UntCode, c.UntName, c.NationalPartCod }).FirstOrDefault();

                if (itemdata != null)
                {
                    return Ok(new
                    {
                        itemdata.IU.I.PartNoDsc,
                        itemdata.IU.I.PartGrp,
                        PartGrpDsc = itemdata.G != null ? itemdata.G.PartGrpDsc : "",
                        itemdata.IU.I.UntCode,
                        itemdata.IU.U.UntName,
                        itemdata.IU.I.NationalPartCod,
                        itemdata.IU.I.Salable,
                        itemdata.IU.I.IsMchnPart,
                        itemdata.IU.I.IsTools,
                        itemdata.IU.I.IsAsset,
                        itemdata.IU.I.IsMold,
                        itemdata.IU.I.IsMtrl,
                        itemdata.IU.I.ISOfsTool,
                        itemdata.IU.I.IsConsTool,
                        itemdata.IU.I.IsMakePart,
                        itemdata.IU.I.IsShop,
                        itemdata.IU.I.IsPack,
                        itemdata.IU.I.IsLabTools,
                        itemdata.IU.I.IsExpired,
                        itemdata.IU.I.IsPrdPart,
                        itemdata.IU.I.NotActiv,
                        editable = !db.InvDtlDatas.AsNoTracking().Any(d => d.PartNo == itemdata.IU.I.PartNo),
                    });
                    //    if (itemdata.FirstOrDefault().NotActiv.HasValue && itemdata.FirstOrDefault().NotActiv.Value == 1)
                    //        return new NotFoundActionResult("کالای وارد شده در سیستم غیرفعال گردیده است و قابل استفاده نمی باشد.");
                    //    else
                    //        return Ok(itemdata.FirstOrDefault());
                }
                return new NotFoundActionResult("کالا یافت نشد.");
            }
        }

        [Route("getItemInfo/v2")]
        [Exception]
        [HttpPost]
        public IHttpActionResult getItemInfo_v2(getItemInfoModel fm)
        {
            if (!ModelState.IsValid)
                return new BadRequestActionResult(ModelState.Values);

            using (var repo = new Repo(this, "ray.InvAssistantSp_itemData_GetItemInfo", "itemData_getItemInfo_v2", initAsReader: true))
            {

                repo.cmd.Parameters.AddWithValue("@partCode", fm.partCode);
                repo.cmd.Parameters.AddWithValue("@docNo", fm.docNo);
                repo.cmd.Parameters.AddWithValue("@refDoctype", fm.refDoctype);
                repo.cmd.Parameters.AddWithValue("@refDocNo", fm.refDocNo);
                repo.cmd.Parameters.AddWithValue("@refFiscalYear", fm.refFiscalYear);

                var rCode = repo.cmd.Parameters.Add("@rCode", SqlDbType.TinyInt);
                rCode.Direction = ParameterDirection.Output;
//              var rMsg = repo.cmd.Parameters.Add("@rMsg", SqlDbType.VarChar, -1);
//              rMsg.Direction = ParameterDirection.Output;

                repo.ExecuteAdapter();
                return Ok(repo.ds.Tables[0].AsEnumerable().Select(b => new
                {
                    PartNoDsc = b.Field<object>("PartNoDsc").dbNullCheckString(),
                    PartGrp = b.Field<object>("PartGrp").dbNullCheckString(),
                    PartGrpDsc = b.Field<object>("PartGrpDsc").dbNullCheckString(),
                    UntCode = b.Field<object>("UntCode").dbNullCheckString(),
                    UntName = b.Field<object>("UntName").dbNullCheckString(),
                    NationalPartCod = b.Field<object>("NationalPartCod").dbNullCheckInt(),
                    Salable = b.Field<object>("Salable").dbNullCheckInt(),
                    IsMchnPart = b.Field<object>("IsMchnPart").dbNullCheckInt(),
                    IsTools = b.Field<object>("IsTools").dbNullCheckInt(),
                    IsAsset = b.Field<object>("IsAsset").dbNullCheckInt(),
                    IsMold = b.Field<object>("IsMold").dbNullCheckInt(),
                    IsMtrl = b.Field<object>("IsMtrl").dbNullCheckInt(),
                    ISOfsTool = b.Field<object>("ISOfsTool").dbNullCheckInt(),
                    IsConsTool = b.Field<object>("IsConsTool").dbNullCheckInt(),
                    IsMakePart = b.Field<object>("IsMakePart").dbNullCheckInt(),
                    IsShop = b.Field<object>("IsShop").dbNullCheckInt(),
                    IsPack = b.Field<object>("IsPack").dbNullCheckInt(),
                    IsLabTools = b.Field<object>("IsLabTools").dbNullCheckInt(),
                    IsExpired = b.Field<object>("IsExpired").dbNullCheckInt(),
                    IsPrdPart = b.Field<object>("IsPrdPart").dbNullCheckInt(),
                    NotActiv = b.Field<object>("NotActiv").dbNullCheckInt(),
                    editable = b.Field<object>("editable").dbNullCheckBoolean(),
                }));


            }
        }

        /// <summary>
        /// دریافت لیست موجودی سریالی
        /// </summary>
        /// <param name="fm"></param>
        /// <returns></returns>
        [Route("serialSohGetList")]
        [Exception]
        [HttpPost]
        public IHttpActionResult serialSohGetList([FromBody] serialSohGetListFilterModel fm)
        {
            if (!ModelState.IsValid)
                return new BadRequestActionResult(ModelState.Values);
            filterModel = fm;
            using (var repo = new Repo(this, "ray.InvAssistantSp_SerialSoh_GetList", "itemData_serialSohGetList", initAsReader: true))
            {
                repo.cmd.Parameters.AddWithValue("@partNo", fm.partNo.getDbValue());
                repo.cmd.Parameters.AddWithValue("@f_serial", fm.f_serial.getDbValue());

                repo.ExecuteAdapter();
                return Ok(
                    repo.ds.Tables[0].AsEnumerable().Select(b => new
                    {
                        //Serail = b.Field<object>("Serail"),
                        //Soh = b.Field<object>("Soh"),
                        //SupName = b.Field<object>("SupName"),
                        //PakgTypeDesc = b.Field<object>("PakgTypeDesc"),
                        FiscalYear = b.Field<object>("FiscalYear"),
                        StoreNo = b.Field<object>("StoreNo"),
                        PartNo = b.Field<object>("PartNo"),
                        Serail = b.Field<object>("Serail"),
                        TotRcpt = b.Field<object>("TotRcpt"),
                        TotIssu = b.Field<object>("TotIssu"),
                        Supplier = b.Field<object>("Supplier"),
                        SupName = b.Field<object>("SupName"),
                        TrnsPkgTyp = b.Field<object>("TrnsPkgTyp"),
                        PakgTypeDesc = b.Field<object>("PakgTypeDesc"),
                        SuppSerial = b.Field<object>("SuppSerial"),
                        SuppEntrDate = b.Field<object>("SuppEntrDate"),
                        SuppExpDate = b.Field<object>("SuppExpDate"),
                        ExpDate = b.Field<object>("ExpDate"),
                        ConfirmDate = b.Field<object>("ConfirmDate"),
                        Soh = b.Field<object>("Soh"),
                        OrdrNO = b.Field<object>("OrdrNO"),
                        OrdrDsc = b.Field<object>("OrdrDsc"),
                    })
                );

            }
        }
    }

    public class itemDataModel : publicFilterModel
    {
        public string PartNo { get; set; }
        public string PartNoDsc { get; set; }
        public string PartLtnDsc { get; set; }
        public string PartGrp { get; set; }
        public string UntCode { get; set; }
        public string Salable { get; set; }
        public short NotActiv { get; set; }
        public string TechnicalNo { get; set; }
        public short NationalPartCod { get; set; }
        public short IsMchnPart { get; set; }
        public short IsTools { get; set; }
        public short IsAsset { get; set; }
        public short IsMold { get; set; }
        public short IsMtrl { get; set; }
        public short ISOfsTool { get; set; }
        public short IsMakePart { get; set; }
        public short IsShop { get; set; }
        public short IsPack { get; set; }
        public short IsLabTools { get; set; }
        public short IsExpired { get; set; }
        public short IsPrdPart { get; set; }
        public short IsConsTool { get; set; }
        public long StoreNo { get; set; }
    }

    public class getItemInfoModel
    {
        public string partCode { get; set; }
        public string docNo { get; set; }
        public int refDoctype { get; set; }
        public string refDocNo { get; set; }
        public int refFiscalYear { get; set; }
    }

   
    /// <summary>
    /// مدل فیلتر دریافت لیست موجودی سریالی
    /// </summary>
    public class serialSohGetListFilterModel : publicFilterModel
    {
        /// <summary>
        /// شناسه کالا
        /// </summary>
        public string partNo { get; set; }
        /// <summary>
        /// فیلتر_سریال
        /// </summary>
        public string f_serial { get; set; }
        public string group { get; set; }
        internal int limit = 1000;//int.MaxValue
        /// <summary>
        /// کلید واژه
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// از کد
        /// </summary>
        public int? _from { get; set; }
        /// <summary>
        /// از کد
        /// </summary>
        public string from
        {
            get
            {
                return _from.ToString();
            }
            set
            {
                int tmp;
                _from = string.IsNullOrEmpty(value) || !int.TryParse(value, out tmp) ? new int?() : tmp;
                //_from = string.IsNullOrEmpty(value) && !int.TryParse(value,) ? new int?() : int.Parse(value);
            }
        }
        /// <summary>
        /// تا کد
        /// </summary>
        public int? _to { get; set; }
        /// <summary>
        /// تا کد
        /// </summary>
        public string to
        {
            get
            {
                return _to.ToString();
            }
            set
            {
                int tmp;
                _to = string.IsNullOrEmpty(value) || !int.TryParse(value, out tmp) ? new int?() : tmp;
                //_to = string.IsNullOrEmpty(value) ? new int?() : int.Parse(value);
            }
        }
        /// <summary>
        /// از ایندکس
        /// </summary>
        public int _fromIndex { get; set; }
        /// <summary>
        /// تا ایندکس
        /// </summary>
        public string fromIndex
        {
            get
            {
                return _fromIndex.ToString();
            }
            set
            {
                int tmp;
                _fromIndex = string.IsNullOrEmpty(value) || !int.TryParse(value, out tmp) ? 0 : tmp;
                //_fromIndex = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
            }
        }
        /// <summary>
        /// تعداد خروجی
        /// </summary>
        public int _take { get; set; }
        /// <summary>
        /// تعداد خروجی
        /// </summary>
        public string take
        {
            get
            {
                return _take.ToString();
            }
            set
            {
                int tmp;
                _take = string.IsNullOrEmpty(value) || !int.TryParse(value, out tmp) ? limit : tmp;
                //_take = string.IsNullOrEmpty(value) ? limit : int.Parse(value);
            }
        }

        /// <summary>
        /// عنوان فیلد مورد مرتب سازی
        /// </summary>
        public string orderBy { get; set; }
        /// <summary>
        /// مرتب سازی نزولی است؟
        /// </summary>
        public bool? isDescOrder { get; set; }
        public string FiscalYear { get; set; }
        public string StoreNo { get; set; }
        public string PartNo { get; set; }
        public string Serail { get; set; }
        public string TotRcpt { get; set; }
        public string TotIssu { get; set; }
        public string Supplier { get; set; }
        public string SupName { get; set; }
        public string TrnsPkgTyp { get; set; }
        public string PakgTypeDesc { get; set; }
        public string SuppEntrDate { get; set; }
        public string SuppExpDate { get; set; }
        public string SuppSerial { get; set; }
        public string ExpDate { get; set; }
        public string ConfirmDate { get; set; }
        public string Soh { get; set; }
        public string OrdrNO { get; set; }
        public string OrdrDsc { get; set; }
        public string userId { get; set; }
        public string branch { get; set; }
        public string storeNo { get; set; }
        public string clientIp { get; set; }

    }
}