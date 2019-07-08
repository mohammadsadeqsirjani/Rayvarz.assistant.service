CREATE PROCEDURE [ray].[InvAssistantSp_GetLastAvailableParNoOfGrp]
 @userId AS VARCHAR(50) = null
,@branch AS INT = null
,@storeNo AS VARCHAR(6) = null
,@clientIp as varchar(50) = null
,@group as varchar(128) = null
,@key as varchar (128) = null
,@from as int = null
,@to as int = null
,@fromIndex as int = 0
,@take as int = 10
,@orderBy as varchar(128) = null
,@isDescOrder as bit = 1

,@PartGrp as varchar(20)
,@maxval as varchar(20) out
as
SET @maxval = RIGHT('00000000' + ISNULL(@PartGrp, 0), 8) + 
RIGHT('000000000000' + CAST((CAST( ISNULL((SELECT MAX(PartNo) FROM ItemData WHERE PartGrp = @PartGrp), 0) AS BIGINT) + 1) AS VARCHAR), 12) 
