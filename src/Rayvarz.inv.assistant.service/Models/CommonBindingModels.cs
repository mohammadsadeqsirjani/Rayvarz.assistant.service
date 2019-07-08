using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rayvarz.inv.assistant.service.Models
{
    public class DocumentDefaults
    {
        public string fieldName { get; set; }
        public string fieldDsc { get; set; }
        public fieldStatus status { get; set; }
        public keyValuePair defaults { get; set; }

    }
    public enum fieldStatus
    {
        enable = 1, disable, invisible, required
    }
    public class keyValuePair
    {
        public string id { get; set; }
        public string dsc { get; set; }
    }
    public class keyValuePair<T>
    {
        public T id { get; set; }
        public string dsc { get; set; }
    }

    public class getDefaults_DtlBindingModel
    {
        public byte docType { get; set; }
        public string partNo { get; set; }
    }


    public class getItemInfoBindingModel
    {
        /// <summary>
        /// کد کالا
        /// </summary>
        [Display(Name = "کد کالا")]
        [Required(ErrorMessage = "ورود {0} اجباریست")]
        public string partCode { get; set; }
        public string docNo { get; set; }
        /// <summary>
        /// نوع سند مرجع
        /// </summary>
        public byte? refDoctype { get; set; }
        /// <summary>
        /// شماره سند مرجع
        /// </summary>
        public string refDocNo { get; set; }
        public int? refFiscalYear { get; set; }

    }
    public class ArcSohModelBindingModel
    {
        public string PartNo { get; set; }
        public int count { get; set; }
    }
}