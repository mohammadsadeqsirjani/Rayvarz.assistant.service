using Rayvarz.invHServ.Api.Models.Common;
using Rayvarz.invHServ.Api.Models.Ray;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rayvarz.invHServ.Api.Controllers
{
    //[Authorize]
    [RoutePrefix("api/InvHdrDatas")]
    public class InvHdrDatasController : ApiController
    {
        [HttpPost]
        [Route("GetReferences")]
        public HttpResponseMessage GetReferences([FromBody] referenceInfo ri)
        {
            using (var db = new RayvarzEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;

                return Request.CreateResponse(HttpStatusCode.OK,
                db.InvRfDoc.AsNoTracking().Join(db.InvPrd.AsNoTracking()/*.Where(p => p.FiscalStatus.Equals(1))*/, oks => oks.FiscalYear, iks => iks.FiscalYear, (o, i) => new { refs = o, i.FiscalStatus })
                    .Where(rd => (rd.FiscalStatus.Equals(1)) && (ri._docTyp.HasValue ? rd.refs.RefDocType.Equals(ri._docTyp) : 1 == 1) &&
               (string.IsNullOrEmpty(ri.StoreNo) ? 1 == 1 : rd.refs.RefStore.Equals(ri.StoreNo))).Select(s => s.refs)
               //.GroupJoin(db.InvDtlData,oks=>new {oks.DocType,oks.FiscalYear,oks.sto },iks=>new {iks.d, },(rs,ds) => new {refs = rs,dss = ds })
               .Select(s => new { s.RefFiscalYear, s.RefStore, s.RefDocType, s.RefDocRow, s.RefDocNo, s.RefPArtNo }));



                //db.InvDtlData.AsNoTracking().Where(idd => ri._docTyp.HasValue ? idd.DocType.Equals(ri._docTyp) : 1 == 1)
                //    .Join(db.InvPrd.Where(p => p.FiscalStatus.Equals(1)), oks => oks.FiscalYear, iks => iks.FiscalYear, (o, i) => o)
                //    .Join(db.ref)
            }
        }
    }

    public class referenceInfo : publicFilterModel
    {
        public byte? _docTyp { get; set; }
        public string docTyp
        {
            get
            {
                return _docTyp.ToString();
            }
            set
            {
                _docTyp = string.IsNullOrEmpty(value) ? new byte?() : byte.Parse(value);
            }
        }
        public string StoreNo { get; set; }
    }

}
