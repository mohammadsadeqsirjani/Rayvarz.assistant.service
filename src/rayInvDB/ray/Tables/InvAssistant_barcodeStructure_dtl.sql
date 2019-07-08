CREATE TABLE [ray].[InvAssistant_barcodeStructure_dtl]
(
	[id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [fk_barcodeStructureHdr_id] BIGINT NOT NULL, 
    [fieldName] VARCHAR(50) NOT NULL, 
    [order] TINYINT NULL, 
    [length] TINYINT NULL, 
    CONSTRAINT [FK_InvAssistant_barcodeStructure_dtl_InvAssistant_barcodeStructure_hdr] FOREIGN KEY ([fk_barcodeStructureHdr_id]) REFERENCES [ray].[InvAssistant_barcodeStructure_hdr]([id])
)
