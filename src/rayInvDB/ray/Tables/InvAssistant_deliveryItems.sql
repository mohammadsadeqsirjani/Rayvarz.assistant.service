CREATE TABLE [ray].[InvAssistant_deliveryItems]
(
	[FiscalYear] INT NOT NULL , 
    [StoreNo] VARCHAR(6) NOT NULL, 
    [DocType] TINYINT NOT NULL, 
    [DocNo] INT NOT NULL, 
    [DocRow] SMALLINT NOT NULL, 
    [barcode] VARCHAR(100) NOT NULL, 
    [qty] MONEY NULL, 
    [fk_barcodeStructure_id] BIGINT NULL, 
    PRIMARY KEY ([FiscalYear], [StoreNo], [DocType], [DocNo], [DocRow], [barcode]), 
    CONSTRAINT [FK_InvAssistant_deliveryItems_InvAssistant_barcodeStructure_hdr] FOREIGN KEY ([fk_barcodeStructure_id]) REFERENCES [ray].[InvAssistant_barcodeStructure_hdr]([id])
)
