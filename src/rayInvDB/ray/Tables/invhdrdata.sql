CREATE TABLE [ray].[invhdrdata](
	[FiscalYear] [int] NOT NULL,
	[StoreNo] [varchar](6) NOT NULL,
	[DocType] [tinyint] NOT NULL,
	[DocNo] [int] NOT NULL,
	[CountNo] [smallint] NOT NULL,
	[DocSeq] [int] NULL,
	[DestStoreNo] [varchar](6) NULL,
	[StkCountNo] [smallint] NULL,
	[DocEntStatus] [tinyint] NULL,
	[DocStatus] [tinyint] NULL,
	[UpdateSeq] [smallint] NULL,
	[SendSeq] [smallint] NULL,
	[ValueSeq] [smallint] NULL,
	[AccDocSeq] [smallint] NULL,
	[SumQty] [money] NULL,
	[SumAmt] [money] NULL,
	[CreateDate] [char](8) NULL,
	[UpdateDate] [char](8) NULL,
	[MultyMediaDoc] [int] NULL,
	[MultyMediaDocX] [char](36) NULL,
	[OrgDocNo] [int] NULL,
	[OrgRefNo] [int] NULL,
	[LockInd] [tinyint] NULL,
	[CnfrmLvl] [smallint] NULL,
	[EndRcpt] [tinyint] NULL,
	[RaySys] [varchar](30) NULL,
	[PriceDocSt] [tinyint] NULL,
	[DocCounter] [int] NULL,
	[IsSendAst] [tinyint] NULL,
	[GdiRefDocNo] [int] NULL,
	[GdiRefFiscalYear] [int] NULL,
	[GdiRefDocType] [tinyint] NULL,
	[BuyType] [smallint] NULL,
	[SumWeight] [money] NULL,
	[IsEditAcnt] [tinyint] NULL,
	[UserId] [varchar](12) NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_INVHDRDATA] PRIMARY KEY CLUSTERED 
(
	[FiscalYear] ASC,
	[StoreNo] ASC,
	[DocType] ASC,
	[DocNo] ASC,
	[CountNo] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO
--ALTER TABLE [ray].[invhdrdata]  WITH CHECK ADD  CONSTRAINT [FK_IInvHdrDataImage] FOREIGN KEY([MultyMediaDoc])
--REFERENCES [ray].[MultyMediaDoc] ([MultyMediaDoc])
--GO

--ALTER TABLE [ray].[invhdrdata] CHECK CONSTRAINT [FK_IInvHdrDataImage]
--GO
--ALTER TABLE [ray].[invhdrdata]  WITH CHECK ADD  CONSTRAINT [FK_InvHdrData_UserId] FOREIGN KEY([UserId])
--REFERENCES [ray].[UserId] ([UserId])
--GO

--ALTER TABLE [ray].[invhdrdata] CHECK CONSTRAINT [FK_InvHdrData_UserId]
--GO
--ALTER TABLE [ray].[invhdrdata]  WITH CHECK ADD  CONSTRAINT [FK_InvHdrDataDocEntSt] FOREIGN KEY([DocEntStatus])
--REFERENCES [ray].[InvDocEntSt] ([DocEntStatus])
--GO

--ALTER TABLE [ray].[invhdrdata] CHECK CONSTRAINT [FK_InvHdrDataDocEntSt]
--GO
--ALTER TABLE [ray].[invhdrdata]  WITH CHECK ADD  CONSTRAINT [FK_InvHdrDataDocSt] FOREIGN KEY([DocStatus])
--REFERENCES [ray].[InvDocSt] ([DocStatus])
--GO

--ALTER TABLE [ray].[invhdrdata] CHECK CONSTRAINT [FK_InvHdrDataDocSt]
--GO
--ALTER TABLE [ray].[invhdrdata]  WITH CHECK ADD  CONSTRAINT [FK_InvHdrDataDocTyp] FOREIGN KEY([DocType])
--REFERENCES [ray].[InvDocTyp] ([DocType])
--GO

--ALTER TABLE [ray].[invhdrdata] CHECK CONSTRAINT [FK_InvHdrDataDocTyp]
--GO
--ALTER TABLE [ray].[invhdrdata]  WITH CHECK ADD  CONSTRAINT [FK_InvHdrDataPrd] FOREIGN KEY([FiscalYear])
--REFERENCES [ray].[InvPrd] ([FiscalYear])
--GO

--ALTER TABLE [ray].[invhdrdata] CHECK CONSTRAINT [FK_InvHdrDataPrd]
--GO
--ALTER TABLE [ray].[invhdrdata]  WITH CHECK ADD  CONSTRAINT [FK_InvHdrDataStore] FOREIGN KEY([StoreNo])
--REFERENCES [ray].[Store] ([StoreNo])
--GO

--ALTER TABLE [ray].[invhdrdata] CHECK CONSTRAINT [FK_InvHdrDataStore]
--GO
ALTER TABLE [ray].[invhdrdata] ADD  CONSTRAINT [DF__InvHdrDat__Count__7BF04F28]  DEFAULT ((0)) FOR [CountNo]
GO
ALTER TABLE [ray].[invhdrdata] ADD  CONSTRAINT [DF__InvHdrDat__StkCo__7CE47361]  DEFAULT ((0)) FOR [StkCountNo]
GO
ALTER TABLE [ray].[invhdrdata] ADD  CONSTRAINT [DF__InvHdrDat__IsSen__7DD8979A]  DEFAULT ((0)) FOR [IsSendAst]
GO
ALTER TABLE [ray].[invhdrdata] ADD  DEFAULT ((0)) FOR [IsEditAcnt]
GO
ALTER TABLE [ray].[invhdrdata] ADD  DEFAULT (newid()) FOR [RowGuid]