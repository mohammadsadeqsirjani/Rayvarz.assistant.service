CREATE TABLE [ray].[CenterGrp](
	[CenterGrp] [smallint] NOT NULL,
	[CenterGrpDsc] [varchar](100) NULL,
	[CenterGrpLtnDsc] [varchar](100) NULL,
	[OpnCenter] [int] NULL,
	[RefTbl] [varchar](50) NULL,
	[branch_filter_spec] [varchar](2000) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[CenterGrpShortDsc] [varchar](20) NULL,
	[CenterGrpShortLtnDsc] [varchar](20) NULL,
	[Person] [tinyint] NULL,
 CONSTRAINT [PK_CENTERGRP] PRIMARY KEY CLUSTERED 
(
	[CenterGrp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[CenterGrp] ADD  DEFAULT (newid()) FOR [RowGUID]
GO
ALTER TABLE [ray].[CenterGrp] ADD  CONSTRAINT [DF__CenterGrp__Perso__4FD1D5C8]  DEFAULT ((0)) FOR [Person]