create PROCEDURE [ray].[InvAssistantSp_itemData_GetList]
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

,@Salable as varchar(128)
,@IsTools as tinyint
,@IsMold as tinyint
,@IsMtrl as tinyint
,@ISOfsTool as tinyint
,@IsConsTool as tinyint
,@IsBisector as tinyint
,@IsAsset as tinyint
,@IsPack as tinyint
as

	select @Salable = Salable, @IsTools = IsTools, @IsMold = IsMold, @IsMtrl = IsMtrl, @ISOfsTool = ISOfsTool,
	@IsConsTool = IsConsTool, @IsBisector = IsBisector, @IsAsset = IsAsset, @IsPack = IsPack 
	from Store with(nolock)
	where StoreNo = @storeNo
 

select PartNo, TechnicalNo, PartNoDsc, PartLtnDsc, item.UntCode, UntName, NotActiv, NationalPartCod, item.PartGrp, fullpath 
from ItemData as item
join Unit as unit with(nolock)

on item.UntCode = unit.UntCode
left join InvAssistantVw_getPartGrpFullPath as func
on func.PartGrp = item.PartGrp
where (@Salable <> 1 or @Salable is null or item.Salable = 1) 
and (@IsTools <> 1 or @IsTools is null or item.IsTools = 1)
and (@IsMold <> 1 or @IsMold is null or item.IsMold = 1)
and (@IsMtrl <> 1 or @IsMtrl is null or item.IsMtrl = 1)
and (@ISOfsTool <> 1 or @ISOfsTool is null or item.ISOfsTool = 1)
and (@IsConsTool <> 1 or @IsConsTool is null or item.IsConsTool = 1)
and (@IsBisector <> 1 or @IsBisector is null or item.IsMakePart = 1)
and (@IsAsset <> 1 or @IsAsset is null or item.IsAsset = 1)
and (@IsPack <> 1 or	@IsPack is null or item.IsPack = 1)
and (@group is null or @group = '')
and (@key is null or @key = '')
and  (item.PartNo like '%' + @key + '%')
and  (item.PartNoDsc like '%' + @key + '%')
and  (item.PartLtnDsc like '%' + @key + '%')
and  (item.NationalPartCod like '%' + @key + '%')
and  (item.TechnicalNo like '%' + @key + '%')
order by PartNo
offset @fromIndex rows
fetch next @take rows only
return 0;
