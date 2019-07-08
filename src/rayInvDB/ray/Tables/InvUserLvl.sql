CREATE TABLE [ray].[InvUserLvl](
	[UserId] [varchar](12) NOT NULL,
	[StoreNo] [varchar](6) NOT NULL,
	[DocType] [tinyint] NOT NULL,
	[InvCnfrmLvl] [tinyint] NULL,
	[AutoDocNo] [tinyint] NULL,
	[cnfrmSeri] [tinyint] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_INVUSERLVL] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[StoreNo] ASC,
	[DocType] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[InvUserLvl]  WITH CHECK ADD  CONSTRAINT [FK_InvUserLvl_DocTyp] FOREIGN KEY([DocType])
REFERENCES [ray].[InvDocTyp] ([DocType])
GO

ALTER TABLE [ray].[InvUserLvl] CHECK CONSTRAINT [FK_InvUserLvl_DocTyp]
GO
ALTER TABLE [ray].[InvUserLvl]  WITH CHECK ADD  CONSTRAINT [FK_InvUserLvl_Store] FOREIGN KEY([StoreNo])
REFERENCES [ray].[Store] ([StoreNo])
GO

ALTER TABLE [ray].[InvUserLvl] CHECK CONSTRAINT [FK_InvUserLvl_Store]
GO
ALTER TABLE [ray].[InvUserLvl]  WITH CHECK ADD  CONSTRAINT [FK_InvUserLvl_UserId] FOREIGN KEY([UserId])
REFERENCES [ray].[UserId] ([UserId])
GO

ALTER TABLE [ray].[InvUserLvl] CHECK CONSTRAINT [FK_InvUserLvl_UserId]
GO
ALTER TABLE [ray].[InvUserLvl] ADD  DEFAULT (newid()) FOR [RowGuid]