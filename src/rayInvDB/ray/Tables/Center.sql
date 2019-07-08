CREATE TABLE [ray].[Center](
	[Center] [int] NOT NULL,
	[CenterGrp] [smallint] NULL,
	[CenterDsc] [varchar](100) NULL,
	[CenterLtnDsc] [varchar](100) NULL,
	[Equal_code] [varchar](30) NULL,
	[ActvFlg] [tinyint] NULL,
	[CostCenter1] [int] NULL,
	[CostCenter2] [int] NULL,
	[CostCenter3] [int] NULL,
	[CrnEf] [char](4) NULL,
	[UnqDsc] [varchar](500) NULL,
	[IsStoreAmt] [tinyint] NULL,
	[Detail] [tinyint] NULL,
	[RefCenter] [int] NULL,
	[MultyMediaDoc] [int] NULL,
	[MultyMediaDocX] [char](36) NULL,
	[branch_filter_spec] [varchar](2000) NULL,
	[_branch_filter_spec] [varchar](2000) NULL,
	[CntrSpec1] [int] NULL,
	[CntrSpec2] [int] NULL,
	[CntrSpec3] [int] NULL,
	[CntrSpec4] [int] NULL,
	[CntrSpec5] [int] NULL,
	[CntrSpec6] [int] NULL,
	[CntrSpec7] [int] NULL,
	[CntrSpec8] [int] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[NationalCode] [varchar](15) NULL,
	[NonIsuueOpening] [tinyint] NULL,
	[AlternativeCenter] [int] NULL,
 CONSTRAINT [PK_CENTER] PRIMARY KEY CLUSTERED 
(
	[Center] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO
--ALTER TABLE [ray].[Center]  WITH CHECK ADD  CONSTRAINT [FK_Center_AlternativeCente] FOREIGN KEY([AlternativeCenter])
--REFERENCES [ray].[Center] ([Center])
--GO

--ALTER TABLE [ray].[Center] CHECK CONSTRAINT [FK_Center_AlternativeCente]
--GO
--ALTER TABLE [ray].[Center]  WITH CHECK ADD  CONSTRAINT [FK_Center_CntrSpec1] FOREIGN KEY([CntrSpec1])
--REFERENCES [ray].[CntrSpec1] ([CntrSpec1])
--GO

--ALTER TABLE [ray].[Center] CHECK CONSTRAINT [FK_Center_CntrSpec1]
--GO
--ALTER TABLE [ray].[Center]  WITH CHECK ADD  CONSTRAINT [FK_Center_CntrSpec2] FOREIGN KEY([CntrSpec2])
--REFERENCES [ray].[CntrSpec2] ([CntrSpec2])
--GO

--ALTER TABLE [ray].[Center] CHECK CONSTRAINT [FK_Center_CntrSpec2]
--GO
--ALTER TABLE [ray].[Center]  WITH CHECK ADD  CONSTRAINT [FK_Center_CntrSpec3] FOREIGN KEY([CntrSpec3])
--REFERENCES [ray].[CntrSpec3] ([CntrSpec3])
--GO

--ALTER TABLE [ray].[Center] CHECK CONSTRAINT [FK_Center_CntrSpec3]
--GO
--ALTER TABLE [ray].[Center]  WITH CHECK ADD  CONSTRAINT [FK_Center_CntrSpec4] FOREIGN KEY([CntrSpec4])
--REFERENCES [ray].[CntrSpec4] ([CntrSpec4])
--GO

--ALTER TABLE [ray].[Center] CHECK CONSTRAINT [FK_Center_CntrSpec4]
--GO
--ALTER TABLE [ray].[Center]  WITH CHECK ADD  CONSTRAINT [FK_Center_CntrSpec5] FOREIGN KEY([CntrSpec5])
--REFERENCES [ray].[CntrSpec5] ([CntrSpec5])
--GO

--ALTER TABLE [ray].[Center] CHECK CONSTRAINT [FK_Center_CntrSpec5]
--GO
--ALTER TABLE [ray].[Center]  WITH CHECK ADD  CONSTRAINT [FK_Center_CntrSpec6] FOREIGN KEY([CntrSpec6])
--REFERENCES [ray].[CntrSpec6] ([CntrSpec6])
--GO

--ALTER TABLE [ray].[Center] CHECK CONSTRAINT [FK_Center_CntrSpec6]
--GO
--ALTER TABLE [ray].[Center]  WITH CHECK ADD  CONSTRAINT [FK_Center_CntrSpec7] FOREIGN KEY([CntrSpec7])
--REFERENCES [ray].[CntrSpec7] ([CntrSpec7])
--GO

--ALTER TABLE [ray].[Center] CHECK CONSTRAINT [FK_Center_CntrSpec7]
--GO
--ALTER TABLE [ray].[Center]  WITH CHECK ADD  CONSTRAINT [FK_Center_CntrSpec8] FOREIGN KEY([CntrSpec8])
--REFERENCES [ray].[CntrSpec8] ([CntrSpec8])
--GO

--ALTER TABLE [ray].[Center] CHECK CONSTRAINT [FK_Center_CntrSpec8]
--GO
--ALTER TABLE [ray].[Center]  WITH CHECK ADD  CONSTRAINT [FK_Center_Crncy] FOREIGN KEY([CrnEf])
--REFERENCES [ray].[Crncy] ([Crncy])
--GO

--ALTER TABLE [ray].[Center] CHECK CONSTRAINT [FK_Center_Crncy]
--GO
--ALTER TABLE [ray].[Center]  WITH CHECK ADD  CONSTRAINT [FK_Center_Person] FOREIGN KEY([NationalCode])
--REFERENCES [ray].[Person] ([NationalCode])
--GO

--ALTER TABLE [ray].[Center] CHECK CONSTRAINT [FK_Center_Person]
--GO
ALTER TABLE [ray].[Center]  WITH CHECK ADD  CONSTRAINT [FK_CenterCenetrGrp] FOREIGN KEY([CenterGrp])
REFERENCES [ray].[CenterGrp] ([CenterGrp])
GO

ALTER TABLE [ray].[Center] CHECK CONSTRAINT [FK_CenterCenetrGrp]
GO
ALTER TABLE [ray].[Center] ADD  DEFAULT ((0)) FOR [IsStoreAmt]
GO
ALTER TABLE [ray].[Center] ADD  DEFAULT ((0)) FOR [Detail]
GO
ALTER TABLE [ray].[Center] ADD  DEFAULT (newid()) FOR [RowGUID]
GO
ALTER TABLE [ray].[Center] ADD  DEFAULT ((0)) FOR [NonIsuueOpening]