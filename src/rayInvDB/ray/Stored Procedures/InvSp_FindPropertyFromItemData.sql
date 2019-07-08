CREATE PROCEDURE [ray].[InvSp_FindPropertyFromItemData] 
(@PartNo as varchar(50))
with Encryption
 AS

Select isnull(item.TechnicalNo,'') as TechnicalNo ,isnull(unit1.UntName,'') as UntName,isnull(unit2.UntName,'') as UntTot,isnull(unit3.UntName,'') as UntTotCode,isnull(unit.UntName,'') as wUntName,isnull(PartNoDsc,'') as PartNoDsc
,Item.InvDefCenter as Center,isnull(center.CenterDsc,'') as CenterDsc
,item.RcptType as RcptType,isnull(RcptTypeDesc,'') as RcptTypeDesc
,Item.ConsType as ConsType,isnull(ConsTypeDesc,'') as ConsTypeDesc
,Item.OrdrNo as OrdrNo,isnull(OrdrDsc,'') as OrdrDsc
,Item.Supplier as Supplier,isnull(SupName,'') as supname,item.ExchangeTtlUntToUnt, item.UntCode
,Item.act3 as act3,isnull(act.CenterDsc,'') as FactorDsc,isnull(PartLtnDsc,'') as PartLtnDsc,isnull(TechnicalNo,'') as TechnicalNo,
isnull(MapNo,'') as MapNo
 from Ray.ItemData  as Item with(nolock)
left join Ray.center as center  with(nolock) on Item.InvDefcenter=center.center
left join Ray.InvOrdr as  ordr  with(nolock)on Item.OrdrNo=ordr.OrdrNo
left join Ray.InvRcptTyp as RcptTyp with(nolock) on Item.RcptType=RcptTyp.RcptType
left join Ray.InvCnsTyp as ConsType with(nolock) on Item.consType=ConsType.ConsType
left join Ray.Unit  as unit with(nolock) on item.WghtUnt=unit.UntCode
left join Ray.Unit  as unit1 with(nolock) on item.UntCode=unit1.UntCode
left join Ray.Unit  as unit2 with(nolock) on item.TtlUnt=unit2.UntCode
left join Ray.Unit  as unit3 with(nolock) on item.TtlUntCode=unit3.UntCode
left join Ray.Supplier  as Sup with(nolock) on item.Supplier=Sup.Supplier
left join Ray.center  as act with(nolock) on item.act3=act.center
 where 
item.partno=@PartNo