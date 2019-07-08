CREATE TABLE [ray].[PartGrp](
	[PartGrp] [varchar](20) NOT NULL,
	[PartGrpDsc] [varchar](255) NULL,
	[PartGrpLtnDsc] [varchar](80) NULL,
	[PrntPartGrp] [varchar](20) NULL,
	[LotTyp] [tinyint] NULL,
	[OrdSize] [money] NULL,
	[SftyStk] [money] NULL,
	[LeadTime] [smallint] NULL,
	[StkCst] [money] NULL,
	[OrdCst] [money] NULL,
	[StoreNo] [varchar](6) NULL,
	[Rmrk] [varchar](500) NULL,
	[IsuGrp] [tinyint] NULL,
	[ConsAst] [tinyint] NULL,
	[UnSaleableAst] [tinyint] NULL,
	[Clssify] [int] NULL,
	[EqualCodingCod] [varchar](20) NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PARTGRP] PRIMARY KEY CLUSTERED 
(
	[PartGrp] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO
--ALTER TABLE [ray].[PartGrp]  WITH CHECK ADD  CONSTRAINT [FK_PartGrp_AstClassify] FOREIGN KEY([Clssify])
--REFERENCES [ray].[AstClassify] ([Clssify])
--GO

--ALTER TABLE [ray].[PartGrp] CHECK CONSTRAINT [FK_PartGrp_AstClassify]
--GO
--ALTER TABLE [ray].[PartGrp]  WITH CHECK ADD  CONSTRAINT [FK_PartGrp_Store] FOREIGN KEY([StoreNo])
--REFERENCES [ray].[Store] ([StoreNo])
--GO

--ALTER TABLE [ray].[PartGrp] CHECK CONSTRAINT [FK_PartGrp_Store]
--GO
--ALTER TABLE [ray].[PartGrp]  WITH CHECK ADD  CONSTRAINT [FK_PartGrpLotTyp] FOREIGN KEY([LotTyp])
--REFERENCES [ray].[PrdLotTyp] ([LotTyp])
--GO

--ALTER TABLE [ray].[PartGrp] CHECK CONSTRAINT [FK_PartGrpLotTyp]
--GO
--ALTER TABLE [ray].[PartGrp]  WITH CHECK ADD  CONSTRAINT [FK_PartGrpMainGrp] FOREIGN KEY([PrntPartGrp])
--REFERENCES [ray].[PartGrp] ([PartGrp])
--GO

--ALTER TABLE [ray].[PartGrp] CHECK CONSTRAINT [FK_PartGrpMainGrp]
--GO
ALTER TABLE [ray].[PartGrp] ADD  DEFAULT ((0)) FOR [ConsAst]
GO
ALTER TABLE [ray].[PartGrp] ADD  DEFAULT ((0)) FOR [UnSaleableAst]
GO
ALTER TABLE [ray].[PartGrp] ADD  DEFAULT (newid()) FOR [RowGuid]