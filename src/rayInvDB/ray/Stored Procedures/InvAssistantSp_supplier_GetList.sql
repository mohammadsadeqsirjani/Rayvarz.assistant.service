CREATE PROCEDURE [ray].[InvAssistantSp_supplier_GetList]
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
as
select Supplier, SupName, SupFirstName, SupLastName, SupLtnName
from Supplier with (nolock)
where @key is null or @key = '' or Supplier like '%' + @key + '%' 
				   or SupName like '%' + @key + '%' 
			       or SupFirstName like '%' + @key + '%' 
				   or SupLastName like '%' + @key + '%' 
				   or SupLtnName like '%' + @key + '%'
order by Supplier
offset @fromIndex rows
fetch next @take rows only
return 0
