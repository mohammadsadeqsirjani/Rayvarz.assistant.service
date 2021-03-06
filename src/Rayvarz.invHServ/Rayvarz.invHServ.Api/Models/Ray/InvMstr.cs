//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rayvarz.invHServ.Api.Models.Ray
{
    using System;
    using System.Collections.Generic;
    
    public partial class InvMstr
    {
        public int FiscalYear { get; set; }
        public string StoreNo { get; set; }
        public string PartNo { get; set; }
        public string BinNo { get; set; }
        public Nullable<decimal> Soh { get; set; }
        public Nullable<decimal> OpnSoh { get; set; }
        public Nullable<decimal> NoopnSoh { get; set; }
        public Nullable<decimal> OpnValue { get; set; }
        public string UntCode { get; set; }
        public Nullable<decimal> AvgRate { get; set; }
        public Nullable<decimal> LastRcptRate { get; set; }
        public Nullable<decimal> TotIssue { get; set; }
        public string LastIssueDate { get; set; }
        public Nullable<decimal> TotRecipt { get; set; }
        public string LastReciptDate { get; set; }
        public string UpdateDate { get; set; }
        public Nullable<int> TagNo { get; set; }
        public Nullable<decimal> StandardRate { get; set; }
        public Nullable<decimal> OrderPoint { get; set; }
    
        public virtual ItemData ItemData { get; set; }
        public virtual InvPrd InvPrd { get; set; }
        public virtual Store Store { get; set; }
    }
}
