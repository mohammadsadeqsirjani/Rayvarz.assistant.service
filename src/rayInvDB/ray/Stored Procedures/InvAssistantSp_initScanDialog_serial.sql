CREATE PROCEDURE [ray].[InvAssistantSp_initScanDialog_serial]
		@userId AS VARCHAR(50),
	@branch AS INT,
	@storeNo AS VARCHAR(6),
	@clientIp as varchar(50),

	@docType as tinyint,
	@partNo as varchar(20),
	@serial as varchar(50),

	--abfa
	--@validateSerialWithOrderNo as bit = 0,
	@refDocType as tinyint = null,
	@refDocNo as int = null,
	@refDocRow as int = null,
	@hasWarning as bit = 0 out,
	--endofAbfa


	@isValid as bit = 1 out,
	@validationMsg as varchar(150) out,
	@soh as money out,
	@isNew as bit out,

				@orderBy as varchar (50) = null,
	@isDescOrder as bit = 1,

	@key AS VARCHAR(100),
	@fromIndex AS INT,
	@take as int

	----serial info list
	--@siLst_serial_caption varchar(50) out


AS
begin





declare @PakgTypeDesc as varchar(50),
	@SupplierIdDsc as varchar(300),
	@ExpDate as varchar(8);


	declare 
	@StoreSerialType as tinyint,
	@defaultQty as money;



select @hasWarning = hasWarning,
@isValid = isValid ,
@validationMsg = validationMsg,
@soh = soh,
@isNew = isNew,
@PakgTypeDesc = PakgTypeDesc,
@SupplierIdDsc = SupplierIdDsc,
@ExpDate = ExpDate,
@StoreSerialType = StoreSerialType,
@defaultQty = defaultQty
from
ray.InvAssistantTVFUNC_initScanDialog_serial_validations(@userId,@branch,@storeNo,@clientIp,@docType,@partNo,@serial,@refDocType,@refDocNo,@refDocRow)


select id,dsc
from ray.InvAssistantTVFUNC_initScanDialog_serial_infoList
(@PakgTypeDesc,@SupplierIdDsc,@ExpDate,@serial,@soh);


select id ,title,[state],defaultValue_id ,defaultValue_dsc
from ray.InvAssistantTVFUNC_initScanDialog_serial_controls(@serial,@StoreSerialType,@defaultQty,@soh);


end
