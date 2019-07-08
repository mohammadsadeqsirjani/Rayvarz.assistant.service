CREATE TABLE [ray].[InvMstr](
	[FiscalYear] [int] NOT NULL,
	[StoreNo] [varchar](6) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[BinNo] [varchar](35) NULL,
	[Soh] [money] NULL,
	[OpnSoh] [money] NULL,
	[NoopnSoh] [money] NULL,
	[OpnValue] [money] NULL,
	[UntCode] [char](2) NULL,
	[AvgRate] [money] NULL,
	[LastRcptRate] [money] NULL,
	[TotIssue] [money] NULL,
	[LastIssueDate] [char](8) NULL,
	[TotRecipt] [money] NULL,
	[LastReciptDate] [char](8) NULL,
	[UpdateDate] [char](8) NULL,
	[TagNo] [int] NULL,
	[StandardRate] [money] NULL,
	[OrderPoint] [money] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_INVMSTR] PRIMARY KEY CLUSTERED 
(
	[FiscalYear] ASC,
	[StoreNo] ASC,
	[PartNo] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[InvMstr]  WITH NOCHECK ADD  CONSTRAINT [FK_InvMstrItemData] FOREIGN KEY([PartNo])
REFERENCES [ray].[ItemData] ([PartNo])
GO

ALTER TABLE [ray].[InvMstr] CHECK CONSTRAINT [FK_InvMstrItemData]
GO
ALTER TABLE [ray].[InvMstr]  WITH CHECK ADD  CONSTRAINT [FK_InvMstrPrd] FOREIGN KEY([FiscalYear])
REFERENCES [ray].[InvPrd] ([FiscalYear])
GO

ALTER TABLE [ray].[InvMstr] CHECK CONSTRAINT [FK_InvMstrPrd]
GO
ALTER TABLE [ray].[InvMstr]  WITH CHECK ADD  CONSTRAINT [FK_InvMstrStore] FOREIGN KEY([StoreNo])
REFERENCES [ray].[Store] ([StoreNo])
GO

ALTER TABLE [ray].[InvMstr] CHECK CONSTRAINT [FK_InvMstrStore]
GO
ALTER TABLE [ray].[InvMstr] ADD  DEFAULT (newid()) FOR [RowGuid]