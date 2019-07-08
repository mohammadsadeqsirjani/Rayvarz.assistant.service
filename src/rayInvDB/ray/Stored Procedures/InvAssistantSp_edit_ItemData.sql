CREATE PROCEDURE [ray].[InvAssistantSp_edit_ItemData]
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
--,@PartLtnDsc as varchar(255)
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

,@rCode as int out
,@rMsg as varchar(max) out
as

if(not exists(select PartNo from ItemData where Partno = @PartNo))
begin 
	set @rCode = 0;
	set @rMsg = 'کد کالا یافت نشد';
	return 0;
end

if((select count(PartNo) from InvDtlData where PartNo = @PartNo) > 0)
begin
	set @rCode = 0;
	set @rMsg = 'کالا در سیستم مورد استفاده قرار گرفته است. امکان ویرایش وجود ندارد';
	return 0;	
end

Begin try

update ItemData
set PartNoDsc =  @PartNoDsc, NotActiv = @NotActiv, UntCode = @UntCode,
PartGrp =  @PartGrp, TechnicalNo = @TechnicalNo, NationalPartCod = @NationalPartCod, Salable = @Salable,
IsMchnPart = @IsMchnPart, IsTools = @IsTools, IsAsset = @IsAsset, IsMold = @IsMold, IsMtrl = @IsMtrl , ISOfsTool = @ISOfsTool,
IsConsTool = @IsConsTool , IsMakePart = @IsMakePart, IsShop = @IsShop, IsPack = @IsPack,
IsLabTools = @IsLabTools, IsExpired = @IsExpired, IsPrdPart = @IsPrdPart, LastChngDate = getdate()
where PartNo = @PartNo
set @rMsg = 'ویرایش اطلاعات با موفقیت انجام شد';
set @rCode = 1;

End try

Begin catch
set @rCode = ERROR_NUMBER();
set @rMsg = ERROR_MESSAGE();

End catch