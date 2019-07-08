CREATE TABLE [ray].[InvCnsTyp](
	[ConsType] [int] NOT NULL,
	[ConsTypeDesc] [varchar](100) NULL,
	[Account] [int] NULL,
	[AstDoctyp] [tinyint] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
	[ConsTypeLtnDsc] [varchar](100) NULL,
 CONSTRAINT [PK_INVCNSTYP] PRIMARY KEY CLUSTERED 
(
	[ConsType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
--ALTER TABLE [ray].[InvCnsTyp]  WITH CHECK ADD  CONSTRAINT [FK_InvCnsTyp_Account] FOREIGN KEY([Account])
--REFERENCES [ray].[Account] ([Account])
--GO

--ALTER TABLE [ray].[InvCnsTyp] CHECK CONSTRAINT [FK_InvCnsTyp_Account]
--GO
--ALTER TABLE [ray].[InvCnsTyp]  WITH CHECK ADD  CONSTRAINT [FK_InvCnsTyp_AstDocTyp] FOREIGN KEY([AstDoctyp])
--REFERENCES [ray].[AstDocTyp] ([DocTyp])
--GO

--ALTER TABLE [ray].[InvCnsTyp] CHECK CONSTRAINT [FK_InvCnsTyp_AstDocTyp]
--GO
ALTER TABLE [ray].[InvCnsTyp] ADD  DEFAULT (newid()) FOR [RowGuid]