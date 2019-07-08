using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rayvarz.inv.assistant.service.Models
{
    /// <summary>
    /// مدل سربرگ (هدر) سند
    /// </summary>
    public class invHdrDataBindingModel
    {
        /// <summary>
        /// نوع سند
        /// </summary>
        [Display(Name = "نوع سند")]
        [Required(ErrorMessage = "ورود {0} اجباریست")]
        public byte DocType { get; set; }
        /// <summary>
        /// نوع سند مرجع
        /// </summary>
        public byte? RefDocType { get; set; }
        /// <summary>
        /// سال سند مرجع
        /// </summary>
        public int? RefFiscalYear { get; set; }

        /// <summary>
        /// شماره سند
        /// </summary>
        [Display(Name = "شماره سند")]
        //[Required(ErrorMessage = "ورود {0} اجباریست")]
        public int? DocNo { get; set; }
        /// <summary>
        /// شماره سند مرجع
        /// </summary>
        public int? RefDocNo { get; set; }
        /// <summary>
        /// تاریخ سند
        /// </summary>
        [Display(Name = "تاریخ سند")]
        [Required(ErrorMessage = "ورود {0} اجباریست")]
        public string DocDate { get; set; }

        /// <summary>
        /// ردیف های سند
        /// </summary>
        [Display(Name = "ردیف ها")]
        [Required(ErrorMessage = "ورود {0} اجباریست")]
        public List<invDtlDataBindingModel> items { get; set; }


        /// <summary>
        /// قسمت درخواست کننده
        /// </summary>
        [Display(Name = "قسمت درخواست کننده")]
        //[Required(ErrorMessage = "ورود {0} اجباریست")]
        public int? ReqCenter { get; set; }
        /// <summary>
        /// شرح خطا
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// علت برگشت به انبار (در نگارش های جدید به هدر منتقل شده است)
        /// </summary>
        public byte? RtrnStrReason { get; set; }
        /// <summary>
        /// علت برگشت از خرید (در نگارشهای جدید به هدر منتقل شده است)
        /// </summary>
        public byte? RtrnBuyReason { get; set; }
        /// <summary>
        /// تاریخ سند مرجع
        /// </summary>
        public string RefDocdate { get; set; }
        /// <summary>
        /// سند مرجع دار است؟
        /// </summary>
        public bool? HasRefrence { get; set; }
    }


    public class invHdrDataBindingModel_offisial
    {
        public byte inventoryDocumentTypeId { get; set; }
        public string warehouseId { get; set; }
        public string documentDate { get; set; }
        public byte? referenceDocumentTypeId { get; set; }
        public int? referenceDocumentNo { get; set; }
        public string destinationWarehouseId { get; set; }
        public int? referenceBetweenDocumentNo { get; set; }
        public byte? returnToInventoryReasonId { get; set; }
        public bool? isSaleIssue { get; set; }
        public List<invDtlDataBindingModel_offisial> inventoryJournalItems { get; set; }
        public string outDocno { get; set; }
    }

}