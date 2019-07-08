CREATE FUNCTION [ray].[InvAssistantFUNC_getCurrentBarcodeStructureId]
(

)
RETURNS bigint
AS
BEGIN
	RETURN (select top 1 id from ray.InvAssistant_barcodeStructure_hdr where isActive = 1  order by id desc)
END
