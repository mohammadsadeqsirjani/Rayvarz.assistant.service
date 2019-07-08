CREATE TABLE [ray].[InvDocSt](
	[DocStatus] [tinyint] NOT NULL,
	[DocStatusDsc] [varchar](60) NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_INVDOCST] PRIMARY KEY CLUSTERED 
(
	[DocStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[InvDocSt] ADD  DEFAULT (newid()) FOR [RowGuid]