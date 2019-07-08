CREATE PROCEDURE [ray].[InvAssistantSp_invHdrData_GetList]
	@userId AS VARCHAR(50),
	@branch AS INT,
	@storeNo AS VARCHAR(6),
	@clientIp as varchar(50),

	@docType AS TINYINT,
	@f_docNo AS INT,
	@f_createDate AS VARCHAR(8),
	@f_fiscalYear AS INT,
	--@f_center AS INT,
    @f_orderNo AS INT,
    


			@orderBy as varchar (50) = null,
	@isDescOrder as bit = 1,


	@key AS VARCHAR(100) = null,
	@fromIndex AS INT,
	@take as int
AS


select 
h.StoreNo,
h.FiscalYear,
h.DocType,
h.DocNo,
h.CountNo,
h.CreateDate,
h.DocStatus,
abs(h.SumQty) as SumQty,
d.OrderNo,
o.OrdrDsc,
max(d.DocRow) as cntDtls

from ray.invhdrData as h
join ray.InvDtlData as d
on h.StoreNo = d.StoreNo and h.FiscalYear = d.FiscalYear and h.DocType = d.DocType and h.DocNo = d.DocNo and h.CountNo = d.CountNo
left join ray.InvOrdr as o on d.OrderNo = o.OrdrNO
where
h.StoreNo = @storeNo
and h.DocType = @docType
and h.FiscalYear = isnull(@f_fiscalYear,[ray].[InvAssistantFUNC_getCurrentFiscalYear]())
and (@f_createDate is null or h.CreateDate = @f_createDate)
and (@f_docNo is null or h.DocNo = @f_docNo)
--and (@f_center is null or h.)
and (@f_orderNo is null or d.OrderNo = @f_orderNo)

group by
h.StoreNo,
h.FiscalYear,
h.DocType,
h.DocNo,
h.CountNo,
h.CreateDate,
h.DocStatus,
h.SumQty,
d.OrderNo,
o.OrdrDsc


order by
case when lower(@orderBy) = 'docdate' and isnull(@isDescOrder,1) = 1 then h.CreateDate else 0 end desc,
case when lower(@orderBy) = 'docdate' and isnull(@isDescOrder,1) = 0 then h.CreateDate else 0 end asc,
case when lower(@orderBy) = 'docno' and isnull(@isDescOrder,1) = 1 then h.DocNo else 0 end desc,
case when lower(@orderBy) = 'docno' and isnull(@isDescOrder,1) = 0 then h.DocNo else 0 end asc
OFFSET @fromIndex ROWS --(isnull(@pageNo,0) * 10 ) ROWS
    FETCH NEXT @take ROWS ONLY;
RETURN 0
