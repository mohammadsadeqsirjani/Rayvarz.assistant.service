CREATE FUNCTION [ray].[InvAssistantFUNC_getRaySysSpcInfVal]
(
	@infTit varchar(100)
)
RETURNS varchar
AS
BEGIN
	RETURN (select InfVal from ray.RaySysSpc where RaySys = 'InvAssistant' and InfTit = @infTit)
END
