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
    
    public partial class InvUserLvl
    {
        public string UserId { get; set; }
        public string StoreNo { get; set; }
        public byte DocType { get; set; }
        public Nullable<byte> InvCnfrmLvl { get; set; }
        public Nullable<byte> AutoDocNo { get; set; }
        public Nullable<byte> CnfrmSeri { get; set; }
        public byte[] RowVersion { get; set; }
        public System.Guid RowGuid { get; set; }
    
        public virtual InvDocTyp InvDocTyp { get; set; }
        public virtual Store Store { get; set; }
        public virtual UserId UserId1 { get; set; }
    }
}
