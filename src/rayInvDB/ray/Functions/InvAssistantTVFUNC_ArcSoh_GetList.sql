CREATE FUNCTION [ray].[InvAssistantTVFUNC_ArcSoh_GetList]
(
	@storeNo varchar(6),
	@f_partNo as varchar(20)
)
RETURNS @returntable TABLE
(
	FiscalYear  int,
	StoreNo varchar(6),
	PartNo varchar(20),	
	DocDate char(8),
	Qty money
)
AS
BEGIN
	INSERT @returntable
	SELECT 
	ias.FiscalYear,
	ias.StoreNo,
	ias.PartNo,
    ias.DocDate,
	ias.Qty
	from ray.InvArcSoh as ias
        WHERE
		ias.StoreNo = @storeNo
		and
		([ray].[InvAssistantFUNC_getCurrentFiscalYear]() = ias.FiscalYear)
		and
		(@f_partNo is null or @f_partNo = '' or ias.PartNo = @f_partNo)
		order by ias.DocDate desc

	RETURN
END
