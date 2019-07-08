CREATE PROCEDURE [ray].[InvAssistantSp_itemData_GetItemInfo]
@PartCode as varchar(128)
as
select PartNoDsc, PartGrp, PartGrpDsc, UntCode, UntName, NationalPartCod, Salable, IsMchnPart,
IsTools,IsAsset, IsMold, IsMtrl, ISOfsTool, IsConsTool, IsMakePart,
IsShop, IsPack, IsLabTools, IsExpired, IsPrdPart, NotActiv, editable = (select * from InvDtlDatas where PartNo = itm.PartNo)
from ItemDatas as itm with(nolock) 
inner join Units as unt
on itm.UntCode = unt.UntCode
inner join PartGrps as pgr
on itm.PartGrp = pgr.PartGrp
where PartNo = @PartCode