CREATE TABLE [ray].[WrkShop](
	[WrkShp] [int] NOT NULL,
	[WrkShpDsc] [varchar](45) NOT NULL,
	[WrkShpLtnDsc] [varchar](45) NULL,
	[Branch] [int] NULL,
	[FromPlaque] [money] NULL,
	[FrHokmMasrafi] [money] NULL,
	[FrAmani] [money] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_WRKSHOP] PRIMARY KEY CLUSTERED 
(
	[WrkShp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[WrkShop]  WITH CHECK ADD  CONSTRAINT [FK_WrkShop_Branch] FOREIGN KEY([Branch])
REFERENCES [ray].[Branch] ([Branch])
GO

ALTER TABLE [ray].[WrkShop] CHECK CONSTRAINT [FK_WrkShop_Branch]
GO
ALTER TABLE [ray].[WrkShop] ADD  DEFAULT (newid()) FOR [RowGuid]