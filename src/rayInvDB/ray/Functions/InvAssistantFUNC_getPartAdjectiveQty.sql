CREATE FUNCTION [ray].[InvAssistantFUNC_getPartAdjectiveQty]
(
	@PartNo varchar(20),
	@Adjective smallint
)
RETURNS money
AS
BEGIN
	RETURN (select AdjectiveQty from ray.PartAdjective where PartNo = @PartNo and Adjective = @Adjective)
END
