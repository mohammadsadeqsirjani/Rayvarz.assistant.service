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
    
    public partial class InvPrd
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvPrd()
        {
            this.InvDtlData = new HashSet<InvDtlData>();
            this.InvHdrData = new HashSet<InvHdrData>();
            this.InvMstr = new HashSet<InvMstr>();
            this.InvRfDoc = new HashSet<InvRfDoc>();
        }
    
        public int FiscalYear { get; set; }
        public string StartFiscalYear { get; set; }
        public string EndFiscalYear { get; set; }
        public Nullable<byte> FiscalStatus { get; set; }
        public Nullable<byte> IsMoveAMT { get; set; }
        public Nullable<byte> IsMoveRemainQty { get; set; }
        public Nullable<byte> IsSixteenMonth { get; set; }
        public string EndSixteenMonthYear { get; set; }
        public Nullable<byte> IsMovePermanently { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvDtlData> InvDtlData { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvHdrData> InvHdrData { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvMstr> InvMstr { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvRfDoc> InvRfDoc { get; set; }
    }
}
