declare @p11 bit
set @p11=0
exec ray.InvSp_ValidSoh 1396,'15','91112799991030',2.0000,0,'13960423','',0.0000,0,41,@p11 output
select @p11
---------
exec ray.InvSp_ExistInMaster 1396,'9','91112799991030',False
---------
select top 30000 
D.Fiscalyear,D.Storeno,D.doctype,D.docno,D.CountNo,D.StkCountNo,D.DocEntStatus,D.docrow,D.PartNo,i.PartNoDsc,i.TechnicalNo,'*',D.Binno,D.Center,c.CenterDsc,D.OrderNo,O.OrdrDsc,D.RcptType,R.RcptTypeDesc,D.ReqType,Q.ReqTypeDesc,D.ConsType,CnsTyp.ConsTypeDesc,D.Supplier,s.SupName,D.act3,actcnt.CenterDsc,D.AudtReason,audt.AudtReasonDsc,NULL,D.Serial,abs(qty),D.UntCode,u.UntName,NULL,'÷—Ì» 
 »œÌ·',NULL,NULL,NULL,abs(Cntqty),D.Weight,NULL,D.NeedDate,D.PrcDate,D.PrcSeqDoc,D.UpdTime,NULL,D.ReqCenter,OldRow,ISNULL(d.setvoid,0),abs(amt),'„»·€ „Õ«”»Â ‘œÂ',abs(compamt),NULL,NULL,D.docdate,D.RefOrder,D.RefConsQty,D.AudtQty,D.AudtAmt,D.IsNewYear,D.DestStoreNo,D.AccDocNo,NULL,'%',abs(TollAmt),NULL,'%',abs(TaxAmt),D.DocDsc,GdiRefDocNo,GdiRefDocRow,GdiRefPartNo,GdiRefStore,D.RegNo,D.OrgPelakNo,NULL,D.FlwCode,D.SaleQty,D.MeetingDate,NULL,NULL,D.DespositRng,NULL,NULL,NULL,D.act4,act4.CenterDsc,D.FactorNo,D.act5,D.FactorDate,act5.CenterDsc,D.RfDocNo,D.RfDocRow,ISNULL(d.LCNO,0) as LCNO,D.RtrnBuyReason,D.SetPrice,D.RtrnStrReason,D.SalePartNo,D.SaleRow,D.TreatyNo,i.MapNo,i.PartLtnDsc,D.FromPlaque,D.ToPlaque,NULL,NULL,NULL,NULL,NULL,NULL from ray.#InvDtldata_gdi_tmp5077841 D WITH (NOLOCK) left outer join ray.ItemData i WITH (NOLOCK) on i.PartNo = D.PartNo left outer join ray.Unit u WITH (NOLOCK) on u.UntCode = D.UntCode left outer join ray.InvOrdr o WITH (NOLOCK) on o.OrdrNO = D.OrderNo left outer join ray.Center c WITH (NOLOCK) on c.Center = D.Center left outer join ray.Center actcnt WITH (NOLOCK) on actcnt.Center = D.Act3 left outer join ray.InvCnsTyp CnsTyp WITH (NOLOCK) on CnsTyp.ConsType = D.ConsType left outer join ray.InvRcptTyp R WITH (NOLOCK) on R.RcptType = D.RcptType left outer join ray.InvReqTyp Q WITH (NOLOCK) on Q.ReqType = D.ReqType left outer join ray.Supplier s WITH (NOLOCK) on s.Supplier = D.Supplier left outer join ray.InvAudRsn audt WITH (NOLOCK) on audt.AudtReason = D.AudtReason left outer join ray.Center act4 WITH (NOLOCK) on act4.Center = D.Act4 left outer join ray.Center act5 WITH (NOLOCK) on act5.Center = D.Act5  WHERE (D.Fiscalyear = 1396 and D.Storeno = '9' and D.doctype = 40 and D.docno = 666)

---------
exec ray.INVsp_ArcSoh 1396,'9','91112799991030','13960423'
---------
exec ray.InvSp_FindPropertyFromItemData '91112799991030'
---------
exec ray.INVsp_ArcSoh 1396,'9','91112799991030','13960423'
---------
select binno from ray.invmstr with(nolock) where fiscalyear=1396and storeno='9' and partno='91112799991030'