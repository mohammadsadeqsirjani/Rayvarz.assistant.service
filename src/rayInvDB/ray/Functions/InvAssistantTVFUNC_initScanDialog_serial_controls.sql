CREATE FUNCTION [ray].[InvAssistantTVFUNC_initScanDialog_serial_controls]
(
	@serial as varchar(50),
	@StoreSerialType as tinyint,
	@defaultQty as money,
	@soh money
)
RETURNS @returntable TABLE
(
		id varchar(50),
	title VARCHAR(128),
	[state] TINYINT,
	defaultValue_id varchar(50),
	defaultValue_dsc varchar(50)
)
AS
BEGIN
	INSERT @returntable
	 

select 'weight',
'وزن',
1,
'',
''

union

select 'serial',
'سریال / ش بچ',
4,
isnull(@serial,'-'),
''
union

select 'qty',
'مقدار',
case when @StoreSerialType = 2 and @soh is not null then 2 else 1 end,
case when @StoreSerialType = 2 and @soh is not null then cast(@soh as varchar(15)) else cast(@defaultQty as varchar(15))end,
''

union
select 'mergeRows',
'ادغام ردیف های با کد یکسان',
case when @StoreSerialType = 2 then 2 else 1 end,
case when @StoreSerialType = 2 then '0' else '' end,
'';



	RETURN
END
