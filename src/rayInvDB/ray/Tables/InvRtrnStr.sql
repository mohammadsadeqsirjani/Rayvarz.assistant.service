CREATE TABLE [ray].[InvRtrnStr](
	[RtrnStore] [tinyint] NOT NULL,
	[RtrnStoreDesc] [varchar](50) NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
	[RtrnStoreLtnDsc] [varchar](50) NULL,
 CONSTRAINT [PK_INVRTRNSTR] PRIMARY KEY CLUSTERED 
(
	[RtrnStore] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[InvRtrnStr] ADD  DEFAULT (newid()) FOR [RowGuid]