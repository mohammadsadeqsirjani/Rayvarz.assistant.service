CREATE PROCEDURE [dbo].[InvAssistantSp_SelectDocRefer]
	@userId AS VARCHAR(50),
	@branch AS INT,
	@storeNo AS VARCHAR(6),
	@clientIp as varchar(50),

	@docType AS TINYINT,
	@docNo AS INT,
	@f_ref_docType AS TINYINT,
	@f_ref_docDate AS VARCHAR(8),
	@f_ref_fiscalYear AS INT,
	@f_ref_docNo AS INT,
	@f_ref_center AS INT,
    @f_ref_orderNo AS INT,
    
			@orderBy as varchar (50) = null,
	@isDescOrder as bit = 1,


	@key AS VARCHAR(100),
	@fromIndex AS INT,
	@take as int
AS
	
	declare @f_ref_docTypes as [dbo].[InvAssistantUdt_tinyIntList];	
	DECLARE @RC int
	declare @q as nvarchar(max);

	if(@f_ref_fiscalYear is null)
	 set @f_ref_fiscalYear = 1397;






	if(@f_ref_docType is not null)
	insert into @f_ref_docTypes (id) values (@f_ref_docType)
	else
	begin
	insert into @f_ref_docTypes (id)
	select docTypeId from [ray].[InvAssistantTVFUNC_RefDocTypes_GetList](@docType)
	end

	DECLARE cr cursor for select id from @f_ref_docTypes;
	open cr;
	fetch next from cr into @f_ref_docType;
	while(@@FETCH_STATUS = 0)
	begin

	EXECUTE @RC = [ray].[InvSp_SelectDocRefer] 
					  @FiscalYear = @f_ref_fiscalYear
					 ,@StoreNo =@StoreNo
					 ,@DocType =@f_ref_docType
					 ,@Docno =@f_ref_docNo
					 ,@Partno =''
					 ,@BaseDocType =@docType
					 ,@IsStandardIssue =0
					 ,@usr =@userId
					 ,@FilterPartno =''
					 ,@FilterCenter =@f_ref_center
					 ,@FilterDate =@f_ref_docDate
					 ,@strIsInvWithParameter =''
					 ,@FilterOrderNo =@f_ref_orderNo
					 ,@FilterSerial =''
					 ,@NotRemain =0
					 ,@IsSaleIssueForRtrn =0 





					 
                    set @q = 'select [Row],[FiscalYear],[DocType],[doctypedesc],[DocNo],[DocDate],[DocStatus],[reqcenterDesc],[orgdocno],[orgRefno],[ReqCenter],[DestStoreNo],[LcNo],[raysys]';
                        if(exists(
                    SELECT 1
                    FROM   tempdb.sys.columns
                    WHERE  object_id = Object_id('tempdb..##InvDocSelect'+ @userId) and name = 'OrdrDsc'))

                    set @q = @q + ',[OrdrDsc]';
                    else
                    set @q = @q + ',''-'' as OrdrDsc';
                    set @q = @q + ' from ##InvDocSelect' + @userId;

                    exec sp_executesql @q







	fetch next from cr into @f_ref_docType;
	end
	close cr;
	deallocate cr;




	--order by s.StoreNo
	--OFFSET @fromIndex ROWS --(isnull(@pageNo,0) * 10 ) ROWS
 --   FETCH NEXT @take ROWS ONLY;


RETURN 0
