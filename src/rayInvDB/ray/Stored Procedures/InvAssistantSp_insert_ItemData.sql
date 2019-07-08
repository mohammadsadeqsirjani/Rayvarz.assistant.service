CREATE PROCEDURE [ray].[InvAssistantSp_insert_ItemData]
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


,@PartNo as varchar(20)
,@PartNoDsc as varchar(1000)
,@NotActiv as tinyint
,@UntCode as char(2)
,@PartGrp as varchar(20) 
,@TechnicalNo as varchar(60) 
,@NationalPartCod as tinyint 
,@Salable as tinyint 
,@IsMchnPart as tinyint  
,@IsTools as tinyint
,@IsAsset as tinyint
,@IsMold as tinyint
,@IsMtrl as tinyint
,@ISOfsTool as tinyint
,@IsConsTool as tinyint
,@IsMakePart as tinyint
,@IsShop as tinyint
,@IsPack as tinyint
,@IsLabTools as tinyint
,@IsExpired as tinyint
,@IsPrdPart as tinyint
--,@PartLtnDsc as varchar(255)

,@rMsg as varchar(max) out
,@rCode as int  = 1 out 
as

Begin try
if((@PartNo is null or @PartNo = '') and (@PartNoDsc is null or @PartNoDsc = ''))
begin 
	set @rMsg = 'ورود کد و عنوان کالا اجباری است'
	set @rCode = 0
end
else if(@PartNo is null or @PartNoDsc = '')
begin 
	set @rMsg = 'ورود کد کالا اجباری است'
	set @rCode = 0
	return 0;
end
else if(@PartNoDsc is null or @PartNoDsc = '')
begin 
	set @rMsg = 'ورود عنوان کالا اجباری است'
	set @rCode = 0
	return 0;
end

if(@rCode <> 0)
begin
Insert into ItemData(PartNo, PartNoDsc, NotActiv ,UntCode, PartGrp, TechnicalNo, NationalPartCod, Salable,
IsMchnPart, IsTools, IsAsset, IsMold, IsMtrl, ISOfsTool, IsConsTool, IsMakePart, IsShop, IsPack, IsLabTools, IsExpired,
IsPrdPart, RowGuid, CreateDate)
Values(@PartNoDsc, @PartNoDsc, @NotActiv, @UntCode, @PartGrp, @TechnicalNo, @NationalPartCod, @Salable,
@IsMchnPart, @IsTools, @IsAsset, @IsMold, @IsMtrl, @ISOfsTool, @IsConsTool, @IsMakePart, @IsShop, @IsPack, @IsLabTools,
@IsExpired, @IsPrdPart, newid(), getdate())

set @rMsg = 'اطلاعات با موفقیت ثبت شد';
set @rCode = 1;
end
End try

Begin catch
set @rMsg = ERROR_MESSAGE();
set @rCode = ERROR_NUMBER()
--Set @rcode = 0;
--Set @rMsg = 'با عرض پوزش خطایی رخ داده است'; 
End catch

return 0;
