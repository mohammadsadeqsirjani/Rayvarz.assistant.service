CREATE TABLE [ray].InvAssistant_barcodeStructure_hdr
(
	[id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [codeIdentificationType] TINYINT NOT NULL DEFAULT 0, 
    [barcodeType] VARCHAR(10) NOT NULL DEFAULT 'code128', 
    [saveDateTime] DATETIME NOT NULL DEFAULT getdate(), 
    [splitter] CHAR NULL, 
    [isActive] BIT NOT NULL DEFAULT 1
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'0:indexBase,1:spliter',
    @level0type = N'SCHEMA',
    @level0name = N'ray',
    @level1type = N'TABLE',
    @level1name = N'InvAssistant_barcodeStructure_hdr',
    @level2type = N'COLUMN',
    @level2name = 'codeIdentificationType'