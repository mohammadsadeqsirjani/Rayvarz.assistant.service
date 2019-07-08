create PROCEDURE Ray.InvSp_FillForceQty(@DocType as tinyint,@Fieldname as varchar(20)=null,@Storeno as varchar(6)=null)
with encryption
 AS
set nocount on

if isnull(@Storeno,'')='' set @Storeno='همه انبارها'
--فيلدهائي كه مقدار دارند
if isnull(@Fieldname,'')='' 
  begin
   --در خروجي سلکت ، اگر انبار مورد نظر مقدار داشته باشد ، اول آن خوانده مي شود و اگر مقدار نداشته باشد ، همه انبارها
       select FieldLtnName,DefaultVal from ray.InvForceField with(nolock) where doctype=@DocType and isnull(DefaultVal,'')<>'' and (storeno=@Storeno or storeno='همه انبارها')
	    order by DefaultVal desc
   end 
else
   begin
      --در خروجي سلکت ، اگر انبار مورد نظر مقدار داشته باشد ، اول آن خوانده مي شود و اگر مقدار نداشته باشد ، همه انبارها
       select FieldLtnName,DefaultVal from ray.InvForceField with(nolock) where doctype=@DocType and
	    FieldLtnName=@Fieldname and (storeno=@Storeno or storeno='همه انبارها')
	   order by DefaultVal desc
   end 
set nocount off