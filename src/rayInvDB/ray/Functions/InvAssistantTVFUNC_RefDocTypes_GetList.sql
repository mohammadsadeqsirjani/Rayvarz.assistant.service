CREATE FUNCTION [ray].[InvAssistantTVFUNC_RefDocTypes_GetList]
(
	@baseDocType tinyint
)
RETURNS @returntable TABLE
(
	docTypeId tinyint,
	docTypeDesc varchar(150)
)
AS
BEGIN
	INSERT @returntable
	select ref.RefDocType as docTypeId , doc.DocTypeDesc as docTypeDesc
	from ray.invRfDocTyp ref with(nolock)
	left join ray.invDocTyp doc with(nolock) on  ref.RefDocType=doc.doctype where ref.doctype= @baseDocType
	RETURN
END
