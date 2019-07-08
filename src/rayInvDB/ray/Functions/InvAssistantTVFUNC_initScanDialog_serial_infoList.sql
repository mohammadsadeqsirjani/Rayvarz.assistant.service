CREATE FUNCTION [ray].[InvAssistantTVFUNC_initScanDialog_serial_infoList]
(
	@PakgTypeDesc as varchar(50),
	@SupplierIdDsc as varchar(300),
	@ExpDate as varchar(8),
	@serial as varchar(50),
	@soh as money
)
RETURNS @returntable TABLE
(
	id varchar(50), dsc VARCHAR(500)
)
AS
BEGIN
	INSERT @returntable
	
select 'سریال',isnull(@serial,'-')
union select 'موجودی',isnull(cast(@soh as varchar(30)),'0')
union select 'تامین کننده',isnull(@SupplierIdDsc,'-')
union  select 'نوع بسته بندی',isnull(@PakgTypeDesc,'-')
union select 'تاریخ انقضاء',isnull(@ExpDate,'-');



	RETURN
END
