CREATE PROCEDURE [ray].[InvAssistantSp_invDoc_GetDocReferHdr]
@key as varchar(128)
,@orderby as varchar(128)
,@isDescOrder as bit
,@fromIndex as int 
,@take as int 
as
with res as
(
	select DestStoreNo, DocDate, DocNo, DocStatus, DocType, doctypedesc, FiscalYear, LcNo, orgdocno, orgRefno, raysys, ReqCenter, reqcenterDesc, [Row], OrderDsc
	from InvDtlData	
	where @key is null or @key = '' or DocDate like '%' + @key + '%' or DocNo like '%' + @key + '%' or reqcenterDesc like '%' + @key + '%'
) select * into #result from res

if(@orderby is not null and @orderby <> '' and Lower(@orderBy) = 'decdate')
begin
	if(@isDescOrder is not null and @isDescOrder = 0)
	begin
		select * from #result order by DocDate 
		offset @fromIndex rows
		fetch next @take rows only
	end
	select * from #result order by DocDate desc  
	offset @fromIndex rows
	fetch next @take rows only 
end

if(@isDescOrder is not null and @isDescOrder = 0)
begin
	select * from #result order by DocNo 
	offset @fromIndex rows
	fetch next @take rows only 
end
	select * from #result order by DocNo  desc
	offset @fromIndex rows
	fetch next @take rows only 








