CREATE TABLE [ray].[InvDoc](
	[RaySys] [varchar](12) NOT NULL,
	[FiscalYear] [int] NOT NULL,
	[StoreNo] [varchar](6) NOT NULL,
	[DocType] [tinyint] NOT NULL,
	[DocNo] [int] NOT NULL,
	[DocRow] [smallint] NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[Serial] [varchar](50) NULL,
	[Center] [int] NULL,
	[DocDate] [char](8) NULL,
	[UntCode] [char](2) NULL,
	[Qty] [money] NULL,
	[Amt] [money] NULL,
	[ConsType] [int] NULL,
	[RcptType] [int] NULL,
	[ReqType] [tinyint] NULL,
	[RtrnStrReason] [tinyint] NULL,
	[RtrnBuyReason] [tinyint] NULL,
	[ReqCenter] [int] NULL,
	[Supplier] [varchar](10) NULL,
	[OrderNo] [bigint] NULL,
	[BinNo] [varchar](35) NULL,
	[RegNo] [int] NULL,
	[LcNo] [varchar](12) NULL,
	[NeedDate] [char](8) NULL,
	[RunNo] [smallint] NULL,
	[Flag] [tinyint] NULL,
	[Error] [int] NULL,
	[InvRfDocNo] [int] NULL,
	[InvRfDocRow] [smallint] NULL,
	[RefConsQty] [money] NULL,
	[Cstmr] [varchar](10) NULL,
	[InvRfFiscalYear] [int] NULL,
	[SetVoid] [tinyint] NULL,
	[SalePartNo] [varchar](20) NULL,
	[SaleQty] [money] NULL,
	[SaleRow] [smallint] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
	[DocDsc] [varchar](255) NULL,
 CONSTRAINT [PK_INVDOC] PRIMARY KEY CLUSTERED 
(
	[RaySys] ASC,
	[FiscalYear] ASC,
	[StoreNo] ASC,
	[DocType] ASC,
	[DocNo] ASC,
	[DocRow] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[InvDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvDoc_Center] FOREIGN KEY([Center])
REFERENCES [ray].[Center] ([Center])
GO

ALTER TABLE [ray].[InvDoc] CHECK CONSTRAINT [FK_InvDoc_Center]
GO
ALTER TABLE [ray].[InvDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvDoc_CnsTyp] FOREIGN KEY([ConsType])
REFERENCES [ray].[InvCnsTyp] ([ConsType])
GO

ALTER TABLE [ray].[InvDoc] CHECK CONSTRAINT [FK_InvDoc_CnsTyp]
GO
--ALTER TABLE [ray].[InvDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvDoc_Cstmr] FOREIGN KEY([Cstmr])
--REFERENCES [ray].[Cstmr] ([Cstmr])
--GO

--ALTER TABLE [ray].[InvDoc] CHECK CONSTRAINT [FK_InvDoc_Cstmr]
--GO
ALTER TABLE [ray].[InvDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvDoc_DocType] FOREIGN KEY([DocType])
REFERENCES [ray].[InvDocTyp] ([DocType])
GO

ALTER TABLE [ray].[InvDoc] CHECK CONSTRAINT [FK_InvDoc_DocType]
GO
ALTER TABLE [ray].[InvDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvDoc_RaySys] FOREIGN KEY([RaySys])
REFERENCES [ray].[RaySys] ([RaySys])
GO

ALTER TABLE [ray].[InvDoc] CHECK CONSTRAINT [FK_InvDoc_RaySys]
GO
ALTER TABLE [ray].[InvDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvDoc_RcptTyp] FOREIGN KEY([RcptType])
REFERENCES [ray].[InvRcptTyp] ([RcptType])
GO

ALTER TABLE [ray].[InvDoc] CHECK CONSTRAINT [FK_InvDoc_RcptTyp]
GO
ALTER TABLE [ray].[InvDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvDoc_ReqCenter] FOREIGN KEY([ReqCenter])
REFERENCES [ray].[Center] ([Center])
GO

ALTER TABLE [ray].[InvDoc] CHECK CONSTRAINT [FK_InvDoc_ReqCenter]
GO
ALTER TABLE [ray].[InvDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvDoc_ReqTyp] FOREIGN KEY([ReqType])
REFERENCES [ray].[InvReqTyp] ([ReqType])
GO

ALTER TABLE [ray].[InvDoc] CHECK CONSTRAINT [FK_InvDoc_ReqTyp]
GO
ALTER TABLE [ray].[InvDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvDoc_RtrnBuy] FOREIGN KEY([RtrnBuyReason])
REFERENCES [ray].[InvRtrnBuy] ([RtrnBuyReason])
GO

ALTER TABLE [ray].[InvDoc] CHECK CONSTRAINT [FK_InvDoc_RtrnBuy]
GO
ALTER TABLE [ray].[InvDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvDoc_RtrnStr] FOREIGN KEY([RtrnStrReason])
REFERENCES [ray].[InvRtrnStr] ([RtrnStore])
GO

ALTER TABLE [ray].[InvDoc] CHECK CONSTRAINT [FK_InvDoc_RtrnStr]
GO
ALTER TABLE [ray].[InvDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvDoc_SalePartNo] FOREIGN KEY([SalePartNo])
REFERENCES [ray].[ItemData] ([PartNo])
GO

ALTER TABLE [ray].[InvDoc] CHECK CONSTRAINT [FK_InvDoc_SalePartNo]
GO
ALTER TABLE [ray].[InvDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvDoc_Store] FOREIGN KEY([StoreNo])
REFERENCES [ray].[Store] ([StoreNo])
GO

ALTER TABLE [ray].[InvDoc] CHECK CONSTRAINT [FK_InvDoc_Store]
GO
ALTER TABLE [ray].[InvDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvDoc_Supplier] FOREIGN KEY([Supplier])
REFERENCES [ray].[Supplier] ([Supplier])
GO

ALTER TABLE [ray].[InvDoc] CHECK CONSTRAINT [FK_InvDoc_Supplier]
GO
ALTER TABLE [ray].[InvDoc]  WITH CHECK ADD  CONSTRAINT [FK_InvDoc_Unit] FOREIGN KEY([UntCode])
REFERENCES [ray].[Unit] ([UntCode])
GO

ALTER TABLE [ray].[InvDoc] CHECK CONSTRAINT [FK_InvDoc_Unit]
GO
ALTER TABLE [ray].[InvDoc]  WITH NOCHECK ADD  CONSTRAINT [FK_InvDocPartNo] FOREIGN KEY([PartNo])
REFERENCES [ray].[ItemData] ([PartNo])
GO

ALTER TABLE [ray].[InvDoc] CHECK CONSTRAINT [FK_InvDocPartNo]
GO
ALTER TABLE [ray].[InvDoc] ADD  DEFAULT (newid()) FOR [RowGuid]