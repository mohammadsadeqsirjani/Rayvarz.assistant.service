CREATE PROCEDURE [ray].[InvAssistantSp_DeliveryItems_AddUpdate]
	@userId AS VARCHAR(50),
	@branch AS INT,
	@storeNo AS VARCHAR(6),
	@clientIp as varchar(50),


	@refDocFiscalYear as int,
	@refDocType as tinyint,
	@refDocNo as int,
	@refDocRow as smallint,
	--@Barcode as varchar(max),

	@deliveryItems as [ray].[InvAssistantUdt_deliveryItems] readonly,
	@ignoreWarning as bit = 0,


			@orderBy as varchar (50) = null,
	@isDescOrder as bit = 1,

	@key AS VARCHAR(100),
	@fromIndex AS INT,
	@take as int,
	@hasError as bit out,
	@hasWarnning as bit out,

	@rcode as tinyint out,
	@rmsg as varchar(max) out

	
AS

set @hasWarnning = 0;
set @hasError = 0;
set @rmsg = 'اطلاعات با موفقیت ذخیره شد';
declare @deliveryItems_output as [ray].[InvAssistantUdt_deliveryItems];
insert into @deliveryItems_output (barcode,qty) select barcode,qty from @deliveryItems;

declare @refDocPartNo as varchar(20),@refDocQty as money;--,@refDocFiscalYear as int,@FiscalYear
declare @deliveryItemsSumQty as money;

select @refDocPartNo = id.partno ,@refDocQty = id.Qty
from ray.InvDtlData as id where id.FiscalYear = @refDocFiscalYear and id.StoreNo = @storeNo and
id.DocType = @refDocType and id.DocNo = @refDocNo and id.DocRow = @refDocRow;

select @deliveryItemsSumQty = sum(its.qty) from @deliveryItems as its;




declare @sumQty_ReciptDoc as money,
@SumQty_Barcode as money;








if(@deliveryItemsSumQty > (@refDocQty * -1))
begin
set @rmsg = 'مجموع مقادیر اسکن شده از مقدار ردیف سند بیشتر است.';
set @hasError = 1;
goto success;
end

declare @crntBarcodeStructureId as bigint;
set @crntBarcodeStructureId = [ray].[InvAssistantFUNC_getCurrentBarcodeStructureId]();

if(@deliveryItemsSumQty < (@refDocQty * -1))
begin
	if([ray].[InvAssistantFUNC_userHasMaxInvCnfrmLvl](@userId) = 0)
	begin--error
	set @rmsg = 'مجموع مقدارهای اسکن شده از مقدار ردیف سند کمتر است.';
	set @hasError = 1;
	goto success;
	end
	else if(@ignoreWarning = 0)
	begin
	set @rmsg = 'مجموع مقدارهای اسکن شده از مقدار ردیف سند کمتر می باشد. آیا اطمینان دارید؟.';
	set @hasWarnning = 1;
	goto success;
	end
end


declare @barcodeStructureIncludesPartno as bit;--TODO?





if(exists(select 1 from ray.InvAssistant_barcodeStructure_dtl as bsd where bsd.fk_barcodeStructureHdr_id = @crntBarcodeStructureId
and bsd.fieldName = 'partno'))
set @barcodeStructureIncludesPartno = 1;
else set @barcodeStructureIncludesPartno = 0;


update @deliveryItems_output set errorMessage =  'بارکد غیر مجاز است' where @refDocPartNo <>  ray.InvAssistantFUNC_exportFromBarcode(barcode,@crntBarcodeStructureId,'partno');
if(@@ROWCOUNT > 0)
begin
set @hasError = 1;
set @rmsg = 'برخی از اقلام شامل خطا می باشند.';
goto success;
--goto invalid;
end


--@refDocPartNo <> ray.InvAssistantFUNC_exportFromBarcode(@Barcode,@crntBarcodeStructureId,'partno')
--begin
--set @rmsg = 'بارکد غیر مجاز است';
--goto fail;
--end





begin try
begin tran t

delete from ray.InvAssistant_deliveryItems 
where FiscalYear = @refDocFiscalYear and StoreNo = @storeNo and DocType = @refDocType and DocNo = @refDocNo and DocRow = @refDocRow;

insert into ray.InvAssistant_deliveryItems
select @refDocFiscalYear,@storeNo,@refDocType,@refDocNo,@refDocRow,its.barcode,its.qty,@crntBarcodeStructureId from @deliveryItems as its;
commit tran t;
end try
begin catch
rollback tran t;
set @rmsg = ERROR_MESSAGE();
goto unexpectedFail;
end catch












success:
set @rcode = 1;

select * from @deliveryItems_output; 
RETURN 0;
--hasWarning:
--set @rcode = 2;
--select * from @deliveryItems_output;
--RETURN 0;
--invalid:
--set @rcode = 3;
--select * from @deliveryItems_output;
--RETURN 0;
fail:
set @rcode = 0;
--select * from @deliveryItems_output;
RETURN 0;
unexpectedFail:
set @rcode = 2;
--select * from @deliveryItems_output;
RETURN 0;