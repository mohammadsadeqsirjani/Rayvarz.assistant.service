CREATE TABLE [ray].[InvArcSoh](
	[FiscalYear] [int] NOT NULL,
	[StoreNo] [varchar](6) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[DocDate] [char](8) NOT NULL,
	[Qty] [money] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_INVARCSOH] PRIMARY KEY CLUSTERED 
(
	[FiscalYear] ASC,
	[StoreNo] ASC,
	[PartNo] ASC,
	[DocDate] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[InvArcSoh]  WITH NOCHECK ADD  CONSTRAINT [FK_InvArcSohItemData] FOREIGN KEY([PartNo])
REFERENCES [ray].[ItemData] ([PartNo])
GO

ALTER TABLE [ray].[InvArcSoh] CHECK CONSTRAINT [FK_InvArcSohItemData]
GO
ALTER TABLE [ray].[InvArcSoh]  WITH CHECK ADD  CONSTRAINT [FK_InvArcSohPrd] FOREIGN KEY([FiscalYear])
REFERENCES [ray].[InvPrd] ([FiscalYear])
GO

ALTER TABLE [ray].[InvArcSoh] CHECK CONSTRAINT [FK_InvArcSohPrd]
GO
ALTER TABLE [ray].[InvArcSoh]  WITH CHECK ADD  CONSTRAINT [FK_InvArcSohStore] FOREIGN KEY([StoreNo])
REFERENCES [ray].[Store] ([StoreNo])
GO

ALTER TABLE [ray].[InvArcSoh] CHECK CONSTRAINT [FK_InvArcSohStore]
GO
ALTER TABLE [ray].[InvArcSoh] ADD  DEFAULT (newid()) FOR [RowGuid]