CREATE TABLE [ray].[Supplier](
	[Supplier] [varchar](10) NOT NULL,
	[SupName] [varchar](60) NULL,
	[SupLtnName] [varchar](60) NULL,
	[Country] [smallint] NULL,
	[SupPostCode] [varchar](15) NULL,
	[Supaddr] [varchar](120) NULL,
	[Supaddr2] [varchar](120) NULL,
	[SupTelNo] [varchar](60) NULL,
	[SupTelNo2] [varchar](60) NULL,
	[SupFaxNo] [varchar](60) NULL,
	[SupTlxNo] [varchar](60) NULL,
	[SupEMail] [varchar](60) NULL,
	[URL] [varchar](60) NULL,
	[Mngr] [varchar](40) NULL,
	[SaleMngr] [varchar](40) NULL,
	[SupEcnmcNo] [varchar](15) NULL,
	[SupNationCode] [varchar](15) NULL,
	[Center] [int] NULL,
	[SupDstnc] [money] NULL,
	[SupFirstName] [varchar](25) NULL,
	[SupLastName] [varchar](50) NULL,
	[SupFthrName] [varchar](25) NULL,
	[SupIdNo] [varchar](15) NULL,
	[SupIdNoSrl] [varchar](15) NULL,
	[SupIdNoIssue] [varchar](100) NULL,
	[IsProducer] [tinyint] NULL,
	[SrcSup] [tinyint] NULL,
	[MultyMediaDoc] [int] NULL,
	[MultyMediaDocX] [char](36) NULL,
	[SaleRegionVCode] [int] NULL,
	[County] [int] NULL,
	[IsAgent] [bit] NULL,
	[isBuyOperator] [bit] NULL,
	[IsFarmer] [bit] NULL,
	[IsBusinessMan] [bit] NULL,
	[SupplierField] [smallint] NULL,
	[Ratio] [int] NULL,
	[Zone] [tinyint] NULL,
	[CityZone] [tinyint] NULL,
	[RancherType] [tinyint] NULL,
	[RancherGrp] [tinyint] NULL,
	[Province] [tinyint] NULL,
	[IsInGvrmntLongList] [tinyint] NULL,
	[IsInCntrLongList] [tinyint] NULL,
	[IsInOurLongList] [tinyint] NULL,
	[Deactive] [tinyint] NULL,
	[CityTax] [int] NULL,
	[TaxMainId] [tinyint] NULL,
	[TaxSubId] [tinyint] NULL,
	[IsTransporter] [tinyint] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
	[ContactPerson] [int] NULL,
	[ExecutiveUnit] [int] NULL,
	[ResponsibleUnit] [int] NULL,
	[HoldingCompany] [varchar](10) NULL,
	[POBox] [varchar](30) NULL,
	[Remarks] [varchar](4000) NULL,
	[VATExpireDate] [char](8) NULL,
	[CstmrStatus] [tinyint] NULL,
 CONSTRAINT [PK_SUPPLIER] PRIMARY KEY CLUSTERED 
(
	[Supplier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_CityTax] FOREIGN KEY([CityTax])
--REFERENCES [ray].[CityTax] ([CityTax])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_CityTax]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_CityZone] FOREIGN KEY([CityZone])
--REFERENCES [ray].[CityZone] ([CityZone])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_CityZone]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_ContactPerson] FOREIGN KEY([ContactPerson])
--REFERENCES [ray].[Center] ([Center])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_ContactPerson]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_County] FOREIGN KEY([County])
--REFERENCES [ray].[County] ([County])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_County]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [fk_Supplier_ExecutiveUnit] FOREIGN KEY([ExecutiveUnit])
--REFERENCES [ray].[Center] ([Center])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [fk_Supplier_ExecutiveUnit]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_HoldingCompany] FOREIGN KEY([HoldingCompany])
--REFERENCES [ray].[Supplier] ([Supplier])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_HoldingCompany]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_MultyMediaDoc] FOREIGN KEY([MultyMediaDoc])
--REFERENCES [ray].[MultyMediaDoc] ([MultyMediaDoc])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_MultyMediaDoc]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_Province] FOREIGN KEY([Province])
--REFERENCES [ray].[Province] ([Province])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_Province]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_RancherGrp] FOREIGN KEY([RancherGrp])
--REFERENCES [ray].[RancherGrp] ([RancherGrp])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_RancherGrp]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_RancherType] FOREIGN KEY([RancherType])
--REFERENCES [ray].[RancherType] ([RancherType])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_RancherType]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_ResponsibleUnit] FOREIGN KEY([ResponsibleUnit])
--REFERENCES [ray].[Center] ([Center])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_ResponsibleUnit]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_SaleRegion] FOREIGN KEY([SaleRegionVCode])
--REFERENCES [ray].[SaleRegion] ([VCode])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_SaleRegion]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_SrcSup] FOREIGN KEY([SrcSup])
--REFERENCES [ray].[BuySrcSup] ([SrcSup])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_SrcSup]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_SupplierField] FOREIGN KEY([SupplierField])
--REFERENCES [ray].[SupplierField] ([SupplierField])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_SupplierField]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_TaxMainId] FOREIGN KEY([TaxMainId])
--REFERENCES [ray].[TaxMainId] ([TaxMainId])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_TaxMainId]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_TaxSubId] FOREIGN KEY([TaxSubId])
--REFERENCES [ray].[TaxSubId] ([TaxSubId])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_TaxSubId]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_Zone] FOREIGN KEY([Zone])
--REFERENCES [ray].[Zone] ([Zone])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_Supplier_Zone]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_SupplierCenter] FOREIGN KEY([Center])
--REFERENCES [ray].[Center] ([Center])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_SupplierCenter]
--GO
--ALTER TABLE [ray].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_SupplierCountry] FOREIGN KEY([Country])
--REFERENCES [ray].[Country] ([Country])
--GO

--ALTER TABLE [ray].[Supplier] CHECK CONSTRAINT [FK_SupplierCountry]
--GO
ALTER TABLE [ray].[Supplier] ADD  DEFAULT ((0)) FOR [IsAgent]
GO
ALTER TABLE [ray].[Supplier] ADD  DEFAULT ((0)) FOR [isBuyOperator]
GO
ALTER TABLE [ray].[Supplier] ADD  DEFAULT ((0)) FOR [IsFarmer]
GO
ALTER TABLE [ray].[Supplier] ADD  DEFAULT ((0)) FOR [IsBusinessMan]
GO
ALTER TABLE [ray].[Supplier] ADD  DEFAULT ((0)) FOR [IsInGvrmntLongList]
GO
ALTER TABLE [ray].[Supplier] ADD  DEFAULT ((0)) FOR [IsInCntrLongList]
GO
ALTER TABLE [ray].[Supplier] ADD  DEFAULT ((0)) FOR [IsInOurLongList]
GO
ALTER TABLE [ray].[Supplier] ADD  DEFAULT ((0)) FOR [Deactive]
GO
ALTER TABLE [ray].[Supplier] ADD  DEFAULT (newid()) FOR [RowGuid]