CREATE TABLE [ray].[Store](
	[StoreNo] [varchar](6) NOT NULL,
	[StoreDsc] [varchar](45) NULL,
	[StoreLtnDsc] [varchar](45) NULL,
	[WrkShp] [int] NULL,
	[Center] [int] NULL,
	[StoreMan] [varchar](40) NULL,
	[StoreAddr] [varchar](120) NULL,
	[PostCode] [varchar](15) NULL,
	[PostBoxNo] [varchar](15) NULL,
	[StoreTelNo] [varchar](60) NULL,
	[FaxNo] [varchar](20) NULL,
	[PriceMet] [tinyint] NULL,
	[RtrnBuyMet] [tinyint] NULL,
	[RtrnStrReason] [tinyint] NULL,
	[OpnPrcMet] [tinyint] NULL,
	[OpnPrcMetShop] [tinyint] NULL,
	[StoreCountNo] [smallint] NULL,
	[AnbarGarType] [tinyint] NULL,
	[StoreStatus] [tinyint] NULL,
	[ChkStartDate] [char](8) NULL,
	[ChkEndDate] [char](8) NULL,
	[SerialAcc] [tinyint] NULL,
	[Sorttag] [tinyint] NULL,
	[Nexttagp] [tinyint] NULL,
	[Gentagp] [tinyint] NULL,
	[ValueTag] [money] NULL,
	[Countno] [smallint] NULL,
	[SerialTyp] [tinyint] NULL,
	[Salable] [tinyint] NULL,
	[IsMtrl] [tinyint] NULL,
	[IsMchnPart] [tinyint] NULL,
	[IsTools] [tinyint] NULL,
	[IsAsset] [tinyint] NULL,
	[IsMold] [tinyint] NULL,
	[ISOfsTool] [tinyint] NULL,
	[IsFccPart] [tinyint] NULL,
	[IsConsTool] [tinyint] NULL,
	[IsPrice] [tinyint] NULL,
	[RelatedBuy] [tinyint] NULL,
	[SerialInOrder] [tinyint] NULL,
	[IsIssueWithRef] [tinyint] NULL,
	[IsRcptWithRef] [tinyint] NULL,
	[MstrUpdDate] [char](8) NULL,
	[IsOwnerShip] [tinyint] NULL,
	[ShopCode] [varchar](6) NULL,
	[RelatedShop] [tinyint] NULL,
	[DayRcpt] [smallint] NULL,
	[PerDiscount] [money] NULL,
	[MaxDiscount] [money] NULL,
	[DiscountUpdDate] [char](8) NULL,
	[RcptDocDis] [money] NULL,
	[Active] [tinyint] NULL,
	[TwoUnt] [tinyint] NULL,
	[IsUnique] [tinyint] NULL,
	[IsRtrnBuyWithRef] [tinyint] NULL,
	[IsRtrnStrWithRef] [tinyint] NULL,
	[DestStore] [varchar](6) NULL,
	[RelatedAst] [tinyint] NULL,
	[SerialFactor] [tinyint] NULL,
	[IsShop] [tinyint] NULL,
	[IsPrdPart] [tinyint] NULL,
	[IsResQtySale] [tinyint] NULL,
	[IsLaboratory] [tinyint] NULL,
	[NotCnfrmLab] [tinyint] NULL,
	[TestLab] [tinyint] NULL,
	[PeriodTestLab] [tinyint] NULL,
	[CnfrmLab] [tinyint] NULL,
	[LabReng] [int] NULL,
	[LabOrderBy] [varchar](100) NULL,
	[Mobile] [varchar](60) NULL,
	[TypeId] [char](36) NULL,
	[StateId] [char](36) NULL,
	[AndPartGrp] [tinyint] NULL,
	[RelatedAmtBuy] [tinyint] NULL,
	[StoreBinNo] [tinyint] NULL,
	[IsWoFeed] [tinyint] NULL,
	[IsMrpStk] [tinyint] NULL,
	[IsMrpReq] [tinyint] NULL,
	[ChkSohRqst] [tinyint] NULL,
	[ChkSohIssue] [tinyint] NULL,
	[BinReng] [int] NULL,
	[Differcnt] [tinyint] NULL,
	[IsStoreAmt] [tinyint] NULL,
	[IsShowTowUntInRef] [tinyint] NULL,
	[FiFoOrder] [tinyint] NULL,
	[IsBatchSplit] [tinyint] NULL,
	[PriceGroup] [tinyint] NULL,
	[ValueTagMain] [money] NULL,
	[IsSamePart] [tinyint] NULL,
	[IsBisector] [tinyint] NULL,
	[IsNotActvCnfrmLvl] [tinyint] NULL,
	[IsBreakRow] [tinyint] NULL,
	[IsResQtyPrd] [tinyint] NULL,
	[SaveDoc] [tinyint] NULL,
	[NotCheckOrderPoint] [tinyint] NULL,
	[RelatedCrdt] [tinyint] NULL,
	[Diffper] [money] NULL,
	[IsStoreAnalysis] [tinyint] NULL,
	[IsSendBuyCnfrm] [tinyint] NULL,
	[IsEnableAmtForissue] [tinyint] NULL,
	[IsWbsKAnalysis] [tinyint] NULL,
	[IsChangeAfterRef] [tinyint] NULL,
	[IsItemRcptWithRef] [tinyint] NULL,
	[IsTempRcptWithRef] [tinyint] NULL,
	[NotSerialPrice] [tinyint] NULL,
	[IsCloseCount] [tinyint] NULL,
	[IsPack] [tinyint] NULL,
	[IsRealatedQc] [tinyint] NULL,
	[IsTaxTollPrice] [tinyint] NULL,
	[IsAllYrOrdrPoint] [tinyint] NULL,
	[TmpIssueReng] [int] NULL,
	[RcvWeight] [tinyint] NULL,
	[IsOpenYear] [tinyint] NULL,
	[ConnectToStoreSystem] [bit] NULL,
	[UseSerial] [bit] NULL,
	[UseSerialStep] [tinyint] NULL,
	[IsRqstBuyWithRef] [tinyint] NULL,
	[IsShowSohInRqst] [tinyint] NULL,
	[IsPlaqueEqualSerial] [tinyint] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
	[IsBetweenRcptWithRef] [tinyint] NULL,
	[IsLabTools] [tinyint] NULL,
 CONSTRAINT [PK_STORE] PRIMARY KEY CLUSTERED 
(
	[StoreNo] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[Store]  WITH CHECK ADD  CONSTRAINT [FK_Store_DestStore] FOREIGN KEY([DestStore])
REFERENCES [ray].[Store] ([StoreNo])
GO

--ALTER TABLE [ray].[Store] CHECK CONSTRAINT [FK_Store_DestStore]
--GO
--ALTER TABLE [ray].[Store]  WITH CHECK ADD  CONSTRAINT [FK_Store_OpnPrcMetShop] FOREIGN KEY([OpnPrcMetShop])
--REFERENCES [ray].[InvOpnRcptPrc] ([OpnPriceMet])
--GO

--ALTER TABLE [ray].[Store] CHECK CONSTRAINT [FK_Store_OpnPrcMetShop]
--GO
--ALTER TABLE [ray].[Store]  WITH CHECK ADD  CONSTRAINT [FK_Store_Store] FOREIGN KEY([ShopCode])
--REFERENCES [ray].[Store] ([StoreNo])
--GO

--ALTER TABLE [ray].[Store] CHECK CONSTRAINT [FK_Store_Store]
--GO
--ALTER TABLE [ray].[Store]  WITH CHECK ADD  CONSTRAINT [FK_Store_StoreState] FOREIGN KEY([StateId])
--REFERENCES [ray].[StoreState] ([Id])
--GO

--ALTER TABLE [ray].[Store] CHECK CONSTRAINT [FK_Store_StoreState]
--GO
--ALTER TABLE [ray].[Store]  WITH CHECK ADD  CONSTRAINT [FK_Store_StoreType] FOREIGN KEY([TypeId])
--REFERENCES [ray].[StoreType] ([Id])
--GO

--ALTER TABLE [ray].[Store] CHECK CONSTRAINT [FK_Store_StoreType]
--GO
--ALTER TABLE [ray].[Store]  WITH CHECK ADD  CONSTRAINT [FK_StoreCenter] FOREIGN KEY([Center])
--REFERENCES [ray].[Center] ([Center])
--GO

--ALTER TABLE [ray].[Store] CHECK CONSTRAINT [FK_StoreCenter]
--GO
--ALTER TABLE [ray].[Store]  WITH CHECK ADD  CONSTRAINT [FK_StoreOpnRcptPrc] FOREIGN KEY([OpnPrcMet])
--REFERENCES [ray].[InvOpnRcptPrc] ([OpnPriceMet])
--GO

--ALTER TABLE [ray].[Store] CHECK CONSTRAINT [FK_StoreOpnRcptPrc]
--GO
--ALTER TABLE [ray].[Store]  WITH CHECK ADD  CONSTRAINT [FK_StorePrcMtTyp] FOREIGN KEY([PriceMet])
--REFERENCES [ray].[InvPrcMtTyp] ([PriceMet])
--GO

--ALTER TABLE [ray].[Store] CHECK CONSTRAINT [FK_StorePrcMtTyp]
--GO
--ALTER TABLE [ray].[Store]  WITH CHECK ADD  CONSTRAINT [FK_StoreRtrnBuyRs] FOREIGN KEY([RtrnBuyMet])
--REFERENCES [ray].[InvRtrnBuyRs] ([RtrnBuyMet])
--GO

--ALTER TABLE [ray].[Store] CHECK CONSTRAINT [FK_StoreRtrnBuyRs]
--GO
--ALTER TABLE [ray].[Store]  WITH CHECK ADD  CONSTRAINT [FK_StoreRtrStrRs] FOREIGN KEY([RtrnStrReason])
--REFERENCES [ray].[InvRtrStrRs] ([RtrnStrReason])
--GO

--ALTER TABLE [ray].[Store] CHECK CONSTRAINT [FK_StoreRtrStrRs]
--GO
--ALTER TABLE [ray].[Store]  WITH CHECK ADD  CONSTRAINT [FK_StoreStrvzcnt] FOREIGN KEY([StoreStatus])
--REFERENCES [ray].[InvStrvzcnt] ([StoreStatus])
--GO

--ALTER TABLE [ray].[Store] CHECK CONSTRAINT [FK_StoreStrvzcnt]
--GO
--ALTER TABLE [ray].[Store]  WITH CHECK ADD  CONSTRAINT [FK_StoreWkrShop] FOREIGN KEY([WrkShp])
--REFERENCES [ray].[WrkShop] ([WrkShp])
--GO

--ALTER TABLE [ray].[Store] CHECK CONSTRAINT [FK_StoreWkrShop]
--GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [StoreCountNo]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [TwoUnt]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsUnique]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsRtrnBuyWithRef]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsRtrnStrWithRef]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [RelatedAst]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsShop]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsPrdPart]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsResQtySale]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsLaboratory]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [NotCnfrmLab]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [TestLab]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [PeriodTestLab]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [CnfrmLab]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [AndPartGrp]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [RelatedAmtBuy]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [StoreBinNo]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsWoFeed]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsMrpStk]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsMrpReq]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [ChkSohRqst]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [ChkSohIssue]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [Differcnt]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsStoreAmt]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsShowTowUntInRef]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((1)) FOR [FiFoOrder]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsBatchSplit]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsSamePart]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsBisector]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsNotActvCnfrmLvl]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsBreakRow]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsResQtyPrd]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [SaveDoc]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [NotCheckOrderPoint]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [RelatedCrdt]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsStoreAnalysis]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsSendBuyCnfrm]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsEnableAmtForissue]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsWbsKAnalysis]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsChangeAfterRef]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsItemRcptWithRef]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsTempRcptWithRef]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [NotSerialPrice]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsCloseCount]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsPack]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsRealatedQc]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsTaxTollPrice]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsAllYrOrdrPoint]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [RcvWeight]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsOpenYear]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [ConnectToStoreSystem]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [UseSerial]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsRqstBuyWithRef]
GO
ALTER TABLE [ray].[Store] ADD  CONSTRAINT [DF__Store__IsShowSoh__6CCE0729]  DEFAULT ((0)) FOR [IsShowSohInRqst]
GO
ALTER TABLE [ray].[Store] ADD  CONSTRAINT [DF__Store__IsPlaqueE__6DC22B62]  DEFAULT ((0)) FOR [IsPlaqueEqualSerial]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT (newid()) FOR [RowGuid]
GO
ALTER TABLE [ray].[Store] ADD  DEFAULT ((0)) FOR [IsBetweenRcptWithRef]