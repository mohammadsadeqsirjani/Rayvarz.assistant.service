CREATE TABLE [ray].[InvRtrnBuy](
	[RtrnBuyReason] [tinyint] NOT NULL,
	[RtrnBuyDesc] [varchar](50) NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
	[RtrnBuyLtnDsc] [varchar](50) NULL,
 CONSTRAINT [PK_INVRTRNBUY] PRIMARY KEY CLUSTERED 
(
	[RtrnBuyReason] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[InvRtrnBuy] ADD  DEFAULT (newid()) FOR [RowGuid]