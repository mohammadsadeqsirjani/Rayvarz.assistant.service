CREATE PROCEDURE [ray].[InvAssistantSp_DeliveryItems_GetList]
	@userId AS VARCHAR(50),
	@branch AS INT,
	@storeNo AS VARCHAR(6),
	@clientIp as varchar(50),


	@refDocFiscalYear as int,
	@refDocType as tinyint,
	@refDocNo as int,
	@refDocRow as smallint,

				@orderBy as varchar (50) = null,
	@isDescOrder as bit = 1,

	@isEditable as bit = null out,

	@key AS VARCHAR(100) = null,
	@fromIndex AS INT = null,
	@take as int = null
AS

set @isEditable = [ray].[InvAssistantFUNC_userHasMaxInvCnfrmLvl](@userId);

	SELECT [barcode],qty
	from [ray].[InvAssistant_deliveryItems] where
	FiscalYear = @refDocFiscalYear and StoreNo = @storeNo and DocType = @refDocType
	and DocNo = @refDocNo and DocRow = @refDocRow;
RETURN 0
