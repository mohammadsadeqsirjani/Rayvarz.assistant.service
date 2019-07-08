CREATE TABLE [ray].[InvDtlData](
	[FiscalYear] [int] NOT NULL,
	[StoreNo] [varchar](6) NOT NULL,
	[DocType] [tinyint] NOT NULL,
	[DocNo] [int] NOT NULL,
	[DestStoreNo] [varchar](6) NULL,
	[StkCountNo] [smallint] NULL,
	[CountNo] [smallint] NOT NULL,
	[DocEntStatus] [tinyint] NULL,
	[DocRow] [smallint] NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[Serial] [varchar](50) NULL,
	[Center] [int] NULL,
	[DocDate] [char](8) NULL,
	[UntCode] [char](2) NULL,
	[Qty] [money] NULL,
	[AudtQty] [money] NULL,
	[RefConsQty] [money] NULL,
	[RefOrder] [money] NULL,
	[Amt] [money] NULL,
	[CompAmt] [money] NULL,
	[AudtAmt] [money] NULL,
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
	[AudtReason] [tinyint] NULL,
	[PrcDate] [char](8) NULL,
	[PrcSeqDoc] [smallint] NULL,
	[RefStatus] [tinyint] NULL,
	[DetValSeq] [smallint] NULL,
	[AccDocSeq] [smallint] NULL,
	[Weight] [money] NULL,
	[RtnQty] [money] NULL,
	[RfDocNo] [int] NULL,
	[RfDocRow] [smallint] NULL,
	[AccDocNo] [varchar](50) NULL,
	[SetPrice] [tinyint] NULL,
	[UpdTime] [int] NULL,
	[SetVoid] [tinyint] NULL,
	[IsNewYear] [tinyint] NULL,
	[FactorNo] [varchar](30) NULL,
	[SalePartNo] [varchar](20) NULL,
	[SaleQty] [money] NULL,
	[SaleRow] [smallint] NULL,
	[TraceKey] [varchar](500) NULL,
	[StoreAmt] [money] NULL,
	[Act3] [int] NULL,
	[TollAmt] [money] NULL,
	[TaxAmt] [money] NULL,
	[DocDsc] [varchar](6000) NULL,
	[FactorDate] [char](8) NULL,
	[TreatyNo] [varchar](20) NULL,
	[OldRow] [smallint] NULL,
	[GdiRefDocNo] [int] NULL,
	[GdiRefDocRow] [smallint] NULL,
	[GdiRefPartNo] [varchar](20) NULL,
	[GdiRefStore] [varchar](6) NULL,
	[FromPlaque] [varchar](20) NULL,
	[ToPlaque] [varchar](20) NULL,
	[CntQty] [money] NULL,
	[OrgPelakNo] [varchar](20) NULL,
	[FlwCode] [varchar](20) NULL,
	[DespositRng] [int] NULL,
	[MeetingDate] [char](8) NULL,
	[Act4] [int] NULL,
	[Act5] [int] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
	[MultyMediaDocX] [char](36) NULL,
	[MultyMediaDoc] [int] NULL,
 CONSTRAINT [PK_INVDTLDATA] PRIMARY KEY CLUSTERED 
(
	[FiscalYear] ASC,
	[StoreNo] ASC,
	[DocType] ASC,
	[DocNo] ASC,
	[CountNo] ASC,
	[DocRow] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FK_IInvDtlData_ReqCenter] FOREIGN KEY([ReqCenter])
REFERENCES [ray].[Center] ([Center])
GO

ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_IInvDtlData_ReqCenter]
GO
ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FK_InvDtlData_Act3] FOREIGN KEY([Act3])
REFERENCES [ray].[Center] ([Center])
GO

ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlData_Act3]
GO
ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FK_InvDtlData_Act4] FOREIGN KEY([Act4])
REFERENCES [ray].[Center] ([Center])
GO

ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlData_Act4]
GO
ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FK_InvDtlData_Act5] FOREIGN KEY([Act5])
REFERENCES [ray].[Center] ([Center])
GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlData_Act5]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FK_InvDtlDataAudRsn] FOREIGN KEY([AudtReason])
--REFERENCES [ray].[InvAudRsn] ([AudtReason])
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlDataAudRsn]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FK_InvDtlDataCenter] FOREIGN KEY([Center])
--REFERENCES [ray].[Center] ([Center])
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlDataCenter]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FK_InvDtlDataCnsTyp] FOREIGN KEY([ConsType])
--REFERENCES [ray].[InvCnsTyp] ([ConsType])
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlDataCnsTyp]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH NOCHECK ADD  CONSTRAINT [FK_InvDtlDataHdrData] FOREIGN KEY([FiscalYear], [StoreNo], [DocType], [DocNo], [CountNo])
--REFERENCES [ray].[invhdrdata] ([FiscalYear], [StoreNo], [DocType], [DocNo], [CountNo])
--ON UPDATE CASCADE
--ON DELETE CASCADE
--NOT FOR REPLICATION
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlDataHdrData]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH NOCHECK ADD  CONSTRAINT [FK_InvDtlDataItemData] FOREIGN KEY([PartNo])
--REFERENCES [ray].[ItemData] ([PartNo])
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlDataItemData]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FK_InvDtlDataOrdr] FOREIGN KEY([OrderNo])
--REFERENCES [ray].[InvOrdr] ([OrdrNO])
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlDataOrdr]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FK_InvDtlDataRcptTyp] FOREIGN KEY([RcptType])
--REFERENCES [ray].[InvRcptTyp] ([RcptType])
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlDataRcptTyp]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FK_InvDtlDataRfSt] FOREIGN KEY([RefStatus])
--REFERENCES [ray].[InvRfSt] ([RefStatus])
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlDataRfSt]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FK_InvDtlDataRqTyp] FOREIGN KEY([ReqType])
--REFERENCES [ray].[InvReqTyp] ([ReqType])
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlDataRqTyp]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FK_InvDtlDataRtrnBuy] FOREIGN KEY([RtrnBuyReason])
--REFERENCES [ray].[InvRtrnBuy] ([RtrnBuyReason])
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlDataRtrnBuy]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FK_InvDtlDataRtrnStr] FOREIGN KEY([RtrnStrReason])
--REFERENCES [ray].[InvRtrnStr] ([RtrnStore])
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlDataRtrnStr]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FK_InvDtlDataSupplier] FOREIGN KEY([Supplier])
--REFERENCES [ray].[Supplier] ([Supplier])
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlDataSupplier]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FK_InvDtlDataUnit] FOREIGN KEY([UntCode])
--REFERENCES [ray].[Unit] ([UntCode])
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FK_InvDtlDataUnit]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FKF_InvDtlData_InvDocTyp_DocType] FOREIGN KEY([DocType])
--REFERENCES [ray].[InvDocTyp] ([DocType])
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FKF_InvDtlData_InvDocTyp_DocType]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FKF_InvDtlData_InvPrd_FiscalYear] FOREIGN KEY([FiscalYear])
--REFERENCES [ray].[InvPrd] ([FiscalYear])
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FKF_InvDtlData_InvPrd_FiscalYear]
--GO
--ALTER TABLE [ray].[InvDtlData]  WITH CHECK ADD  CONSTRAINT [FKF_InvDtlData_Store_StoreNo] FOREIGN KEY([StoreNo])
--REFERENCES [ray].[Store] ([StoreNo])
--GO

--ALTER TABLE [ray].[InvDtlData] CHECK CONSTRAINT [FKF_InvDtlData_Store_StoreNo]
--GO
ALTER TABLE [ray].[InvDtlData] ADD  DEFAULT ((0)) FOR [StkCountNo]
GO
ALTER TABLE [ray].[InvDtlData] ADD  DEFAULT ((0)) FOR [CountNo]
GO
ALTER TABLE [ray].[InvDtlData] ADD  DEFAULT (newid()) FOR [RowGuid]