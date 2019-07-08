CREATE PROCEDURE [ray].[InvSp_SelectDocReferDtl] 
 (
 @FiscalYear as int
 ,@StoreNo as varchar(6)
 ,@DocType as tinyint
 ,@Docno as int
 ,@RefDocType  as tinyint
 ,@IsExistsItmCircle as bit
) 
with Encryption
as
set nocount on
declare @Type as Tinyint
if (@RefDocType>=60 and @RefDocType<80 ) and  (@DocType>=60 and @DocType<80 )
  set @Type=1
else
 begin
      --req72549 : برگشت رسيد موقت در رسيد موقت مرجع رسيد خريد و رسيد كالا موثر باشد
     if (@DocType>=66 ) and  (@RefDocType=14 or @RefDocType=12 )
               set  @Type=4
     else  if (@DocType>=60 and @DocType<80 ) and  (@RefDocType=14 or @RefDocType=40 or @RefDocType=46 or @RefDocType=18 )
               set  @Type=2   
      else
               set @Type= 3
 end
 Declare  @StandardReqRange as int
  select @StandardReqRange=isnull(InfVal,0) from ray.Raysysspc with(nolock) where  RaySys='Inv' and InfTit='StReqReng' 
--'req106005 :فعاليت3 از مرجع 
--'REQ106123 : در ثبت اسناد براساس مرجع
--'، مانده قابل مصرف مرجع شامل اسناد سفارشات و اسناد اصلي باشد

--Req112455 : در صورتيكه سيكل گردش اسناد در تنظيمات سيستم تعريف شده باشد ، سيستم اجازه دهد
--'كه يك سند از نوع سفارشات بتواند هم مرجع سند سفارش ديگري قرار بگيرد و هم مرجع سند اصلي (رسيد-حواله)
--'ولي اگر سيكل گردش اسناد تعريف نشده باشد ، سيستم كنترل مي كند كه يك سند از نوع سفارشات فقط يكبار مرجع شود
if @IsExistsItmCircle=0

					select  0 as Selected,0 as IsSelected,i.docrow,i.partno,itemdata.partnodsc,i.center,center.centerdsc,qty= case when (DocType.catgtype=2  or DocType.catgtype=6  or DocType.catgtype=4) then -(i.qty) 
							else i.qty end  ,ConsRemainQty=case  @Type when 1 then isnull(i.RefOrder,0)+isnull(i.RefConsQty,0)   --در سفارش به سفارش مقدار سفارش ده ومقدار ارجاع داده شده
							when 2 then  isnull(i.RefConsQty,0)+ isnull(i.RefOrder,0)  --در اسنادديگر تنها مقدار ارجاع داده شده
							when 4 then isnull(i.RefOrder,0)+isnull(i.RefConsQty,0) 
							else   isnull(i.RefConsQty,0)  end      --تعديلات                     
							, RemainQty=case  @Type when 1 then isnull(abs(i.qty),0)-isnull(i.RefOrder,0)-isnull(i.RefConsQty,0)
							when 2 then  isnull(abs(i.qty),0)-isnull(i.RefConsQty,0) -isnull(i.RefOrder,0)
							when 4 then isnull(abs(i.qty),0)-isnull(i.RefOrder,0)-isnull(i.RefConsQty,0)
							else   isnull(abs(i.qty),0)+isnull(i.AudtQty,0)-isnull(i.RefConsQty,0)  end --تعديلات
							, RemainQtycopy=case  @Type when 1 then isnull(abs(i.qty),0)-isnull(i.RefOrder,0)-isnull(i.RefConsQty,0) 
							when 2 then  isnull(i.qty,0)-isnull(i.RefConsQty,0) -isnull(i.RefOrder,0)
							when 4 then isnull(abs(i.qty),0)-isnull(i.RefOrder,0)-isnull(i.RefConsQty,0) 
							else   isnull(abs(i.qty),0)+isnull(i.AudtQty,0)-isnull(i.RefConsQty,0)  end  --تعديلات
							,isnull(i.RtnQty,0)-isnull(i.RefConsQty,0) as RtnQty  -- ارتباط با فروش
                            ,0 as RemainQtyKol,0 as RemainQtyJoz,isnull(itemData.ExchangeTtlUntToUnt,0) as ExchangeUnit  --دو واحده
							,unit.untname,i.untcode,ord.PartNo as PartNoRef,i.serial,i.RcptType,i.ConsType,i.OrderNo,i.Amt,i.Supplier,supplier.SupName as SupName,isnull(ord.ordrDsc,'') as ordrDsc,isnull(RcptType.RcptTypeDesc,'') as RcptTypeDesc
							,isnull(CnsTyp.ConsTypeDesc,'') as ConsTypeDesc ,cast(updtime as int) as updtime,CompAmt,weight,isnull(SalePartNo,'') as SalePartNo,isnull(SaleQty,0) as SaleQty,isnull(SaleRow,0) as SaleRow,isnull(ord.Stqty,0) as Stqty
							,i.Binno,i.act3 ,isnull(i.taxamt,0) as taxamt,isnull(i.tollamt,0) as tollamt,isnull(i.NeedDate,'') as NeedDate,isnull(i.TreatyNo,0) as TreatyNo,i.docdsc,i.FactorNo,i.FactorDate,i.act4,i.act5
							,i.AccDocNo as AccDocNo,i.docno,@StandardReqRange as streng
					FROM         ray.InvDtlData i WITH (nolock)  INNER JOIN
							ray.ItemData itemData WITH (nolock)  ON i.PartNo = itemData.PartNo LEFT OUTER JOIN
							ray.InvCnsTyp CnsTyp WITH (nolock)  ON i.ConsType = CnsTyp.ConsType LEFT OUTER JOIN
							ray.InvRcptTyp RcptType ON i.RcptType = RcptType.RcptType LEFT OUTER JOIN
							ray.Supplier supplier WITH (nolock)  ON i.Supplier = supplier.Supplier LEFT OUTER JOIN
							ray.InvDocTyp DocType  WITH (nolock)  ON i.DocType = DocType.DocType LEFT OUTER JOIN
							ray.Unit unit WITH (nolock)  ON i.UntCode = unit.UntCode LEFT OUTER JOIN
							ray.InvOrdr ord WITH (nolock)  ON i.OrderNo = ord.OrdrNO LEFT OUTER JOIN
							ray.Center  center WITH (nolock)  ON i.Center = center.Center
					where  i.fiscalyear=@FiscalYear  and i.storeno=@Storeno  and i.docno=@DocNo  and i.doctype=@DocType  and (i.SetVoid is null or i.SetVoid=0)
                    order by i.docrow

else if @IsExistsItmCircle=1

					select  0 as Selected,0 as IsSelected,i.docrow,i.partno,itemdata.partnodsc,i.center,center.centerdsc,qty= case when (DocType.catgtype=2  or DocType.catgtype=6 or DocType.catgtype=4) then -(i.qty) 
							else i.qty end  ,ConsRemainQty=case  @Type when 1 then isnull(i.RefOrder,0)+isnull(i.RefConsQty,0)   --در سفارش به سفارش مقدار سفارش ده ومقدار ارجاع داده شده
							when 2 then  isnull(i.RefConsQty,0)  --در اسنادديگر تنها مقدار ارجاع داده شده
							when 4 then isnull(i.RefOrder,0)+isnull(i.RefConsQty,0) 
							else   isnull(i.RefConsQty,0)  end                           
							, RemainQty=case  @Type when 1 then isnull(abs(i.qty),0)-isnull(i.RefOrder,0)-isnull(i.RefConsQty,0)
							when 2 then  isnull(abs(i.qty),0)-isnull(i.RefConsQty,0) 
							when 4 then isnull(abs(i.qty),0)-isnull(i.RefOrder,0)-isnull(i.RefConsQty,0)
							else   isnull(abs(i.qty),0)+isnull(i.AudtQty,0)-isnull(i.RefConsQty,0)  end
							, RemainQtycopy=case  @Type when 1 then isnull(abs(i.qty),0)-isnull(i.RefOrder,0)-isnull(i.RefConsQty,0) 
							when 2 then  isnull(i.qty,0)-isnull(i.RefConsQty,0) 
							when 4 then isnull(abs(i.qty),0)-isnull(i.RefOrder,0)-isnull(i.RefConsQty,0) 
							else   isnull(abs(i.qty),0)+isnull(i.AudtQty,0)-isnull(i.RefConsQty,0)  end
                            ,isnull(i.RtnQty,0)-isnull(i.RefConsQty,0) as RtnQty
                            ,0 as RemainQtyKol,0 as RemainQtyJoz,isnull(itemData.ExchangeTtlUntToUnt,0) as ExchangeUnit  --دو واحده
							,unit.untname,i.untcode,ord.PartNo as PartNoRef,i.serial,i.RcptType,i.ConsType,i.OrderNo,i.Amt,i.Supplier,supplier.SupName as SupName,isnull(ord.ordrDsc,'') as ordrDsc,isnull(RcptType.RcptTypeDesc,'') as RcptTypeDesc
							,isnull(CnsTyp.ConsTypeDesc,'') as ConsTypeDesc ,cast(updtime as int) as updtime,CompAmt,weight,isnull(SalePartNo,'') as SalePartNo,isnull(SaleQty,0) as SaleQty,isnull(SaleRow,0) as SaleRow,isnull(ord.Stqty,0) as Stqty
							,i.Binno,i.act3 ,isnull(i.taxamt,0) as taxamt,isnull(i.tollamt,0) as tollamt,isnull(i.NeedDate,'') as NeedDate,isnull(i.TreatyNo,0) as TreatyNo,i.docdsc,i.FactorNo,i.FactorDate,i.act4,i.act5
							,i.AccDocNo as AccDocNo,i.docno,@StandardReqRange as streng
					FROM         ray.InvDtlData i WITH (nolock)  INNER JOIN
							ray.ItemData itemData WITH (nolock)  ON i.PartNo = itemData.PartNo LEFT OUTER JOIN
							ray.InvCnsTyp CnsTyp WITH (nolock)  ON i.ConsType = CnsTyp.ConsType LEFT OUTER JOIN
							ray.InvRcptTyp RcptType ON i.RcptType = RcptType.RcptType LEFT OUTER JOIN
							ray.Supplier supplier WITH (nolock)  ON i.Supplier = supplier.Supplier LEFT OUTER JOIN
							ray.InvDocTyp DocType  WITH (nolock)  ON i.DocType = DocType.DocType LEFT OUTER JOIN
							ray.Unit unit WITH (nolock)  ON i.UntCode = unit.UntCode LEFT OUTER JOIN
							ray.InvOrdr ord WITH (nolock)  ON i.OrderNo = ord.OrdrNO LEFT OUTER JOIN
							ray.Center  center WITH (nolock)  ON i.Center = center.Center
					where  i.fiscalyear=@FiscalYear  and i.storeno=@Storeno  and i.docno=@DocNo  and i.doctype=@DocType  and (i.SetVoid is null or i.SetVoid=0)
                     order by i.docrow
set nocount off