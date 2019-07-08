using Newtonsoft.Json;
using Rayvarz.inv.assistant.service.Models;
using Rayvarz.inv.assistant.service.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data;
using System.Data.Entity.SqlServer;
using System.ComponentModel.DataAnnotations;
using Rayvarz.inv.assistant.service.Models.ray;

namespace Rayvarz.inv.assistant.service.Controllers
{
    /// <summary>
    /// سرویس های اسناد انبار
    /// </summary>
    [RoutePrefix("api/invDoc")]
    public class InvDocController : AdvancedApiController
    {
        private List<DocRefer_HdrModel> getDocReferHdr(getDocReferHdrBindingModel input)
        {
            var retlist = new List<DocRefer_HdrModel>();
            using (var conn = new SqlConnection(conStr))// @"Data Source=192.168.1.105;Initial Catalog=_ab;Persist Security Info=True;User ID=sa;Password=sa"))//new Entities().Database.Connection.ConnectionString))
            {
                conn.Open();
                foreach (var rfDtyp in input.f_ref_docType.HasValue ? new byte[] { input.f_ref_docType.Value } : getRefDocTypes(input.DocType, new Entities()).Select(s => s.id))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ray.InvSp_SelectDocRefer";
                        cmd.Parameters.AddWithValue("@FiscalYear", input.f_ref_fiscalYear.HasValue ? input.f_ref_fiscalYear.Value : invTools.CurrentFiscalYear.FiscalYear);
                        cmd.Parameters.AddWithValue("@StoreNo", storeNo);
                        cmd.Parameters.AddWithValue("@DocType", rfDtyp); //getRefDocType(input.DocType));
                        cmd.Parameters.AddWithValue("@Docno", input.f_ref_docNo.HasValue ? input.f_ref_docNo.Value : 0);
                        cmd.Parameters.AddWithValue("@Partno", "");
                        cmd.Parameters.AddWithValue("@BaseDocType", input.DocType);
                        cmd.Parameters.AddWithValue("@IsStandardIssue", 0);
                        cmd.Parameters.AddWithValue("@usr", userId);
                        cmd.Parameters.AddWithValue("@FilterPartno", "");
                        cmd.Parameters.AddWithValue("@FilterCenter", input.f_ref_center.HasValue ? input.f_ref_center.Value : 0);
                        cmd.Parameters.AddWithValue("@FilterDate", input.f_ref_docDate ?? "");
                        cmd.Parameters.AddWithValue("@strIsInvWithParameter", "");
                        cmd.Parameters.AddWithValue("@FilterOrderNo", input.f_ref_orderNo.HasValue ? input.f_ref_orderNo.Value : 0);
                        cmd.Parameters.AddWithValue("@FilterSerial", "");
                        cmd.Parameters.AddWithValue("@NotRemain", 0);
                        cmd.Parameters.AddWithValue("@IsSaleIssueForRtrn", 0);

                        cmd.ExecuteNonQuery();
                    }
                    using (var cmd2 = conn.CreateCommand())
                    {
                        cmd2.CommandType = CommandType.Text;
                        string q = string.Format(@"declare @q as nvarchar(max);
                                    set @q = 'select [Row],[FiscalYear],[DocType],[doctypedesc],[DocNo],[DocDate],[DocStatus],[reqcenterDesc],[orgdocno],[orgRefno],[ReqCenter],[DestStoreNo],[LcNo],[raysys]'
                                     if(exists(
                                    SELECT 1
                                    FROM   tempdb.sys.columns
                                    WHERE  object_id = Object_id('tempdb..##InvDocSelect{0}') and name = 'OrdrDsc'))
                                    set @q = @q + ',[OrdrDsc]';
                                    else
                                    set @q = @q + ',''-'' as OrdrDsc';
                                    set @q = @q + ' from ##InvDocSelect{0}';

                                    exec sp_executesql @q", userId);
                        //if (input.f_ref_docNo.HasValue)
                        //    q += string.Format(" where DocNo = {0} ", input.f_ref_docNo.Value);
                        //if (!string.IsNullOrEmpty(input.key))
                        //    q += string.Format(" where ReqCenter like '%{0}%' reqcenterDesc like '%{0}%' or DocNo ", input.key);
                        cmd2.CommandText = q;
                        var rd = cmd2.ExecuteReader();
                        while (rd.Read())
                        {
                            retlist.Add(new DocRefer_HdrModel()
                            {
                                DestStoreNo = rd["DestStoreNo"].dbNullCheckString(),
                                DocDate = rd["DocDate"].dbNullCheckString(),
                                DocNo = rd["DocNo"].dbNullCheckInt(),
                                DocStatus = rd["DocStatus"].dbNullCheckByte(),
                                DocType = rd["DocType"].dbNullCheckByte(),
                                doctypedesc = rd["doctypedesc"].dbNullCheckString(),
                                FiscalYear = rd["FiscalYear"].dbNullCheckInt(),
                                LcNo = rd["LcNo"].dbNullCheckString(),
                                orgdocno = rd["orgdocno"].dbNullCheckInt(),
                                orgRefno = rd["orgRefno"].dbNullCheckInt(),
                                raysys = rd["raysys"].dbNullCheckString(),
                                ReqCenter = rd["ReqCenter"].dbNullCheckInt(),
                                reqcenterDesc = rd["reqcenterDesc"].dbNullCheckString(),
                                Row = rd["Row"].dbNullCheckInt(),
                                OrderDsc = rd["OrdrDsc"].dbNullCheckString(),
                            });
                        }
                    }
                }
                conn.Close();
            }
            var res = retlist.AsQueryable().Where(c => string.IsNullOrEmpty(input.key) || c.DocDate.Contains(input.key) ||
                    c.DocNo.ToString().Contains(input.key) || c.reqcenterDesc.Contains(input.key)
                   /* && (fm._from.HasValue ? c.Branch1 >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.Branch1 <= fm._to.Value : 1 == 1)*/
                   );//.OrderByDescending(o => o.DocNo)

            if (!string.IsNullOrEmpty(input.orderBy) && input.orderBy.ToLower() == "docdate")
            {
                if (input.isDescOrder != null && input.isDescOrder.HasValue && !input.isDescOrder.Value)
                    return res.OrderBy(o => o.DocDate).Skip(input._fromIndex).Take(input._take).ToList()/*.Select(c => new {  })*/;
                return res.OrderByDescending(o => o.DocDate).Skip(input._fromIndex).Take(input._take).ToList()/*.Select(c => new {  })*/;
            }
            else//if (string.IsNullOrEmpty(input.orderBy) || input.orderBy.ToLower() == "docno")
            {
                if (input.isDescOrder != null && input.isDescOrder.HasValue && !input.isDescOrder.Value)
                    return res.OrderBy(o => o.DocNo).Skip(input._fromIndex).Take(input._take).ToList()/*.Select(c => new {  })*/;
                return res.OrderByDescending(o => o.DocNo).Skip(input._fromIndex).Take(input._take).ToList()/*.Select(c => new {  })*/;
            }
        }

       
        /// <summary>
        /// دریافت لیست سربرگ اسناد مرجع
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private List<DocRefer_HdrModel> getDocReferHdr_v2(getDocReferHdrBindingModel input)
        {
            var retlist = new List<DocRefer_HdrModel>();
            //using (var repo = new Repo(this, "InvAssistantSp_Store_GetList", "invDoc_getDocReferHdr", initAsReader: true))
            //{

            //}
            return retlist.Where(c => (string.IsNullOrEmpty(input.key) || (c.DocDate.Contains(input.key) ||
                    c.DocNo.ToString().Contains(input.key) || c.reqcenterDesc.Contains(input.key)))
                   /* && (fm._from.HasValue ? c.Branch1 >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.Branch1 <= fm._to.Value : 1 == 1)*/).OrderBy(o => o.Row)
                    .Skip(input._fromIndex).Take(input._take).ToList()/*.Select(c => new {  })*/;
        }
        //--------------------------------
        /// <summary>
        /// دریافت لیست هدینگ مرجع های قابل استفاده
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("getDocReferHdr")]
        [Exception]
        [HttpPost]
        public IHttpActionResult getDocRefer_Hdr([FromBody] getDocReferHdrBindingModel input)
        {
            return Ok(getDocReferHdr(input));
        }

        private List<InvSp_SelectDocReferDtl_Result_> getDocReferDtl(getDocReferDtlBindingModel input)
        {

            //if (!input.FiscalYear.HasValue)//TODO:باگ  اپ رفع شود
            //    input.FiscalYear = 1398;



            //declare @p2 int
            //set @p2 = NULL
            //exec ray.InvSp_FindRefDocTypeCrclSrc 66,@p2 output
            //select @p2
            using (var db = new Entities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                short CountNo = 0;
                // var fy = invTools.CurrentFiscalYear.FiscalYear;
                var res = db.InvSp_SelectDocReferDtl(input.FiscalYear.Value, storeNo, /*getRefDocType(*/input.DocType/*)*/, input.DocNo, input.baseDocType, true)//WARNING: isExistsItemCircle ?!??!?
                    .Where(c => (string.IsNullOrEmpty(input.key) || (c.partno.Contains(input.key) ||
                    c.partnodsc.Contains(input.key) || c.center.ToString().Contains(input.key) || c.OrderNo.ToString().Contains(input.key) || c.ordrDsc.Contains(input.key)))
                   /* && (fm._from.HasValue ? c.Branch1 >= fm._from.Value : 1 == 1) && (fm._to.HasValue ? c.Branch1 <= fm._to.Value : 1 == 1)*/)
                   //.Select(s => new { s.AccDocNo, s.act3, s.act4, s.act5, s.Amt, s.Binno, s.center, s.centerdsc, s.CompAmt, s.ConsRemainQty, s.ConsType, s.ConsTypeDesc, s.docdsc, s.docrow, s.ExchangeUnit, s.FactorDate, s.FactorNo, s.IsSelected, s.NeedDate, s.OrderNo, s.ordrDsc, s.partno, s.partnodsc, s.PartNoRef, s.qty, s.RcptType, s.RcptTypeDesc, s.RemainQty, s.RemainQtycopy, s.RemainQtyJoz, s.RemainQtyKol, s.RtnQty, s.SalePartNo, s.SaleQty, s.SaleRow, s.Selected, s.serial, s.Stqty, s.SupName, s.Supplier, s.taxamt, s.tollamt, s.TreatyNo, s.untcode, s.untname, s.updtime, s.weight, soh = invTools.GetItemDataSoh(new Entities(), s.partno, storeNo, invTools.CurrentFiscalYear.FiscalYear) })
                   .GroupJoin(db.InvDtlDatas.AsNoTracking(), oks => new { FiscalYear = input.FiscalYear.Value, StoreNo = storeNo, DocType = input.DocType.Value, DocNo = input.DocNo.Value, CountNo, DocRow = oks.docrow },
                                                iks => new { iks.FiscalYear, iks.StoreNo, iks.DocType, iks.DocNo, iks.CountNo, iks.DocRow }, (dr, idd) => new { docrf = dr, idds = idd })
                                                .SelectMany(s => s.idds.DefaultIfEmpty(), (x, y) => new { x.docrf, dtl = y })



                   .OrderBy(o => o.docrf.docrow)
                .Skip(input._fromIndex).Take(input._take).ToList();


                return
                   (from s in res
                    select new InvSp_SelectDocReferDtl_Result_()
                    {
                        AccDocNo = s.docrf.AccDocNo,
                        act3 = s.docrf.act3,
                        act4 = s.docrf.act4,
                        act5 = s.docrf.act5,
                        Amt = s.docrf.Amt,
                        Binno = s.docrf.Binno,
                        center = s.docrf.center,
                        centerdsc = s.docrf.centerdsc,
                        CompAmt = s.docrf.CompAmt,
                        ConsRemainQty = s.docrf.ConsRemainQty,
                        ConsType = s.docrf.ConsType,
                        ConsTypeDesc = s.docrf.ConsTypeDesc,
                        docdsc = s.docrf.docdsc,
                        docrow = s.docrf.docrow,
                        ExchangeUnit = s.docrf.ExchangeUnit,
                        FactorDate = s.docrf.FactorDate,
                        FactorNo = s.docrf.FactorNo,
                        IsSelected = s.docrf.IsSelected,
                        NeedDate = s.docrf.NeedDate,
                        OrderNo = s.docrf.OrderNo,
                        ordrDsc = s.docrf.ordrDsc,
                        partno = s.docrf.partno,
                        partnodsc = s.docrf.partnodsc,
                        PartNoRef = s.docrf.PartNoRef,
                        qty = s.docrf.qty,
                        RcptType = s.docrf.RcptType,
                        RcptTypeDesc = s.docrf.RcptTypeDesc,
                        RemainQty = s.docrf.RemainQty,
                        RemainQtycopy = s.docrf.RemainQtycopy,
                        RemainQtyJoz = s.docrf.RemainQtyJoz,
                        RemainQtyKol = s.docrf.RemainQtyKol,
                        RtnQty = s.docrf.RtnQty,
                        SalePartNo = s.docrf.SalePartNo,
                        SaleQty = s.docrf.SaleQty,
                        SaleRow = s.docrf.SaleRow,
                        Selected = s.docrf.Selected,
                        serial = s.docrf.serial,
                        Stqty = s.docrf.Stqty,
                        SupName = s.docrf.SupName,
                        Supplier = s.docrf.Supplier,
                        taxamt = s.docrf.taxamt,
                        tollamt = s.docrf.tollamt,
                        TreatyNo = s.docrf.TreatyNo,
                        untcode = s.docrf.untcode,
                        untname = s.docrf.untname,
                        updtime = s.docrf.updtime,
                        weight = s.docrf.weight,
                        ReqType = s.dtl.ReqType,
                        ReqTypeDesc = s.dtl.ReqType.HasValue ? db.InvReqTyps.AsNoTracking().Where(sd => sd.ReqType == s.dtl.ReqType).FirstOrDefault().ReqTypeDesc : ""
                    }).ToList();
            }
        }
        /// <summary>
        /// دریافت لیست جزئیات یک سند مرجع
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("getDocReferDtl")]
        [Exception]
        [HttpPost]
        public IHttpActionResult getDocRefer_Dtl([FromBody] getDocReferDtlBindingModel input)
        {
            var invIsSerialOrBatch = new byte?[] { 2, 3 }.Contains(new Entities().Stores.AsNoTracking()
                .Where(s => s.StoreNo == storeNo).FirstOrDefault().SerialTyp);
            return Ok(getDocReferDtl(input)
                .Select(s => new
                {
                    s.AccDocNo,
                    s.act3,
                    s.act4,
                    s.act5,
                    s.Amt,
                    s.Binno,
                    s.center,
                    s.centerdsc,
                    s.CompAmt,
                    s.ConsRemainQty,
                    s.ConsType,
                    s.ConsTypeDesc,
                    s.docdsc,
                    s.docrow,
                    s.ExchangeUnit,
                    s.FactorDate,
                    s.FactorNo,
                    s.IsSelected,
                    s.NeedDate,
                    s.OrderNo,
                    s.ordrDsc,
                    s.partno,
                    s.partnodsc,
                    s.PartNoRef,
                    s.qty,
                    s.RcptType,
                    s.RcptTypeDesc,
                    s.RemainQty,
                    s.RemainQtycopy,
                    s.RemainQtyJoz,
                    s.RemainQtyKol,
                    s.RtnQty,
                    s.SalePartNo,
                    s.SaleQty,
                    s.SaleRow,
                    s.Selected,
                    s.serial,
                    s.Stqty,
                    s.SupName,
                    s.Supplier,
                    s.taxamt,
                    s.tollamt,
                    s.TreatyNo,
                    s.untcode,
                    s.untname,
                    s.updtime,
                    s.weight,
                    s.ReqType,
                    s.ReqTypeDesc,
                    soh = invIsSerialOrBatch ?
                    invTools.GetSerialSoh(new Entities(), s.partno, s.serial, storeNo) :
                    invTools.GetItemDataSoh(new Entities(), s.partno, storeNo, invTools.CurrentFiscalYear.FiscalYear)

                }));
        }
        /// <summary>
        /// دریافت لیست سربرگ های اسناد
        /// به همراه لیست نوع سند اسناد زیر مجموعه
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("getListHdr")]
        [Exception]
        [HttpPost]
        public IHttpActionResult getListHdr([FromBody] getDocListHdrBindingModel input)
        {
            using (var db = new Entities())
            {
                return Ok(db.InvHdrDatas.AsNoTracking()
                    .Join(db.InvDtlDatas.AsNoTracking(), oks => new { oks.FiscalYear, oks.StoreNo, oks.DocType, oks.DocNo }, iks => new { iks.FiscalYear, iks.StoreNo, iks.DocType, iks.DocNo }, (hdr, dtl) => new { h = hdr, d = dtl })
                    .GroupJoin(db.Centers.AsNoTracking(), oks => oks.d.ReqCenter, iks => iks.Center1, (hds, cs) => new { hd = hds, reqc = cs })
                    .SelectMany(sm => sm.reqc.DefaultIfEmpty(), (hds, reqCen) => new { hds.hd.h, hds.hd.d, reqCen })


                    .GroupJoin(db.InvOrdrs.AsNoTracking(), oks => oks.d.OrderNo, iks => iks.OrdrNO, (hds, cs) => new { hd = hds, reqc = cs })
                    .SelectMany(sm => sm.reqc.DefaultIfEmpty(), (hdss, ordr) => new { hdss.hd.h, hdss.hd.d, hdss.hd.reqCen, ordr })


                    .Where(jr => jr.h.DocType == input.DocType
                    && (jr.h.DocStatus == 3 || jr.h.DocStatus == 1)
                    //&& jr.d.SetVoid
                    && jr.h.FiscalYear == invTools.CurrentFiscalYear.FiscalYear
                    && jr.h.StoreNo == storeNo
                    && (string.IsNullOrEmpty(input.key) || jr.reqCen.CenterDsc.Contains(input.key) || jr.d.DocDsc.Contains(input.key)))
                    .GroupBy(s => new
                    {
                        s.h.DocNo,
                        s.h.CountNo,
                        s.h.CreateDate,
                        s.h.UpdateDate,
                        s.d.ReqCenter,
                        ReqCenterDsc = s.reqCen.CenterDsc,
                        s.h.SumQty,

                        s.d.OrderNo,
                        s.ordr.OrdrDsc
                        //s.d.SetVoid
                        // s.d.DocDsc
                    })
                    .Select(s =>
                    new
                    {
                        s.FirstOrDefault().h.FiscalYear,
                        s.FirstOrDefault().h.DocNo,
                        s.FirstOrDefault().h.CountNo,
                        s.FirstOrDefault().h.CreateDate,
                        s.FirstOrDefault().h.UpdateDate,
                        s.FirstOrDefault().d.ReqCenter,
                        ReqCenterDsc = s.FirstOrDefault().reqCen.CenterDsc,
                        s.FirstOrDefault().h.SumQty,
                        cntDtls = s.Count(),

                        s.FirstOrDefault().d.OrderNo,
                        s.FirstOrDefault().ordr.OrdrDsc,
                        // s.FirstOrDefault().d.DocDsc
                    }

                    ).OrderByDescending(o => o.DocNo).Skip(input._fromIndex).Take(input._take).ToList()
                    .Select(s =>
                    new
                    {
                        s.FiscalYear,
                        s.CountNo,
                        s.CreateDate,
                        s.DocNo,
                        s.ReqCenter,
                        s.ReqCenterDsc,
                        SumQty = s.SumQty < 0 ? -1 * s.SumQty : s.SumQty,
                        s.UpdateDate,
                        s.cntDtls,
                        subDocTypes = getSubDocTypes(input.DocType, db),
                        s.OrderNo,
                        s.OrdrDsc
                    }).ToList());
            }
        }
        /// <summary>
        /// دریافت لیست سربرگ های اسناد
        /// به همراه لیست نوع سند اسناد زیر مجموعه
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("getListHdr/v2")]
        [Exception]
        [HttpPost]
        public IHttpActionResult getListHdr_v2([FromBody] getDocListHdrBindingModel input)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestActionResult(ModelState.Values);
            }

            var sdt = getSubDocTypes(input.DocType, new Entities());

            filterModel = input;
            using (var repo = new Repo(this, "ray.InvAssistantSp_invHdrData_GetList", "invDoc_invHdrData_GetList", initAsReader: true))
            {
                repo.cmd.Parameters.AddWithValue("@docType", input.DocType.getDbValue());
                repo.cmd.Parameters.AddWithValue("@f_docNo", input.f_docNo.getDbValue());
                repo.cmd.Parameters.AddWithValue("@f_createDate", input.f_createDate.getDbValue());
                repo.cmd.Parameters.AddWithValue("@f_fiscalYear", input.f_fiscalYear.getDbValue());
                repo.cmd.Parameters.AddWithValue("@f_orderNo", input.f_orderNo.getDbValue());
                repo.ExecuteAdapter();
                return Ok(repo.ds.Tables[0].AsEnumerable().Select(b => new
                {
                    FiscalYear = b.Field<object>("FiscalYear"),
                    DocType = b.Field<object>("DocType"),
                    DocNo = b.Field<object>("DocNo"),
                    CountNo = b.Field<object>("CountNo"),
                    CreateDate = b.Field<object>("CreateDate"),
                    DocStatus = b.Field<object>("DocStatus"),
                    SumQty = b.Field<object>("SumQty"),
                    OrderNo = b.Field<object>("OrderNo"),
                    OrdrDsc = b.Field<object>("OrdrDsc"),
                    cntDtls = b.Field<object>("cntDtls"),
                    subDocTypes = sdt//TODO: به صورت سلکت مجزا داخل sp باشد
                }));


            }
        }
        [Route("getRefDocTypes")]
        [Exception]
        [HttpPost]
        public IHttpActionResult getRefDocTypes([FromBody]  publicFilterModel input)
        {
            //return Ok(getRefDocTypes(int.Parse(input.key)).Select(s => new { docType = s.id, DocTypeDesc = s.dsc }));
            return Ok(getRefDocTypes(int.Parse(input.key), new Entities()).Select(s => new { docType = s.id, DocTypeDesc = s.dsc }));
        }

        /// <summary>
        /// دریافت لیست ردیف های یک سند
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("getListDtl")]
        [Exception]
        [HttpPost]
        public IHttpActionResult getListDtl([FromBody] getDocListDtlBindingModel input)
        {
            //int cy = invTools.CurrentFiscalYear.FiscalYear;
            using (var db = new Entities())
            {
                db.Configuration.ProxyCreationEnabled = false;

                var res = db.Database.SqlQuery<getListDtlModel>(@"select D.docrow, D.PartNo,i.PartNoDsc,D.DocDsc,D.NeedDate,abs(D.Qty) as Qty  ,D.Center,c.CenterDsc
    ,D.OrderNo,O.OrdrDsc,D.RcptType,R.RcptTypeDesc,D.ReqType,Q.ReqTypeDesc,D.ConsType,CnsTyp.ConsTypeDesc,D.Supplier,s.SupName
    ,D.act3,actcnt.CenterDsc as Act3Dsc,D.AudtReason,audt.AudtReasonDsc,D.Serial,D.UntCode,u.UntName
    ,D.CntQty,D.Weight,D.docdate,D.RefOrder,D.RefConsQty,(D.Qty - D.RefOrder) as remainingRefOrder,(D.Qty - D.RefConsQty) as remainingRefConsQty , D.ReqCenter,
	case when isnull(sum(di.qty ),0) = 0 then 0 when isnull(sum(di.qty),0) < abs(D.Qty) then 1 when isnull(sum(di.qty),0) >= abs(D.Qty) then 2 end as deliveryItemsStatus,
	sum(di.qty) as sumDeliveryItems
    from ray.InvDtldata D  
    left outer join ray.ItemData i WITH(NOLOCK) on i.PartNo = D.PartNo
    left outer join ray.Unit u WITH(NOLOCK) on u.UntCode = D.UntCode
    left outer join ray.InvOrdr o WITH(NOLOCK) on o.OrdrNO = D.OrderNo
    left outer join ray.Center c WITH(NOLOCK) on c.Center = D.Center
    left outer join ray.Center actcnt WITH(NOLOCK) on actcnt.Center = D.Act3
    left outer join ray.InvCnsTyp CnsTyp WITH(NOLOCK) on CnsTyp.ConsType = D.ConsType
    left outer join ray.InvRcptTyp R WITH(NOLOCK) on R.RcptType = D.RcptType
    left outer join ray.InvReqTyp Q WITH(NOLOCK) on Q.ReqType = D.ReqType
    left outer join ray.Supplier s WITH(NOLOCK) on s.Supplier = D.Supplier
    left outer join ray.InvAudRsn audt WITH(NOLOCK) on audt.AudtReason = D.AudtReason
    left outer join ray.Center act4 WITH(NOLOCK) on act4.Center = D.Act4
    left outer join ray.Center act5 WITH(NOLOCK) on act5.Center = D.Act5
	left outer join ray.InvAssistant_deliveryItems as di on di.FiscalYear = D.Fiscalyear and di.StoreNo = D.Storeno and di.DocType = D.doctype and di.DocNo = D.docno and di.DocRow = D.DocRow
        WHERE((d.setvoid is null or d.setvoid = 0)
                and D.Fiscalyear = {0}
                and D.Storeno = {1}
                and D.doctype = {2}
                and D.docno = {3})
        group by 
			 D.docrow, D.PartNo,i.PartNoDsc,D.DocDsc,D.NeedDate, D.Qty  ,D.Center,c.CenterDsc
            ,D.OrderNo,O.OrdrDsc,D.RcptType,R.RcptTypeDesc,D.ReqType,Q.ReqTypeDesc,D.ConsType,CnsTyp.ConsTypeDesc,D.Supplier,s.SupName
            ,D.act3,actcnt.CenterDsc,D.AudtReason,audt.AudtReasonDsc,D.Serial,D.UntCode,u.UntName
            ,D.CntQty,D.Weight,D.docdate,D.RefOrder,D.RefConsQty ,D.ReqCenter"
    , input.FiscalYear.Value, storeNo, input.DocType, input.DocNo)
    .OrderBy(o => o.docrow).Skip(input._fromIndex).Take(input._take).ToList();
                //cy.ToString() + "-" + storeNo + "-" + ConfigurationManager.AppSettings["GenerateSerialDocType"] + "-" +


                /*   دپریکیت شده (برای مدل قدیمی آب و فاضلاب بود)
                foreach (var itm in res)
                {

                    string pattern = serialPattern(itm.ReqCenter, itm.OrderNo, itm.Supplier, itm.Center, itm.PartNo);
                    itm.availableSerials.AddRange(db.InvSerials.AsNoTracking()
                        .Where(s => s.PartNo == itm.PartNo && s.StoreNo == storeNo && s.Soh > 0 && SqlFunctions.PatIndex(pattern, s.Serail) > 0).ToList());//"%-%-%-%-5730248-%-%-10101001-%"
                }
                */
                var crntFiscalYear = invTools.CurrentFiscalYear;

                foreach (var itm in res)
                {//TODO: برای انبار سریالی و بچ باید موجودی سریال ست شود
                    itm.soh = invTools.GetItemDataSoh(db, itm.PartNo, storeNo, crntFiscalYear.FiscalYear);
                }

                return Ok(res);
            }
        }
        /// <summary>
        /// تولید پترن سریال بر اساس ساختار به منظور جست و جو در اس کیو ال
        /// </summary>
        /// <param name="ReqCenter"></param>
        /// <param name="OrderNo"></param>
        /// <param name="Supplier"></param>
        /// <param name="Center"></param>
        /// <param name="PartNo"></param>
        /// <returns></returns>
        private string serialPattern(int? ReqCenter, long? OrderNo, string Supplier, int? Center, string PartNo)
        {

            var serialStrucStr = ConfigurationManager.AppSettings["serialNoStructue"].ToLower();
            var serialStruc = serialStrucStr.Split('-');

            if (serialStruc.Count() == 0 || serialStruc.Except(new string[] { "reqcenter", "reqcenter*", "orderno", "orderno*", "supplier", "supplier*", "center", "center*" }).Count() > 0)
                throw new Exception("ساختار سریال صحیح نیست");

            if (!serialStruc.Any(i => i.Contains("*")))
                throw new Exception("تعریف حداقل یکی از اقلام سریال به عنوان ساختار عنصر اصلی الزامی ست.");
            for (int i = 0; i < serialStruc.Count(); i++)
            {
                if (serialStruc[i].Contains("*"))
                {
                    serialStruc[i] = serialStruc[i].Replace("*", "");
                    if (serialStruc[i] == "reqcenter")
                        serialStruc[i] = ReqCenter.ToString();
                    else if (serialStruc[i] == "orderno")
                        serialStruc[i] = OrderNo.ToString();
                    else if (serialStruc[i] == "supplier")
                        serialStruc[i] = Supplier;
                    else if (serialStruc[i] == "center")
                        serialStruc[i] = Center.ToString();
                }
                else
                    serialStruc[i] = "%";
            }
            return string.Format("%-%-%-{0}-{1}-%", string.Join("-", serialStruc), PartNo);
        }
        /// <summary>
        /// ارایه لیست بچ های غیر مرتبط با درخواست کالای مرجع انتخابی قابل استفاده در سند حواله انبار
        /// خروجی لیست شامل اقلام عمومی (شارژ انباری) 
        /// و (بچ های غیر مرتبط منوط به بالاترین رده تایید بودن کاربر) خواهد بود.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("getOtherUsableSerialNos")]
        [Exception]
        [HttpPost]
        public IHttpActionResult getOtherUsableSerialNos(getOtherUsableSerialNosBindingModel input)
        {

            var retList = new List<getOtherUsableSerialNos>();
            int cy = invTools.CurrentFiscalYear.FiscalYear;
            using (var db = new Entities())
            {
                db.Configuration.ProxyCreationEnabled = false;

                var refds = getDocReferDtl(new getDocReferDtlBindingModel()
                {
                    DocNo = input.RefDocNo,
                    DocType = input.RefDocType,
                    _fromIndex = 0,
                    _take = int.MaxValue,
                    FiscalYear = input.FiscalYear
                });
                if (refds == null || refds.Count == 0)
                    throw new Exception("اطلاعات مرجع معتبر نیست");

                var refRow = refds.FirstOrDefault(r => r.docrow == input.RefDocrow);
                if (refRow == null)
                    throw new Exception("اطلاعات مرجع معتبر نیست");

                string pattern = serialPattern(new int?(), refRow.OrderNo, refRow.Supplier, refRow.center, refRow.partno);



                var serialStrucStr = ConfigurationManager.AppSettings["serialNoStructue"].ToLower();
                var serialStruc = serialStrucStr.Split('-');

                if (serialStruc.Count() == 0 || serialStruc.Except(new string[] { "reqcenter", "reqcenter*", "orderno", "orderno*", "supplier", "supplier*", "center", "center*" }).Count() > 0)
                    throw new Exception("ساختار سریال صحیح نیست");

                if (!serialStruc.Any(i => i.Contains("*")))
                    throw new Exception("تعریف حداقل یکی از اقلام سریال به عنوان ساختار عنصر اصلی الزامی ست.");


                //add common to retList

                var maxConfLevel = invTools.maxConfLevel;
                var userClevel = input.DocType.HasValue ? getUserConfLevel(db, input.DocType.Value) : null;
                if (!maxConfLevel.HasValue || userClevel == null || !userClevel.InvCnfrmLvl.HasValue || userClevel.InvCnfrmLvl.Value != maxConfLevel.Value)
                    return Ok(retList);



                var dbSerials = db.InvSerials.AsNoTracking()
                    .Where(s => /*(input.serial == null || s.Serail == input.serial) &&*/
                     s.PartNo == refRow.partno
                    && s.StoreNo == storeNo && s.Soh > 0
                    && SqlFunctions.PatIndex(pattern, s.Serail) == 0);



                foreach (var dbSerial in dbSerials)
                {
                    var retItm = new getOtherUsableSerialNos() { Serial = dbSerial.Serail, Soh = dbSerial.Soh };
                    for (int i = 0; i < serialStruc.Count(); i++)
                    {
                        string cl = serialStruc[i].Replace("*", "").ToLower();

                        if (cl == "orderno")
                        {
                            long orderNo;
                            if (long.TryParse(dbSerial.Serail.Split('-')[3 + i], out orderNo))
                            {
                                retItm.OrderNo = orderNo;
                                retItm.OrderNoDsc = (db.InvOrdrs.AsNoTracking().Where(o => o.OrdrNO == orderNo).FirstOrDefault() ?? new InvOrdr()).OrdrDsc;
                            }
                        }
                        if (cl == "supplier")
                        {
                            if (!string.IsNullOrEmpty(dbSerial.Serail.Split('-')[3 + i]))
                            {
                                retItm.Supplier = dbSerial.Serail.Split('-')[3 + i];
                                retItm.SupplierDsc = (db.Suppliers.AsNoTracking().Where(s => s.Supplier1 == retItm.Supplier)
                                    .FirstOrDefault() ?? new Supplier()).SupName;
                            }
                        }
                        if (cl == "center")
                        {
                            int center;
                            if (int.TryParse(dbSerial.Serail.Split('-')[3 + i], out center))
                            {
                                retItm.Center = center;
                                retItm.CenterDsc = (db.Centers.AsNoTracking().Where(o => o.Center1 == center).FirstOrDefault() ?? new Center()).CenterDsc;
                            }
                        }
                    }
                    retList.Add(retItm);
                }


                //var dt = byte.Parse(ConfigurationManager.AppSettings["GenerateSerialDocType"]);

                ////string pattern = serialPattern(itm.ReqCenter, itm.OrderNo, itm.Supplier, itm.Center, itm.PartNo);
                //var serials = db.InvSerials.AsNoTracking()
                //    .Join(db.InvDtlDatas.AsNoTracking()
                //    .Where(dtl=>dtl.StoreNo == storeNo && dtl.DocType == dt),oks=>oks.Serail,iks=>iks.Serial,(s,dtls)=>new {serial = s,docDtls = dtls })
                //    .Where(s => s. == storeNo && s.PartNo == d.partno && s.Soh > 0)
                //    /*.Select(s => new { s.Serail, s.Soh })*/.ToList();

                //foreach (var ser in serials)
                //{

                //}










            }
            return Ok(retList);
        }
        ///// <summary>
        ///// اعتبار سنجی و دریافت اطلاعات تکمیلی یک شماره سریال (بچ)
        ///// اعتبارسنجی متناسب با اطلاعات سند مرجع خواهد بود
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[Route("getAndValidateBatchInfo")]
        //[Exception]
        //[HttpPost]
        //public IHttpActionResult getAndValidateBatchInfo(getAndValidateBatchInfoBindingModel input)
        //{
        //    using (var db = new Entities())
        //    {
        //        db.Configuration.ProxyCreationEnabled = false;

        //        var dbSerial = db.InvSerials.Where(s => s.StoreNo == storeNo && s.Serail == input.serial).FirstOrDefault();
        //        if (dbSerial == null)
        //            throw new Exception("سریال / شماره دسته مورد نظر در انبار جاری یافت نشد.");

        //        var refds = getDocReferDtl(new getDocReferDtlBindingModel()//TODO : اگر تنظیمات برای نوع سند ورودی و انبار جاری با مرجع اجباری بود
        //        {
        //            DocNo = input.RefDocNo,
        //            DocType = input.RefDocType,
        //            _fromIndex = 0,
        //            _take = int.MaxValue
        //        });
        //        if (refds == null || refds.Count == 0)
        //            throw new Exception("اطلاعات مرجع معتبر نیست");


        //        if (!refds.Any(r => r.partno == dbSerial.PartNo))
        //            throw new Exception("سریال مورد نظر با توجه به اطلاعات سند مرجع قابل استفاده نمی باشد.");



        //        //string pattern = serialPattern(new int?(), refRow.OrderNo, refRow.Supplier, refRow.center, refRow.partno);
        //        var serialStrucStr = ConfigurationManager.AppSettings["serialNoStructue"].ToLower();
        //        var serialStruc = serialStrucStr.Split('-');

        //        var retItm = new getOtherUsableSerialNos();
        //        for (int i = 0; i < serialStruc.Count(); i++)
        //        {
        //            string cl = serialStruc[i].Replace("*", "").ToLower();

        //            if (cl == "orderno")
        //            {
        //                long orderNo;
        //                if (long.TryParse(dbSerial.Serail.Split('-')[3 + i], out orderNo))
        //                {
        //                    retItm.OrderNo = orderNo;
        //                    retItm.OrderNoDsc = (db.InvOrdrs.AsNoTracking().Where(o => o.OrdrNO == orderNo).FirstOrDefault() ?? new InvOrdr()).OrdrDsc;
        //                }
        //            }
        //            if (cl == "supplier")
        //            {
        //                if (!string.IsNullOrEmpty(dbSerial.Serail.Split('-')[3 + i]))
        //                {
        //                    retItm.Supplier = dbSerial.Serail.Split('-')[3 + i];
        //                    retItm.SupplierDsc = (db.Suppliers.AsNoTracking().Where(s => s.Supplier1 == retItm.Supplier)
        //                        .FirstOrDefault() ?? new Supplier()).SupName;
        //                }
        //            }
        //            if (cl == "center")
        //            {
        //                int center;
        //                if (int.TryParse(dbSerial.Serail.Split('-')[3 + i], out center))
        //                {
        //                    retItm.Center = center;
        //                    retItm.CenterDsc = (db.Centers.AsNoTracking().Where(o => o.Center1 == center).FirstOrDefault() ?? new Center()).CenterDsc;
        //                }
        //            }
        //        }




        //    }
        //}
        private InvUserLvl getUserConfLevel(Entities db, byte docType)
        {
            return db.InvUserLvls.AsNoTracking().Where(t => t.DocType == docType && t.UserId == userId && t.StoreNo == storeNo).FirstOrDefault();
        }
        /// <summary>
        /// دریافت لیست نوع سند های زیر مجموعه یک نوع سند
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        private List<keyValuePair<byte>> getSubDocTypes(int? dt, Entities db)
        {
            var t = db.Database.SqlQuery<keyValuePair<byte>>(string.Format("select doc.DocTypeDesc as dsc,Ref.DocType as id from ray.invRfDocTyp ref with(nolock)  left join ray.invDocTyp doc with(nolock) on  Ref.DocType=doc.doctype where ref.refdoctype={0}", dt));
            var retList = new List<keyValuePair<byte>>();
            retList.AddRange(from itm in t select new keyValuePair<byte>() { id = itm.id, dsc = itm.dsc });
            return retList.Where(r => invTools.supportedDocTypes.Contains(r.id)).ToList();
        }
        private List<keyValuePair<byte>> getRefDocTypes(int? dt, Entities db)
        {
            var t = db.Database.SqlQuery<keyValuePair<byte>>(string.Format("select doc.DocTypeDesc as dsc,Ref.RefDocType as id from ray.invRfDocTyp ref with(nolock)  left join ray.invDocTyp doc with(nolock) on  Ref.RefDocType=doc.doctype where ref.doctype={0}", dt));
            var retList = new List<keyValuePair<byte>>();
            retList.AddRange(from itm in t select new keyValuePair<byte>() { id = itm.id, dsc = itm.dsc });
            return retList;
        }
        private List<keyValuePair<byte?>> getRefDocTypes_v2(int? dt)
        {
            //var t = db.Database.SqlQuery<keyValuePair<byte>>(string.Format("select doc.DocTypeDesc as dsc,Ref.RefDocType as id from ray.invRfDocTyp ref with(nolock)  left join ray.invDocTyp doc with(nolock) on  Ref.RefDocType=doc.doctype where ref.doctype={0}", dt));
            //var retList = new List<keyValuePair<byte>>();
            //retList.AddRange(from itm in t select new keyValuePair<byte>() { id = itm.id, dsc = itm.dsc });
            //return retList;
            using (var repo = new Repo(this, "InvAssistantSp_RefDocTypes_GetList", "invDoc_getRefDocTypes", initAsReader: true))
            {

                repo.cmd.Parameters.AddWithValue("@baseDocType", dt.getDbValue());
                repo.ExecuteAdapter();

                return repo.ds.Tables[0].AsEnumerable().Select(b => new keyValuePair<byte?>
                {
                    id = b.Field<object>("docTypeId").dbNullCheckByte(),
                    dsc = b.Field<object>("docTypeDesc").dbNullCheckString()
                }).ToList();
            }


        }

        //select * from RpgTableSpec where TName like '%InvDtlData%'
        //SELECT* FROM ray.RpgFieldSpec where TableSpecId = '1311'
        /// <summary>
        /// دریافت ساختار فرم سربرگ سند
        /// </summary>
        /// <param name="DocType"></param>
        /// <returns></returns>
        [Route("getDefaults_Hdr")]
        [Exception]
        [HttpPost]
        public IHttpActionResult getDefaults_Hdr([FromBody]byte DocType)
        {
            var output = new List<DocumentDefaults>();
            //string storeNo = Request.Headers.FirstOrDefault(h => h.Key.Equals("StoreNo")).Value.FirstOrDefault();
            using (var db = new Entities())
            {//exec ray.InvSp_FillForceQty 40,'','9'
                db.Configuration.ProxyCreationEnabled = false;

                switch (DocType)
                {
                    case invTools.TYPE_DOC_BUY_REQ_62:
                        #region  درخواست خرید
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocNo",
                            fieldDsc = "شماره سند",
                            status = docNoStatus(db, DocType),
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDate",
                            fieldDsc = "تاریخ سند",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = dateTimeService.crntPersianDate,
                                dsc = ""
                            }

                        });

                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "ReqCenter",
                            fieldDsc = "قسمت درخواست كننده",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(DocType, "ReqCenter", storeNo)

                        });
                        #endregion
                        break;
                    case invTools.TYPE_DOC_RECEIPT_OF_PURCHASE_14:
                        #region رسيد خريد كالا
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocNo",
                            fieldDsc = "شماره سند",
                            status = docNoStatus(db, DocType),
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDate",
                            fieldDsc = "تاریخ سند",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = dateTimeService.crntPersianDate,
                                dsc = ""
                            }

                        });
                        //output.Add(new DocumentDefaults()
                        //{
                        //    fieldName = "ConsType",
                        //    fieldDsc = "شماره سفارش",
                        //    status = fieldStatus.enable,
                        //    defaults = new keyValuePair
                        //    {
                        //        id = "1",
                        //        dsc = "عمومی"
                        //    }

                        //});
                        //output.Add(new DocumentDefaults()
                        //{
                        //    fieldName = "Act3",
                        //    fieldDsc = "فعالیت 3",
                        //    status = fieldStatus.enable,
                        //    defaults = new keyValuePair
                        //    {
                        //        id = "1",
                        //        dsc = "عمومی"
                        //    }
                        //});
                        #endregion
                        break;
                    case invTools.TYPE_DOC_RETURN_FROM_BUY_16:
                        #region برگشت از خرید
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocNo",
                            fieldDsc = "شماره سند",
                            status = docNoStatus(db, DocType),
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDate",
                            fieldDsc = "تاریخ سند",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = dateTimeService.crntPersianDate,
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "RtrnBuyReason",
                            fieldDsc = "علت برگشت از خرید",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(DocType, "RtrnBuyReason", storeNo)
                        });
                        #endregion
                        break;
                    case invTools.TYPE_DOC_DRAFT_40:
                        #region حواله
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocNo",
                            fieldDsc = "شماره سند",
                            status = docNoStatus(db, DocType),
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDate",
                            fieldDsc = "تاریخ سند",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = dateTimeService.crntPersianDate,
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "OrderNo",
                            fieldDsc = "شماره سفارش",
                            status = fieldStatus.enable,
                            defaults = db.FillForceQty(DocType, "OrderNo", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act3",
                            fieldDsc = "فعالیت 3",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair() //db.FillForceQty(DocType, "Act3", storeNo)
                        });
                        #endregion
                        break;
                    case invTools.TYPE_DOC_RETURN_TO_STORE_44:
                        #region برگشت به انبار
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocNo",
                            fieldDsc = "شماره سند",
                            status = docNoStatus(db, DocType),
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDate",
                            fieldDsc = "تاریخ سند",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = dateTimeService.crntPersianDate,
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "RtrnStrReason",
                            fieldDsc = "علت برگشت به انبار",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(DocType, "RtrnStrReason", storeNo)
                        });
                        #endregion
                        break;
                    case invTools.TYPE_DOC_RECEIPT_OF_PRODUCTION_12:
                        #region رسيد كالا/توليد
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocNo",
                            fieldDsc = "شماره سند",
                            status = docNoStatus(db, DocType),
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDate",
                            fieldDsc = "تاریخ سند",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = dateTimeService.crntPersianDate,
                                dsc = ""
                            }

                        });
                        #endregion
                        break;
                    case invTools.TYPE_DOC_RETURN_FROM_RECEIPT_OF_PRODUCTION_22:
                        #region برگشت رسيد كالا/توليد
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocNo",
                            fieldDsc = "شماره سند",
                            status = docNoStatus(db, DocType),
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }
                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDate",
                            fieldDsc = "تاریخ سند",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = dateTimeService.crntPersianDate,
                                dsc = ""
                            }
                        });
                        #endregion
                        break;
                    case invTools.TYPE_DOC_TEMPORARY_RECEIPT_66:
                        #region رسيد موقت
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocNo",
                            fieldDsc = "شماره سند",
                            status = docNoStatus(db, DocType),
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDate",
                            fieldDsc = "تاریخ سند",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = dateTimeService.crntPersianDate,
                                dsc = ""
                            }

                        });
                        //output.Add(new DocumentDefaults()
                        //{
                        //    fieldName = "ConsType",
                        //    fieldDsc = "شماره سفارش",
                        //    status = fieldStatus.enable,
                        //    defaults = new keyValuePair
                        //    {
                        //        id = "1",
                        //        dsc = "عمومی"
                        //    }

                        //});
                        //output.Add(new DocumentDefaults()
                        //{
                        //    fieldName = "Act3",
                        //    fieldDsc = "فعالیت 3",
                        //    status = fieldStatus.enable,
                        //    defaults = new keyValuePair
                        //    {
                        //        id = "1",
                        //        dsc = "عمومی"
                        //    }
                        //});
                        #endregion
                        break;
                }
            }
            return Ok(output);
        }
        /// <summary>
        /// دریافت ساختار فرم جزئیات سند
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("getDefaults_Dtl")]
        [Exception]
        [HttpPost]
        public IHttpActionResult getDefaults_Dtl([FromBody] getDefaults_DtlBindingModel input)
        {
            var output = new List<DocumentDefaults>();
            //string storeNo = Request.Headers.FirstOrDefault(h => h.Key.Equals("StoreNo")).Value.FirstOrDefault();
            using (var db = new Entities())
            {
                db.Configuration.ProxyCreationEnabled = false;


                switch (input.docType)
                {
                    case invTools.TYPE_DOC_BUY_REQ_62:
                        #region درخواست خرید
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "PartNo",
                            fieldDsc = "کالا",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Center",
                            fieldDsc = "مرکز هزینه",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "Center", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "OrderNo",
                            fieldDsc = "شماره سفارش",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "OrderNo", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "RcptType",
                            fieldDsc = "نوع رسید",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "RcptType", storeNo)

                        });

                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "ReqType",
                            fieldDsc = "نوع درخواست",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "ReqType", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act3",
                            fieldDsc = "فعالیت 3",
                            status = fieldStatus.enable,
                            defaults = db.act3Defaults(input.partNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Qty",
                            fieldDsc = "مقدار ردیف",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });

                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Weight",
                            fieldDsc = "وزن",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "NeedDate",
                            fieldDsc = "تاریخ نیاز",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });

                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "CntQty",
                            fieldDsc = "مقدار شمارش",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });

                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Amt",
                            fieldDsc = "مبلغ برآوردی خرید",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });

                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDsc",
                            fieldDsc = "توضیحات",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });

                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act4",
                            fieldDsc = "فعالیت 4",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair() //db.FillForceQty(input.docType, "Act4", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act5",
                            fieldDsc = "فعالیت 5",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair() //db.FillForceQty(input.docType, "Act5", storeNo)

                        });

                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "UntCode",
                            fieldDsc = "واحد کالا",
                            status = fieldStatus.required,
                            defaults = new keyValuePair()

                        });
                        #endregion
                        break;
                    case invTools.TYPE_DOC_RECEIPT_OF_PURCHASE_14:
                        #region رسيد خريد كالا
                        #region اطلاعات جزئیات سند
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "PartNo",
                            fieldDsc = "کالا",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Serial",
                            fieldDsc = "سریال",
                            status = new byte?[] { 2, 3 }.Contains(db.Stores.AsNoTracking().Where(s => s.StoreNo == storeNo).FirstOrDefault().SerialTyp) ? fieldStatus.required : fieldStatus.invisible,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "BinNo",
                            fieldDsc = "شماره قفسه",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair()//db.FillForceQty(input.docType, "BinNo", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Center",
                            fieldDsc = "مرکز هزینه",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "Center", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "OrderNo",
                            fieldDsc = "شماره سفارش",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "OrderNo", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "RcptType",
                            fieldDsc = "نوع رسید",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "RcptType", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Supplier",
                            fieldDsc = "فروشنده",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "Supplier", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act3",
                            fieldDsc = "فعالیت 3",
                            status = fieldStatus.enable,
                            defaults = db.act3Defaults(input.partNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Qty",
                            fieldDsc = "مقدار ردیف",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "CntQty",
                            fieldDsc = "مقدار شمارش",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Weight",
                            fieldDsc = "وزن",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "amt",
                            fieldDsc = "مبلغ رسید",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "TollAmt",
                            fieldDsc = "عوارض",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "TaxAmt",
                            fieldDsc = "مالیات",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDsc",
                            fieldDsc = "توضیحات",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "AccDocNo",
                            fieldDsc = "شماره سند مالی",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "FactorNo",
                            fieldDsc = "شماره فاکتور",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "FactorDate",
                            fieldDsc = "تاریخ فاکتور",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "RfDocNo",
                            fieldDsc = "شماره سند سفارش",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "RfDocRow",
                            fieldDsc = "شماره ردیف سفارش",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act4",
                            fieldDsc = "فعالیت 4",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair() //db.FillForceQty(input.docType, "Act4", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act5",
                            fieldDsc = "فعالیت 5",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair() //db.FillForceQty(input.docType, "Act5", storeNo)

                        });
                        #endregion
                        #region جدول شناسنامه سریال
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["getSerialInfoOnThisDocType"]) && ConfigurationManager.AppSettings["getSerialInfoOnThisDocType"] == input.docType.ToString())
                        {
                            output.Add(new DocumentDefaults()
                            {
                                fieldName = "InvSerial_Supplier",
                                fieldDsc = "نام سازنده",
                                status = fieldStatus.enable,
                                defaults = new keyValuePair
                                {
                                    id = "",
                                    dsc = ""
                                }

                            });
                            output.Add(new DocumentDefaults()
                            {
                                fieldName = "InvSerial_Model",
                                fieldDsc = "مدل",
                                status = fieldStatus.enable,
                                defaults = new keyValuePair
                                {
                                    id = "",
                                    dsc = ""
                                }

                            });
                            output.Add(new DocumentDefaults()
                            {
                                fieldName = "InvSerial_QcNo",
                                fieldDsc = "شناسه کنترل کیفیت",
                                status = fieldStatus.enable,
                                defaults = new keyValuePair
                                {
                                    id = "",
                                    dsc = ""
                                }

                            });
                            output.Add(new DocumentDefaults()
                            {
                                fieldName = "InvSerial_SuppExpDate",
                                fieldDsc = "تاریخ انقضاء",
                                status = fieldStatus.enable,
                                defaults = new keyValuePair
                                {
                                    id = "",
                                    dsc = ""
                                }

                            });
                            output.Add(new DocumentDefaults()
                            {
                                fieldName = "InvSerial_OptmIsuDat",
                                fieldDsc = "تاریخ بهینه مصرف",
                                status = fieldStatus.enable,
                                defaults = new keyValuePair
                                {
                                    id = "",
                                    dsc = ""
                                }

                            });
                        }
                        #endregion
                        #endregion
                        break;
                    case invTools.TYPE_DOC_RETURN_FROM_BUY_16:
                        #region برگشت از خرید
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "PartNo",
                            fieldDsc = "کالا",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Serial",
                            fieldDsc = "سریال",
                            status = new byte?[] { 2, 3 }.Contains(db.Stores.AsNoTracking().Where(s => s.StoreNo == storeNo).FirstOrDefault().SerialTyp) ? fieldStatus.required : fieldStatus.invisible,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Center",
                            fieldDsc = "مرکز هزینه",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "Center", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "OrderNo",
                            fieldDsc = "شماره سفارش",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "OrderNo", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "RcptType",
                            fieldDsc = "نوع رسید",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "RcptType", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Supplier",
                            fieldDsc = "فروشنده",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "Supplier", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act3",
                            fieldDsc = "فعالیت 3",
                            status = fieldStatus.enable,
                            defaults = db.act3Defaults(input.partNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Qty",
                            fieldDsc = "مقدار ردیف",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "CntQty",
                            fieldDsc = "مقدار شمارش",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Weight",
                            fieldDsc = "وزن",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "amt",
                            fieldDsc = "مبلغ برگشتی",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "TollAmt",
                            fieldDsc = "عوارض",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "TaxAmt",
                            fieldDsc = "مالیات",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDsc",
                            fieldDsc = "توضیحات",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "AccDocNo",
                            fieldDsc = "شماره سند مالی",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "FactorNo",
                            fieldDsc = "شماره فاکتور",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act4",
                            fieldDsc = "فعالیت 4",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair() //db.FillForceQty(input.docType, "Act4", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act5",
                            fieldDsc = "فعالیت 5",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair()//db.FillForceQty(input.docType, "Act5", storeNo)

                        });




                        #endregion
                        break;
                    case invTools.TYPE_DOC_DRAFT_40:
                        #region حواله
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "PartNo",
                            fieldDsc = "کالا",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Serial",
                            fieldDsc = "سریال",
                            status = new byte?[] { 2, 3 }.Contains(db.Stores.AsNoTracking().Where(s => s.StoreNo == storeNo).FirstOrDefault().SerialTyp) ? fieldStatus.required : fieldStatus.invisible,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Center",
                            fieldDsc = "مرکز هزینه",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "OrderNo",
                            fieldDsc = "شماره سفارش",//TODO: برای آب و فاضلاب غیر فعال و سایرین اجباری
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "OrderNo", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "ConsType",
                            fieldDsc = "نوع مصرف",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "ConsType", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act3",
                            fieldDsc = "فعالیت 3",
                            status = fieldStatus.enable,
                            defaults = db.act3Defaults(input.partNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Qty",
                            fieldDsc = "مقدار ردیف",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Weight",
                            fieldDsc = "وزن",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "CntQty",
                            fieldDsc = "مقدار شمارش",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDsc",
                            fieldDsc = "توضیحات",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "TreatyNo",
                            fieldDsc = "شماره قرارداد",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act4",
                            fieldDsc = "فعالیت 4",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair() //db.FillForceQty(input.docType, "Act4", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act5",
                            fieldDsc = "فعالیت 5",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair() //db.FillForceQty(input.docType, "Act5", storeNo)

                        });


                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "GdiRefDocNo",
                            fieldDsc = "ش سند درخواست کالا",
                            status = fieldStatus.invisible,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });

                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "GdiRefDocRow",
                            fieldDsc = "ش ردیف درخواست کالا",
                            status = fieldStatus.invisible,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "UntCode",
                            fieldDsc = "واحد کالا",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "QcCode",
                            fieldDsc = "شناسه کنترل کیفیت",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        #endregion
                        break;
                    case invTools.TYPE_DOC_RETURN_TO_STORE_44:
                        #region برگشت به انبار
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "PartNo",
                            fieldDsc = "کالا",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Serial",
                            fieldDsc = "سریال",
                            status = new byte?[] { 2, 3 }.Contains(db.Stores.AsNoTracking().Where(s => s.StoreNo == storeNo).FirstOrDefault().SerialTyp) ? fieldStatus.required : fieldStatus.invisible,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Center",
                            fieldDsc = "مرکز هزینه",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "OrderNo",
                            fieldDsc = "شماره سفارش",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "OrderNo", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "ConsType",
                            fieldDsc = "نوع مصرف",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "ConsType", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act3",
                            fieldDsc = "فعالیت 3",
                            status = fieldStatus.enable,
                            defaults = db.act3Defaults(input.partNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Qty",
                            fieldDsc = "مقدار ردیف",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Weight",
                            fieldDsc = "وزن",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "CntQty",
                            fieldDsc = "مقدار شمارش",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "amt",
                            fieldDsc = "مبلغ برگشتی",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDsc",
                            fieldDsc = "توضیحات",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "TreatyNo",
                            fieldDsc = "شماره قرارداد",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act4",
                            fieldDsc = "فعالیت 4",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair() //db.FillForceQty(input.docType, "Act4", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act5",
                            fieldDsc = "فعالیت 5",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair() //db.FillForceQty(input.docType, "Act5", storeNo)

                        });
                        #endregion
                        break;
                    case invTools.TYPE_DOC_RECEIPT_OF_PRODUCTION_12:
                        #region رسيد كالا/توليد
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "PartNo",
                            fieldDsc = "کالا",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Serial",
                            fieldDsc = "سریال",
                            status = new byte?[] { 2, 3 }.Contains(db.Stores.AsNoTracking().Where(s => s.StoreNo == storeNo).FirstOrDefault().SerialTyp) ? fieldStatus.required : fieldStatus.invisible,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "BinNo",
                            fieldDsc = "شماره قفسه",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair()//db.FillForceQty(input.docType, "BinNo", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Center",
                            fieldDsc = "مرکز هزینه",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "Center", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "OrderNo",
                            fieldDsc = "شماره سفارش",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "OrderNo", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "RcptType",
                            fieldDsc = "نوع رسید",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "RcptType", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act3",
                            fieldDsc = "فعالیت 3",
                            status = fieldStatus.enable,
                            defaults = db.act3Defaults(input.partNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Qty",
                            fieldDsc = "مقدار ردیف",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "CntQty",
                            fieldDsc = "مقدار شمارش",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Weight",
                            fieldDsc = "وزن",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "amt",
                            fieldDsc = "مبلغ رسید",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDsc",
                            fieldDsc = "توضیحات",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "AccDocNo",
                            fieldDsc = "شماره سند مالی",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act4",
                            fieldDsc = "فعالیت 4",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair() //db.FillForceQty(input.docType, "Act4", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act5",
                            fieldDsc = "فعالیت 5",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair() //db.FillForceQty(input.docType, "Act5", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "QcCode",
                            fieldDsc = "شناسه کنترل کیفیت",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        #endregion
                        break;
                    case invTools.TYPE_DOC_RETURN_FROM_RECEIPT_OF_PRODUCTION_22:
                        #region برگشت رسيد كالا/توليد
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "PartNo",
                            fieldDsc = "کالا",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }
                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Serial",
                            fieldDsc = "سریال",
                            status = new byte?[] { 2, 3 }.Contains(db.Stores.AsNoTracking().Where(s => s.StoreNo == storeNo).FirstOrDefault().SerialTyp) ? fieldStatus.required : fieldStatus.invisible,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Center",
                            fieldDsc = "مرکز هزینه",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "Center", storeNo)
                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "OrderNo",
                            fieldDsc = "شماره سفارش",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "OrderNo", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "RcptType",
                            fieldDsc = "نوع رسید",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "RcptType", storeNo)
                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act3",
                            fieldDsc = "فعالیت 3",
                            status = fieldStatus.enable,
                            defaults = db.act3Defaults(input.partNo)
                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Qty",
                            fieldDsc = "مقدار ردیف",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }
                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "CntQty",
                            fieldDsc = "مقدار شمارش",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }
                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Weight",
                            fieldDsc = "وزن",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }
                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "amt",
                            fieldDsc = "مبلغ برگشتی",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }
                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDsc",
                            fieldDsc = "توضیحات",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }
                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act4",
                            fieldDsc = "فعالیت 4",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair() //db.FillForceQty(input.docType, "Act4", storeNo)
                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act5",
                            fieldDsc = "فعالیت 5",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair()//db.FillForceQty(input.docType, "Act5", storeNo)
                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "QcCode",
                            fieldDsc = "شناسه کنترل کیفیت",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        #endregion
                        break;
                    case invTools.TYPE_DOC_TEMPORARY_RECEIPT_66:
                        #region رسيد موقت
                        #region جدول جزئیات
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "PartNo",
                            fieldDsc = "کالا",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "BinNo",
                            fieldDsc = "شماره قفسه",
                            status = fieldStatus.invisible,
                            defaults = new keyValuePair()//db.FillForceQty(input.docType, "BinNo", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Center",
                            fieldDsc = "مرکز هزینه",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "Center", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "OrderNo",
                            fieldDsc = "شماره سفارش",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "OrderNo", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "RcptType",
                            fieldDsc = "نوع رسید",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "RcptType", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Supplier",
                            fieldDsc = "فروشنده",
                            status = fieldStatus.required,
                            defaults = db.FillForceQty(input.docType, "Supplier", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act3",
                            fieldDsc = "فعالیت 3",
                            status = fieldStatus.enable,
                            defaults = db.act3Defaults(input.partNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Serial",
                            fieldDsc = "سریال",
                            status = new byte?[] { 2, 3 }.Contains(db.Stores.AsNoTracking().Where(s => s.StoreNo == storeNo).FirstOrDefault().SerialTyp) ? fieldStatus.required : fieldStatus.invisible,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Qty",
                            fieldDsc = "مقدار ردیف",
                            status = fieldStatus.required,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "CntQty",
                            fieldDsc = "مقدار شمارش",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Weight",
                            fieldDsc = "وزن",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "amt",
                            fieldDsc = "مبلغ رسید",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "TollAmt",
                            fieldDsc = "عوارض",
                            status = fieldStatus.invisible,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "TaxAmt",
                            fieldDsc = "مالیات",
                            status = fieldStatus.invisible,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "DocDsc",
                            fieldDsc = "توضیحات",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "AccDocNo",
                            fieldDsc = "شماره سند مالی",
                            status = fieldStatus.invisible,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "FactorNo",
                            fieldDsc = "شماره فاکتور",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "FactorDate",
                            fieldDsc = "تاریخ فاکتور",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "RfDocNo",
                            fieldDsc = "شماره سند سفارش",
                            status = fieldStatus.invisible,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "RfDocRow",
                            fieldDsc = "شماره ردیف سفارش",
                            status = fieldStatus.invisible,
                            defaults = new keyValuePair
                            {
                                id = "",
                                dsc = ""
                            }

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act4",
                            fieldDsc = "فعالیت 4",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair() //db.FillForceQty(input.docType, "Act4", storeNo)

                        });
                        output.Add(new DocumentDefaults()
                        {
                            fieldName = "Act5",
                            fieldDsc = "فعالیت 5",
                            status = fieldStatus.enable,
                            defaults = new keyValuePair() //db.FillForceQty(input.docType, "Act5", storeNo)

                        });
                        #endregion
                        #region جدول شناسنامه سریال
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["getSerialInfoOnThisDocType"]) && ConfigurationManager.AppSettings["getSerialInfoOnThisDocType"] == input.docType.ToString())
                        {
                            output.Add(new DocumentDefaults()
                            {
                                fieldName = "InvSerial_Supplier",
                                fieldDsc = "نام سازنده",
                                status = fieldStatus.enable,
                                defaults = new keyValuePair
                                {
                                    id = "",
                                    dsc = ""
                                }

                            });
                            output.Add(new DocumentDefaults()
                            {
                                fieldName = "InvSerial_Model",
                                fieldDsc = "مدل",
                                status = fieldStatus.enable,
                                defaults = new keyValuePair
                                {
                                    id = "",
                                    dsc = ""
                                }

                            });
                            output.Add(new DocumentDefaults()
                            {
                                fieldName = "InvSerial_QcNo",
                                fieldDsc = "شناسه کنترل کیفیت",
                                status = fieldStatus.enable,
                                defaults = new keyValuePair
                                {
                                    id = "",
                                    dsc = ""
                                }

                            });
                            output.Add(new DocumentDefaults()
                            {
                                fieldName = "InvSerial_SuppExpDate",
                                fieldDsc = "تاریخ انقضاء",
                                status = fieldStatus.enable,
                                defaults = new keyValuePair
                                {
                                    id = "",
                                    dsc = ""
                                }

                            });
                            output.Add(new DocumentDefaults()
                            {
                                fieldName = "InvSerial_OptmIsuDat",
                                fieldDsc = "تاریخ بهینه مصرف",
                                status = fieldStatus.enable,
                                defaults = new keyValuePair
                                {
                                    id = "",
                                    dsc = ""
                                }

                            });
                        }
                        #endregion
                        #endregion
                        break;
                }
            }
            return Ok(output);
        }

        private static readonly object lck_insert = new object();

        /// <summary>
        /// ثبت سند جدید
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [Route("insert")]
        [Exception]
        [HttpPost]
        public async Task<IHttpActionResult> insert(invHdrDataBindingModel doc)
        {
            //lock (lck_insert)
            //{
            if (!ModelState.IsValid)
            {
                return new BadRequestActionResult(ModelState.Values);
            }

            var crntFiscalYear = invTools.CurrentFiscalYear;


            if (doc.DocNo == null)
                doc.DocNo = new int?();
            doc.Msg = string.Empty;

            if (ConfigurationManager.AppSettings["api"].ToLower() == "official")
            {
                //var rs = await insertOfficial(doc);
                doc = insertOfficialUsingSp(doc);
                if (string.IsNullOrEmpty(doc.Msg))
                    return Ok(doc);
                return new NotFoundActionResult(doc.Msg);
            }
            if (ConfigurationManager.AppSettings["api"].ToLower() == "official_web")
            {

                var hdr = new InventoryJournal()
                {
                    DocumentNo = doc.DocNo.HasValue ? doc.DocNo.Value : 0,
                    CreateDate = doc.DocDate,  //dateTimeService.crntPersianDate,
                    FiscalYear = crntFiscalYear.FiscalYear,
                    InventoryDocumentTypeId = doc.DocType,
                    WarehouseId = storeNo,
                    CenterId = doc.items.FirstOrDefault().Center,
                    ReturnToInventoryReasonId = doc.RtrnStrReason,
                    InventoryOrderId = doc.items.FirstOrDefault().OrderNo,
                    DemandedCenterId = (doc.ReqCenter.HasValue && doc.ReqCenter.Value != 0) ? doc.ReqCenter.Value : new int?(),


                    ReferenceDocumentNo = doc.RefDocNo,
                    ReferenceDocumentTypeId = doc.RefDocType,
                    ReferenceFiscalYear = doc.RefFiscalYear,


                    //InventoryJournalItems = new List<InventoryJournalItem>()
                };
                short i = 0;
                doc.items.ForEach(d => d.docrow = ++i);

                var q =
                from d in doc.items
                select new InventoryJournalItem()
                {
                    DocumentNo = doc.DocNo.HasValue ? doc.DocNo.Value : 0,
                    FiscalYear = crntFiscalYear.FiscalYear,
                    InventoryDocumentTypeId = doc.DocType,
                    WarehouseId = storeNo,
                    ItemDataId = d.PartNo,
                    DocumentRow = d.docrow.Value,
                    Qty = d.Qty,
                    CenterId = d.Center,
                    InventoryOrderId = d.OrderNo,
                    ItemReceiptTypeId = d.RcptType,
                    SupplierId = d.Supplier,
                    UnitId = d.UntCode,
                    Center3Id = d.Act3,
                    NeedDate = d.NeedDate,
                    ItemConsumptionTypeId = d.ConsType,
                    Serial = d.Serial,
                    Center4Id = d.Act4,
                    Center5Id = d.Act5,
                    CountedQty = d.CntQty,
                    Amount = new decimal?(),// d.Amt,//TODO: فعلا در دستیار نیست.
                    Description = d.DocDsc,
                    ReturnFromBuyReason = doc.RtrnBuyReason, //d.RtrnBuyReason,
                    InvoiceDate = d.FactorDate,
                    InvoiceNo = d.FactorNo,
                    RequestTypeId = d.ReqType,
                    Weight = d.Weight,
                    TollAmount = d.TollAmt,
                    TaxAmount = d.TaxAmt,
                    ReturnToWarehouseReason = doc.RtrnStrReason, //d.RtrnStrReason,
                    FinancialDocumentNo = d.AccDocNo,
                    InventoryRackId = d.BinNo,
                    ReferenceDocumentNo = doc.RefDocNo,
                    ReferenceWarehouseId = storeNo,
                    ReferenceRowNo = d.RefDocRow,
                    ReferenceItemDataId = d.PartNo, // warrning

                };

                hdr.InventoryJournalItems = q.ToList();

                ApiValidationResult res = null;
                try
                {
                    res = await GenerateDocument(hdr, new InventoryJournalLogicDto()
                    {
                        ComputerName = "dastyar",
                        HasRefrence = doc.HasRefrence != null && doc.HasRefrence.HasValue && doc.HasRefrence.Value,
                        RefDocdate = doc.RefDocdate,
                        MultiReference = false,
                    });
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Unauthorized();
                }
                catch (Exception ex)
                {
                    return new UnauthorizedActionResult(ex.Message);
                }


                if (res == null || res.validationResults == null /*|| res.documentNo == 0*/)
                    return new NotFoundActionResult("پاسخ بازگشتی از سیستم حسابداری انبار نامعتبر است.");

                if (res.documentNo == 0)
                    return new NotFoundActionResult(/*res.validationResults != null &&*/ res.validationResults.Count > 0 ? string.Join(" - ", res.validationResults) : "پاسخ بازگشتی از سیستم حسابداری انبار نامعتبر است.");

                if (res.validationResults.Where(r => !r.Contains("M452")).Count() == 0)
                {
                    doc.DocNo = res.documentNo;
                    doc.Msg = res.validationResults.Count == 0 ? string.Format("سند با شماره {0} با موفقیت ثبت شد.", res.documentNo) : string.Join(" - ", res.validationResults);
                    return Ok(doc);
                }
                else
                    return new NotFoundActionResult(string.Join(" - ", res.validationResults));

                //using (var client = new HttpClient())
                //{
                //    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["RayvarzApiAddress"]);
                //    var arrayList = new ArrayList
                //{
                //    loginData.UserId, // User Id
                //    sha256Str, //Password
                //};
                //    var response = client.PostAsJsonAsync("RayvarzApi/Core/Sso/Authenticate", arrayList).Result;
                //    if (response.IsSuccessStatusCode)
                //    {
                //        string resContent = await response.Content.ReadAsStringAsync();
                //        resContent = resContent.Replace("\"", "");
                //        //accessToken = resContent;
                //        if (string.IsNullOrEmpty(resContent))
                //            return Unauthorized();
                //        return Ok(resContent);
                //    }
                //    else
                //        return Unauthorized();
                //    //return new NotFoundActionResult(response.ReasonPhrase);
                //}
            }
            ////\temporary





            using (var db = new Entities())
            {
                #region doc no
                if (docNoStatus(db, doc.DocType) == fieldStatus.disable)
                {
                    var lastDoc = db.InvHdrDatas.Where(h => h.DocType == doc.DocType && h.FiscalYear == crntFiscalYear.FiscalYear && h.StoreNo == storeNo).ToList().OrderByDescending(o => o.DocNo).FirstOrDefault();
                    if (lastDoc == null)
                    {
                        var seri = db.InvSeris.AsNoTracking().Where(s => s.StoreNo == storeNo && s.DocType == doc.DocType).FirstOrDefault();
                        if (seri == null || !seri.BgnSeri.HasValue)
                            return new NotFoundActionResult("سری شماره سند اتوماتیک تعریف نشده است.");
                        doc.DocNo = Convert.ToInt32(seri.BgnSeri.ToString() + doc.DocType.ToString().PadLeft(2, '0') + "00001");
                    }
                    else
                        doc.DocNo = lastDoc.DocNo + 1;
                }
                else
                {
                    if (db.InvHdrDatas.Any(h => h.DocNo == doc.DocNo && h.DocType == doc.DocType && h.FiscalYear == crntFiscalYear.FiscalYear && h.StoreNo == storeNo))
                        return new NotFoundActionResult("شماره سند تکراری می باشد.");
                }
                #endregion



                var referHdr = new DocRefer_HdrModel();
                var referDtls = new List<InvSp_SelectDocReferDtl_Result_>();

                #region refer
                if (doc.RefDocNo.HasValue && doc.RefDocType.HasValue)
                {
                    if (!getRefDocTypes(doc.DocType, db).Select(s => s.id).Contains(doc.RefDocType.Value))
                        throw new Exception("نوع سند مرجع معتبر نمی باشد");


                    referHdr = getDocReferHdr(new getDocReferHdrBindingModel() { DocNo = doc.RefDocNo.Value, DocType = doc.RefDocType.Value, _fromIndex = 0, _take = 1 }).FirstOrDefault();
                    if (referHdr == null)
                        throw new Exception("اطلاعات سند مرجع یافت نشد");
                    referDtls = getDocReferDtl(new getDocReferDtlBindingModel() { FiscalYear = doc.RefFiscalYear, DocNo = doc.RefDocNo.Value, DocType = doc.RefDocType.Value, _fromIndex = 0, _take = int.MaxValue });
                    if (referDtls == null || referDtls.Count == 0)
                        throw new Exception("ردیف های سند مرجع یافت نشد");



                    var er = doc.items.GroupBy(g => g.docrow)
                                 .Join(referDtls, oks => oks.Key, iks => iks.docrow, (d, r) => new { d, r })
                                 .FirstOrDefault(i => i.d.Sum(s => s.Qty) > i.r.RemainQty);//ردیفی که مجموع مقدار آن از مقدار مجاز تعیین شده در مرجع بیشتر است

                    if (er != null)
                        throw new Exception(string.Format("مجموع مقدار مربوط به ردیف {0} با کد کالای {1} از مقدار قابل استفاده در سند مرجع انتخابی تجاوز کرده است. حداکثر مقدار قابل استفاده {3} می باشد.", er.d.Key, er.d.FirstOrDefault().PartNo, er.r.RemainQty));


                }
                #endregion





                if (doc.DocType == invTools.TYPE_DOC_BUY_REQ_62)
                {
                    #region ثبت درخواست خرید
                    //do hdr validations
                    if (int.Parse(doc.DocDate) > int.Parse(crntFiscalYear.EndFiscalYear) || int.Parse(dateTimeService.crntPersianDate) < int.Parse(crntFiscalYear.StartFiscalYear))
                        throw new Exception("تاریخ سند معتبر نمی باشد.");
                    db.InvHdrDatas.Add(new InvHdrData()
                    {
                        FiscalYear = crntFiscalYear.FiscalYear,
                        StoreNo = storeNo,
                        DocType = doc.DocType,
                        DocNo = doc.DocNo.Value,
                        CountNo = 0,
                        StkCountNo = 0,
                        DocEntStatus = 1,
                        DocStatus = 3,
                        SumQty = doc.items.Sum(i => i.Qty),
                        SumAmt = doc.items.Sum(i => i.Amt),
                        CreateDate = dateTimeService.crntPersianDate,
                        UpdateDate = dateTimeService.crntPersianDate,
                        LockInd = 0,
                        IsSendAst = 0,
                        GdiRefFiscalYear = crntFiscalYear.FiscalYear,
                        GdiRefDocType = 60,
                        SumWeight = doc.items.Sum(i => i.Weight),
                        IsEditAcnt = 0,
                        UserId = userId,
                        RowGuid = Guid.NewGuid()
                    });
                    short docRow = 0;
                    //var dbDtls = new List<InvDtlData>();
                    foreach (var dtl in doc.items)
                    {
                        dtl.Msg = string.Empty;
                        //var soh = invTools.GetItemDataSoh(dtl.PartNo, StoreNo, crntFiscalYear.FiscalYear);
                        //if (dtl.Qty > soh)
                        //    dtl.Msg = "مقدار موجودی انبار با توجه به مقدار سند منفی می گردد.";

                        db.InvDtlDatas.Add(new Models.ray.InvDtlData()
                        {
                            FiscalYear = crntFiscalYear.FiscalYear,
                            StoreNo = storeNo,
                            DocType = doc.DocType,
                            DocNo = doc.DocNo.Value,
                            StkCountNo = 0,
                            CountNo = 0,
                            DocEntStatus = 1,
                            DocRow = ++docRow,
                            PartNo = dtl.PartNo,
                            Center = dtl.Center,
                            DocDate = doc.DocDate,
                            UntCode = dtl.UntCode,
                            Qty = dtl.Qty > 0 ? dtl.Qty : dtl.Qty * -1,
                            RefConsQty = 0,
                            RefOrder = 0,
                            Amt = dtl.Amt,
                            RcptType = dtl.RcptType,
                            ReqType = dtl.ReqType,
                            ReqCenter = doc.ReqCenter,
                            NeedDate = dtl.NeedDate,
                            OrderNo = dtl.OrderNo,
                            BinNo = "",
                            PrcDate = doc.DocDate,
                            PrcSeqDoc = doc.DocType,
                            Weight = dtl.Weight,
                            UpdTime = 0,
                            SetVoid = 0,
                            Act3 = dtl.Act3,
                            Act4 = dtl.Act4,
                            Act5 = dtl.Act5,
                            DocDsc = dtl.DocDsc,
                            CntQty = dtl.CntQty,
                            OldRow = docRow,
                            RowGuid = Guid.NewGuid()

                        });



                    }
                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.Any(d => !string.IsNullOrEmpty(d.Msg)))
                        doc.Msg = "یک یا بیش از یکی از ردیف های سند شامل خطا می باشند.";
                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.All(i => string.IsNullOrEmpty(i.Msg)))
                    {
                        try
                        {
                            db.SaveChanges();
                            try
                            {
                                db.InvSp_UpdateInvMstrOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, new bool?());
                                db.InvSp_DelInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                db.InvSp_UpdateInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                db.InvSp_UpdateInvArcSohOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, doc.DocDate);
                                db.InvSp_UpdateInvArcSerialSoh(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo);
                            }
                            catch (Exception ex) {/*ignore*/ }
                        }
                        catch { throw; }
                        finally
                        {

                        }
                    }
                    #endregion
                }
                else if (doc.DocType == invTools.TYPE_DOC_RECEIPT_OF_PURCHASE_14)
                {
                    #region ثبت رسید خرید
                    //do hdr validations
                    if (int.Parse(doc.DocDate) > int.Parse(crntFiscalYear.EndFiscalYear) || int.Parse(dateTimeService.crntPersianDate) < int.Parse(crntFiscalYear.StartFiscalYear))
                        throw new Exception("تاریخ سند معتبر نمی باشد.");
                    db.InvHdrDatas.Add(new InvHdrData()
                    {
                        FiscalYear = crntFiscalYear.FiscalYear,
                        DocNo = doc.DocNo.Value,
                        UpdateDate = dateTimeService.crntPersianDate,
                        StoreNo = storeNo,
                        DocType = doc.DocType,
                        DocStatus = 3,
                        SumQty = doc.items.Sum(i => i.Qty),
                        SumAmt = doc.items.Sum(i => i.Amt),
                        SumWeight = doc.items.Sum(i => i.Weight),
                        CountNo = 0,
                        StkCountNo = 0,
                        DocEntStatus = 1,
                        CreateDate = dateTimeService.crntPersianDate,
                        LockInd = 0,
                        GdiRefFiscalYear = crntFiscalYear.FiscalYear,
                        GdiRefDocType = 66,
                        UserId = userId,
                        //IsSendAst = 0,
                        //IsEditAcnt = 0,
                        RowGuid = Guid.NewGuid()
                    });
                    short docRow = 0;
                    //var dbDtls = new List<InvDtlData>();
                    foreach (var dtl in doc.items.OrderBy(o => o.docrow))
                    {
                        dtl.Msg = string.Empty;
                        //var soh = invTools.GetItemDataSoh(dtl.PartNo, StoreNo, crntFiscalYear.FiscalYear);
                        //if (dtl.Qty > soh)
                        //    dtl.Msg = "مقدار موجودی انبار با توجه به مقدار سند منفی می گردد.";

                        db.InvDtlDatas.Add(new Models.ray.InvDtlData()
                        {
                            FiscalYear = crntFiscalYear.FiscalYear,
                            StoreNo = storeNo,
                            DocType = doc.DocType,
                            DocNo = doc.DocNo.Value,
                            CountNo = 0,
                            StkCountNo = 0,
                            DocEntStatus = 1,
                            DocRow = ++docRow,
                            PartNo = dtl.PartNo,
                            Serial = dtl.Serial,
                            BinNo = dtl.BinNo,
                            Center = dtl.Center,
                            OrderNo = dtl.OrderNo,
                            RcptType = dtl.RcptType,
                            Supplier = dtl.Supplier,
                            Act3 = dtl.Act3,
                            Qty = dtl.Qty > 0 ? dtl.Qty : dtl.Qty * -1,
                            UntCode = db.ItemDatas.Where(i => i.PartNo == dtl.PartNo).FirstOrDefault().UntCode, //dtl.UntCode,
                                                                                                                //CntQty = dtl.CntQty,
                            Weight = dtl.Weight,
                            PrcDate = doc.DocDate,
                            PrcSeqDoc = 14,
                            UpdTime = 0,
                            OldRow = docRow,
                            SetVoid = 0,
                            Amt = dtl.Amt,
                            DocDate = doc.DocDate,
                            RefOrder = 0,
                            RefConsQty = 0,
                            AccDocNo = dtl.AccDocNo,
                            TollAmt = dtl.TollAmt,
                            TaxAmt = dtl.TaxAmt,
                            DocDsc = dtl.DocDsc,
                            SaleQty = 0,
                            Act4 = dtl.Act4,
                            FactorNo = dtl.FactorNo,
                            Act5 = dtl.Act5,
                            FactorDate = dtl.FactorDate,
                            RfDocNo = dtl.RfDocNo,
                            RfDocRow = dtl.RfDocRow,
                            //ConsType = dtl.ConsType,
                            RowGuid = Guid.NewGuid()
                        });
                        if (doc.RefDocNo.HasValue && doc.RefDocType.HasValue)
                        {//insert to rf
                            db.InvRfDocs.Add(new Models.ray.InvRfDoc()
                            {
                                FiscalYear = crntFiscalYear.FiscalYear,
                                StoreNo = storeNo,
                                DocType = doc.DocType,
                                DocNo = doc.DocNo.Value,
                                CountNo = 0,
                                DocRow = docRow,

                                RefFiscalYear = crntFiscalYear.FiscalYear,
                                RefStore = storeNo,
                                RefDocType = doc.RefDocType.Value,
                                RefDocNo = doc.RefDocNo.Value,
                                RefDocRow = dtl.docrow.Value,
                                PartNo = dtl.PartNo,
                                RefQty = Math.Abs(dtl.Qty),
                                RefPArtNo = dtl.PartNo,
                                RowGuid = Guid.NewGuid()

                            });
                        }


                    }
                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.Any(d => !string.IsNullOrEmpty(d.Msg)))
                        doc.Msg = "یک یا بیش از یکی از ردیف های سند شامل خطا می باشند.";
                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.All(i => string.IsNullOrEmpty(i.Msg)))
                    {
                        try
                        {
                            db.SaveChanges();
                            try
                            {

                                if (doc.RefDocNo.HasValue && doc.RefDocType.HasValue)
                                {
                                    db.InvSp_UpdateRefDocQty(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, false, "RefConsQty");
                                }

                                db.InvSp_UpdateInvMstrOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, new bool?());
                                db.InvSp_DelInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                db.InvSp_UpdateInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                db.InvSp_UpdateInvArcSerialSoh(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo);
                                db.InvSp_UpdateInvArcSohOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, doc.DocDate);

                                db.InvSp_DocCounter(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo);
                                //foreach (var item in doc.items)
                                //    db.InvSp_FillDateInMstr(crntFiscalYear.FiscalYear, storeNo, doc.DocType, item.PartNo);

                                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["getSerialInfoOnThisDocType"]) && ConfigurationManager.AppSettings["getSerialInfoOnThisDocType"] == doc.DocType.ToString())
                                {
                                    foreach (var dtl in doc.items)
                                    {
                                        var dbserial = db.InvSerials.Where(s => s.Serail == dtl.Serial).FirstOrDefault();
                                        if (dbserial == null) continue;
                                        dbserial.SuppExpDate = dtl.InvSerial_SuppExpDate;
                                        dbserial.Supplier = dtl.InvSerial_Supplier;
                                        dbserial.Model = dtl.InvSerial_Model;
                                        dbserial.OptmIsuDat = dtl.InvSerial_OptmIsuDat;
                                        dbserial.QcNo = dtl.InvSerial_QcNo;
                                    }
                                    db.SaveChanges();
                                }





                            }
                            catch (Exception ex) { }
                        }
                        catch { throw; }
                        finally
                        {

                        }
                    }
                    #endregion
                }
                else if (doc.DocType == invTools.TYPE_DOC_RETURN_FROM_BUY_16)
                {
                    #region ثبت برگشت از خرید
                    //do hdr validations
                    if (int.Parse(doc.DocDate) > int.Parse(crntFiscalYear.EndFiscalYear) || int.Parse(dateTimeService.crntPersianDate) < int.Parse(crntFiscalYear.StartFiscalYear))
                        throw new Exception("تاریخ سند معتبر نمی باشد.");
                    db.InvHdrDatas.Add(new InvHdrData()
                    {
                        FiscalYear = crntFiscalYear.FiscalYear,
                        DocNo = doc.DocNo.Value,
                        UpdateDate = dateTimeService.crntPersianDate,
                        StoreNo = storeNo,
                        DocType = doc.DocType,
                        DocStatus = 3,
                        SumQty = doc.items.Sum(i => i.Qty),
                        SumAmt = doc.items.Sum(i => i.Amt),
                        SumWeight = doc.items.Sum(i => i.Weight),
                        CountNo = 0,
                        StkCountNo = 0,
                        DocEntStatus = 1,
                        CreateDate = dateTimeService.crntPersianDate,
                        LockInd = 0,
                        GdiRefFiscalYear = crntFiscalYear.FiscalYear,
                        GdiRefDocType = 14,
                        UserId = userId,
                        //IsSendAst = 0,
                        //IsEditAcnt = 0,
                        RowGuid = Guid.NewGuid()
                    });
                    short docRow = 0;
                    //var dbDtls = new List<InvDtlData>();
                    foreach (var dtl in doc.items)
                    {
                        dtl.Msg = string.Empty;
                        var soh = invTools.GetItemDataSoh(db, dtl.PartNo, storeNo, crntFiscalYear.FiscalYear);
                        if (dtl.Qty > soh)
                            dtl.Msg = "مقدار موجودی انبار با توجه به مقدار سند منفی می گردد.";

                        db.InvDtlDatas.Add(new Models.ray.InvDtlData()
                        {
                            FiscalYear = crntFiscalYear.FiscalYear,
                            StoreNo = storeNo,
                            DocType = doc.DocType,
                            DocNo = doc.DocNo.Value,
                            CountNo = 0,
                            StkCountNo = 0,
                            DocEntStatus = 1,
                            DocRow = ++docRow,
                            PartNo = dtl.PartNo,
                            BinNo = dtl.BinNo,
                            Center = dtl.Center,
                            OrderNo = dtl.OrderNo,
                            RcptType = dtl.RcptType,
                            Supplier = dtl.Supplier,
                            Act3 = dtl.Act3,
                            Qty = dtl.Qty < 0 ? dtl.Qty : dtl.Qty * -1,
                            UntCode = db.ItemDatas.Where(i => i.PartNo == dtl.PartNo).FirstOrDefault().UntCode, //dtl.UntCode,
                            CntQty = dtl.CntQty,
                            Weight = dtl.Weight,
                            PrcDate = doc.DocDate,
                            PrcSeqDoc = 16,
                            UpdTime = 0,
                            OldRow = docRow,
                            SetVoid = 0,
                            //Amt = dtl.amt,
                            DocDate = doc.DocDate,
                            RefOrder = 0,
                            RefConsQty = 0,
                            AccDocNo = dtl.AccDocNo,
                            //TollAmt = dtl.TollAmt,
                            //TaxAmt = dtl.TaxAmt,
                            DocDsc = dtl.DocDsc,
                            //SaleQty = 0,
                            Act4 = dtl.Act4,
                            FactorNo = dtl.FactorNo,
                            Act5 = dtl.Act5,
                            RtrnBuyReason = doc.RtrnBuyReason, //dtl.RtrnBuyReason,
                            //FactorDate = dtl.FactorDate,
                            //RfDocNo = dtl.RfDocNo,
                            //RfDocRow = dtl.RfDocRow,
                            //ConsType = dtl.ConsType,
                            RowGuid = Guid.NewGuid()
                        });



                    }
                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.Any(d => !string.IsNullOrEmpty(d.Msg)))
                        doc.Msg = "یک یا بیش از یکی از ردیف های سند شامل خطا می باشند.";
                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.All(i => string.IsNullOrEmpty(i.Msg)))
                    {
                        try
                        {
                            db.SaveChanges();
                            try
                            {
                                db.InvSp_UpdateInvMstrOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, new bool?());
                                db.InvSp_DelInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                db.InvSp_UpdateInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                db.InvSp_UpdateInvArcSohOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, doc.DocDate);
                                db.InvSp_UpdateInvArcSerialSoh(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo);
                            }
                            catch (Exception ex) { }
                        }
                        catch { throw; }
                        finally
                        {

                        }
                    }
                    #endregion
                }
                else if (doc.DocType == invTools.TYPE_DOC_DRAFT_40)
                {
                    #region ثبت حواله




                    //do hdr validations
                    if (int.Parse(doc.DocDate) > int.Parse(crntFiscalYear.EndFiscalYear) || int.Parse(dateTimeService.crntPersianDate) < int.Parse(crntFiscalYear.StartFiscalYear))
                        throw new Exception("تاریخ سند معتبر نمی باشد.");
                    db.InvHdrDatas.Add(new InvHdrData()
                    {
                        FiscalYear = crntFiscalYear.FiscalYear,
                        StoreNo = storeNo,
                        DocType = doc.DocType,
                        DocNo = doc.DocNo.Value,
                        CountNo = 0,
                        StkCountNo = 0,
                        DocEntStatus = 1,
                        DocStatus = 3,
                        SumQty = doc.items.Sum(i => i.Qty),
                        SumAmt = doc.items.Sum(i => i.Amt),
                        CreateDate = dateTimeService.crntPersianDate,
                        UpdateDate = dateTimeService.crntPersianDate,
                        LockInd = 0,
                        IsSendAst = 0,
                        GdiRefFiscalYear = crntFiscalYear.FiscalYear,
                        GdiRefDocType = 60,
                        SumWeight = doc.items.Sum(i => i.Weight),
                        IsEditAcnt = 0,
                        UserId = userId,
                        RowGuid = Guid.NewGuid()

                    });
                    short docRow = 0;
                    //var dbDtls = new List<InvDtlData>();
                    foreach (var dtl in doc.items.OrderBy(o => o.docrow))
                    {
                        dtl.Msg = string.Empty;


                        #region بررسی مانده موجودی کالا / بچ / سریال
                        if (false)//اگر انبار بچی / سریالی نیست
                        {


                            var soh = invTools.GetItemDataSoh(db, dtl.PartNo, storeNo, crntFiscalYear.FiscalYear);
                            if (dtl.Qty > soh)
                                dtl.Msg = "مقدار موجودی انبار با توجه به مقدار سند منفی می گردد.";
                        }
                        else
                        {//انبار بچی - سریالی
                            var dbSerial = db.InvSerials.AsNoTracking().Where(s => s.Serail == dtl.Serial && s.StoreNo == storeNo).FirstOrDefault();
                            if (dbSerial == null)
                                dtl.Msg = string.Format("سریال وارد شده {0} معتبر نمی باشد", dtl.Serial);
                            if (dbSerial.Soh < dtl.Qty)
                                dtl.Msg = string.Format("مقدار وارد شده برای سریال {0} بیشتر از موجوی آن می باشد. موجودی قابل استفاده : {1}", dtl.Serial, dbSerial.Soh);
                        }
                        #endregion





                        db.InvDtlDatas.Add(new Models.ray.InvDtlData()
                        {
                            FiscalYear = crntFiscalYear.FiscalYear,
                            StoreNo = storeNo,
                            DocType = doc.DocType,
                            DocNo = doc.DocNo.Value,
                            StkCountNo = 0,
                            CountNo = 0,
                            DocEntStatus = 1,
                            DocRow = ++docRow,
                            PartNo = dtl.PartNo,
                            Center = dtl.Center,
                            DocDate = doc.DocDate,
                            UntCode = dtl.UntCode,
                            Qty = dtl.Qty < 0 ? dtl.Qty : dtl.Qty * -1,
                            RefConsQty = 0,
                            RefOrder = 0,
                            ConsType = dtl.ConsType,
                            OrderNo = dtl.OrderNo,
                            BinNo = dtl.BinNo,
                            PrcDate = doc.DocDate,
                            PrcSeqDoc = 40,
                            Weight = dtl.Weight,
                            UpdTime = 0,
                            SetVoid = 0,
                            Act3 = dtl.Act3,
                            Act4 = dtl.Act4,
                            Act5 = dtl.Act5,
                            DocDsc = dtl.DocDsc,
                            OldRow = docRow,
                            RowGuid = Guid.NewGuid()

                        });
                        if (!string.IsNullOrEmpty(dtl.QcCode))
                        {
                            db.InvAddDocInfs.Add(new InvAddDocInf()
                            {
                                FiscalYear = crntFiscalYear.FiscalYear,
                                StoreNo = storeNo,
                                DocType = doc.DocType,
                                DocNo = doc.DocNo.Value,
                                StkCountNo = 0,
                                CountNo = 0,
                                DocRow = docRow,
                                FldCod = 100,
                                CompData = dtl.QcCode,
                                RowGuid = Guid.NewGuid()
                            });
                        }
                        if (doc.RefDocNo.HasValue && doc.RefDocType.HasValue)
                        {//insert to rf
                            db.InvRfDocs.Add(new Models.ray.InvRfDoc()
                            {
                                FiscalYear = crntFiscalYear.FiscalYear,
                                StoreNo = storeNo,
                                DocType = doc.DocType,
                                DocNo = doc.DocNo.Value,
                                CountNo = 0,
                                DocRow = docRow,

                                RefFiscalYear = crntFiscalYear.FiscalYear,
                                RefStore = storeNo,
                                RefDocType = doc.RefDocType.Value,
                                RefDocNo = doc.RefDocNo.Value,
                                RefDocRow = dtl.docrow.Value,
                                PartNo = dtl.PartNo,
                                RefQty = Math.Abs(dtl.Qty),
                                RefPArtNo = dtl.PartNo,
                                RowGuid = Guid.NewGuid()

                            });
                        }

                    }
                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.Any(d => !string.IsNullOrEmpty(d.Msg)))
                        doc.Msg = "یک یا بیش از یکی از ردیف های سند شامل خطا می باشند.";
                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.All(i => string.IsNullOrEmpty(i.Msg)))
                    {
                        try
                        {
                            db.SaveChanges();
                            try
                            {
                                if (doc.RefDocNo.HasValue && doc.RefDocType.HasValue)
                                {
                                    db.InvSp_UpdateRefDocQty(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, false, "RefConsQty");
                                    //while (docRow > 0)
                                    //{
                                    //    db.InvSp_UpdateRtrnQty(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, docRow, null, null);
                                    //    docRow--;
                                    //}
                                }
                                db.InvSp_UpdateInvMstrOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, new bool?());
                                db.InvSp_DelInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                db.InvSp_UpdateInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                db.InvSp_UpdateInvArcSerialSoh(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo);
                                db.InvSp_UpdateInvArcSohOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, doc.DocDate);
                                db.InvSp_DocCounter(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo);
                            }
                            catch (Exception ex) { }
                        }
                        catch { throw; }
                        finally
                        {

                        }
                    }
                    #endregion
                }
                else if (doc.DocType == invTools.TYPE_DOC_RETURN_TO_STORE_44)
                {
                    #region برگشت به انبار
                    //do hdr validations
                    if (int.Parse(doc.DocDate) > int.Parse(crntFiscalYear.EndFiscalYear) || int.Parse(dateTimeService.crntPersianDate) < int.Parse(crntFiscalYear.StartFiscalYear))
                        throw new Exception("تاریخ سند معتبر نمی باشد.");
                    db.InvHdrDatas.Add(new InvHdrData()
                    {
                        FiscalYear = crntFiscalYear.FiscalYear,
                        DocNo = doc.DocNo.Value,
                        UpdateDate = dateTimeService.crntPersianDate,
                        StoreNo = storeNo,
                        DocType = doc.DocType,
                        DocStatus = 3,
                        SumQty = doc.items.Sum(i => i.Qty),
                        SumAmt = doc.items.Sum(i => i.Amt),
                        SumWeight = doc.items.Sum(i => i.Weight),
                        CountNo = 0,
                        StkCountNo = 0,
                        DocEntStatus = 1,
                        CreateDate = dateTimeService.crntPersianDate,
                        LockInd = 0,
                        GdiRefFiscalYear = crntFiscalYear.FiscalYear,
                        GdiRefDocType = 40,
                        UserId = userId,
                        //IsSendAst = 0,
                        //IsEditAcnt = 0,
                        RowGuid = Guid.NewGuid()

                    });
                    short docRow = 0;
                    //var dbDtls = new List<InvDtlData>();
                    foreach (var dtl in doc.items)
                    {
                        dtl.Msg = string.Empty;
                        //var soh = invTools.GetItemDataSoh(db, dtl.PartNo, storeNo, crntFiscalYear.FiscalYear);
                        //if (dtl.Qty > soh)
                        //    dtl.Msg = "مقدار موجودی انبار با توجه به مقدار سند منفی می گردد.";

                        db.InvDtlDatas.Add(new Models.ray.InvDtlData()
                        {
                            FiscalYear = crntFiscalYear.FiscalYear,
                            StoreNo = storeNo,
                            DocType = doc.DocType,
                            DocNo = doc.DocNo.Value,
                            CountNo = 0,
                            StkCountNo = 0,
                            DocEntStatus = 1,
                            DocRow = ++docRow,
                            PartNo = dtl.PartNo,
                            Center = dtl.Center,
                            OrderNo = dtl.OrderNo,
                            ConsType = dtl.ConsType,
                            Act3 = dtl.Act3,
                            Qty = dtl.Qty > 0 ? dtl.Qty : dtl.Qty * -1,
                            UntCode = dtl.UntCode,
                            CntQty = dtl.CntQty,
                            Weight = dtl.Weight,
                            PrcDate = doc.DocDate,
                            PrcSeqDoc = 44,
                            UpdTime = 0,
                            OldRow = docRow,
                            SetVoid = 0,
                            Amt = dtl.Amt,
                            DocDate = doc.DocDate,
                            RefOrder = 0,
                            RefConsQty = 0,
                            DocDsc = dtl.DocDsc,
                            Act4 = dtl.Act4,
                            Act5 = dtl.Act5,
                            //BinNo = dtl.BinNo,
                            RtrnStrReason = doc.RtrnStrReason, //dtl.RtrnStrReason,
                            TreatyNo = dtl.TreatyNo,
                            RowGuid = Guid.NewGuid()

                        });



                    }
                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.Any(d => !string.IsNullOrEmpty(d.Msg)))
                        doc.Msg = "یک یا بیش از یکی از ردیف های سند شامل خطا می باشند.";
                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.All(i => string.IsNullOrEmpty(i.Msg)))
                    {
                        try
                        {
                            db.SaveChanges();
                            try
                            {
                                db.InvSp_UpdateInvMstrOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, new bool?());
                                db.InvSp_DelInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                db.InvSp_UpdateInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                db.InvSp_UpdateInvArcSohOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, doc.DocDate);
                                db.InvSp_UpdateInvArcSerialSoh(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo);
                            }
                            catch (Exception ex) { }
                        }
                        catch { throw; }
                        finally
                        {

                        }
                    }
                    #endregion
                }
                else if (doc.DocType == invTools.TYPE_DOC_RECEIPT_OF_PRODUCTION_12)
                {
                    #region رسيد كالا/توليد
                    //do hdr validations
                    if (int.Parse(doc.DocDate) > int.Parse(crntFiscalYear.EndFiscalYear) || int.Parse(dateTimeService.crntPersianDate) < int.Parse(crntFiscalYear.StartFiscalYear))
                        throw new Exception("تاریخ سند معتبر نمی باشد.");
                    db.InvHdrDatas.Add(new InvHdrData()
                    {
                        FiscalYear = crntFiscalYear.FiscalYear,
                        DocNo = doc.DocNo.Value,
                        UpdateDate = dateTimeService.crntPersianDate,
                        StoreNo = storeNo,
                        DocType = doc.DocType,
                        DocStatus = 3,
                        SumQty = doc.items.Sum(i => i.Qty),
                        SumAmt = doc.items.Sum(i => i.Amt),
                        SumWeight = doc.items.Sum(i => i.Weight),
                        CountNo = 0,
                        StkCountNo = 0,
                        DocEntStatus = 1,
                        CreateDate = dateTimeService.crntPersianDate,
                        LockInd = 0,
                        GdiRefFiscalYear = crntFiscalYear.FiscalYear,
                        //GdiRefDocType = 66,
                        UserId = userId,
                        //IsSendAst = 0,
                        //IsEditAcnt = 0,
                        RowGuid = Guid.NewGuid()
                    });
                    short docRow = 0;
                    //var dbDtls = new List<InvDtlData>();
                    foreach (var dtl in doc.items)
                    {
                        dtl.Msg = string.Empty;
                        //var soh = invTools.GetItemDataSoh(dtl.PartNo, StoreNo, crntFiscalYear.FiscalYear);
                        //if (dtl.Qty > soh)
                        //    dtl.Msg = "مقدار موجودی انبار با توجه به مقدار سند منفی می گردد.";

                        db.InvDtlDatas.Add(new Models.ray.InvDtlData()
                        {
                            FiscalYear = crntFiscalYear.FiscalYear,
                            StoreNo = storeNo,
                            DocType = doc.DocType,
                            DocNo = doc.DocNo.Value,
                            CountNo = 0,
                            StkCountNo = 0,
                            DocEntStatus = 1,
                            DocRow = ++docRow,
                            PartNo = dtl.PartNo,
                            BinNo = dtl.BinNo,
                            Center = dtl.Center,
                            OrderNo = dtl.OrderNo,
                            RcptType = dtl.RcptType,
                            //Supplier = dtl.Supplier,
                            Act3 = dtl.Act3,
                            Qty = dtl.Qty > 0 ? dtl.Qty : dtl.Qty * -1,
                            UntCode = db.ItemDatas.Where(i => i.PartNo == dtl.PartNo).FirstOrDefault().UntCode, //dtl.UntCode,
                            CntQty = dtl.CntQty,
                            Weight = dtl.Weight,
                            PrcDate = doc.DocDate,
                            PrcSeqDoc = 14,
                            UpdTime = 0,
                            OldRow = docRow,
                            SetVoid = 0,
                            Amt = dtl.Amt,
                            DocDate = doc.DocDate,
                            RefOrder = 0,
                            RefConsQty = 0,
                            //AccDocNo = dtl.AccDocNo,
                            //TollAmt = dtl.TollAmt,
                            //TaxAmt = dtl.TaxAmt,
                            AccDocNo = dtl.AccDocNo,
                            DocDsc = dtl.DocDsc,
                            //SaleQty = 0,
                            Act4 = dtl.Act4,
                            //FactorNo = dtl.FactorNo,
                            Act5 = dtl.Act5,
                            //FactorDate = dtl.FactorDate,
                            //RfDocNo = dtl.RfDocNo,
                            //RfDocRow = dtl.RfDocRow,
                            //ConsType = dtl.ConsType,
                            RowGuid = Guid.NewGuid()
                        });
                        if (!string.IsNullOrEmpty(dtl.QcCode))
                        {
                            db.InvAddDocInfs.Add(new InvAddDocInf()
                            {
                                FiscalYear = crntFiscalYear.FiscalYear,
                                StoreNo = storeNo,
                                DocType = doc.DocType,
                                DocNo = doc.DocNo.Value,
                                StkCountNo = 0,
                                CountNo = 0,
                                DocRow = docRow,
                                FldCod = 100,
                                CompData = dtl.QcCode,
                                RowGuid = Guid.NewGuid()
                            });
                        }


                    }
                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.Any(d => !string.IsNullOrEmpty(d.Msg)))
                        doc.Msg = "یک یا بیش از یکی از ردیف های سند شامل خطا می باشند.";
                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.All(i => string.IsNullOrEmpty(i.Msg)))
                    {
                        try
                        {
                            db.SaveChanges();
                            try
                            {
                                db.InvSp_UpdateInvMstrOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, new bool?());
                                db.InvSp_DelInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                db.InvSp_UpdateInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                db.InvSp_UpdateInvArcSohOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, doc.DocDate);
                                db.InvSp_UpdateInvArcSerialSoh(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo);
                                foreach (var item in doc.items)
                                    db.InvSp_FillDateInMstr(crntFiscalYear.FiscalYear, storeNo, doc.DocType, item.PartNo);
                            }
                            catch (Exception ex) { }
                        }
                        catch { throw; }
                        finally
                        {

                        }
                    }
                    #endregion
                }
                else if (doc.DocType == invTools.TYPE_DOC_RETURN_FROM_RECEIPT_OF_PRODUCTION_22)
                {
                    #region برگشت رسيد كالا/توليد
                    if (int.Parse(doc.DocDate) > int.Parse(crntFiscalYear.EndFiscalYear) || int.Parse(dateTimeService.crntPersianDate) < int.Parse(crntFiscalYear.StartFiscalYear))
                        throw new Exception("تاریخ سند معتبر نمی باشد.");
                    db.InvHdrDatas.Add(new InvHdrData()
                    {
                        FiscalYear = crntFiscalYear.FiscalYear,
                        DocNo = doc.DocNo.Value,
                        UpdateDate = dateTimeService.crntPersianDate,
                        StoreNo = storeNo,
                        DocType = doc.DocType,
                        DocStatus = 3,
                        SumQty = doc.items.Sum(i => i.Qty),
                        SumAmt = doc.items.Sum(i => i.Amt),
                        SumWeight = doc.items.Sum(i => i.Weight),
                        CountNo = 0,
                        StkCountNo = 0,
                        DocEntStatus = 1,
                        CreateDate = dateTimeService.crntPersianDate,
                        LockInd = 0,
                        GdiRefFiscalYear = crntFiscalYear.FiscalYear,
                        GdiRefDocType = 12,
                        UserId = userId,
                        //IsSendAst = 0,
                        //IsEditAcnt = 0,
                        RowGuid = Guid.NewGuid()
                    });
                    short docRow = 0;
                    //var dbDtls = new List<InvDtlData>();
                    foreach (var dtl in doc.items)
                    {
                        dtl.Msg = string.Empty;
                        var soh = invTools.GetItemDataSoh(db, dtl.PartNo, storeNo, crntFiscalYear.FiscalYear);
                        if (dtl.Qty > soh)
                            dtl.Msg = "مقدار موجودی انبار با توجه به مقدار سند منفی می گردد.";

                        db.InvDtlDatas.Add(new Models.ray.InvDtlData()
                        {
                            FiscalYear = crntFiscalYear.FiscalYear,
                            StoreNo = storeNo,
                            DocType = doc.DocType,
                            DocNo = doc.DocNo.Value,
                            CountNo = 0,
                            StkCountNo = 0,
                            DocEntStatus = 1,
                            DocRow = ++docRow,
                            PartNo = dtl.PartNo,
                            //BinNo = dtl.BinNo,
                            Center = dtl.Center,
                            OrderNo = dtl.OrderNo,
                            RcptType = dtl.RcptType,
                            //Supplier = dtl.Supplier,
                            Act3 = dtl.Act3,
                            Qty = dtl.Qty < 0 ? dtl.Qty : dtl.Qty * -1,
                            UntCode = db.ItemDatas.Where(i => i.PartNo == dtl.PartNo).FirstOrDefault().UntCode, //dtl.UntCode,
                            CntQty = dtl.CntQty,
                            Weight = dtl.Weight,
                            PrcDate = doc.DocDate,
                            PrcSeqDoc = 22,
                            UpdTime = 0,
                            OldRow = docRow,
                            SetVoid = 0,
                            Amt = dtl.Amt,
                            DocDate = doc.DocDate,
                            RefOrder = 0,
                            RefConsQty = 0,
                            //AccDocNo = dtl.AccDocNo,
                            //TollAmt = dtl.TollAmt,
                            //TaxAmt = dtl.TaxAmt,
                            DocDsc = dtl.DocDsc,
                            //SaleQty = 0,
                            Act4 = dtl.Act4,
                            //FactorNo = dtl.FactorNo,
                            Act5 = dtl.Act5,
                            //RtrnBuyReason = dtl.RtrnBuyReason,
                            //FactorDate = dtl.FactorDate,
                            //RfDocNo = dtl.RfDocNo,
                            //RfDocRow = dtl.RfDocRow,
                            //ConsType = dtl.ConsType,
                            RowGuid = Guid.NewGuid()
                        });
                        if (!string.IsNullOrEmpty(dtl.QcCode))
                        {
                            db.InvAddDocInfs.Add(new InvAddDocInf()
                            {
                                FiscalYear = crntFiscalYear.FiscalYear,
                                StoreNo = storeNo,
                                DocType = doc.DocType,
                                DocNo = doc.DocNo.Value,
                                StkCountNo = 0,
                                CountNo = 0,
                                DocRow = docRow,
                                FldCod = 100,
                                CompData = dtl.QcCode,
                                RowGuid = Guid.NewGuid()
                            });
                        }


                    }
                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.Any(d => !string.IsNullOrEmpty(d.Msg)))
                        doc.Msg = "یک یا بیش از یکی از ردیف های سند شامل خطا می باشند.";
                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.All(i => string.IsNullOrEmpty(i.Msg)))
                    {
                        try
                        {
                            db.SaveChanges();
                            try
                            {
                                db.InvSp_UpdateInvMstrOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, new bool?());
                                db.InvSp_DelInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                db.InvSp_UpdateInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                db.InvSp_UpdateInvArcSohOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, doc.DocDate);
                                db.InvSp_UpdateInvArcSerialSoh(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo);
                            }
                            catch (Exception ex) { }
                        }
                        catch { throw; }
                        finally
                        {

                        }
                    }
                    #endregion
                }
                else if (doc.DocType == invTools.TYPE_DOC_TEMPORARY_RECEIPT_66)
                {
                    #region ثبت رسید موقت
                    //do hdr validations
                    if (int.Parse(doc.DocDate) > int.Parse(crntFiscalYear.EndFiscalYear) || int.Parse(dateTimeService.crntPersianDate) < int.Parse(crntFiscalYear.StartFiscalYear))
                        throw new Exception("تاریخ سند معتبر نمی باشد.");
                    db.InvHdrDatas.Add(new InvHdrData()
                    {
                        FiscalYear = crntFiscalYear.FiscalYear,
                        DocNo = doc.DocNo.Value,
                        UpdateDate = dateTimeService.crntPersianDate,
                        StoreNo = storeNo,
                        DocType = doc.DocType,
                        DocStatus = 3,
                        SumQty = doc.items.Sum(i => i.Qty),
                        SumAmt = doc.items.Sum(i => i.Amt),
                        SumWeight = doc.items.Sum(i => i.Weight),
                        CountNo = 0,
                        StkCountNo = 0,
                        DocEntStatus = 1,
                        CreateDate = dateTimeService.crntPersianDate,
                        LockInd = 0,
                        GdiRefFiscalYear = crntFiscalYear.FiscalYear,
                        GdiRefDocType = 66,
                        UserId = userId,
                        //IsSendAst = 0,
                        //IsEditAcnt = 0,
                        RowGuid = Guid.NewGuid()
                    });
                    short docRow = 0;
                    //var dbDtls = new List<InvDtlData>();
                    foreach (var dtl in generateSerialedDtls(doc.items, crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.ReqCenter))//TODO: reqCenter from ref
                    {
                        dtl.Msg = string.Empty;
                        //var soh = invTools.GetItemDataSoh(dtl.PartNo, StoreNo, crntFiscalYear.FiscalYear);
                        //if (dtl.Qty > soh)
                        //    dtl.Msg = "مقدار موجودی انبار با توجه به مقدار سند منفی می گردد.";


                        db.InvDtlDatas.Add(new Models.ray.InvDtlData()
                        {
                            FiscalYear = crntFiscalYear.FiscalYear,
                            StoreNo = storeNo,
                            DocType = doc.DocType,
                            DocNo = doc.DocNo.Value,
                            CountNo = 0,
                            StkCountNo = 0,
                            DocEntStatus = 1,
                            DocRow = ++docRow,
                            PartNo = dtl.PartNo,
                            Serial = dtl.Serial,
                            BinNo = dtl.BinNo,
                            Center = dtl.Center,
                            OrderNo = dtl.OrderNo,
                            RcptType = dtl.RcptType,
                            Supplier = dtl.Supplier,
                            Act3 = dtl.Act3,
                            Qty = dtl.Qty > 0 ? dtl.Qty : dtl.Qty * -1,
                            UntCode = db.ItemDatas.Where(i => i.PartNo == dtl.PartNo).FirstOrDefault().UntCode, //dtl.UntCode,
                                                                                                                //CntQty = dtl.CntQty,
                            Weight = dtl.Weight,
                            PrcDate = doc.DocDate,
                            PrcSeqDoc = 14,
                            UpdTime = 0,
                            OldRow = docRow,
                            SetVoid = 0,
                            Amt = dtl.Amt,
                            DocDate = doc.DocDate,
                            RefOrder = 0,
                            RefConsQty = 0,
                            AccDocNo = dtl.AccDocNo,
                            TollAmt = dtl.TollAmt,
                            TaxAmt = dtl.TaxAmt,
                            DocDsc = dtl.DocDsc,
                            SaleQty = 0,
                            Act4 = dtl.Act4,
                            FactorNo = dtl.FactorNo,
                            Act5 = dtl.Act5,
                            FactorDate = dtl.FactorDate,
                            RfDocNo = dtl.RfDocNo,
                            RfDocRow = dtl.RfDocRow,
                            //ConsType = dtl.ConsType,
                            RowGuid = Guid.NewGuid()
                        });
                        if (doc.RefDocNo.HasValue && doc.RefDocType.HasValue)
                        {//insert to rf
                            db.InvRfDocs.Add(new InvRfDoc()
                            {
                                FiscalYear = crntFiscalYear.FiscalYear,
                                StoreNo = storeNo,
                                DocType = doc.DocType,
                                DocNo = doc.DocNo.Value,
                                CountNo = 0,
                                DocRow = docRow,

                                RefFiscalYear = crntFiscalYear.FiscalYear,
                                RefStore = storeNo,
                                RefDocType = doc.RefDocType.Value,
                                RefDocNo = doc.RefDocNo.Value,
                                RefDocRow = dtl.docrow.Value,
                                PartNo = dtl.PartNo,
                                RefQty = Math.Abs(dtl.Qty),
                                RefPArtNo = dtl.PartNo,
                                RowGuid = Guid.NewGuid()

                            });
                        }
                    }




                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.Any(d => !string.IsNullOrEmpty(d.Msg)))
                        doc.Msg = "یک یا بیش از یکی از ردیف های سند شامل خطا می باشند.";
                    if (string.IsNullOrEmpty(doc.Msg) && doc.items.All(i => string.IsNullOrEmpty(i.Msg)))
                    {
                        try
                        {
                            db.SaveChanges();
                            try
                            {
                                if (doc.RefDocNo.HasValue && doc.RefDocType.HasValue)
                                {
                                    db.InvSp_UpdateRefDocQty(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, false, "RefOrder");
                                    while (docRow > 0)
                                    {
                                        db.InvSp_UpdateRtrnQty(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, docRow, null, null);
                                        docRow--;
                                    }
                                }
                                db.InvSp_DocCounter(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo);
                                //db.InvSp_UpdateInvMstrOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo);
                                //db.InvSp_DelInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                //db.InvSp_UpdateInvSerial(crntFiscalYear.FiscalYear, storeNo);
                                //db.InvSp_UpdateInvArcSohOneDoc(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo, doc.DocDate);
                                //db.InvSp_UpdateInvArcSerialSoh(crntFiscalYear.FiscalYear, storeNo, doc.DocType, doc.DocNo);
                                //foreach (var item in doc.items)
                                //    db.InvSp_FillDateInMstr(crntFiscalYear.FiscalYear, storeNo, doc.DocType, item.PartNo);
                            }
                            catch (Exception ex) { }
                        }
                        catch (Exception ex) { throw ex; }
                        finally
                        {

                        }
                    }
                    #endregion
                }

            }
            if (string.IsNullOrEmpty(doc.Msg))
                return Ok(doc);
            return new NotFoundActionResult(doc.Msg);

            //}
        }

        public List<invDtlDataBindingModel> generateSerialedDtls(List<invDtlDataBindingModel> dtls, int fyear, string store, byte docType, /*int? Center, int? OrderNo, int? Supplier, */int? ReqCenter)
        {
            bool autoGenerateSerial;
            if (!(bool.TryParse(ConfigurationManager.AppSettings["autoGenerateSerial"], out autoGenerateSerial) && autoGenerateSerial))
                return dtls;


            byte audocType;

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["autoGenerateSerial"]) || !byte.TryParse(ConfigurationManager.AppSettings["GenerateSerialDocType"], out audocType))
                throw new Exception("نوع سند مورد نظر جهت تولید خودکار معتبر صحیح نمی باشد.");


            if (audocType != docType)
                return dtls;

            if (!new byte[] { invTools.TYPE_DOC_TEMPORARY_RECEIPT_66, invTools.TYPE_DOC_RECEIPT_OF_PURCHASE_14 }.Contains(audocType))
                throw new Exception("نوع سند مشخص شده جهت تولید خودکار سریال صحیح نمی باشد.");

            var serialStrucStr = ConfigurationManager.AppSettings["serialNoStructue"].ToLower();
            var serialStruc = serialStrucStr.Split('-');

            if (serialStruc.Count() == 0 || serialStruc.Except(new string[] { "reqcenter", "reqcenter*", "orderno", "orderno*", "supplier", "supplier*", "center", "center*" }).Count() > 0)
                throw new Exception("ساختار سریال صحیح نیست");

            if (!serialStruc.Any(i => i.Contains("*")))
                throw new Exception("تعریف حداقل یکی از اقلام سریال به عنوان ساختار عنصر اصلی الزامی ست.");

            List<string> structedSerialList = new List<string>();



            var removeList = new List<invDtlDataBindingModel>();
            var newList = new List<invDtlDataBindingModel>();



            //string structedSerial = string.Join("-", structedSerialList);
            using (var db = new Entities())
            {
                //var retList = new List<invDtlDataBindingModel>();
                foreach (var dtl in dtls)
                {
                    #region init structedSerialList with values of struct
                    structedSerialList.Clear();
                    structedSerialList.Add(fyear.ToString());
                    structedSerialList.Add(store);
                    structedSerialList.Add(audocType.ToString());
                    foreach (var itm in serialStruc)
                    {
                        if (itm.Replace("*", "") == "reqcenter")
                        {
                            if (!ReqCenter.HasValue)
                                throw new Exception("قسمت درخواست کننده مشخص نشده است");
                            structedSerialList.Add(ReqCenter.Value.ToString());
                        }
                        if (itm.Replace("*", "") == "orderno")
                        {
                            //if (!dtl.OrderNo.HasValue)
                            //    throw new Exception("شماره سفارش مشخص نشده است");
                            structedSerialList.Add(dtl.OrderNo.ToString());
                        }
                        if (itm.Replace("*", "") == "supplier")
                        {
                            if (string.IsNullOrEmpty(dtl.Supplier))
                                throw new Exception("فروشنده مشخص نشده است");
                            structedSerialList.Add(dtl.Supplier);
                        }
                        if (itm.Replace("*", "") == "center")
                        {
                            if (!dtl.Center.HasValue)
                                throw new Exception("مرکز هزینه مشخص نشده است");
                            structedSerialList.Add(dtl.Center.Value.ToString());
                        }
                    }
                    structedSerialList.Add(dtl.PartNo);
                    #endregion

                    var structedSerial = string.Join("-", structedSerialList) + "-";
                    int seri = 0;



                    var dbExistingSerialList = db.InvDtlDatas.AsNoTracking()
                        .Where(d => d.DocType == audocType && d.StoreNo == storeNo && d.Serial.StartsWith(structedSerial)).Select(s => s.Serial).ToList();

                    var seris = dbExistingSerialList.Where(s => s.Split('-').Count() > 1).Select(s => s.Split('-').LastOrDefault());
                    if (seris.Count() > 0)
                        seri = seris.Max(s => int.Parse(s));

                    //last == null ? 1 : ;

                    //if (last != null && !int.TryParse(last.Serial.Split('-').LastOrDefault() ?? "a", out seri))
                    // throw new Exception(string.Format("مقدار سریال قبلی مربوط به کالا ی {0} صحیح نمی باشد", last.Serial));

                    // bool ok = last != null && last.Serial.Split('-').Count() > 1 && int.TryParse(last.Serial.Split('-').LastOrDefault(), out seri);



                    if (dtl.cntInOnePackage.HasValue && dtl.cntInOnePackage.Value > 0 && dtl.cntInOnePackage.Value < dtl.Qty)
                    {
                        removeList.Add(dtl);
                        while (dtl.Qty > 0)
                        {
                            var ni = new invDtlDataBindingModel()
                            {
                                AccDocNo = dtl.AccDocNo,
                                Act3 = dtl.Act3,
                                Act4 = dtl.Act4,
                                Act5 = dtl.Act5,
                                Amt = dtl.Amt,
                                BinNo = dtl.BinNo,
                                Center = dtl.Center,
                                cntInOnePackage = dtl.cntInOnePackage,
                                CntQty = dtl.CntQty,
                                ConsType = dtl.ConsType,
                                DocDsc = dtl.DocDsc,
                                docrow = dtl.docrow,
                                FactorDate = dtl.FactorDate,
                                FactorNo = dtl.FactorNo,
                                Msg = dtl.Msg,
                                NeedDate = dtl.NeedDate,
                                OrderNo = dtl.OrderNo,
                                PartNo = dtl.PartNo,
                                QcCode = dtl.QcCode,
                                Qty = dtl.Qty,
                                RcptType = dtl.RcptType,
                                ReqType = dtl.ReqType,
                                RfDocNo = dtl.RfDocNo,
                                RfDocRow = dtl.RfDocRow,
                                RtrnBuyReason = dtl.RtrnBuyReason,//TODO?!
                                RtrnStrReason = dtl.RtrnStrReason,//TODO?!
                                Serial = dtl.Serial,
                                Supplier = dtl.Supplier,
                                TaxAmt = dtl.TaxAmt,
                                TollAmt = dtl.TollAmt,
                                TreatyNo = dtl.TreatyNo,
                                UntCode = dtl.UntCode,
                                Weight = dtl.Weight
                            }; //DeepCopy(dtl);
                            ni.Qty = dtl.Qty - dtl.cntInOnePackage.Value < 0 ? dtl.Qty : dtl.cntInOnePackage.Value;
                            ni.Serial = structedSerial + (++seri).ToString();
                            dtl.Qty = dtl.Qty - dtl.cntInOnePackage.Value;
                            newList.Add(ni);
                        }

                    }
                    else
                    {
                        dtl.Serial = structedSerial + seri.ToString();
                    }
                }

            }
            dtls = dtls.Except(removeList).ToList();
            dtls.AddRange(newList);

            return dtls;
        }



        /// <summary>
        /// مقدار دهی به فرم اسکن -  کد کالا 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("initScanDialog_item")]
        [Exception]
        [Obsolete(message: "please use initScanDialog instead")]
        [HttpPost]
        public IHttpActionResult initScanDialog_item(initScanDialog_itemBindingModel input)
        {//use "ray.InvSp_FindPropertyFromItemData" instead!!!
            if (!ModelState.IsValid)
            {
                return new BadRequestActionResult(ModelState.Values);
            }
            var crntFiscalYear = invTools.CurrentFiscalYear;
            //var token = Request.Headers.Authorization.Scheme;
            //var StoreNo = Request.Headers.FirstOrDefault(h => h.Key.Equals("StoreNo")).Value.FirstOrDefault();
            using (var db = new Entities())
            {
                //var soh = db.InvArcSohs.Where(a => a.PartNo == input.partCode && a.StoreNo == StoreNo && a.FiscalYear == 1396).ToList().OrderByDescending(o => int.Parse(o.DocDate)).FirstOrDefault();
                //var qty = soh == null ? 0 : soh.Qty;
                var soh = invTools.GetItemDataSoh(db, input.partNo, storeNo, crntFiscalYear.FiscalYear);
                var itemdata =
                db.ItemDatas.AsNoTracking()
                    .Join(db.Units.AsNoTracking(), oks => oks.UntCode, iks => iks.UntCode, (I, U) =>
                    new { I.NationalPartCod, I.TechnicalNo, U.UntName, I.PartNo, I.PartGrp, I.PartNoDsc, I.PartLtnDsc, I.NotActiv, I.UntCode })
                    .Where(c => c.PartNo == input.partNo /*&& (!c.NotActiv.HasValue || c.NotActiv.Value == 1)*/)
                    .Select(c => new { c.PartNo, c.TechnicalNo, c.PartNoDsc, c.NotActiv, c.PartLtnDsc, c.UntCode, c.UntName, c.NationalPartCod, soh = soh }).FirstOrDefault();

                var store = db.Stores.AsNoTracking().Where(s => s.StoreNo == storeNo).FirstOrDefault();

                if (itemdata != null)
                {
                    //if (itemdata.FirstOrDefault().NotActiv.HasValue && itemdata.FirstOrDefault().NotActiv.Value == 1)
                    //    return new NotFoundActionResult("کالای وارد شده در سیستم غیرفعال گردیده است و قابل استفاده نمی باشد.");
                    //else
                    //    return Ok(itemdata.FirstOrDefault());

                    var retlist = new List<keyValuePair>();
                    bool isValid = true;
                    string validationMsg = "";
                    retlist.Add(new keyValuePair() { id = "کد کالا", dsc = itemdata.PartNo });
                    retlist.Add(new keyValuePair() { id = "عنوان کالا", dsc = itemdata.PartNoDsc });
                    retlist.Add(new keyValuePair() { id = "واحد شمارش", dsc = itemdata.UntName });
                    retlist.Add(new keyValuePair() { id = "موجودی", dsc = itemdata.soh.ToString() });
                    retlist.Add(new keyValuePair() { id = "شماره فنی", dsc = itemdata.TechnicalNo });
                    retlist.Add(new keyValuePair() { id = "مشخصه ملی", dsc = itemdata.NationalPartCod });
                    if (itemdata.NotActiv.HasValue && itemdata.NotActiv.Value == 1)
                    {
                        isValid = false;
                        validationMsg = "کد کالای وارد شده در سیستم غیر فعال گردیده است و قابل استفاده نمی باشد.";
                    }
                    var _serialStatus = new byte?[] { 2, 3 }.ToList().Contains(store.SerialTyp)
                        && new byte[] { 66, 14, 16, 40, 44, 12, 22 }.Contains(input.docType) ? fieldStatus.required : fieldStatus.invisible;
                    return Ok(new
                    {
                        PartNo = itemdata.PartNo,
                        PartNoDsc = itemdata.PartNoDsc,
                        UntCode = itemdata.UntCode,
                        UntName = itemdata.UntName,
                        soh = itemdata.soh,
                        isValid = isValid,
                        validationMsg = validationMsg,
                        infoList = retlist,
                        weight = new { title = "وزن", state = _serialStatus == fieldStatus.required ? fieldStatus.disable : fieldStatus.enable, defaultValue = new { id = "", dsc = "" } },
                        serial = new { title = "سریال / ش بچ", state = _serialStatus, defaultValue = new { id = "", dsc = "" } },
                        qty = new { title = "مقدار", state = _serialStatus == fieldStatus.required ? fieldStatus.disable : fieldStatus.enable, defaultValue = new { id = "", dsc = "" } },
                        mergeRows = new { title = "ادغام ردیف های با کد یکسان", state = store.SerialTyp == 2 ? fieldStatus.disable : fieldStatus.enable, defaultValue = new { id = false, dsc = "" } },
                    });
                }
                return new NotFoundActionResult("کالا یافت نشد.");
            }
        }
        /// <summary>
        /// مقدار دهی به فرم اسکن 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("initScanDialog")]
        [Exception]
        [HttpPost]
        public IHttpActionResult initScanDialog(initScanDialog_BindingModel input)
        {//use "ray.InvSp_FindPropertyFromItemData" instead!!!
            if (!ModelState.IsValid)
            {
                return new BadRequestActionResult(ModelState.Values);
            }
            using (var repo = new Repo(this, "ray.InvAssistantSp_initScanDialog", "invDoc_initScanDialog", initAsReader: true))
            {
                repo.cmd.Parameters.AddWithValue("@docType", input.docType.getDbValue());
                repo.cmd.Parameters.AddWithValue("@barcode", input.barcode.Trim().getDbValue());


                //repo.cmd.Parameters.AddWithValue("@validateSerialWithOrderNo", bool.Parse(ConfigurationManager.AppSettings["validateSerialWithOrderNo"]));
                //repo.cmd.Parameters.AddWithValue("@refDocType", input.refDocType.getDbValue());
                //repo.cmd.Parameters.AddWithValue("@refDocNo", input.refDocNo.getDbValue());
                //repo.cmd.Parameters.AddWithValue("@refDocRow", input.refDocRow.getDbValue());




                var par_isValid = repo.cmd.Parameters.Add("@isValid", SqlDbType.Bit);
                par_isValid.Direction = ParameterDirection.Output;

                var par_validationMsg = repo.cmd.Parameters.Add("@validationMsg", SqlDbType.VarChar, 150);
                par_validationMsg.Direction = ParameterDirection.Output;

                var par_soh = repo.cmd.Parameters.Add("@soh", SqlDbType.Money);
                par_soh.Direction = ParameterDirection.Output;

                var par_serial_isNew = repo.cmd.Parameters.Add("@serial_isNew", SqlDbType.Bit);
                par_serial_isNew.Direction = ParameterDirection.Output;

                //var par_hasWarning = repo.cmd.Parameters.Add("@hasWarning", SqlDbType.Bit);
                //par_hasWarning.Direction = ParameterDirection.Output;





                var par_partNo = repo.cmd.Parameters.Add("@partNo", SqlDbType.VarChar, 20);
                par_partNo.Direction = ParameterDirection.Output;

                var par_PartNoDsc = repo.cmd.Parameters.Add("@PartNoDsc", SqlDbType.VarChar, 1000);
                par_PartNoDsc.Direction = ParameterDirection.Output;

                var par_UntCode = repo.cmd.Parameters.Add("@UntCode", SqlDbType.Char, 2);
                par_UntCode.Direction = ParameterDirection.Output;

                var par_UntName = repo.cmd.Parameters.Add("@UntName", SqlDbType.VarChar, 36);
                par_UntName.Direction = ParameterDirection.Output;



                repo.ExecuteAdapter();



                //if ()

                return Ok(

                    new
                    {
                        partNo = par_partNo.Value.dbNullCheckString(),
                        PartNoDsc = par_PartNoDsc.Value.dbNullCheckString(),
                        UntCode = par_UntCode.Value.dbNullCheckString(),
                        UntName = par_UntName.Value.dbNullCheckString(),
                        soh = par_soh.Value.dbNullCheckDecimal(),
                        isValid = par_isValid.Value.dbNullCheckBoolean(),
                        validationMsg = par_validationMsg.Value.dbNullCheckString(),
                        serial_isNew = par_serial_isNew.Value.dbNullCheckBoolean(),




                        infoList_item = repo.ds.Tables[0].AsEnumerable().Select(b => new { id = b.Field<object>("id").dbNullCheckString(), dsc = b.Field<object>("dsc").dbNullCheckString() }),
                        infoList_serial = repo.ds.Tables[1].AsEnumerable().Select(b => new { id = b.Field<object>("id").dbNullCheckString(), dsc = b.Field<object>("dsc").dbNullCheckString() }),

                        weight = repo.ds.Tables[2].AsEnumerable().Where(w => w.Field<object>("id").dbNullCheckString() == "weight").Select(s => new { title = s.Field<object>("title").dbNullCheckString(), state = s.Field<object>("state").dbNullCheckByte(), defaultValue = new { id = s.Field<object>("defaultValue_id").dbNullCheckString(), dsc = s.Field<object>("defaultValue_dsc").dbNullCheckString() } }).FirstOrDefault(),
                        serial = repo.ds.Tables[2].AsEnumerable().Where(w => w.Field<object>("id").dbNullCheckString() == "serial").Select(s => new { title = s.Field<object>("title").dbNullCheckString(), state = s.Field<object>("state").dbNullCheckByte(), defaultValue = new { id = s.Field<object>("defaultValue_id").dbNullCheckString(), dsc = s.Field<object>("defaultValue_dsc").dbNullCheckString() } }).FirstOrDefault(),
                        qty = repo.ds.Tables[2].AsEnumerable().Where(w => w.Field<object>("id").dbNullCheckString() == "qty").Select(s => new { title = s.Field<object>("title").dbNullCheckString(), state = s.Field<object>("state").dbNullCheckByte(), defaultValue = new { id = s.Field<object>("defaultValue_id").dbNullCheckString(), dsc = s.Field<object>("defaultValue_dsc").dbNullCheckString() } }).FirstOrDefault(),
                        mergeRows = repo.ds.Tables[2].AsEnumerable().Where(w => w.Field<object>("id").dbNullCheckString() == "mergeRows").Select(s => new { title = s.Field<object>("title").dbNullCheckString(), state = s.Field<object>("state").dbNullCheckByte(), defaultValue = new { id = s.Field<object>("defaultValue_id").dbNullCheckString(), dsc = s.Field<object>("defaultValue_dsc").dbNullCheckString() } }).FirstOrDefault(),
                    }
                );

            }
        }

        /// <summary>
        /// مقدار دهی به فرم اسکن - سریال 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("initScanDialog_serial")]
        [Exception]
        [HttpPost]
        public IHttpActionResult initScanDialog_serial(initScanDialog_serialBindingModel input)
        {//use "ray.InvSp_FindPropertyFromItemData" instead!!!
            if (!ModelState.IsValid)
            {
                return new BadRequestActionResult(ModelState.Values);
            }
            using (var repo = new Repo(this, "ray.InvAssistantSp_initScanDialog_serial", "invDoc_initScanDialog_serial", initAsReader: true))
            {
                repo.cmd.Parameters.AddWithValue("@docType", input.docType.getDbValue());
                repo.cmd.Parameters.AddWithValue("@partNo", input.partNo.getDbValue());
                repo.cmd.Parameters.AddWithValue("@serial", input.serial.getDbValue());


                //repo.cmd.Parameters.AddWithValue("@validateSerialWithOrderNo", bool.Parse(ConfigurationManager.AppSettings["validateSerialWithOrderNo"]));
                repo.cmd.Parameters.AddWithValue("@refDocType", input.refDocType.getDbValue());
                repo.cmd.Parameters.AddWithValue("@refDocNo", input.refDocNo.getDbValue());
                repo.cmd.Parameters.AddWithValue("@refDocRow", input.refDocRow.getDbValue());




                var par_isValid = repo.cmd.Parameters.Add("@isValid", SqlDbType.Bit);
                par_isValid.Direction = ParameterDirection.Output;

                var par_validationMsg = repo.cmd.Parameters.Add("@validationMsg", SqlDbType.VarChar, 150);
                par_validationMsg.Direction = ParameterDirection.Output;

                var par_soh = repo.cmd.Parameters.Add("@soh", SqlDbType.Money);
                par_soh.Direction = ParameterDirection.Output;

                var par_isNew = repo.cmd.Parameters.Add("@isNew", SqlDbType.Bit);
                par_isNew.Direction = ParameterDirection.Output;

                var par_hasWarning = repo.cmd.Parameters.Add("@hasWarning", SqlDbType.Bit);
                par_hasWarning.Direction = ParameterDirection.Output;


                repo.ExecuteAdapter();



                //if ()

                return Ok(

                    new
                    {
                        isValid = par_isValid.Value.dbNullCheckBoolean(),
                        hasWarning = par_hasWarning.Value.dbNullCheckBoolean(),
                        validationMsg = par_validationMsg.Value.dbNullCheckString(),
                        isNew = par_isNew.Value.dbNullCheckBoolean(),
                        soh = par_soh.Value.dbNullCheckDecimal(),
                        infoList = repo.ds.Tables[0].AsEnumerable().Select(b => new { id = b.Field<object>("id").dbNullCheckString(), dsc = b.Field<object>("dsc").dbNullCheckString() }),

                        weight = repo.ds.Tables[1].AsEnumerable().Where(w => w.Field<object>("id").dbNullCheckString() == "weight").Select(s => new { title = s.Field<object>("title").dbNullCheckString(), state = s.Field<object>("state").dbNullCheckByte(), defaultValue = new { id = s.Field<object>("defaultValue_id").dbNullCheckString(), dsc = s.Field<object>("defaultValue_dsc").dbNullCheckString() } }).FirstOrDefault(),
                        serial = repo.ds.Tables[1].AsEnumerable().Where(w => w.Field<object>("id").dbNullCheckString() == "serial").Select(s => new { title = s.Field<object>("title").dbNullCheckString(), state = s.Field<object>("state").dbNullCheckByte(), defaultValue = new { id = s.Field<object>("defaultValue_id").dbNullCheckString(), dsc = s.Field<object>("defaultValue_dsc").dbNullCheckString() } }).FirstOrDefault(),
                        qty = repo.ds.Tables[1].AsEnumerable().Where(w => w.Field<object>("id").dbNullCheckString() == "qty").Select(s => new { title = s.Field<object>("title").dbNullCheckString(), state = s.Field<object>("state").dbNullCheckByte(), defaultValue = new { id = s.Field<object>("defaultValue_id").dbNullCheckString(), dsc = s.Field<object>("defaultValue_dsc").dbNullCheckString() } }).FirstOrDefault(),
                        mergeRows = repo.ds.Tables[1].AsEnumerable().Where(w => w.Field<object>("id").dbNullCheckString() == "mergeRows").Select(s => new { title = s.Field<object>("title").dbNullCheckString(), state = s.Field<object>("state").dbNullCheckByte(), defaultValue = new { id = s.Field<object>("defaultValue_id").dbNullCheckString(), dsc = s.Field<object>("defaultValue_dsc").dbNullCheckString() } }).FirstOrDefault(),
                    }
                    //repo.ds.Tables[0].AsEnumerable().Select(b => new
                    //{
                    //    //Serail = b.Field<object>("Serail"),
                    //    //Soh = b.Field<object>("Soh"),
                    //    //SupName = b.Field<object>("SupName"),
                    //    //PakgTypeDesc = b.Field<object>("PakgTypeDesc"),
                    //    FiscalYear = b.Field<object>("FiscalYear"),
                    //    StoreNo = b.Field<object>("StoreNo"),
                    //    PartNo = b.Field<object>("PartNo"),
                    //    Serail = b.Field<object>("Serail"),
                    //    TotRcpt = b.Field<object>("TotRcpt"),
                    //    TotIssu = b.Field<object>("TotIssu"),
                    //    Supplier = b.Field<object>("Supplier"),
                    //    SupName = b.Field<object>("SupName"),
                    //    TrnsPkgTyp = b.Field<object>("TrnsPkgTyp"),
                    //    PakgTypeDesc = b.Field<object>("PakgTypeDesc"),
                    //    SuppSerial = b.Field<object>("SuppSerial"),
                    //    SuppEntrDate = b.Field<object>("SuppEntrDate"),
                    //    SuppExpDate = b.Field<object>("SuppExpDate"),
                    //    ExpDate = b.Field<object>("ExpDate"),
                    //    ConfirmDate = b.Field<object>("ConfirmDate"),
                    //    Soh = b.Field<object>("Soh"),
                    //})
                );

            }
        }

        /// <summary>
        /// دریافت اطلاعات موجودی کالا
        /// </summary>
        /// <param name="PartNo"></param>
        /// <returns></returns>
        [Route("GetsohInfo")]
        [Exception]
        [HttpPost]
        public IHttpActionResult GetsohInfo([FromBody]string PartNo)
        {
            using (var db = new Entities())
            {
                var FiscalYear = invTools.CurrentFiscalYear.FiscalYear;
                var IncomingSoh = db.Database.SqlQuery<IncomingSohModel>(@"select a.PartNo,SUM(a.Qty-a.refconsqty) as IncomingSoh from ray.invdtldata a join ray.InvRfDoc b
                                                            on a.DocNo = b.RefDocNo and a.FiscalYear = b.FiscalYear and a.PartNo = b.PartNo
                                                            and a.StoreNo = b.StoreNo and a.DocType = b.RefDocType
                                                            where a.FiscalYear = @FiscalYear and a.StoreNo = @StoreNo and a.DocType = 62 and a.PartNo = @PartNo
                                                            group by a.PartNo
                                                            union all
                                                            select PartNo, SUM(qty) as IncomingSoh from ray.InvDtlData a join ray.invhdrdata b
                                                             on a.DocNo = b.DocNo and a.FiscalYear = b.FiscalYear and a.StoreNo = b.StoreNo and a.DocType = b.DocType
                                                            where a.DocType = 62 and RefConsQty = 0 and CnfrmLvl = 2 and DocStatus > 2 and a.FiscalYear = @FiscalYear and a.StoreNo = @StoreNo and a.PartNo = @PartNo
                                                            group by PartNo",

                                             new SqlParameter("@FiscalYear", FiscalYear), new SqlParameter("@StoreNo", storeNo), new SqlParameter("@PartNo", PartNo));

                var ReserveSoh = db.Database.SqlQuery<ReserveSohModel>(@"select a.PartNo,SUM(a.Qty-a.refconsqty) as ReserveSoh from ray.invdtldata a join ray.InvRfDoc b
                                                            on a.DocNo=b.RefDocNo and a.FiscalYear=b.FiscalYear and a.PartNo=b.PartNo 
                                                            join ray.invhdrdata c
                                                            on a.DocNo=c.DocNo and a.FiscalYear=c.FiscalYear and a.StoreNo=c.StoreNo and a.DocType=c.DocType
                                                            and a.StoreNo=b.StoreNo and a.DocType=b.RefDocType
                                                            where a.FiscalYear= @FiscalYear and a.StoreNo=@StoreNo and a.DocType=60 and b.DocType<>68 and DocStatus>2  and a.PartNo = @PartNo
                                                            group by a.PartNo
                                                            union all
                                                            select PartNo,SUM(qty) as ReserveSoh from ray.InvDtlData a join ray.invhdrdata b
                                                            on a.DocNo=b.DocNo and a.FiscalYear=b.FiscalYear and a.StoreNo=b.StoreNo and a.DocType=b.DocType
                                                              where a.DocType=60 and RefConsQty=0 and CnfrmLvl=2 and DocStatus>2 and a.FiscalYear= @FiscalYear and a.StoreNo=@StoreNo and a.PartNo = @PartNo
                                                            group by PartNo"
                                          , new SqlParameter("@FiscalYear", FiscalYear), new SqlParameter("@StoreNo", storeNo), new SqlParameter("@PartNo", PartNo));


                var soh = invTools.GetItemDataSoh(db, PartNo, storeNo, FiscalYear);
                var act3Defauls = db.act3Defaults(PartNo);
                var itemdata =
                db.ItemDatas.AsNoTracking()
                    .Join(db.Units.AsNoTracking(), oks => oks.UntCode, iks => iks.UntCode, (I, U) =>
                    new { I.NationalPartCod, I.TechnicalNo, U.UntName, I.PartNo, I.PartGrp, I.PartNoDsc, I.PartLtnDsc, I.NotActiv, I.UntCode, I.OrderPoint })
                    .Where(c => c.PartNo == PartNo /*&& (!c.NotActiv.HasValue || c.NotActiv.Value == 1)*/).Select(c => new
                    {
                        c.PartNo,
                        c.TechnicalNo,
                        c.PartNoDsc,
                        c.NotActiv,
                        c.PartLtnDsc,
                        c.UntCode,
                        c.UntName,
                        Act3 = act3Defauls.id,
                        Act3Dsc = act3Defauls.dsc,
                        c.NationalPartCod,
                        soh = soh,

                        OrderPoint = c.OrderPoint //(db.ItemDatas.Where(i => i.PartNo == PartNo).FirstOrDefault() ?? new ItemData()).OrderPoint,
                    });

                if (itemdata.FirstOrDefault() != null)
                    return Ok(new
                    {
                        PartNo = itemdata.FirstOrDefault().PartNo,
                        TechnicalNo = itemdata.FirstOrDefault().TechnicalNo,
                        PartNoDsc = itemdata.FirstOrDefault().PartNoDsc,
                        NotActiv = itemdata.FirstOrDefault().NotActiv,
                        PartLtnDsc = itemdata.FirstOrDefault().PartLtnDsc,
                        UntCode = itemdata.FirstOrDefault().UntCode,
                        UntName = itemdata.FirstOrDefault().UntName,
                        Act3 = itemdata.FirstOrDefault().Act3,
                        Act3Dsc = itemdata.FirstOrDefault().Act3Dsc,
                        NationalPartCod = itemdata.FirstOrDefault().NationalPartCod,
                        soh = itemdata.FirstOrDefault().soh,
                        IncomingSoh = (IncomingSoh.FirstOrDefault() ?? new IncomingSohModel()).IncomingSoh,
                        ReserveSoh = (ReserveSoh.FirstOrDefault() ?? new ReserveSohModel()).ReserveSoh
                    });
                return new NotFoundActionResult("کالا یافت نشد.");
            }
        }
        /// <summary>
        /// دریافت موجودی لحظه ای کالا
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("GetArcSohInfo")]
        [Exception]
        [HttpPost]
        public IHttpActionResult GetArcSohInfo(ArcSohModelBindingModel input)
        {
            using (var db = new Entities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                var FiscalYear = invTools.CurrentFiscalYear.FiscalYear;
                //var todayDate = dateTimeService.crntPersianDate;
                return Ok(db.InvArcSohs
                    .Where(a => a.PartNo == input.PartNo &&
                    a.StoreNo == storeNo &&
                    a.FiscalYear == FiscalYear)
                    .ToList().OrderByDescending(o => int.Parse(o.DocDate))
                    .Take(input.count)
                    .Select(s => new { s.DocDate, s.Qty }));
            }
        }
        private fieldStatus docNoStatus(Entities db, byte docType)
        {
            var au = new System.Data.Entity.Core.Objects.ObjectParameter("AutoDocNo", typeof(byte));
            db.InvSp_UserAutoDoc(userId, storeNo, docType, au);
            if (Convert.ToByte(au.Value) == 1)
                return fieldStatus.disable;
            return fieldStatus.required;
        }
        [Route("insertOfficialUsingSp")]
        [Exception]
        [HttpPost]
        public invHdrDataBindingModel insertOfficialUsingSp(invHdrDataBindingModel doc)
        {
            //var dtls = new List<invDtlOfficial>();

            //var dtlsDataTable = new DataTable();
            //var cPartNo = new DataColumn("PartNo", typeof(string));
            //var cQty = new DataColumn("Qty", typeof(decimal));
            //var cSerial = new DataColumn("Serial", typeof(string));
            //var ccenter = new DataColumn("center", typeof(int?));
            //var cOrderNo = new DataColumn("OrderNo", typeof(long));
            //var cConsType = new DataColumn("ConsType", typeof(int?));
            //var cRcptType = new DataColumn("RcptType", typeof(int?));
            //var cSupplier = new DataColumn("Supplier", typeof(string));
            //var cRefdocrow = new DataColumn("Refdocrow", typeof(short?));
            //var cact3 = new DataColumn("act3", typeof(int?));
            //var cDocDsc = new DataColumn("DocDsc", typeof(string));
            //var cPartNo = new DataColumn("PartNo", typeof(object));
            //var cQty = new DataColumn("Qty", typeof(object));
            //var cSerial = new DataColumn("Serial", typeof(object));
            //var ccenter = new DataColumn("center", typeof(object));
            //var cOrderNo = new DataColumn("OrderNo", typeof(object));
            //var cConsType = new DataColumn("ConsType", typeof(object));
            //var cRcptType = new DataColumn("RcptType", typeof(object));
            //var cSupplier = new DataColumn("Supplier", typeof(object));
            //var cRefdocrow = new DataColumn("Refdocrow", typeof(object));
            //var cact3 = new DataColumn("act3", typeof(object));
            //var cDocDsc = new DataColumn("DocDsc", typeof(object));
            var dtlsDataTable = new DataTable();
            var cPartNo = new DataColumn("PartNo", typeof(string));
            var cQty = new DataColumn("Qty", typeof(decimal));
            var cSerial = new DataColumn("Serial", typeof(string));
            var ccenter = new DataColumn("center", typeof(int));
            var cOrderNo = new DataColumn("OrderNo", typeof(long));
            var cConsType = new DataColumn("ConsType", typeof(int));
            var cRcptType = new DataColumn("RcptType", typeof(int));
            var cSupplier = new DataColumn("Supplier", typeof(string));
            var cRefdocrow = new DataColumn("Refdocrow", typeof(short));
            var cact3 = new DataColumn("act3", typeof(int));
            var cDocDsc = new DataColumn("DocDsc", typeof(string));
            dtlsDataTable.Columns.Add(cPartNo);
            dtlsDataTable.Columns.Add(cQty);
            dtlsDataTable.Columns.Add(cSerial);
            dtlsDataTable.Columns.Add(ccenter);
            dtlsDataTable.Columns.Add(cOrderNo);
            dtlsDataTable.Columns.Add(cConsType);
            dtlsDataTable.Columns.Add(cRcptType);
            dtlsDataTable.Columns.Add(cSupplier);
            dtlsDataTable.Columns.Add(cRefdocrow);
            dtlsDataTable.Columns.Add(cact3);
            dtlsDataTable.Columns.Add(cDocDsc);



            foreach (var item in doc.items)
            {
                //    dtls.Add(new invDtlOfficial()
                //    {
                //        act3 = item.Act3.HasValue ? item.Act3.Value : 0,
                //        center = item.Center.HasValue ? item.Center.Value : 0,
                //        ConsType = item.ConsType.HasValue ? item.ConsType.Value : 0,
                //        DocDsc = item.DocDsc,
                //        OrderNo = item.OrderNo,
                //        PartNo = item.PartNo,
                //        Qty = item.Qty,
                //        RcptType = item.RcptType.HasValue ? item.RcptType.Value : 0,
                //        Refdocrow = 0,
                //        Serial = "",
                //        Supplier = item.Supplier
                //    });
                dtlsDataTable.Rows.Add(
                    item.PartNo.getDbValue(),
                    item.Qty,
                    DBNull.Value,
                    (item.Center.HasValue ? (object)item.Center.Value : DBNull.Value),
                    item.OrderNo,
                    (item.ConsType.HasValue ? (object)item.ConsType.Value : DBNull.Value),
                    (item.RcptType.HasValue ? (object)item.RcptType.Value : DBNull.Value),
                    item.Supplier.getDbValue(),
                    DBNull.Value,
                    (item.Act3.HasValue ? (object)item.Act3.Value : DBNull.Value),
                    item.DocDsc.getDbValue());
            }



            using (var conn = new SqlConnection(conStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ray.invsp_GeneralGenerateDocument";

                    //var dtlsDataTable = GenericDataTableFormater<invDtlOfficial>.GetDatatable(dtls);
                    //ObjectParameter op_Msg = new ObjectParameter("Msg", typeof(string));
                    //ObjectParameter op_MaxDocNo = new ObjectParameter("MaxDocNo", typeof(int));



                    cmd.Parameters.AddWithValue("@doctype", doc.DocType);

                    cmd.Parameters.AddWithValue("@StoreNo", storeNo.getDbValue());
                    cmd.Parameters.AddWithValue("@docdate", dateTimeService.crntPersianDate);
                    cmd.Parameters.AddWithValue("@Refdoctype", DBNull.Value);
                    cmd.Parameters.AddWithValue("@RefDocno", DBNull.Value);
                    cmd.Parameters.AddWithValue("@DestStoreno", DBNull.Value);
                    cmd.Parameters.AddWithValue("@RefBetweenDocno", DBNull.Value);
                    cmd.Parameters.AddWithValue("@RtrnStore", DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsSaleIssue", DBNull.Value);
                    cmd.Parameters.AddWithValue("@outDocno", DBNull.Value);

                    var par_dtls = cmd.Parameters.AddWithValue("Invdtl", dtlsDataTable);
                    par_dtls.SqlDbType = SqlDbType.Structured;
                    par_dtls.TypeName = "InvDtl";
                    //par_dtls.Direction = ParameterDirection.Input;




                    var par_Msg = cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 1000);
                    par_Msg.Direction = ParameterDirection.Output;
                    var par_MaxDocNo = cmd.Parameters.Add("@MaxDocNo", SqlDbType.Int);
                    par_MaxDocNo.Direction = ParameterDirection.Output;

                    conn.Open();
                    cmd.ExecuteNonQuery();



                    doc.DocNo = new int?(Convert.ToInt32(par_MaxDocNo.Value));
                    doc.Msg = par_Msg.Value == null || par_Msg.Value is DBNull ? "" : par_Msg.Value.ToString();



                }
                return doc;
            }

        }


        #region deliveryItems
        /// <summary>
        /// دریافت لیست اقلام تحویلی سند حواله انبار
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("deliveryItems/getList")]
        [Exception]
        [HttpPost]
        public IHttpActionResult deliveryItems_getList(deliveryItemsGetListBindingModel input)
        {
            if (!ModelState.IsValid)
                return new BadRequestActionResult(ModelState.Values);
            filterModel = input;
            using (var repo = new Repo(this, "ray.InvAssistantSp_DeliveryItems_GetList", "invDoc_deliveryItems_getList", initAsReader: true))
            {
                repo.cmd.Parameters.AddWithValue("@refDocFiscalYear", input.refFiscalYear.getDbValue());
                repo.cmd.Parameters.AddWithValue("@refDocType", input.refDocType.getDbValue());
                repo.cmd.Parameters.AddWithValue("@refDocNo", input.refDocNo.getDbValue());
                repo.cmd.Parameters.AddWithValue("@refDocRow", input.refDocRow.getDbValue());

                var par_isEditable = repo.cmd.Parameters.Add("@isEditable", SqlDbType.Bit);
                par_isEditable.Direction = ParameterDirection.Output;


                repo.ExecuteAdapter();
                return Ok(
                    repo.ds.Tables[0].AsEnumerable().Select(b => new
                    {

                        barcode = b.Field<object>("barcode").dbNullCheckString(),
                        qty = b.Field<object>("qty").dbNullCheckDecimal(),
                    })
                );

            }
        }
        /// <summary>
        /// دریافت لیست اقلام تحویلی سند حواله انبار
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("deliveryItems/getList/v2")]
        [Exception]
        [HttpPost]
        public IHttpActionResult deliveryItems_getList_v2(deliveryItemsGetListBindingModel input)
        {
            if (!ModelState.IsValid)
                return new BadRequestActionResult(ModelState.Values);
            filterModel = input;
            using (var repo = new Repo(this, "ray.InvAssistantSp_DeliveryItems_GetList", "invDoc_deliveryItems_getList", initAsReader: true))
            {
                repo.cmd.Parameters.AddWithValue("@refDocFiscalYear", input.refFiscalYear.getDbValue());
                repo.cmd.Parameters.AddWithValue("@refDocType", input.refDocType.getDbValue());
                repo.cmd.Parameters.AddWithValue("@refDocNo", input.refDocNo.getDbValue());
                repo.cmd.Parameters.AddWithValue("@refDocRow", input.refDocRow.getDbValue());

                var par_isEditable = repo.cmd.Parameters.Add("@isEditable", SqlDbType.Bit);
                par_isEditable.Direction = ParameterDirection.Output;


                repo.ExecuteAdapter();
                return Ok(
                    new
                    {
                        isEditable = par_isEditable.Value.dbNullCheckBoolean(),
                        list = repo.ds.Tables[0].AsEnumerable().Select(b => new
                        {
                            barcode = b.Field<object>("barcode").dbNullCheckString(),
                            qty = b.Field<object>("qty").dbNullCheckDecimal(),
                        })
                    });

            }
        }
        /// <summary>
        /// بروزرسانی اقلام تحویلی سند حواله انبار
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("deliveryItems/addUpdate")]
        [Exception]
        [HttpPost]
        public IHttpActionResult deliveryItems_addUpdate(deliveryItemsAddUpdateBindingModel input)
        {
            if (!ModelState.IsValid)
                return new BadRequestActionResult(ModelState.Values);
            filterModel = input;
            using (var repo = new Repo(this, "ray.InvAssistantSp_DeliveryItems_AddUpdate", "invDoc_deliveryItems_addUpdate", initAsReader: true))
            {
                repo.cmd.Parameters.AddWithValue("@refDocFiscalYear", input.refFiscalYear.getDbValue());
                repo.cmd.Parameters.AddWithValue("@refDocType", input.refDocType.getDbValue());
                repo.cmd.Parameters.AddWithValue("@refDocNo", input.refDocNo.getDbValue());
                repo.cmd.Parameters.AddWithValue("@refDocRow", input.refDocRow.getDbValue());
                repo.cmd.Parameters.AddWithValue("@ignoreWarning", input.ignoreWarning.getDbValue());



                var par_rCode = repo.cmd.Parameters.Add("@rCode", SqlDbType.Int);
                par_rCode.Direction = ParameterDirection.Output;
                var par_rMsg = repo.cmd.Parameters.Add("@rMsg", SqlDbType.VarChar, -1);
                par_rMsg.Direction = ParameterDirection.Output;


                var par_hasError = repo.cmd.Parameters.Add("@hasError", SqlDbType.Bit);
                par_hasError.Direction = ParameterDirection.Output;
                var par_hasWarnning = repo.cmd.Parameters.Add("@hasWarnning", SqlDbType.Bit);
                par_hasWarnning.Direction = ParameterDirection.Output;


                DataTable dtDeliveryItems = ClassToDatatableConvertor.CreateDataTable(input.deliveryItems ?? new List<deliveryItemModel>());
                var param = repo.cmd.Parameters.AddWithValue("@deliveryItems", dtDeliveryItems);
                param.SqlDbType = SqlDbType.Structured;


                repo.ExecuteAdapter();

                if (par_rCode.Value.dbNullCheckInt() == 1)
                    return Ok(new
                    {
                        msg = par_rMsg.Value.dbNullCheckString(),
                        hasError = par_hasError.Value.dbNullCheckBoolean(),
                        hasWarnning = par_hasWarnning.Value.dbNullCheckBoolean(),
                        deliveryItems = repo.ds.Tables[0].AsEnumerable().Select(b => new
                        {
                            barcode = b.Field<object>("barcode").dbNullCheckString(),
                            qty = b.Field<object>("qty").dbNullCheckDecimal(),
                            errorMessage = b.Field<object>("errorMessage").dbNullCheckString(),
                        })
                    });
                else if (par_rCode.Value.dbNullCheckInt() == 0)
                    return new NotFoundActionResult(par_rMsg.Value.dbNullCheckString());
                else
                    throw new Exception(par_rMsg.Value.dbNullCheckString());

                //if (par_rCode.Value.dbNullCheckInt() == 1)//success
                //    return Ok(new { msg = par_rMsg.Value.ToString(),deliveryItems = outlis });
                //else if (par_rCode.Value.dbNullCheckInt() == 2)//haswarning
                //    return new MultipleChoicesActionResult(new { msg = par_rMsg.Value.ToString(), deliveryItems = outlis });
                //else if (par_rCode.Value.dbNullCheckInt() == 3)//invalid
                //    return new (new { msg = par_rMsg.Value.ToString(), deliveryItems = outlis });
                //else if (par_rCode.Value.dbNullCheckInt() == 0)//fail
                //    throw new Exception(par_rMsg.Value.dbNullCheckString());
                //else if (par_rCode.Value.dbNullCheckInt() == -1)//unexpectedFail:
                //    throw new Exception(par_rMsg.Value.dbNullCheckString());



            }
        }
        #endregion


    }
    public static class extentions
    {
        public static keyValuePair act3Defaults(this Entities db, string partNo)
        {
            try
            {
                var res = (db.InvSp_FindPropertyFromItemData(partNo).FirstOrDefault() ??
                new InvSp_FindPropertyFromItemData_Result());
                return new keyValuePair()
                {
                    id = res.act3.ToString(),
                    dsc = res.FactorDsc
                };
            }
            catch
            {
                return new keyValuePair()
                {
                    id = "",
                    dsc = ""
                };
            }
        }
        public static keyValuePair FillForceQty(this Entities db, byte docType, string fldName, string storeNo/*,string partNo*/)
        {
            //id = (res.FirstOrDefault() ?? new InvSp_FillForceQty_Result()).DefaultVal,
            //dsc = (res.FirstOrDefault() != null && !string.IsNullOrEmpty(res.FirstOrDefault().DefaultVal))  ?
            //(db.Suppliers.Where(s=>s.Supplier1 == res.FirstOrDefault().DefaultVal).FirstOrDefault() ?? new Supplier()).SupName : ""


            return new keyValuePair()
            {
                id = (db.InvSp_FillForceQty(docType, fldName, storeNo).FirstOrDefault() ??
            new InvSp_FillForceQty_Result()).DefaultVal
            }.initDefaultsDsc(db, fldName/*, partNo*/);

        }
        public static keyValuePair initDefaultsDsc(this keyValuePair Default, Entities db, string fldName/*, string partNo*/)
        {
            if (string.IsNullOrEmpty(Default.id))
                Default.dsc = "";
            if (new string[] { "ReqCenter", "Center" }.Contains(fldName))
            {
                Default.dsc = (db.Centers.Where(c => c.Center1.ToString() == Default.id).FirstOrDefault() ?? new Center()).CenterDsc;
            }
            else if (fldName == "RtrnBuyReason")
            {
                Default.dsc = (db.InvRtrnBuys.Where(c => c.RtrnBuyReason.ToString() == Default.id).FirstOrDefault() ?? new InvRtrnBuy()).RtrnBuyDesc;
            }
            else if (fldName == "OrderNo")
            {
                Default.dsc = (db.InvOrdrs.Where(c => c.OrdrNO.ToString() == Default.id).FirstOrDefault() ?? new InvOrdr()).OrdrDsc;
            }
            else if (fldName == "RcptType")
            {
                Default.dsc = (db.InvRcptTyps.Where(c => c.RcptType.ToString() == Default.id).FirstOrDefault() ?? new InvRcptTyp()).RcptTypeDesc;
            }
            else if (fldName == "ReqType")
            {
                Default.dsc = (db.InvReqTyps.Where(c => c.ReqType.ToString() == Default.id).FirstOrDefault() ?? new InvReqTyp()).ReqTypeDesc;
            }
            else if (fldName == "Supplier")
            {
                Default.dsc = (db.Suppliers.Where(c => c.Supplier1 == Default.id).FirstOrDefault() ?? new Supplier()).SupName;
            }




            return Default;
        }
    }
    #region Models
    public class getDocListHdrBindingModel : publicFilterModel
    {
        /// <summary>
        /// نوع سند مورد نظر
        /// </summary>
        [Required(ErrorMessage = "docType is required")]
        public byte? DocType { get; set; }
        /// <summary>
        /// فیلتر_شماره سند
        /// </summary>
        public int? f_docNo { get; set; }
        /// <summary>
        /// فیلتر_تاریخ سند
        /// </summary>
        public string f_createDate { get; set; }
        /// <summary>
        /// فیلتر_سال مالی سند
        /// </summary>
        public int? f_fiscalYear { get; set; }
        ///// <summary>
        ///// فیلتر_مرکز هزینه سند
        ///// </summary>
        //public int? f_center { get; set; }
        /// <summary>
        /// فیلتر_شماره سفارش سند
        /// </summary>
        public int? f_orderNo { get; set; }
    }
    public class getDocListDtlBindingModel : publicFilterModel
    {
        /// <summary>
        /// نوع سند مورد نظر
        /// </summary>
        public byte? DocType { get; set; }
        /// <summary>
        /// شماره سند
        /// </summary>
        public int? DocNo { get; set; }
        /// <summary>
        /// سال مالی
        /// </summary>
        public int? FiscalYear { get; set; }

    }
    public class getListDtlModel
    {

        public decimal? soh { get; set; }
        public getListDtlModel()
        {
            availableSerials = new List<InvSerial>();
        }
        public short docrow { get; set; }
        public string PartNo { get; set; }
        public int? ReqCenter { get; set; }
        public List<InvSerial> availableSerials { get; set; }
        public string PartNoDsc { get; set; }
        public string DocDsc { get; set; }
        public string NeedDate { get; set; }
        public decimal Qty { get; set; }
        public int Center { get; set; }
        public string CenterDsc { get; set; }
        public long? OrderNo { get; set; }
        [JsonIgnore]
        public string OrdrDsc { get; set; }

        public string OrderNoDsc
        {
            get
            {
                return OrdrDsc;
            }
        }

        public int? RcptType { get; set; }
        [JsonIgnore]
        public string RcptTypeDesc { get; set; }
        public string RcptTypeDsc
        {
            get
            {
                return RcptTypeDesc;
            }
        }
        public byte? ReqType { get; set; }
        [JsonIgnore]
        public string ReqTypeDesc { get; set; }
        public string ReqTypeDsc
        {
            get
            {
                return ReqTypeDesc;
            }
        }
        public int? ConsType { get; set; }
        [JsonIgnore]
        public string ConsTypeDesc { get; set; }
        public string ConsTypeDsc
        {
            get
            {
                return ConsTypeDesc;
            }
        }
        public string Supplier { get; set; }
        [JsonIgnore]
        public string SupName { get; set; }
        public string SupplierDsc
        {
            get
            {
                return SupName;
            }
        }



        public int? act3 { get; set; }
        [JsonIgnore]
        public string Act3Dsc { get; set; }

        public string act3Dsc
        {
            get
            {
                return Act3Dsc;
            }
        }

        public byte? AudtReason { get; set; }
        public string AudtReasonDsc { get; set; }
        public string Serial { get; set; }
        public string UntCode { get; set; }
        public string UntName { get; set; }
        public decimal? CntQty { get; set; }
        public decimal? Weight { get; set; }
        public string docdate { get; set; }
        public decimal? RefOrder { get; set; }
        public decimal? RefConsQty { get; set; }
        /// <summary>
        /// مقدار قابل مصرف (سند سفارش)
        /// </summary>
        public decimal? remainingRefOrder { get; set; }
        /// <summary>
        /// مقدار قابل مصرف (سند ورودی)
        /// </summary>
        public decimal? remainingRefConsQty { get; set; }


        /// <summary>
        /// کد وضعیت اقلام تحویلی
        /// که وقتی 0 باشه یعنی هنوز اقلام تحویلی برای اون ردیف ثبت نشده
        ///وقتی 1 باشه یعنی یه چیزایی ثبت شده ولی کامل نیست
        ///وقتی 2 باشه یعنی کامل ثبت شده قبلا.
        /// </summary>
        public int? deliveryItemsStatus { get; set; }
        /// <summary>
        /// مجموع مقدار اقلام تحویلی
        /// </summary>
        public decimal? sumDeliveryItems { get; set; }

    }

    public class getDocReferHdrBindingModel : publicFilterModel
    {
        /// <summary>
        /// نوع سند پایه
        /// </summary>
        public byte? DocType { get; set; }
        /// <summary>
        /// شماره سند پایه
        /// </summary>
        public int? DocNo { get; set; }
        /// <summary>
        /// فیلتر_نوع سند مرجع
        /// </summary>
        public byte? f_ref_docType { get; set; }
        /// <summary>
        /// فیلتر_تاریخ سند مرجع
        /// </summary>
        public string f_ref_docDate { get; set; }
        /// <summary>
        /// فیلتر_سال مالی سند مرجع
        /// </summary>
        public int? f_ref_fiscalYear { get; set; }
        /// <summary>
        /// فیلتر_شماره سند مرجع
        /// </summary>
        public int? f_ref_docNo { get; set; }
        /// <summary>
        /// فیلتر_مرکز هزینه سند مرجع
        /// </summary>
        public int? f_ref_center { get; set; }
        /// <summary>
        /// فیلتر_شماره سفارش سند مرجع
        /// </summary>
        public int? f_ref_orderNo { get; set; }


        public string FiscalYear { get; set; }
        public string StoreNo { get; set; }
        //public string DocType { get; set; }
        public string Docno { get; set; }
        public string Partno { get; set; }
        public string BaseDocType { get; set; }
        public short IsStandardIssue { get; set; }
        public string usr { get; set; }
        public string FilterPartno { get; set; }
        public string FilterCenter { get; set; }
        public string FilterDate { get; set; }
        public string strIsInvWithParameter { get; set; }
        public string FilterOrderNo { get; set; }
        public string FilterSerial { get; set; }
        public string NotRemain { get; set; }
        public short IsSaleIssueForRtrn { get; set; }

        ///// <summary>
        ///// <summary>
        ///// عنوان فیلد مورد مرتب سازی
        ///// </summary>
        //public string orderBy { get; set; }
        ///// <summary>
        ///// مرتب سازی نزولی است؟
        ///// </summary>
        //public bool? isDescOrder { get; set; }

    }
    public class getDocReferDtlBindingModel : publicFilterModel
    {
        /// <summary>
        /// نوع سند مورد نظر
        /// </summary>
        public byte? DocType { get; set; }
        /// <summary>
        /// شماره سند (برای دریافت جزئیات کاربرد دارد)
        /// </summary>
        public int? DocNo { get; set; }
        public int? FiscalYear { get; set; }
        /// <summary>
        /// نوع سند پایه
        /// </summary>
        public byte? baseDocType { get; set; }
    }

    public class DocRefer_HdrModel
    {
        public int? Row { get; set; }
        public int? FiscalYear { get; set; }
        public byte? DocType { get; set; }
        public string doctypedesc { get; set; }
        public int? DocNo { get; set; }
        public string DocDate { get; set; }
        public byte? DocStatus { get; set; }
        public string reqcenterDesc { get; set; }
        public string reqCenterIdDsc
        {
            get
            {
                return (ReqCenter.HasValue ? ReqCenter.ToString() + "-" : "") + reqcenterDesc;
            }
        }
        public int? orgdocno { get; set; }
        public int? orgRefno { get; set; }
        public int? ReqCenter { get; set; }
        public string DestStoreNo { get; set; }
        public string LcNo { get; set; }
        public string raysys { get; set; }
        public string OrderDsc { get; set; }
    }

    //public static class GenericDataTableFormater<T>
    //{
    //    public static DataTable GetDatatable(IEnumerable<T> data)
    //    {
    //        DataTable table = new DataTable();
    //        using (var reader = ObjectReader.Create(data))
    //        {
    //            table.Load(reader);
    //        }
    //        return table;
    //    }
    //}
    public class invDtlOfficial
    {
        public string PartNo
        {
            get; set;
        }
        public decimal Qty
        {
            get; set;
        }
        public string Serial
        {
            get; set;
        }
        public int center
        {
            get; set;
        }
        public long OrderNo
        {
            get; set;
        }
        public int ConsType
        {
            get; set;
        }
        public int RcptType
        {
            get; set;
        }
        public string Supplier
        {
            get; set;
        }
        public short Refdocrow
        {
            get; set;
        }
        public int act3
        {
            get; set;
        }
        public string DocDsc { get; set; }


    }

    public class getOtherUsableSerialNosBindingModel : publicFilterModel
    {

        /// <summary>
        /// نوع سند مرجع
        /// </summary>
        //[Required(ErrorMessage = "تعیین نوع سند مرجع الزامی ست.")]
        public byte? RefDocType { get; set; }
        /// <summary>
        /// شماره سند مرجع
        /// </summary>
        public int? RefDocNo { get; set; }
        public int? FiscalYear { get; set; }
        /// <summary>
        /// شماره ردیف سند مرجع
        /// </summary>
        //[Required(ErrorMessage ="تعیین ردیف سند مرجع الزامی ست.")]
        public short? RefDocrow { get; set; }
        /// <summary>
        /// نوع سند
        /// حتما پر شود
        /// </summary>
        public byte? DocType { get; set; }

    }
    public class getAndValidateBatchInfoBindingModel : publicFilterModel
    {
        /// <summary>
        /// سریال خاص مورد نظر
        /// </summary>
        [Required(ErrorMessage = "ورود سریال الزامی ست.")]
        public string serial { get; set; }
        /// <summary>
        /// نوع سند مرجع
        /// </summary>
        public byte? RefDocType { get; set; }
        /// <summary>
        /// شماره سند مرجع
        /// </summary>
        public int? RefDocNo { get; set; }
        /// <summary>
        /// نوع سند
        /// </summary>
        public byte? DocType { get; set; }
    }


    public class getOtherUsableSerialNos
    {
        public string Serial { get; set; }
        public decimal? Soh { get; set; }
        public int? ReqCenter { get; set; }
        public string ReqCenterDsc { get; set; }
        public long? OrderNo { get; set; }
        public string OrderNoDsc { get; set; }
        public string Supplier { get; set; }
        public string SupplierDsc { get; set; }
        public int? Center { get; set; }
        public string CenterDsc { get; set; }
    }


    public class InvSp_SelectDocReferDtl_Result_ : InvSp_SelectDocReferDtl_Result
    {
        public byte? ReqType { get; set; }
        public string ReqTypeDesc { get; set; }
    }
    /// <summary>
    /// مدل ورودی مقدار دهی به فرم اسکن -  بارکد کالا
    /// </summary>
    public class initScanDialog_itemBindingModel
    {
        /// <summary>
        /// کد نوع سند
        /// </summary>
        [Required]
        public byte docType { get; set; }
        /// <summary>
        /// کد کالا (بارکد اسکن شده)
        /// </summary>
        [Required]
        public string partNo { get; set; }
    }
    /// <summary>
    /// مدل ورودی مقدار دهی به فرم اسکن -  بارکد کالا
    /// </summary>
    public class initScanDialog_BindingModel
    {
        /// <summary>
        /// کد نوع سند
        /// </summary>
        [Required]
        public byte? docType { get; set; }
        /// <summary>
        /// بارکد اسکن شده
        /// </summary>
        [Required]
        public string barcode { get; set; }
    }
    /// <summary>
    /// مدل ورودی مقدار دهی به فرم اسکن -  سریال
    /// </summary>
    public class initScanDialog_serialBindingModel
    {
        /// <summary>
        /// کد نوع سند
        /// </summary>
        [Required]
        public byte? docType { get; set; }
        /// <summary>
        /// کد کالا (بارکد اسکن شده)
        /// </summary>
        [Required]
        public string partNo { get; set; }
        /// <summary>
        /// سریال
        /// </summary>
        [Required]
        public string serial { get; set; }
        /// <summary>
        /// نوع سند مرجع
        /// </summary>
        public byte? refDocType { get; set; }
        /// <summary>
        /// شماره سند مرجع
        /// </summary>
        public int? refDocNo { get; set; }
        /// <summary>
        /// شماره ردیف سند مرجع
        /// </summary>
        public short? refDocRow { get; set; }
    }

    public class deliveryItemsGetListBindingModel : publicFilterModel
    {
        /// <summary>
        /// نوع سند مرجع
        /// </summary>
        public byte? refDocType { get; set; }
        /// <summary>
        /// شماره سند مرجع
        /// </summary>
        public int? refDocNo { get; set; }
        /// <summary>
        /// شماره ردیف سند مرجع
        /// </summary>
        public short? refDocRow { get; set; }
        /// <summary>
        /// سال مالی سند مرجع
        /// </summary>
        public int? refFiscalYear { get; set; }
    }
    public class deliveryItemsAddUpdateBindingModel : publicFilterModel
    {
        /// <summary>
        /// نوع سند مرجع
        /// </summary>
        public byte? refDocType { get; set; }
        /// <summary>
        /// شماره سند مرجع
        /// </summary>
        public int? refDocNo { get; set; }
        /// <summary>
        /// شماره ردیف سند مرجع
        /// </summary>
        public short? refDocRow { get; set; }
        /// <summary>
        /// سال مالی سند مرجع
        /// </summary>
        public int? refFiscalYear { get; set; }
        /// <summary>
        /// هشدار نادیده گرفته شود
        /// </summary>
        public bool? ignoreWarning { get; set; }
        /// <summary>
        /// لیست اقلام تحویلی
        /// </summary>
        public List<deliveryItemModel> deliveryItems { get; set; }
    }
    /// <summary>
    /// مدل قلم تحویلی
    /// </summary>
    public class deliveryItemModel
    {
        /// <summary>
        /// بارکد
        /// </summary>
        public string barcode { get; set; }
        /// <summary>
        /// مقدار
        /// </summary>
        public decimal? qty { get; set; }
        /// <summary>
        /// خطا / هشدار
        /// </summary>
        public string errorMessage { get; set; }
    }
    #endregion


}