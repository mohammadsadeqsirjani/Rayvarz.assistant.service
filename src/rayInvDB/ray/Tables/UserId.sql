CREATE TABLE [ray].[UserId](
	[UserId] [varchar](12) NOT NULL,
	[UserName] [varchar](50) NULL,
	[UserLtnName] [varchar](50) NULL,
	[PassWord] [varchar](255) NULL,
	[NetUser] [varchar](50) NULL,
	[Email] [varchar](60) NULL,
	[Active] [tinyint] NULL,
	[PWSettingDat] [char](8) NULL,
	[BuyGrp] [tinyint] NULL,
	[BuyFlwGrp] [tinyint] NULL,
	[BuyGrpMng] [varchar](12) NULL,
	[BuyFlwGrpMng] [varchar](12) NULL,
	[BuyCstmPrs] [tinyint] NULL,
	[BuySrcSup] [tinyint] NULL,
	[AcntLung] [tinyint] NULL,
	[AcntLvl] [tinyint] NULL,
	[AcntEdtTyp] [tinyint] NULL,
	[AcntDspFrSeri1] [tinyint] NULL,
	[AcntDspFrSeri2] [tinyint] NULL,
	[AcntDspFrSeri3] [tinyint] NULL,
	[AcntDspToSeri1] [tinyint] NULL,
	[AcntDspToSeri2] [tinyint] NULL,
	[AcntDspToSeri3] [tinyint] NULL,
	[AcntAddFrSeri1] [tinyint] NULL,
	[AcntAddFrSeri2] [tinyint] NULL,
	[AcntAddFrSeri3] [tinyint] NULL,
	[AcntAddToSeri1] [tinyint] NULL,
	[AcntAddToSeri2] [tinyint] NULL,
	[AcntAddToSeri3] [tinyint] NULL,
	[AcntChgFrSeri1] [tinyint] NULL,
	[AcntChgFrSeri2] [tinyint] NULL,
	[AcntChgFrSeri3] [tinyint] NULL,
	[AcntChgToSeri1] [tinyint] NULL,
	[AcntChgToSeri2] [tinyint] NULL,
	[AcntChgToSeri3] [tinyint] NULL,
	[AcntDltFrSeri1] [tinyint] NULL,
	[AcntDltFrSeri2] [tinyint] NULL,
	[AcntDltFrSeri3] [tinyint] NULL,
	[AcntDltToSeri1] [tinyint] NULL,
	[AcntDltToSeri2] [tinyint] NULL,
	[AcntDltToSeri3] [tinyint] NULL,
	[AcntCnfFrSeri1] [tinyint] NULL,
	[AcntCnfFrSeri2] [tinyint] NULL,
	[AcntCnfFrSeri3] [tinyint] NULL,
	[AcntCnfToSeri1] [tinyint] NULL,
	[AcntCnfToSeri2] [tinyint] NULL,
	[AcntCnfToSeri3] [tinyint] NULL,
	[CashLvl] [tinyint] NULL,
	[OaUnitCod] [smallint] NULL,
	[OaIsResponsible] [tinyint] NULL,
	[InvStore] [varchar](1000) NULL,
	[InvCnfrmLvl] [tinyint] NULL,
	[InvSystemType] [tinyint] NULL,
	[InvImplType] [tinyint] NULL,
	[PayHokmLvl] [tinyint] NULL,
	[PayEblagLvl] [tinyint] NULL,
	[PayExtraLvl] [tinyint] NULL,
	[PayVarLvl] [tinyint] NULL,
	[PayExlLvl] [tinyint] NULL,
	[PayOutLvl] [tinyint] NULL,
	[PayLoanLvl] [tinyint] NULL,
	[PayLoanReqLvl] [tinyint] NULL,
	[PayCostLvl] [tinyint] NULL,
	[PayMissionLvl] [tinyint] NULL,
	[PayEvalLvl] [tinyint] NULL,
	[PayWrkTimLvl] [tinyint] NULL,
	[PayDayoffReqLvl] [tinyint] NULL,
	[ShopStore] [varchar](1000) NULL,
	[ShopSystemType] [tinyint] NULL,
	[ShopCnfrmLvl] [tinyint] NULL,
	[ShopStoreCash] [varchar](255) NULL,
	[SaleStore] [varchar](255) NULL,
	[AstUpdOpt] [datetime] NULL,
	[AstUpdCard] [datetime] NULL,
	[AstUsrTyp] [tinyint] NULL,
	[ActnCenter] [varchar](1000) NULL,
	[EmpTypPermission] [varchar](255) NULL,
	[SaleSerial] [varchar](10) NULL,
	[SaleDocHeader] [bit] NULL,
	[SaleDocDateHdrPermit] [bit] NULL,
	[AstTrustGrp] [int] NULL,
	[SaleCstmrGrp] [varchar](255) NULL,
	[SaleSaleType] [varchar](255) NULL,
	[CrncyDailyPrcHdrPermit] [bit] NULL,
	[FirstLogin] [tinyint] NULL,
	[MstrUserId] [varchar](12) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[SalePartGrp] [varchar](max) NULL,
	[FailAttempts] [int] NULL,
	[StartDate] [varchar](8) NULL,
	[ExpireDate] [varchar](8) NULL,
	[ViewCulture] [varchar](20) NULL,
	[PrimaryInputCulture] [varchar](20) NULL,
	[SecondaryInputCulture] [varchar](20) NULL,
	[LookupCulture] [varchar](20) NULL,
 CONSTRAINT [PK_USERID] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
--ALTER TABLE [ray].[UserId]  WITH CHECK ADD  CONSTRAINT [FK_UserId_AstTrustGrp] FOREIGN KEY([AstTrustGrp])
--REFERENCES [ray].[AstTrustGrp] ([AstTrustGrp])
--GO

--ALTER TABLE [ray].[UserId] CHECK CONSTRAINT [FK_UserId_AstTrustGrp]
--GO
--ALTER TABLE [ray].[UserId]  WITH CHECK ADD  CONSTRAINT [FK_UserId_CashPrmLvl] FOREIGN KEY([CashLvl])
--REFERENCES [ray].[CashPrmLvl] ([PrmLvl])
--GO

--ALTER TABLE [ray].[UserId] CHECK CONSTRAINT [FK_UserId_CashPrmLvl]
--GO
ALTER TABLE [ray].[UserId] ADD  DEFAULT ((1)) FOR [SaleDocHeader]
GO
ALTER TABLE [ray].[UserId] ADD  DEFAULT ((1)) FOR [SaleDocDateHdrPermit]
GO
ALTER TABLE [ray].[UserId] ADD  DEFAULT ((1)) FOR [CrncyDailyPrcHdrPermit]
GO
ALTER TABLE [ray].[UserId] ADD  DEFAULT (newid()) FOR [RowGUID]