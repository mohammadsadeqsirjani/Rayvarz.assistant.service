CREATE TABLE [ray].[InvSeri](
	[StoreNo] [varchar](6) NOT NULL,
	[DocType] [tinyint] NOT NULL,
	[BgnSeri] [smallint] NULL,
	[EndSeri] [smallint] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_INVSERI] PRIMARY KEY CLUSTERED 
(
	[StoreNo] ASC,
	[DocType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[InvSeri]  WITH CHECK ADD  CONSTRAINT [FK_InvSeri_DocTyp] FOREIGN KEY([DocType])
REFERENCES [ray].[InvDocTyp] ([DocType])
GO

ALTER TABLE [ray].[InvSeri] CHECK CONSTRAINT [FK_InvSeri_DocTyp]
GO
ALTER TABLE [ray].[InvSeri]  WITH CHECK ADD  CONSTRAINT [FK_InvSeri_Store] FOREIGN KEY([StoreNo])
REFERENCES [ray].[Store] ([StoreNo])
GO

ALTER TABLE [ray].[InvSeri] CHECK CONSTRAINT [FK_InvSeri_Store]
GO
ALTER TABLE [ray].[InvSeri] ADD  DEFAULT (newid()) FOR [RowGuid]