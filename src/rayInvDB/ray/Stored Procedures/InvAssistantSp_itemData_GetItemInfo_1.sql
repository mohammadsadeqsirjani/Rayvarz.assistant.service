CREATE PROCEDURE [ray].[InvAssistantSp_itemData_GetItemInfo]
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
 
,@partCode as varchar(20)
,@docNo as varchar(128)
,@refDoctype as varchar(128)
,@refDocNo as varchar(128)
,@refFiscalYear as int

,@rCode as tinyint out
--,@rMsg as varchar(max) out
as	
	with res as
	(
		select item.PartNo ,item.PartNoDsc, item.PartGrp, item.NationalPartCod, item.Salable, item.IsMchnPart,
		item.IsTools, item.IsAsset, item.IsMold, item.IsMtrl, item.ISOfsTool, item.IsConsTool,
		item.IsMakePart, item.IsShop, item.IsPack, item.IsLabTools, item.IsExpired,
		item.IsPrdPart, item.IsPrdPart as i, item.NotActiv, unit.UntCode, unit.UntName, partgrp.PartGrpDsc
		from ray.ItemData item with(nolock) 
		join Unit as unit with(nolock)
		on item.UntCode = unit.UntCode
		left join PartGrp as partgrp
		on partgrp.PartGrp = item.PartGrp 
		where item.PartNo = @PartCode
	) select * into #res from res

if(exists(select 1 from #res))
begin
	select PartNoDsc, PartGrp, PartGrpDsc, UntCode,
	UntName, NationalPartCod, Salable, IsMchnPart, IsTools, IsAsset,
	IsMold , IsMtrl, ISOfsTool, IsConsTool , IsMakePart , IsShop , IsPack , 
	IsLabTools , IsExpired ,IsPrdPart, NotActiv ,
	case when exists(select * from InvDtlData as i join #res as r on i.PartNo = r.PartNo) then 1 else 0 end as editable
	from #res
	set @rCode = 1;
	return 0;
end

set @rCode = 0
--set @rMsg = 'کالایی یافت نشد'
return 0;



