CREATE TABLE [ray].[InvAddDocInf](
	[FiscalYear] [int] NOT NULL,
	[StoreNo] [varchar](6) NOT NULL,
	[DocType] [tinyint] NOT NULL,
	[DocNo] [int] NOT NULL,
	[DestStoreNo] [varchar](6) NULL,
	[StkCountNo] [smallint] NULL,
	[CountNo] [smallint] NOT NULL,
	[DocEntStatus] [tinyint] NULL,
	[DocRow] [smallint] NOT NULL,
	[FldCod] [smallint] NOT NULL,
	[CompData] [varchar](2000) NULL,
	[CompNum] [money] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_INVADDDOCINF] PRIMARY KEY CLUSTERED 
(
	[FiscalYear] ASC,
	[StoreNo] ASC,
	[DocType] ASC,
	[DocNo] ASC,
	[CountNo] ASC,
	[DocRow] ASC,
	[FldCod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[InvAddDocInf]  WITH NOCHECK ADD  CONSTRAINT [FK_InvAddDocInfDtlData] FOREIGN KEY([FiscalYear], [StoreNo], [DocType], [DocNo], [CountNo], [DocRow])
REFERENCES [ray].[InvDtlData] ([FiscalYear], [StoreNo], [DocType], [DocNo], [CountNo], [DocRow])
ON UPDATE CASCADE
ON DELETE CASCADE
NOT FOR REPLICATION
GO

ALTER TABLE [ray].[InvAddDocInf] CHECK CONSTRAINT [FK_InvAddDocInfDtlData]
GO
ALTER TABLE [ray].[InvAddDocInf]  WITH CHECK ADD  CONSTRAINT [FKF_InvAddDocInf_InvDocTyp_DocType] FOREIGN KEY([DocType])
REFERENCES [ray].[InvDocTyp] ([DocType])
GO

ALTER TABLE [ray].[InvAddDocInf] CHECK CONSTRAINT [FKF_InvAddDocInf_InvDocTyp_DocType]
GO
ALTER TABLE [ray].[InvAddDocInf]  WITH CHECK ADD  CONSTRAINT [FKF_InvAddDocInf_InvPrd_FiscalYear] FOREIGN KEY([FiscalYear])
REFERENCES [ray].[InvPrd] ([FiscalYear])
GO

ALTER TABLE [ray].[InvAddDocInf] CHECK CONSTRAINT [FKF_InvAddDocInf_InvPrd_FiscalYear]
GO
ALTER TABLE [ray].[InvAddDocInf]  WITH CHECK ADD  CONSTRAINT [FKF_InvAddDocInf_Store_StoreNo] FOREIGN KEY([StoreNo])
REFERENCES [ray].[Store] ([StoreNo])
GO

ALTER TABLE [ray].[InvAddDocInf] CHECK CONSTRAINT [FKF_InvAddDocInf_Store_StoreNo]
GO
ALTER TABLE [ray].[InvAddDocInf] ADD  CONSTRAINT [DF__InvAddDoc__StkCo__332B7579]  DEFAULT ((0)) FOR [StkCountNo]
GO
ALTER TABLE [ray].[InvAddDocInf] ADD  CONSTRAINT [DF__InvAddDoc__Count__341F99B2]  DEFAULT ((0)) FOR [CountNo]
GO
ALTER TABLE [ray].[InvAddDocInf] ADD  DEFAULT (newid()) FOR [RowGuid]