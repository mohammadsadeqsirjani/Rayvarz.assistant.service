CREATE TABLE [ray].[InvRfDoc](
	[FiscalYear] [int] NOT NULL,
	[StoreNo] [varchar](6) NOT NULL,
	[DocType] [tinyint] NOT NULL,
	[DocNo] [int] NOT NULL,
	[CountNo] [smallint] NOT NULL,
	[DestStoreNo] [varchar](6) NULL,
	[DocEntStatus] [tinyint] NULL,
	[DocRow] [smallint] NOT NULL,
	[LnkType] [tinyint] NULL,
	[RefFiscalYear] [int] NOT NULL,
	[RefStore] [varchar](6) NOT NULL,
	[RefDocType] [tinyint] NOT NULL,
	[RefDocNo] [int] NOT NULL,
	[RefDocRow] [smallint] NOT NULL,
	[PartNo] [varchar](20) NULL,
	[RefDestStoreNo] [varchar](6) NULL,
	[RefEntDocStatus] [tinyint] NULL,
	[RefQty] [money] NULL,
	[RefPArtNo] [varchar](20) NULL,
	[RefBinNo] [varchar](35) NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_INVRFDOC] PRIMARY KEY CLUSTERED 
(
	[FiscalYear] ASC,
	[StoreNo] ASC,
	[DocType] ASC,
	[DocNo] ASC,
	[CountNo] ASC,
	[DocRow] ASC,
	[RefFiscalYear] ASC,
	[RefStore] ASC,
	[RefDocType] ASC,
	[RefDocNo] ASC,
	[RefDocRow] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[InvRfDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvRfDoc_ItemData] FOREIGN KEY([RefPArtNo])
REFERENCES [ray].[ItemData] ([PartNo])
GO

ALTER TABLE [ray].[InvRfDoc] CHECK CONSTRAINT [FK_InvRfDoc_ItemData]
GO
ALTER TABLE [ray].[InvRfDoc]  WITH NOCHECK ADD  CONSTRAINT [FK_InvRfDocDtlData] FOREIGN KEY([FiscalYear], [StoreNo], [DocType], [DocNo], [CountNo], [DocRow])
REFERENCES [ray].[InvDtlData] ([FiscalYear], [StoreNo], [DocType], [DocNo], [CountNo], [DocRow])
ON UPDATE CASCADE
ON DELETE CASCADE
NOT FOR REPLICATION
GO

ALTER TABLE [ray].[InvRfDoc] CHECK CONSTRAINT [FK_InvRfDocDtlData]
GO
--ALTER TABLE [ray].[InvRfDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvRfDocLnkTyp] FOREIGN KEY([LnkType])
--REFERENCES [ray].[InvLnkTyp] ([LinkType])
--GO

--ALTER TABLE [ray].[InvRfDoc] CHECK CONSTRAINT [FK_InvRfDocLnkTyp]
--GO
--ALTER TABLE [ray].[InvRfDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvRfDocPart] FOREIGN KEY([PartNo])
--REFERENCES [ray].[ItemData] ([PartNo])
--GO

--ALTER TABLE [ray].[InvRfDoc] CHECK CONSTRAINT [FK_InvRfDocPart]
--GO
--ALTER TABLE [ray].[InvRfDoc]  WITH CHECK ADD  CONSTRAINT [FKF_InvRfDoc_InvDocTyp_DocType] FOREIGN KEY([DocType])
--REFERENCES [ray].[InvDocTyp] ([DocType])
--GO

--ALTER TABLE [ray].[InvRfDoc] CHECK CONSTRAINT [FKF_InvRfDoc_InvDocTyp_DocType]
--GO
--ALTER TABLE [ray].[InvRfDoc]  WITH CHECK ADD  CONSTRAINT [FKF_InvRfDoc_InvPrd_FiscalYear] FOREIGN KEY([FiscalYear])
--REFERENCES [ray].[InvPrd] ([FiscalYear])
--GO

--ALTER TABLE [ray].[InvRfDoc] CHECK CONSTRAINT [FKF_InvRfDoc_InvPrd_FiscalYear]
--GO
--ALTER TABLE [ray].[InvRfDoc]  WITH CHECK ADD  CONSTRAINT [FKF_InvRfDoc_Store_StoreNo] FOREIGN KEY([StoreNo])
--REFERENCES [ray].[Store] ([StoreNo])
--GO

--ALTER TABLE [ray].[InvRfDoc] CHECK CONSTRAINT [FKF_InvRfDoc_Store_StoreNo]
--GO
ALTER TABLE [ray].[InvRfDoc] ADD  CONSTRAINT [DF__InvRfDoc__CountN__2C7E77EA]  DEFAULT ((0)) FOR [CountNo]
GO
ALTER TABLE [ray].[InvRfDoc] ADD  DEFAULT (newid()) FOR [RowGuid]