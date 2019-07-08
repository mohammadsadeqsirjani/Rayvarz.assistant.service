CREATE PROCEDURE [dbo].[InvAssistantSp_RefDocTypes_GetList]
	@userId AS VARCHAR(50),
	@branch AS INT,
	@storeNo AS VARCHAR(6),
	@clientIp as varchar(50),

	@baseDocType as tinyint,

				@orderBy as varchar (50) = null,
	@isDescOrder as bit = 1,

	@key AS VARCHAR(100),
	@fromIndex AS INT,
	@take as int
AS
	

	select docTypeId,docTypeDesc from [dbo].[InvAssistantTVFUNC_RefDocTypes_GetList](@baseDocType)
	order by docTypeId
	OFFSET @fromIndex ROWS --(isnull(@pageNo,0) * 10 ) ROWS
    FETCH NEXT @take ROWS ONLY;


RETURN 0
