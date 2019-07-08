CREATE TABLE [ray].[Unit](
	[UntCode] [char](2) NOT NULL,
	[UntName] [varchar](30) NOT NULL,
	[UntLtnName] [varchar](30) NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UNIT] PRIMARY KEY CLUSTERED 
(
	[UntCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[Unit] ADD  DEFAULT (newid()) FOR [RowGuid]