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
    
    public partial class InvRtrnStr
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvRtrnStr()
        {
            this.InvDocs = new HashSet<InvDoc>();
            this.InvDtlDatas = new HashSet<InvDtlData>();
        }
    
        public byte RtrnStore { get; set; }
        public string RtrnStoreDesc { get; set; }
        public byte[] RowVersion { get; set; }
        public System.Guid RowGuid { get; set; }
        public string RtrnStoreLtnDsc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvDoc> InvDocs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvDtlData> InvDtlDatas { get; set; }
    }
}