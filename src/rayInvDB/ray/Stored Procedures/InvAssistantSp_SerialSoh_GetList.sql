CREATE PROCEDURE [ray].[InvAssistantSp_SerialSoh_GetList]
		@userId AS VARCHAR(50),
	@branch AS INT,
	@storeNo AS VARCHAR(6),
	@clientIp as varchar(50),

	@partNo as varchar(20),
	@f_serial as varchar(50),
				@orderBy as varchar (50) = null,
	@isDescOrder as bit = 1,

	@key AS VARCHAR(100),
	@fromIndex AS INT,
	@take as int
AS
	


begin
select ss.*
from ray.InvAssistantTVFUNC_SerialSoh_GetList(@storeNo,@partNo,@f_serial) as ss
	where
	(@key is null or @key = '' or [ss].[Serail] like '%' + @key + '%' or [ss].[SupName] like '%' + @key + '%' or  [ss].[PakgTypeDesc] like '%' + @key + '%' )





end
