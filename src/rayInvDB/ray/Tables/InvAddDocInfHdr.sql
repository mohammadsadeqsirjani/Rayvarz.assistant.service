CREATE TABLE [ray].[InvAddDocInfHdr](
	[FiscalYear] [int] NOT NULL,
	[StoreNo] [varchar](6) NOT NULL,
	[DocType] [tinyint] NOT NULL,
	[DocNo] [int] NOT NULL,
	[CountNo] [smallint] NOT NULL,
	[DestStoreNo] [varchar](6) NULL,
	[StkCountNo] [smallint] NULL,
	[DocEntStatus] [tinyint] NULL,
	[FldCod] [smallint] NOT NULL,
	[CompData] [varchar](2000) NULL,
	[CompNum] [money] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_INVADDDOCINFHDR] PRIMARY KEY CLUSTERED 
(
	[FiscalYear] ASC,
	[StoreNo] ASC,
	[DocType] ASC,
	[DocNo] ASC,
	[CountNo] ASC,
	[FldCod] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[InvAddDocInfHdr]  WITH NOCHECK ADD  CONSTRAINT [FK_InvAddDocInfHdr_HdrData] FOREIGN KEY([FiscalYear], [StoreNo], [DocType], [DocNo], [CountNo])
REFERENCES [ray].[invhdrdata] ([FiscalYear], [StoreNo], [DocType], [DocNo], [CountNo])
ON UPDATE CASCADE
ON DELETE CASCADE
NOT FOR REPLICATION
GO

ALTER TABLE [ray].[InvAddDocInfHdr] CHECK CONSTRAINT [FK_InvAddDocInfHdr_HdrData]
GO
ALTER TABLE [ray].[InvAddDocInfHdr]  WITH CHECK ADD  CONSTRAINT [FKF_InvAddDocInfHdr_InvDocTyp_DocType] FOREIGN KEY([DocType])
REFERENCES [ray].[InvDocTyp] ([DocType])
GO

ALTER TABLE [ray].[InvAddDocInfHdr] CHECK CONSTRAINT [FKF_InvAddDocInfHdr_InvDocTyp_DocType]
GO
ALTER TABLE [ray].[InvAddDocInfHdr]  WITH CHECK ADD  CONSTRAINT [FKF_InvAddDocInfHdr_InvPrd_FiscalYear] FOREIGN KEY([FiscalYear])
REFERENCES [ray].[InvPrd] ([FiscalYear])
GO

ALTER TABLE [ray].[InvAddDocInfHdr] CHECK CONSTRAINT [FKF_InvAddDocInfHdr_InvPrd_FiscalYear]
GO
ALTER TABLE [ray].[InvAddDocInfHdr]  WITH CHECK ADD  CONSTRAINT [FKF_InvAddDocInfHdr_Store_StoreNo] FOREIGN KEY([StoreNo])
REFERENCES [ray].[Store] ([StoreNo])
GO

ALTER TABLE [ray].[InvAddDocInfHdr] CHECK CONSTRAINT [FKF_InvAddDocInfHdr_Store_StoreNo]
GO
ALTER TABLE [ray].[InvAddDocInfHdr] ADD  DEFAULT ((0)) FOR [CountNo]
GO
ALTER TABLE [ray].[InvAddDocInfHdr] ADD  DEFAULT ((0)) FOR [StkCountNo]
GO
ALTER TABLE [ray].[InvAddDocInfHdr] ADD  DEFAULT (newid()) FOR [RowGuid]