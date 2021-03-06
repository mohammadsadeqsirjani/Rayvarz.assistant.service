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
    
    public partial class UserId
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserId()
        {
            this.InvHdrData = new HashSet<InvHdrData>();
            this.UserDomain = new HashSet<UserDomain>();
        }
    
        public string UserId1 { get; set; }
        public string UserName { get; set; }
        public string UserLtnName { get; set; }
        public string PassWord { get; set; }
        public string NetUser { get; set; }
        public string Email { get; set; }
        public Nullable<byte> Active { get; set; }
        public string PWSettingDat { get; set; }
        public Nullable<byte> BuyGrp { get; set; }
        public Nullable<byte> BuyFlwGrp { get; set; }
        public string BuyGrpMng { get; set; }
        public string BuyFlwGrpMng { get; set; }
        public Nullable<byte> BuyCstmPrs { get; set; }
        public Nullable<byte> BuySrcSup { get; set; }
        public Nullable<byte> AcntLung { get; set; }
        public Nullable<byte> AcntLvl { get; set; }
        public Nullable<byte> AcntEdtTyp { get; set; }
        public Nullable<byte> AcntDspFrSeri1 { get; set; }
        public Nullable<byte> AcntDspFrSeri2 { get; set; }
        public Nullable<byte> AcntDspFrSeri3 { get; set; }
        public Nullable<byte> AcntDspToSeri1 { get; set; }
        public Nullable<byte> AcntDspToSeri2 { get; set; }
        public Nullable<byte> AcntDspToSeri3 { get; set; }
        public Nullable<byte> AcntAddFrSeri1 { get; set; }
        public Nullable<byte> AcntAddFrSeri2 { get; set; }
        public Nullable<byte> AcntAddFrSeri3 { get; set; }
        public Nullable<byte> AcntAddToSeri1 { get; set; }
        public Nullable<byte> AcntAddToSeri2 { get; set; }
        public Nullable<byte> AcntAddToSeri3 { get; set; }
        public Nullable<byte> AcntChgFrSeri1 { get; set; }
        public Nullable<byte> AcntChgFrSeri2 { get; set; }
        public Nullable<byte> AcntChgFrSeri3 { get; set; }
        public Nullable<byte> AcntChgToSeri1 { get; set; }
        public Nullable<byte> AcntChgToSeri2 { get; set; }
        public Nullable<byte> AcntChgToSeri3 { get; set; }
        public Nullable<byte> AcntDltFrSeri1 { get; set; }
        public Nullable<byte> AcntDltFrSeri2 { get; set; }
        public Nullable<byte> AcntDltFrSeri3 { get; set; }
        public Nullable<byte> AcntDltToSeri1 { get; set; }
        public Nullable<byte> AcntDltToSeri2 { get; set; }
        public Nullable<byte> AcntDltToSeri3 { get; set; }
        public Nullable<byte> AcntCnfFrSeri1 { get; set; }
        public Nullable<byte> AcntCnfFrSeri2 { get; set; }
        public Nullable<byte> AcntCnfFrSeri3 { get; set; }
        public Nullable<byte> AcntCnfToSeri1 { get; set; }
        public Nullable<byte> AcntCnfToSeri2 { get; set; }
        public Nullable<byte> AcntCnfToSeri3 { get; set; }
        public Nullable<byte> CashLvl { get; set; }
        public Nullable<short> OaUnitCod { get; set; }
        public Nullable<byte> OaIsResponsible { get; set; }
        public string InvStore { get; set; }
        public Nullable<byte> InvCnfrmLvl { get; set; }
        public Nullable<byte> InvSystemType { get; set; }
        public Nullable<byte> InvImplType { get; set; }
        public Nullable<byte> PayHokmLvl { get; set; }
        public Nullable<byte> PayEblagLvl { get; set; }
        public Nullable<byte> PayExtraLvl { get; set; }
        public Nullable<byte> PayVarLvl { get; set; }
        public Nullable<byte> PayExlLvl { get; set; }
        public Nullable<byte> PayOutLvl { get; set; }
        public Nullable<byte> PayLoanLvl { get; set; }
        public Nullable<byte> PayLoanReqLvl { get; set; }
        public Nullable<byte> PayCostLvl { get; set; }
        public Nullable<byte> PayMissionLvl { get; set; }
        public Nullable<byte> PayEvalLvl { get; set; }
        public Nullable<byte> PayWrkTimLvl { get; set; }
        public Nullable<byte> PayDayoffReqLvl { get; set; }
        public string ShopStore { get; set; }
        public Nullable<byte> ShopSystemType { get; set; }
        public Nullable<byte> ShopCnfrmLvl { get; set; }
        public string ShopStoreCash { get; set; }
        public string SaleStore { get; set; }
        public Nullable<System.DateTime> AstUpdOpt { get; set; }
        public Nullable<System.DateTime> AstUpdCard { get; set; }
        public Nullable<byte> AstUsrTyp { get; set; }
        public string ActnCenter { get; set; }
        public string EmpTypPermission { get; set; }
        public string SaleSerial { get; set; }
        public Nullable<bool> SaleDocHeader { get; set; }
        public Nullable<bool> SaleDocDateHdrPermit { get; set; }
        public Nullable<int> AstTrustGrp { get; set; }
        public string SaleCstmrGrp { get; set; }
        public string SaleSaleType { get; set; }
        public Nullable<bool> CrncyDailyPrcHdrPermit { get; set; }
        public Nullable<byte> FirstLogin { get; set; }
        public string MstrUserId { get; set; }
        public System.Guid RowGUID { get; set; }
        public byte[] RowVersion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvHdrData> InvHdrData { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserDomain> UserDomain { get; set; }
    }
}
