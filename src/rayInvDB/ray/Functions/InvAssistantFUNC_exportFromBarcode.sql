CREATE FUNCTION [ray].[InvAssistantFUNC_exportFromBarcode]
(
	@barcode as varchar(max),
	@barcodeStructureId as bigint,
	@fieldName as varchar(50)
)
RETURNS varchar(100)
AS
BEGIN
declare @codeIdentificationType as tinyint,@splitter as char(1);
--
--if(@fieldName = 'partno')
--return cast( substring(@barcode,0,13) as bigint);
--if(@fieldName = 'serial')
--return case when substring(@barcode,13,8) <> '' then substring(@barcode,13,8) else null end ;
--return '';

declare @barcodeStructure_dtl as table
([id] BIGINT , 
    [fk_barcodeStructureHdr_id] BIGINT , 
    [fieldName] VARCHAR(50) , 
    [order] TINYINT , 
    [length] TINYINT )
	insert into @barcodeStructure_dtl

select *  from ray.InvAssistant_barcodeStructure_dtl as d 
where d.fk_barcodeStructureHdr_id = isnull(@barcodeStructureId,[ray].[InvAssistantFUNC_getCurrentBarcodeStructureId]())
order by d.[order]



declare @startIndex as smallint,@length as tinyint;






select @startIndex = isnull(sum(d.[length]),0) +1
from @barcodeStructure_dtl as d
where d.[order] < (select [order] from @barcodeStructure_dtl as id where id.fieldName = @fieldName)

select @length = [length] from @barcodeStructure_dtl where fieldName = @fieldName;

return case when substring(@barcode,@startIndex,@length) <> '' then cast (substring(@barcode,@startIndex,@length) as bigint ) else null end



END