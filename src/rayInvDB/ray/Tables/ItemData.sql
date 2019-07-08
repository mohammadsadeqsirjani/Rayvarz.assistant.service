CREATE TABLE [ray].[ItemData](
	[PartNo] [varchar](20) NOT NULL,
	[PartNoDsc] [varchar](1000) NULL,
	[PartLtnDsc] [varchar](255) NULL,
	[TechnicalNo] [varchar](60) NULL,
	[PartPrvNo] [varchar](20) NULL,
	[PartGrp] [varchar](20) NULL,
	[Supplier] [varchar](10) NULL,
	[InvDefCenter] [int] NULL,
	[Center] [int] NULL,
	[UntCode] [char](2) NOT NULL,
	[TtlUntCode] [char](2) NULL,
	[TtlUnt] [char](2) NULL,
	[PrtnUnt] [char](2) NULL,
	[OrdrUnt] [char](2) NULL,
	[BomUnt] [char](2) NULL,
	[ExchngTtlToInv] [float] NULL,
	[ExchngPrtnToInv] [float] NULL,
	[ExchngTtlToPrtn] [float] NULL,
	[ExchngOrdrToInv] [float] NULL,
	[ExchngBomToInv] [numeric](38, 9) NULL,
	[ExchangeTtlUntToUnt] [money] NULL,
	[ExchngSaleToDis] [money] NULL,
	[RcptType] [int] NULL,
	[ConsType] [int] NULL,
	[MapNo] [varchar](60) NULL,
	[KeepType] [smallint] NULL,
	[ImpType] [tinyint] NULL,
	[UsedCode] [tinyint] NULL,
	[ProdType] [smallint] NULL,
	[TransportType] [tinyint] NULL,
	[TrnsPkgTyp] [smallint] NULL,
	[PureWght] [money] NULL,
	[UnpureWght] [money] NULL,
	[BoxWght] [money] NULL,
	[WghtUnt] [char](2) NULL,
	[Length] [money] NULL,
	[LenUnt] [char](2) NULL,
	[Width] [money] NULL,
	[WidthUnt] [char](2) NULL,
	[High] [money] NULL,
	[HighUnt] [char](2) NULL,
	[MultyMediaDoc] [int] NULL,
	[MultyMediaDocX] [char](36) NULL,
	[OrderMetType] [tinyint] NULL,
	[ToolRate] [money] NULL,
	[TimeOfProcure] [smallint] NULL,
	[SpanTime] [smallint] NULL,
	[MinOrderPoint] [money] NULL,
	[AvgMonCons] [money] NULL,
	[SafetyStock] [money] NULL,
	[OrderPoint] [money] NULL,
	[OptmOrdQty] [money] NULL,
	[LeadTime] [smallint] NULL,
	[IsPrdBatch] [tinyint] NULL,
	[ExpDue] [smallint] NULL,
	[PartSpecId1] [varchar](10) NULL,
	[PartSpecId2] [varchar](10) NULL,
	[PartSpecId3] [varchar](10) NULL,
	[PartSpecId4] [varchar](10) NULL,
	[PartAdjTyp1] [smallint] NULL,
	[PartAdjTyp2] [smallint] NULL,
	[PartAdjTyp3] [smallint] NULL,
	[PartAdjTyp4] [smallint] NULL,
	[Salable] [tinyint] NULL,
	[IsMakePart] [tinyint] NULL,
	[IsMtrl] [tinyint] NULL,
	[IsMchnPart] [tinyint] NULL,
	[IsTools] [tinyint] NULL,
	[IsAsset] [tinyint] NULL,
	[IsMold] [tinyint] NULL,
	[ISOfsTool] [tinyint] NULL,
	[IsConsTool] [tinyint] NULL,
	[IsFccPart] [tinyint] NULL,
	[OrdrNo] [bigint] NULL,
	[NotActiv] [tinyint] NULL,
	[IsCut] [tinyint] NULL,
	[DisMinReq] [money] NULL,
	[MinFeed] [money] NULL,
	[MaxFeed] [money] NULL,
	[CreateDate] [char](8) NULL,
	[AmtPtn] [money] NULL,
	[DisPrdPercent] [money] NULL,
	[PurityDegree] [money] NULL,
	[PurityDegreeRatio] [money] NULL,
	[IsFeed] [tinyint] NULL,
	[OlympPrice] [money] NULL,
	[IsExpired] [tinyint] NULL,
	[CostAccType] [smallint] NULL,
	[SaleRsv] [money] NULL,
	[PrdPartType] [smallint] NULL,
	[ExpiredDuration] [int] NULL,
	[IsSCMMaterial] [bit] NULL,
	[WUPkgTyp] [money] NULL,
	[IncDeliver] [money] NULL,
	[IsPrdPart] [tinyint] NULL,
	[UnqDsc] [varchar](500) NULL,
	[Sarakperc] [money] NULL,
	[StorePerc] [money] NULL,
	[StoreAmt] [money] NULL,
	[LastChngDate] [char](8) NULL,
	[NationalPartCod] [varchar](20) NULL,
	[Act3] [int] NULL,
	[PurityDegreePharmacy] [money] NULL,
	[CostomsNo] [varchar](10) NULL,
	[PartComent] [varchar](2000) NULL,
	[ItemCode] [varchar](10) NULL,
	[AstCoClassify] [int] NULL,
	[AstDpType] [varchar](20) NULL,
	[AstUsedTime] [int] NULL,
	[AstDpRate] [money] NULL,
	[AstUnt] [tinyint] NULL,
	[PrdPartGrp] [varchar](20) NULL,
	[branch_filter_spec] [varchar](2000) NULL,
	[_branch_filter_spec] [varchar](2000) NULL,
	[IsPack] [tinyint] NULL,
	[IsShop] [tinyint] NULL,
	[Categorize] [int] NULL,
	[IsLabTools] [tinyint] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
	[SeasonalTradePartType] [tinyint] NULL,
 CONSTRAINT [PK_ITEMDATA] PRIMARY KEY CLUSTERED 
(
	[PartNo] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemData_Act3] FOREIGN KEY([Act3])
REFERENCES [ray].[Center] ([Center])
GO

ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemData_Act3]
GO
ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemData_Center] FOREIGN KEY([Center])
REFERENCES [ray].[Center] ([Center])
GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemData_Center]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemData_CodingCategorize] FOREIGN KEY([Categorize])
--REFERENCES [ray].[CodingCategorize] ([Categorize])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemData_CodingCategorize]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemData_CostAccType] FOREIGN KEY([CostAccType])
--REFERENCES [ray].[CostAccType] ([AccType])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemData_CostAccType]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemData_FCC_List] FOREIGN KEY([ItemCode])
--REFERENCES [ray].[FCC_List] ([ItemCode])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemData_FCC_List]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemData_InvDefCenter] FOREIGN KEY([InvDefCenter])
--REFERENCES [ray].[Center] ([Center])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemData_InvDefCenter]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ITEMDATA_ITEMDATA__UNIT] FOREIGN KEY([TtlUntCode])
--REFERENCES [ray].[Unit] ([UntCode])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ITEMDATA_ITEMDATA__UNIT]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemData_ItemDataId1] FOREIGN KEY([PartSpecId1])
--REFERENCES [ray].[PartSpecId1] ([PartSpecId1])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemData_ItemDataId1]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemData_ItemDataId2] FOREIGN KEY([PartSpecId2])
--REFERENCES [ray].[PartSpecId2] ([PartSpecId2])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemData_ItemDataId2]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemData_ItemDataId3] FOREIGN KEY([PartSpecId3])
--REFERENCES [ray].[PartSpecId3] ([PartSpecId3])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemData_ItemDataId3]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemData_ItemDataId4] FOREIGN KEY([PartSpecId4])
--REFERENCES [ray].[PartSpecId4] ([PartSpecId4])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemData_ItemDataId4]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemData_PartAdjTyp1] FOREIGN KEY([PartAdjTyp1])
--REFERENCES [ray].[PartAdjTyp] ([PartAdjTyp])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemData_PartAdjTyp1]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemData_PartAdjTyp2] FOREIGN KEY([PartAdjTyp2])
--REFERENCES [ray].[PartAdjTyp] ([PartAdjTyp])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemData_PartAdjTyp2]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemData_PartAdjTyp3] FOREIGN KEY([PartAdjTyp3])
--REFERENCES [ray].[PartAdjTyp] ([PartAdjTyp])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemData_PartAdjTyp3]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemData_PartAdjTyp4] FOREIGN KEY([PartAdjTyp4])
--REFERENCES [ray].[PartAdjTyp] ([PartAdjTyp])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemData_PartAdjTyp4]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemData_PrdPartGrp] FOREIGN KEY([PrdPartGrp])
--REFERENCES [ray].[PrdPartGrp] ([PrdPartGrp])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemData_PrdPartGrp]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataBomUnt] FOREIGN KEY([BomUnt])
--REFERENCES [ray].[Unit] ([UntCode])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataBomUnt]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataCnsTyp] FOREIGN KEY([ConsType])
--REFERENCES [ray].[InvCnsTyp] ([ConsType])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataCnsTyp]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataHighUnt] FOREIGN KEY([HighUnt])
--REFERENCES [ray].[Unit] ([UntCode])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataHighUnt]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataImage] FOREIGN KEY([MultyMediaDoc])
--REFERENCES [ray].[MultyMediaDoc] ([MultyMediaDoc])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataImage]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataImpDg] FOREIGN KEY([ImpType])
--REFERENCES [ray].[InvImpDg] ([ImpType])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataImpDg]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataInvRcptTyp] FOREIGN KEY([RcptType])
--REFERENCES [ray].[InvRcptTyp] ([RcptType])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataInvRcptTyp]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataKeppTyp] FOREIGN KEY([KeepType])
--REFERENCES [ray].[Inv_KeepTyp] ([KeepType])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataKeppTyp]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataLenUnt] FOREIGN KEY([LenUnt])
--REFERENCES [ray].[Unit] ([UntCode])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataLenUnt]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataOrdrUnt] FOREIGN KEY([OrdrUnt])
--REFERENCES [ray].[Unit] ([UntCode])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataOrdrUnt]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataOrdTyp] FOREIGN KEY([OrderMetType])
--REFERENCES [ray].[OrdrTyp] ([OrderMetType])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataOrdTyp]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataPartGrp] FOREIGN KEY([PartGrp])
--REFERENCES [ray].[PartGrp] ([PartGrp])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataPartGrp]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataPartUser] FOREIGN KEY([UsedCode])
--REFERENCES [ray].[PartUsed] ([UsedCode])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataPartUser]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataPkgTyp] FOREIGN KEY([TrnsPkgTyp])
--REFERENCES [ray].[PkgTyp] ([PakgType])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataPkgTyp]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataPrdSts] FOREIGN KEY([ProdType])
--REFERENCES [ray].[PrdSts] ([ProdType])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataPrdSts]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataPrtnUnt] FOREIGN KEY([PrtnUnt])
--REFERENCES [ray].[Unit] ([UntCode])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataPrtnUnt]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataSupplier] FOREIGN KEY([Supplier])
--REFERENCES [ray].[Supplier] ([Supplier])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataSupplier]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataTrnTyp] FOREIGN KEY([TransportType])
--REFERENCES [ray].[TrnTyp] ([TransportType])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataTrnTyp]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataTtlUnt] FOREIGN KEY([TtlUnt])
--REFERENCES [ray].[Unit] ([UntCode])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataTtlUnt]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataUntCode] FOREIGN KEY([UntCode])
--REFERENCES [ray].[Unit] ([UntCode])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataUntCode]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataWghtUnt] FOREIGN KEY([WghtUnt])
--REFERENCES [ray].[Unit] ([UntCode])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataWghtUnt]
--GO
--ALTER TABLE [ray].[ItemData]  WITH CHECK ADD  CONSTRAINT [FK_ItemDataWidthUnt] FOREIGN KEY([WidthUnt])
--REFERENCES [ray].[Unit] ([UntCode])
--GO

--ALTER TABLE [ray].[ItemData] CHECK CONSTRAINT [FK_ItemDataWidthUnt]
--GO
ALTER TABLE [ray].[ItemData] ADD  DEFAULT ((0)) FOR [Salable]
GO
ALTER TABLE [ray].[ItemData] ADD  DEFAULT ((0)) FOR [IsTools]
GO
ALTER TABLE [ray].[ItemData] ADD  DEFAULT ((100)) FOR [PurityDegree]
GO
ALTER TABLE [ray].[ItemData] ADD  DEFAULT ((100)) FOR [PurityDegreeRatio]
GO
ALTER TABLE [ray].[ItemData] ADD  DEFAULT ((0)) FOR [IsFeed]
GO
ALTER TABLE [ray].[ItemData] ADD  DEFAULT ((0)) FOR [IsExpired]
GO
ALTER TABLE [ray].[ItemData] ADD  DEFAULT ((0)) FOR [IsSCMMaterial]
GO
ALTER TABLE [ray].[ItemData] ADD  DEFAULT ((0)) FOR [WUPkgTyp]
GO
ALTER TABLE [ray].[ItemData] ADD  DEFAULT ((0)) FOR [IsPrdPart]
GO
ALTER TABLE [ray].[ItemData] ADD  DEFAULT ((100)) FOR [PurityDegreePharmacy]
GO
ALTER TABLE [ray].[ItemData] ADD  DEFAULT ((0)) FOR [IsPack]
GO
ALTER TABLE [ray].[ItemData] ADD  DEFAULT ((0)) FOR [IsShop]
GO
ALTER TABLE [ray].[ItemData] ADD  DEFAULT ((0)) FOR [IsLabTools]
GO
ALTER TABLE [ray].[ItemData] ADD  DEFAULT (newid()) FOR [RowGuid]