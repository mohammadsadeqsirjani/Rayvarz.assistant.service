//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rayvarz.inv.assistant.service.Models.ray
{
    using System;
    using System.Collections.Generic;
    
    public partial class PartGrp
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PartGrp()
        {
            this.ItemDatas = new HashSet<ItemData>();
            this.PartGrp11 = new HashSet<PartGrp>();
        }
    
        public string PartGrp1 { get; set; }
        public string PartGrpDsc { get; set; }
        public string PartGrpLtnDsc { get; set; }
        public string PrntPartGrp { get; set; }
        public Nullable<byte> LotTyp { get; set; }
        public Nullable<decimal> OrdSize { get; set; }
        public Nullable<decimal> SftyStk { get; set; }
        public Nullable<short> LeadTime { get; set; }
        public Nullable<decimal> StkCst { get; set; }
        public Nullable<decimal> OrdCst { get; set; }
        public string StoreNo { get; set; }
        public string Rmrk { get; set; }
        public Nullable<byte> ConsAst { get; set; }
        public Nullable<byte> UnSaleableAst { get; set; }
        public Nullable<int> Clssify { get; set; }
        public string EqualCodingCod { get; set; }
        public byte[] RowVersion { get; set; }
        public System.Guid RowGuid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemData> ItemDatas { get; set; }
        public virtual Store Store { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartGrp> PartGrp11 { get; set; }
        public virtual PartGrp PartGrp2 { get; set; }
    }
}
