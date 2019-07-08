using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rayvarz.inv.assistant.service.Models
{
    /// <summary>
    /// مدل جزئیات (ردیف) سند
    /// </summary>
    public class invDtlDataBindingModel
    {
        /// <summary>
        /// کد کالا
        /// </summary>
        [Display(Name = "کالا")]
        [Required(ErrorMessage = "ورود {0} اجباریست")]
        public string PartNo { get; set; }
        /// <summary>
        /// شماره ردیف سند
        /// </summary>
        public short? docrow { get; set; }
        /// <summary>
        /// شماره قفسه
        /// </summary>
        [Display(Name = "شماره قفسه")]
        public string BinNo { get; set; }

        /// <summary>
        /// مرکز هزینه
        /// </summary>
        [Display(Name = "مرکز هزینه")]
        [Required(ErrorMessage = "ورود {0} اجباریست")]
        public int? Center { get; set; }
        //[Required(ErrorMessage = "ورود {0} اجباریست")]
        /// <summary>
        /// شماره سفارش
        /// </summary>
        [Display(Name = "شماره سفارش")]
        public long OrderNo { get; set; }
        //[Required(ErrorMessage = "ورود {0} اجباریست")]
        /// <summary>
        /// نوع رسید
        /// </summary>
        [Display(Name = "نوع رسید")]
        public int? RcptType { get; set; }
        // [Required(ErrorMessage = "ورود {0} اجباریست")]
        /// <summary>
        /// فروشنده
        /// </summary>
        [Display(Name = "فروشنده")]
        public string Supplier { get; set; }

        /// <summary>
        /// فعالیت 3
        /// </summary>
        public int? Act3 { get; set; }
        /// <summary>
        /// مقدار ردیف
        /// </summary>
        [Display(Name = "مقدار ردیف")]
        [Required(ErrorMessage = "ورود {0} اجباریست")]
        public decimal Qty { get; set; }
        /// <summary>
        /// مقدار شمارش
        /// </summary>
        [Display(Name = "مقدار شمارش")]
        public decimal? CntQty { get; set; }
        /// <summary>
        /// وزن
        /// </summary>
        [Display(Name = "وزن")]
        public decimal? Weight { get; set; }

        //[Display(Name = "مبلغ رسید")]
        ///// <summary>
        ///// مبلغ رسید
        ///// </summary>
        //public decimal? amt { get; set; }

        /// <summary>
        /// عوارض
        /// </summary>
        [Display(Name = "عوارض")]
        public decimal? TollAmt { get; set; }

        /// <summary>
        /// مالیات
        /// </summary>
        [Display(Name = "مالیات")]
        public decimal? TaxAmt { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        [Display(Name = "توضیحات")]
        public string DocDsc { get; set; }
        /// <summary>
        /// شماره سند مالی
        /// </summary>
        [Display(Name = "شماره سند مالی")]
        public string AccDocNo { get; set; }
        /// <summary>
        /// شماره فاکتور
        /// </summary>
        public string FactorNo { get; set; }

        /// <summary>
        /// تاریخ فاکتور
        /// </summary>
        public string FactorDate { get; set; }



        /// <summary>
        /// شماره سند سفارش
        /// </summary>
        [Display(Name = "شماره سند سفارش")]
        public int? RfDocNo { get; set; }


        /// <summary>
        /// شماره ردیف سفارش
        /// </summary>
        [Display(Name = "شماره ردیف سفارش")]
        public short? RfDocRow { get; set; }
        /// <summary>
        /// فعالیت 4
        /// </summary>
        public int? Act4 { get; set; }
        /// <summary>
        /// فعالیت 5
        /// </summary>
        public int? Act5 { get; set; }
        /// <summary>
        /// علت برگشت از خرید
        /// </summary>
        public byte? RtrnBuyReason { get; set; }
        /// <summary>
        /// واحد کالا
        /// </summary>
        //برای حواله اجباری
        [Display(Name = "واحد کالا")]
        //[Required(ErrorMessage = "ورود {0} اجباریست")]
        public string UntCode { get; set; }
        //برای حواله اجباری
        //[Required(ErrorMessage = "ورود {0} اجباریست")]
        /// <summary>
        /// نوع مصرف
        /// </summary>
        [Display(Name = "نوع مصرف")]
        public int? ConsType { get; set; }







        /// <summary>
        /// مبلغ برآوردی خرید
        /// </summary>
        public decimal? Amt { get; set; }

        //[Required(ErrorMessage = "ورود {0} اجباریست")]
        /// <summary>
        /// تاریخ نیاز
        /// </summary>
        [Display(Name = "تاریخ نیاز")]
        public string NeedDate { get; set; }

        //[Required(ErrorMessage = "ورود {0} اجباریست")]
        /// <summary>
        /// نوع درخواست
        /// </summary>

        [Display(Name = "نوع درخواست")]
        public byte? ReqType { get; set; }



        /// <summary>
        /// شرح خطا
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// شماره قرارداد
        /// </summary>
        [Display(Name = "شماره قرارداد")]
        public string TreatyNo { get; set; }
        /// <summary>
        /// علت برگشت به انبار
        /// </summary>
        [Display(Name = "علت برگشت به انبار")]
        public byte? RtrnStrReason { get; set; }
        /// <summary>
        /// کد کنترل کیفیت
        /// </summary>
        public string QcCode { get; set; }
        /// <summary>
        /// سریال
        /// </summary>
        public string Serial { get; set; }
        /// <summary>
        /// تعداد در بسته (برای آب و فاضلاب)
        /// </summary>
        public decimal? cntInOnePackage { get; set; }



        /// <summary>
        /// نام سازنده
        /// </summary>
        public string InvSerial_Supplier { get; set; }
        /// <summary>
        /// مدل
        /// </summary>
        public string InvSerial_Model { get; set; }
        /// <summary>
        /// شناسه کنترل کیفیت
        /// </summary>
        public string InvSerial_QcNo { get; set; }
        /// <summary>
        /// تاریخ انقضاء
        /// </summary>
        public string InvSerial_SuppExpDate { get; set; }
        /// <summary>
        /// تاریخ بهینه مصرف
        /// </summary>
        public string InvSerial_OptmIsuDat { get; set; }
        /// <summary>
        /// شماره ردیف سند مرجع
        /// </summary>
        [Display(Name = "شماره ردیف سند مرجع")]
        public short RefDocRow { get; set; }

        //public string Ref
    }

    public class invDtlDataBindingModel_offisial
    {
        /// <summary>
        /// PartNo
        /// </summary>
        public string ItemDataId { get; set; }
        public string Qty { get; set; }
        public string Serial { get; set; }
        /// <summary>
        /// Center
        /// </summary>
        public string CenterId { get; set; }
        /// <summary>
        /// OrderNo
        /// </summary>
        public string InventoryOrderId { get; set; }
        /// <summary>
        /// ConsType
        /// </summary>
        public string ItemConsumptionTypeId { get; set; }
        /// <summary>
        /// RcptType
        /// </summary>
        public string ItemReceiptTypeId { get; set; }
        /// <summary>
        /// Supplier
        /// </summary>
        public string SupplierId { get; set; }
        /// <summary>
        /// GdiRefDocRow
        /// </summary>
        public string ReferenceRowNo { get; set; }
        /// <summary>
        /// act3
        /// </summary>
        public string Center3Id { get; set; }
        /// <summary>
        /// DocDsc
        /// </summary>
        public string Description { get; set; }
    }
}