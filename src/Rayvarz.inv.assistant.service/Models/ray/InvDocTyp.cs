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
    
    public partial class InvDocTyp
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvDocTyp()
        {
            this.InvAddDocInfs = new HashSet<InvAddDocInf>();
            this.InvAddDocInfHdrs = new HashSet<InvAddDocInfHdr>();
            this.InvDocs = new HashSet<InvDoc>();
            this.InvHdrDatas = new HashSet<InvHdrData>();
            this.InvSeris = new HashSet<InvSeri>();
            this.InvUserLvls = new HashSet<InvUserLvl>();
            this.InvDtlDatas = new HashSet<InvDtlData>();
            this.InvRfDocs = new HashSet<InvRfDoc>();
        }
    
        public byte DocType { get; set; }
        public string DocTypeDesc { get; set; }
        public string DescShop { get; set; }
        public short SeqDocType { get; set; }
        public Nullable<byte> CatgType { get; set; }
        public Nullable<byte> IsDocOpn { get; set; }
        public Nullable<byte> OpnRcptPriceTyp { get; set; }
        public string VbFormName { get; set; }
        public string VbDescFrmName { get; set; }
        public Nullable<byte> AudtDocType { get; set; }
        public Nullable<byte> IsAuditDoc { get; set; }
        public Nullable<short> TabNo { get; set; }
        public Nullable<byte> RefDoc { get; set; }
        public Nullable<byte> RefDocNo { get; set; }
        public Nullable<byte> MoveDoc { get; set; }
        public Nullable<byte> SerailAcc { get; set; }
        public Nullable<byte> SortInEdGrp { get; set; }
        public Nullable<byte> IsShop { get; set; }
        public byte[] RowVersion { get; set; }
        public System.Guid RowGuid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvAddDocInf> InvAddDocInfs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvAddDocInfHdr> InvAddDocInfHdrs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvDoc> InvDocs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvHdrData> InvHdrDatas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvSeri> InvSeris { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvUserLvl> InvUserLvls { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvDtlData> InvDtlDatas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvRfDoc> InvRfDocs { get; set; }
    }
}
