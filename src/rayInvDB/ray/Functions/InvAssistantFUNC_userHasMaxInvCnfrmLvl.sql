CREATE FUNCTION [ray].[InvAssistantFUNC_userHasMaxInvCnfrmLvl]
(
	@userId varchar(50)
)
RETURNS bit
AS
BEGIN
	if exists (select 1 from ray.UserId as ou where ou.userId = @userId and ou.InvCnfrmLvl = (select max(iu.InvCnfrmLvl) from ray.UserId as iu))
	return 1;
	return 0;
END
