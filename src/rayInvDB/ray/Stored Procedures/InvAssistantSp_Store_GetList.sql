CREATE PROCEDURE [ray].[InvAssistantSp_Store_GetList]
	@userId AS VARCHAR(50),
	@branch AS INT,
	@storeNo AS VARCHAR(6),
	@clientIp as varchar(50),

	@f_userId AS VARCHAR(50),
	@f_branch AS INT,
	@f_storeNo AS VARCHAR(6),

		@orderBy as varchar (50) = null,
	@isDescOrder as bit = 1,

	@key AS VARCHAR(100),
	@fromIndex AS INT,
	@take as int
AS
	

SELECT distinct s.StoreNo,s.StoreDsc,s.StoreLtnDsc,isnull(s.SerialTyp,1) as SerialTyp
	FROM ray.Store AS s
	left join ray.UserId as u on (@f_userId is not null and @f_userId <> '' and u.UserId = @f_userId and (u.InvStore like s.StoreNo + ';%' or  u.InvStore like '%;' + s.StoreNo + ';%'))
	left join ray.WrkShop as w on (@f_branch is not null and s.WrkShp = w.WrkShp and w.Branch = @f_branch)
	where s.Active = 1
	and (@f_storeNo is null or @f_storeNo = '' or s.StoreNo = @f_storeNo)
	and (@key is null or @key = ''  or s.StoreNo like '%' + @key + '%' or s.StoreDsc like '%' + @key + '%')
	and (@f_userId is null or @f_userId = '' or u.UserId is not null)
	and (@f_branch is null or w.WrkShp is not null)
	order by s.StoreNo
	OFFSET @fromIndex ROWS --(isnull(@pageNo,0) * 10 ) ROWS
    FETCH NEXT @take ROWS ONLY;


RETURN 0
