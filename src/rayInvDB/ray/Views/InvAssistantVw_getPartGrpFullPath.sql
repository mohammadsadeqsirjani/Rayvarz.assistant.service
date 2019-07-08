CREATE view [ray].[InvAssistantVw_getPartGrpFullPath]
AS
select PartGrp , PartGrpDsc , PrntPartGrp , ray.InvAssistantFUNC_getFullPartGrpDscPath(PartGrp) as FullPath
--,case when not exists (select top (1) 1 from  ray.partGrp as ipg with (nolock) where ipg.PrntPartGrp = pg.PartGrp) then 1 else 0 end as isLeaf

from ray.partGrp as pg
where not exists (select top (1) 1 from  ray.partGrp as ipg with (nolock) where ipg.PrntPartGrp = pg.PartGrp)