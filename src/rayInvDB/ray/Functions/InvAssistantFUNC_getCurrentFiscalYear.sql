CREATE FUNCTION [ray].[InvAssistantFUNC_getCurrentFiscalYear]
(
	
)
RETURNS INT
AS
BEGIN
	RETURN (select top 1 FiscalYear from ray.InvPrd where FiscalStatus = 1 order by FiscalYear desc)
END
