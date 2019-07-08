create FUNCTION [ray].[func_getFullPartGrpDscPath]
(
	@partGrp as varchar(20)
)
RETURNS varchar(max)
AS
BEGIN
	declare @partGrpDsc varchar(max)
	if @partGrp IS NOT NULL AND @partGrp <> ''
	BEGIN
			select @partGrpDsc =
			 ISNULL(PartGrpDsc , '') + (CASE WHEN PrntPartGrp IS NULL then '' else  '_____'END )	
			+ ISNULL(ray.func_getFullPartGrpDscPath(PrntPartGrp),'')
			
			from ray.PartGrp where PartGrp = @partGrp
	END


	RETURN ISNULL(@partGrpDsc,'')
	END