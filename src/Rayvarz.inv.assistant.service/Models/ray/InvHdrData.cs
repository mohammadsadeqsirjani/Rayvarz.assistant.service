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
    
    public partial class InvHdrData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvHdrData()
        {
            this.InvAddDocInfHdrs = new HashSet<InvAddDocInfHdr>();
            this.InvDtlDatas = new HashSet<InvDtlData>();
        }
    
        public int FiscalYear { get; set; }
        public string StoreNo { get; set; }
        public byte DocType { get; set; }
        public int DocNo { get; set; }
        public short CountNo { get; set; }
        public Nullable<int> DocSeq { get; set; }
        public string DestStoreNo { get; set; }
        public Nullable<short> StkCountNo { get; set; }
        public Nullable<byte> DocEntStatus { get; set; }
        public Nullable<byte> DocStatus { get; set; }
        public Nullable<short> UpdateSeq { get; set; }
        public Nullable<short> SendSeq { get; set; }
        public Nullable<short> ValueSeq { get; set; }
        public Nullable<short> AccDocSeq { get; set; }
        public Nullable<decimal> SumQty { get; set; }
        public Nullable<decimal> SumAmt { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public Nullable<int> MultyMediaDoc { get; set; }
        public string MultyMediaDocX { get; set; }
        public Nullable<int> OrgDocNo { get; set; }
        public Nullable<int> OrgRefNo { get; set; }
        public Nullable<byte> LockInd { get; set; }
        public Nullable<short> CnfrmLvl { get; set; }
        public Nullable<byte> EndRcpt { get; set; }
        public string RaySys { get; set; }
        public Nullable<byte> PriceDocSt { get; set; }
        public Nullable<int> DocCounter { get; set; }
        public Nullable<byte> IsSendAst { get; set; }
        public Nullable<int> GdiRefDocNo { get; set; }
        public Nullable<int> GdiRefFiscalYear { get; set; }
        public Nullable<byte> GdiRefDocType { get; set; }
        public Nullable<short> BuyType { get; set; }
        public Nullable<decimal> SumWeight { get; set; }
        public Nullable<byte> IsEditAcnt { get; set; }
        public string UserId { get; set; }
        public byte[] RowVersion { get; set; }
        public System.Guid RowGuid { get; set; }
    
        public virtual InvPrd InvPrd { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvAddDocInfHdr> InvAddDocInfHdrs { get; set; }
        public virtual InvDocSt InvDocSt { get; set; }
        public virtual InvDocTyp InvDocTyp { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvDtlData> InvDtlDatas { get; set; }
        public virtual UserId UserId1 { get; set; }
        public virtual Store Store { get; set; }
    }
}
