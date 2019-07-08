CREATE FUNCTION [ray].[InvAssistantTVFUNC_initScanDialog_serial_validations]
(
	@userId AS VARCHAR(50),
	@branch AS INT,
	@storeNo AS VARCHAR(6),
	@clientIp as varchar(50),

	@docType as tinyint,
	@partNo as varchar(20),
	@serial as varchar(50),

	--abfa
	--@validateSerialWithOrderNo as bit = 0,
	@refDocType as tinyint = null,
	@refDocNo as int = null,
	@refDocRow as int = null
)
RETURNS @returntable TABLE
(
	isValid bit,
	validationMsg varchar(150),
	soh money,
	isNew bit,
	hasWarning bit,


	PakgTypeDesc varchar(50),
	SupplierIdDsc varchar(300),
	ExpDate varchar(8),
	serial varchar(50),


	StoreSerialType tinyint,
	defaultQty money

)
AS
BEGIN


declare @isValid as bit,
	@validationMsg as varchar(150) ,
	@soh as money ,
	@isNew as bit,
	@hasWarning as bit = 0;


	--soh , isValid, validationMsg,weight,serial
--declare @infoList as [ray].[InvAssistantUdt_idDsc];
--declare @controls as [dbo].[InvAssistantUdt_uiControlList];
--declare @isValid as bit,@validationMsg as varchar(150);
set @isValid = 1;
set @hasWarning = 0;

declare @defaultQty as money,@ReceiptDocDefaultQtyAdjId as varchar(max)
set @defaultQty = 1;




if(@docType in (12,14,66))-- and [ray].[InvAssistantFUNC_getRaySysSpcInfVal]('ReceiptDocDefaultQtyAdjId') is not null)
begin
	set @ReceiptDocDefaultQtyAdjId = [ray].[InvAssistantFUNC_getRaySysSpcInfVal]('ReceiptDocDefaultQtyAdjId');
	if(@ReceiptDocDefaultQtyAdjId is not null and TRY_CAST(@ReceiptDocDefaultQtyAdjId as smallint) is not null)
	begin
		set @defaultQty = isnull([ray].[InvAssistantFUNC_getPartAdjectiveQty](@partNo,cast(@ReceiptDocDefaultQtyAdjId as smallint)),1);
	end
end


declare @StoreSerialType as tinyint;
select @StoreSerialType = isnull(SerialTyp,1) from ray.Store where StoreNo = @storeNo;

declare @SupplierIdDsc as varchar(300),@PakgTypeDesc as varchar(50),@ExpDate as varchar(8),@_Serail as varchar(50);

select @_Serail = ss.Serail, @soh = isnull(ss.Soh,0),
@SupplierIdDsc = isnull(ss.Supplier,'') + '-' +isnull(ss.SupName,''),@PakgTypeDesc = ss.PakgTypeDesc,@ExpDate = ss.ExpDate
from ray.InvAssistantTVFUNC_SerialSoh_GetList(@storeNo,@partNo,@serial) as ss


set @isNew = case when @_Serail is null then 1 else 0 end;

----if(@_Serail is not null)
----begin
--insert into @infoList select 'سریال',isnull(@_Serail,@serial);
--insert into @infoList select 'موجودی',isnull(@soh,0);
--insert into @infoList select 'تامین کننده',isnull(@SupplierIdDsc,'-');
--insert into @infoList select 'نوع بسته بندی',isnull(@PakgTypeDesc,'-');
--insert into @infoList select 'تاریخ انقضاء',isnull(@ExpDate,'-');
----end

if(@StoreSerialType = 2 and @soh is not null and @docType in (12,14,18))
begin
set @isValid = 0;
set @validationMsg = 'شماره سریال وارده قبلا وارد انبار شده است.';
end


if(@docType not in (12,14,66) and @_Serail is null)
begin
set @isValid = 0;
set @validationMsg = 'شماره سریال وارد شده در انبار موجود نمی باشد';
end

--برای آبفا. اگر حواله بود و کنترل سریال باید انجام شود از تنظیمات سیستم
if( @docType = 40 and lower(isnull([ray].[InvAssistantFUNC_getRaySysSpcInfVal]('validateSerialWithOrderNo'),'0')) in ('1','true'))
begin
	if(@serial like '%999999')
	begin
	set @hasWarning = 1;
	set @validationMsg = 'سریال وارد شده مربوط به اقلام شارژ انباری می باشد. آیا اطمینان دارید؟';
	end
	else
	begin


			declare @refDocOrderNo as int;
			select @refDocOrderNo = rd.OrderNo from ray.InvDtlData as rd where rd.DocType = @refDocType and rd.DocNo = @refDocNo and rd.DocRow = @refDocRow;
			if(@refDocOrderNo is null)
			begin
			--return error
			   set @isValid = 0;
			   set @validationMsg = 'خطا : اطلاعات ردیف مرجع سفارش یافت نشد!';
			end

			if(cast(@refDocOrderNo as varchar) <> @serial)
			begin
				if([ray].[InvAssistantFUNC_userHasMaxInvCnfrmLvl](@userId) = 1)
				begin--warning
				set @hasWarning = 1;
				set @validationMsg = 'سریال وارد شده با توجه به اطلاعات قرارداد درخواست کالا غیر مجاز می باشد. آیا اطمینان دارید؟';
				end
				else
				begin --error
					set @isValid = 0;
					set @validationMsg = 'سریال وارد شده با توجه به اطلاعات سفارش سند مرجع غیر مجاز می باشد.لطفا از سریال های مجاز استفاده نمایید.';
				end
			end

	end
end











--case when @StoreSerialType = 2 and @docType in (40,22) then 2 else 1 end





--select id,dsc from @infoList;
--select id ,title,[state],defaultValue_id ,defaultValue_dsc from @controls;
----select @isValid as isValid,@validationMsg as validationMsg;






	INSERT @returntable
	SELECT @isValid,@validationMsg,@soh,@isNew,@hasWarning,@PakgTypeDesc,@SupplierIdDsc
	,@ExpDate,isnull(@_Serail,@serial),@StoreSerialType,@defaultQty
   



	RETURN
END
