create PROCEDURE [ray].[InvAssistantSp_center_GetList]
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

declare @grp as varchar(128)
if(@group is null or @group = '')
begin 
set @grp = 0;
end
else
begin
set @grp = @group;
end

select Center, CenterDsc, (Cast(c.CenterGrp as varchar) + '_' + cast(g.CenterGrp as varchar)) as info
from Center as c with(nolock) 
join CenterGrp as g with(nolock)
on c.CenterGrp = g.CenterGrp
where (@group is null or @group = '' or c.CenterGrp = @grp)
and c.ActvFlg is not null and c.ActvFlg = 1 
and (@key is null or @key = '' or c.CenterDsc like '%' + @key + '%' or c.CenterLtnDsc like '%' + @key + '%' or c.Center like '%' + @key + '%' or g.CenterGrpDsc like '%' + @key + '%' or c.CenterGrp is null or c.CenterGrp like '%' + @key + '%') 
and (@from is null or Center >= @from) 
and (@to is null or Center <= @to)
order by c.Center
offset @fromIndex rows
fetch next @take rows only