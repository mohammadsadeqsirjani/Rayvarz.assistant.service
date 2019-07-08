CREATE PROCEDURE [ray].[InvAssistantSp_storeGetList]
@key as varchar(128)
,@fromIndex as int 
,@take as int = 10
as
select StoreNo, StoreDsc, StoreLtnDsc, Active
from Stores with(nolock)
where @key is null or @key = '' or StoreDsc like '%' + @key + '%' or StoreLtnDsc like '%' + @key + '%' or StoreNo like '%' + @key + '%'
order by StoreNo
offset @fromIndex rows
fetch next @take rows only 