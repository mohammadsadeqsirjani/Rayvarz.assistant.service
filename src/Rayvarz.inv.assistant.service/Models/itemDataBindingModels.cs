using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rayvarz.inv.assistant.service.Models
{
    public class itemDataBindingModel
    {
        /// <summary>
        /// کد کالا
        /// </summary>
        [Display(Name = "کد کالا")]
        [Required(ErrorMessage = "ورود {0} اجباریست")]
        public string PartNo { get; set; }
        [Display(Name = "عنوان کالا")]
        [Required(ErrorMessage = "ورود {0} اجباریست")]
        public string PartNoDsc { get; set; }
        public byte? NotActiv { get; set; }
        [Display(Name = "واحد شمارش کالا")]
        [Required(ErrorMessage = "ورود {0} اجباریست")]
        public string UntCode { get; set; }
        [Display(Name = "گروه کالا")]
        //[Required(ErrorMessage = "ورود {0} اجباریست")]
        public string PartGrp { get; set; }
        /// <summary>
        /// کد فنی کالا
        /// </summary>
        public string TechnicalNo { get; set; }
        /// <summary>
        /// شناسه ملی کالا
        /// </summary>
        public string NationalPartCod { get; set; }
        /// <summary>
        /// محصول؟
        /// </summary>
        public byte? Salable { get; set; }
        /// <summary>
        /// قطعه؟
        /// </summary>
        public byte? IsMchnPart { get; set; }
        /// <summary>
        /// ابزار؟
        /// </summary>
        public byte? IsTools { get; set; }
        /// <summary>
        /// دارایی ثابت؟
        /// </summary>
        public byte? IsAsset { get; set; }
        /// <summary>
        /// قالب؟
        /// </summary>
        public byte? IsMold { get; set; }
        /// <summary>
        /// مواد؟
        /// </summary>
        public byte? IsMtrl { get; set; }
        /// <summary>
        /// ملزومات؟
        /// </summary>
        public byte? ISOfsTool { get; set; }
        /// <summary>
        /// لوازم مصرفی؟
        /// </summary>
        public byte? IsConsTool { get; set; }
        /// <summary>
        /// نیم ساخته؟
        /// </summary>
        public byte? IsMakePart { get; set; }
        /// <summary>
        /// فروشگاه؟
        /// </summary>
        public byte? IsShop { get; set; }
        /// <summary>
        /// بسته بندی؟
        /// </summary>
        public byte? IsPack { get; set; }
        /// <summary>
        /// لوازم آزمایشگاهی؟
        /// </summary>
        public byte? IsLabTools { get; set; }
        /// <summary>
        /// کالای داغی دار؟
        /// </summary>
        public byte? IsExpired { get; set; }
        /// <summary>
        /// درخواست کالای استاندارد؟
        /// </summary>
        public byte? IsPrdPart { get; set; }
        /// <summary>
        /// شرح لاتین
        /// </summary>
        public string PartLtnDsc { get; set; }

    }
    public class getLastAvailablePartNoOfGrpBindingModel
    {
        [Display(Name = "گروه کالا")]
        [Required(ErrorMessage = "ورود {0} اجباریست")]
        public string PartGrp { get; set; }
    }

    //public class sohInfoModel
    //{
    //    public string PartNo { get; set; }
    //    public decimal IncomingSoh { get; set; }
    //    public decimal ReserveSoh { get; set; }

    //    public decimal OrderPoint { get; set; }
    //}

    public class IncomingSohModel
    {
        public string PartNo { get; set; }
        public decimal IncomingSoh { get; set; }
    }
    public class ReserveSohModel
    {
        public string PartNo { get; set; }
        public decimal ReserveSoh { get; set; }
    }
    
}