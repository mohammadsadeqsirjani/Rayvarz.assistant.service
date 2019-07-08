CREATE TABLE [ray].[InvOrdr](
	[OrdrNO] [bigint] NOT NULL,
	[Cstmr] [varchar](10) NULL,
	[OrdrDsc] [varchar](60) NULL,
	[PartNo] [varchar](20) NULL,
	[Stqty] [money] NULL,
	[Center] [int] NULL,
	[Active] [tinyint] NULL,
	[ManufacturingDate] [char](8) NULL,
	[ExpirationDate] [char](8) NULL,
	[RetestDate] [char](8) NULL,
	[PrdSerial] [varchar](20) NULL,
	[FillingWeight] [float] NULL,
	[PrdBgnProj] [char](8) NULL,
	[OrderNoEq] [varchar](30) NULL,
	[branch_filter_spec] [varchar](2000) NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
	[OrdrLtnDsc] [varchar](60) NULL,
 CONSTRAINT [PK_INVORDR] PRIMARY KEY CLUSTERED 
(
	[OrdrNO] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[InvOrdr]  WITH CHECK ADD  CONSTRAINT [FK_InvOrdr_Center] FOREIGN KEY([Center])
REFERENCES [ray].[Center] ([Center])
GO

ALTER TABLE [ray].[InvOrdr] CHECK CONSTRAINT [FK_InvOrdr_Center]
GO
--ALTER TABLE [ray].[InvOrdr]  WITH CHECK ADD  CONSTRAINT [FK_InvOrdr_Cstmr] FOREIGN KEY([Cstmr])
--REFERENCES [ray].[Cstmr] ([Cstmr])
--GO

--ALTER TABLE [ray].[InvOrdr] CHECK CONSTRAINT [FK_InvOrdr_Cstmr]
--GO
ALTER TABLE [ray].[InvOrdr]  WITH NOCHECK ADD  CONSTRAINT [FK_InvOrdr_PartNo] FOREIGN KEY([PartNo])
REFERENCES [ray].[ItemData] ([PartNo])
GO

ALTER TABLE [ray].[InvOrdr] CHECK CONSTRAINT [FK_InvOrdr_PartNo]
GO
ALTER TABLE [ray].[InvOrdr] ADD  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [ray].[InvOrdr] ADD  DEFAULT (newid()) FOR [RowGuid]