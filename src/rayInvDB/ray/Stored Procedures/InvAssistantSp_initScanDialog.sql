--بارکد و نوع سند را از ورودی دریافت می کند
--و اطلاعات و کنترل های دایالوگ را با توجه به این دو فیلد ورودی مقدار دهی می کند

		--در صورتی که در تنظیمات سیستم برای بارکد ساختار تعریف شده بود
		--کد کالا از آن استخراج می شود و در غیر این صورت بارکد ورودی به عنوان کد کالا در نظر گرفته می شود
		--در صورتی که در تنظیمات سیستم برای بارکد ساختار تعریف شده بود
		--و در آن ساختار سریال نیز تعریف شده بود
		--اطلاعات تکمیلی آن سریال نیز از جدول موجودی سریالی انتخاب می گردد

CREATE PROCEDURE [ray].[InvAssistantSp_initScanDialog]
	@userId AS VARCHAR(50),
	@branch AS INT,
	@storeNo AS VARCHAR(6),
	@clientIp as varchar(50),

	@docType as tinyint,
	@barcode as varchar(max),

	@isValid as bit = 1 out,
	@validationMsg as varchar(150) out,
	@soh as money out,
	@serial_isNew as bit out,


	@partNo as varchar(20) out,
	@PartNoDsc as varchar(1000) out,
	@UntCode as char(2) out,
	@UntName as varchar(30) out,

				@orderBy as varchar (50) = null,
	@isDescOrder as bit = 1,

	@key AS VARCHAR(100),
	@fromIndex AS INT,
	@take as int

AS
begin

declare 
 @serial as varchar(50)
,@crntBarcodeStructureId as bigint
,@barcodeStructureIncludesPartNo as bit
,@barcodeStructureIncludesSerial as bit
,@StoreSerialType as tinyint
,@infoList_item as [ray].[InvAssistantUdt_idDsc]
,@NotActiv as tinyint
,@TechnicalNo as varchar(60)
,@NationalPartCod as varchar(20)
,@infoList_serial as [ray].[InvAssistantUdt_idDsc]
,@controls as [ray].[InvAssistantUdt_uiControlList]
,@isValid_serial as bit
,@validationMsg_serial as varchar(150);

set @isValid = 1;
set @isValid_serial = 1;
set @crntBarcodeStructureId = [ray].[InvAssistantFUNC_getCurrentBarcodeStructureId]();
set @barcodeStructureIncludesPartNo = 0;
set @barcodeStructureIncludesSerial = 0;
select @StoreSerialType = isnull(SerialTyp,1) from ray.Store where StoreNo = @storeNo;

if(@crntBarcodeStructureId is not null)--باید کد کالا رو در ساختارش داشته باشد حتما
begin
	IF OBJECT_ID('tempdb..#tmp') IS NOT NULL DROP TABLE #tmp;
	select fieldName
	into #tmp
	from ray.InvAssistant_barcodeStructure_dtl
	where fk_barcodeStructureHdr_id = @crntBarcodeStructureId and fieldName in ('partno','serial');
	--set @barcodeStructureIncludesPartNo = isnull((select 1 from #tmp where fieldName = 'partno'),0);
	set @barcodeStructureIncludesSerial = isnull((select 1 from #tmp where fieldName = 'serial'),0);
	set @partNo = ray.InvAssistantFUNC_exportFromBarcode(@barcode,@crntBarcodeStructureId,'partno');
	if(@barcodeStructureIncludesSerial = 1)
	set @serial = ray.InvAssistantFUNC_exportFromBarcode(@barcode,@crntBarcodeStructureId,'serial');

	if(@partNo is null or @partNo = '')
	begin
		set @validationMsg = 'خطا : تعریف کالا در ساختار بارکد اجباری می باشد.';
		set @isValid = 0;
		return 0;--TODO:?
	end


end
else--@crntBarcodeStructureId is null
begin
	set @partNo = @barcode;
end


--init soh
if(@StoreSerialType = 1)--انبار عادی
begin
set @soh = (select top 1 Qty from [ray].[InvAssistantTVFUNC_ArcSoh_GetList](@storeNo,@partNo) order by DocDate desc);
end
--else if(@serial is not null and @serial <> '')
--begin
--set @soh = (select top 1 Soh from [ray].[InvAssistantTVFUNC_SerialSoh_GetList](@storeNo,@partNo,@serial));
--end

--init itemData Info List
select @PartNoDsc = i.PartNoDsc
,@UntName = u.UntName
,@TechnicalNo = i.TechnicalNo
,@NationalPartCod = i.NationalPartCod
,@NotActiv = NotActiv
,@UntCode = i.UntCode
from ray.ItemData as i
left join ray.Unit as u on i.UntCode = u.UntCode
where i.PartNo = @partNo;
insert into @infoList_item select 'کد کالا',isnull(@partNo,@serial);
insert into @infoList_item select 'عنوان کالا',isnull(@PartNoDsc,0);
insert into @infoList_item select 'واحد شمارش',isnull(@UntName,'-');
insert into @infoList_item select 'موجودی',isnull(@soh,'-');
insert into @infoList_item select 'شماره فنی',isnull(@TechnicalNo,'-');
insert into @infoList_item select 'مشخصه ملی',isnull(@NationalPartCod,'-');

if(@NotActiv is not null and @NotActiv = 1)
begin
set @isValid = 0;
set @validationMsg = 'کد کالای وارد شده در سیستم غیر فعال گردیده است و قابل استفاده نمی باشد';
end


--init controls
if(@StoreSerialType = 1)--انبار عادی
begin

insert into @controls 
select 'weight',
'وزن',
1 ,
'',
'';

insert into @controls 
select 'serial',
'سریال / ش بچ',
3,
'',
'';

insert into @controls 
select 'qty',
'مقدار',
1,
1,--case when @StoreSerialType = 2 and @soh is not null then @soh else @defaultQty end,
'';

insert into @controls 
select 'mergeRows',
'ادغام ردیف های با کد یکسان',
1,
'',
'';





end
else if(@StoreSerialType in (2,3))
begin
if(@serial is null or @serial = '')--انبار بچ و سریال است و سریال هنوز مشخص نیست
begin

insert into @controls 
select 'weight',
'وزن',
2 ,
'',
'';

insert into @controls 
select 'serial',
'سریال / ش بچ',
4,
'',
'';

insert into @controls 
select 'qty',
'مقدار',
2,
1,--case when @StoreSerialType = 2 and @soh is not null then @soh else @defaultQty end,
'';

insert into @controls 
select 'mergeRows',
'ادغام ردیف های با کد یکسان',
case when @StoreSerialType= 2 then 2 else 1 end,
0,
'';



end
else--انبار بچ و سریال  است و سریال مشخص است
begin


declare @PakgTypeDesc as varchar(50),
	@SupplierIdDsc as varchar(300),
	@ExpDate as varchar(8);


	declare 
	--@StoreSerialType as tinyint,
	@defaultQty as money;


	--abfa- useless in dialog
	declare @hasWarning as bit,
	@refDocType as tinyint,
	@refDocNo as int,
	@refDocRow as int


select @hasWarning = hasWarning,
@isValid_serial = isValid ,
@validationMsg_serial = validationMsg,
@soh = soh,
@serial_isNew = isNew,
@PakgTypeDesc = PakgTypeDesc,
@SupplierIdDsc = SupplierIdDsc,
@ExpDate = ExpDate,
@StoreSerialType = StoreSerialType,
@defaultQty = defaultQty
from
ray.InvAssistantTVFUNC_initScanDialog_serial_validations(@userId,@branch,@storeNo,@clientIp,@docType,@partNo,@serial,@refDocType,@refDocNo,@refDocRow);


set @isValid = case when @isValid = 1 and @isValid_serial = 1 then 1 else 0 end;
set @validationMsg = isnull(@validationMsg,'') + ' ' + isnull(@validationMsg_serial,'')


insert into @infoList_serial
select id,dsc
from ray.InvAssistantTVFUNC_initScanDialog_serial_infoList
(@PakgTypeDesc,@SupplierIdDsc,@ExpDate,@serial,@soh);

insert into @controls
select id ,title,[state],defaultValue_id ,defaultValue_dsc
from ray.InvAssistantTVFUNC_initScanDialog_serial_controls(@serial,@StoreSerialType,@defaultQty,@soh);

end
end

--print to output
select id,dsc
from @infoList_item;

select id,dsc
from @infoList_serial;

select id ,title,[state],defaultValue_id ,defaultValue_dsc
from @controls;


end