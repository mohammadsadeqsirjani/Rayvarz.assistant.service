using Rayvarz.inv.assistant.service.Models.ray;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Rayvarz.inv.assistant.service.Tools
{

    public class invTools
    {
        /// <summary>
        /// حواله
        /// </summary>
        public const byte TYPE_DOC_DRAFT_40 = 40;
        /// <summary>
        /// رسيد خريد كالا
        /// </summary>
        public const byte TYPE_DOC_RECEIPT_OF_PURCHASE_14 = 14;
        /// <summary>
        /// درخواست خرید
        /// </summary>
        public const byte TYPE_DOC_BUY_REQ_62 = 62;
        /// <summary>
        /// برگشت از خرید
        /// </summary>
        public const byte TYPE_DOC_RETURN_FROM_BUY_16 = 16;
        /// <summary>
        /// برگشت به انبار
        /// </summary>
        public const byte TYPE_DOC_RETURN_TO_STORE_44 = 44;
        /// <summary>
        /// رسيد كالا/توليد
        /// </summary>
        public const byte TYPE_DOC_RECEIPT_OF_PRODUCTION_12 = 12;
        /// <summary>
        /// برگشت رسيد كالا/توليد
        /// </summary>
        public const byte TYPE_DOC_RETURN_FROM_RECEIPT_OF_PRODUCTION_22 = 22;
        /// <summary>
        /// رسید موقت
        /// </summary>
        public const byte TYPE_DOC_TEMPORARY_RECEIPT_66 = 66;
        /// <summary>
        /// درخواست کالا
        /// </summary>
        public const byte TYPE_DOC_ITEM_REQUEST_60 = 60;


        public static byte[] supportedDocTypes = new byte[]
        {
            TYPE_DOC_DRAFT_40,
            TYPE_DOC_RECEIPT_OF_PURCHASE_14,
            //TYPE_DOC_BUY_REQ_62,
            //TYPE_DOC_RETURN_FROM_BUY_16,
            //TYPE_DOC_RETURN_TO_STORE_44,
            TYPE_DOC_RECEIPT_OF_PRODUCTION_12,
            //TYPE_DOC_RETURN_FROM_RECEIPT_OF_PRODUCTION_22
            TYPE_DOC_TEMPORARY_RECEIPT_66,
            //TYPE_DOC_ITEM_REQUEST_60
        };


        public static decimal GetItemDataSoh(Entities db, string PartNo, string StoreNo, int FiscalYear)
        {
            //using (var db = new Entities())
            //{
            var Soh = db.InvArcSohs.AsNoTracking().Where(InvSoh => InvSoh.PartNo.Equals(PartNo) &&
            (InvSoh.FiscalYear == (FiscalYear)) &&
            (InvSoh.StoreNo.Equals(StoreNo)))
            .OrderByDescending(InvSoh => InvSoh.DocDate).Select(InvSoh => InvSoh.Qty)
            .FirstOrDefault();
            if (Soh != null)
                return (decimal)Soh;
            return 0;
            //}
        }

        public static decimal GetSerialSoh(Entities db, string PartNo, string serial, string StoreNo)
        {
            //using (var db = new Entities())
            //{
            var serialinfo = db.InvAssistantTVFUNC_SerialSoh_GetList(StoreNo, PartNo, serial).FirstOrDefault();
            return serialinfo != null && serialinfo.Soh.HasValue ? serialinfo.Soh.Value : 0;
            //}
        }


        public static byte? maxConfLevel
        {
            get
            {
                using (var db = new Entities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    var a = db.RaySysSpcs.AsNoTracking().Where(s => s.RaySys.ToUpper() == "INV" && s.InfTit.ToUpper() == "CNFRMLVL").FirstOrDefault();

                    byte bt;
                    if (a == null || !byte.TryParse(a.InfVal, out bt))
                        return new byte?();
                    return bt;
                }
            }
        }

        public static InvPrd CurrentFiscalYear
        {
            get
            {
                using (var db = new Entities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    var a = db.InvPrds.AsNoTracking().Where(p => p.FiscalStatus == 1).OrderByDescending(o => o.FiscalStatus).FirstOrDefault();
                    if (a != null)
                        return a;
                    else
                        throw new Exception("سال مالی جاری فعال نمی باشد");


                }
            }
        }
    }
}