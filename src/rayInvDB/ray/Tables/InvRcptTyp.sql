CREATE TABLE [ray].[InvRcptTyp](
	[RcptType] [int] NOT NULL,
	[RcptTypeDesc] [varchar](100) NULL,
	[Account] [int] NULL,
	[GenAutoPlaque] [tinyint] NULL,
	[ForcePlaque] [tinyint] NULL,
	[AstDocTyp] [tinyint] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
	[RcptTypLtnDsc] [varchar](100) NULL,
 CONSTRAINT [PK_INVRCPTTYP] PRIMARY KEY CLUSTERED 
(
	[RcptType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
--ALTER TABLE [ray].[InvRcptTyp]  WITH CHECK ADD  CONSTRAINT [FK_InvRcptTyp_Account] FOREIGN KEY([Account])
--REFERENCES [ray].[Account] ([Account])
--GO

--ALTER TABLE [ray].[InvRcptTyp] CHECK CONSTRAINT [FK_InvRcptTyp_Account]
--GO
--ALTER TABLE [ray].[InvRcptTyp]  WITH CHECK ADD  CONSTRAINT [FK_InvRcptTyp_AstDocTyp] FOREIGN KEY([AstDocTyp])
--REFERENCES [ray].[AstDocTyp] ([DocTyp])
--GO

--ALTER TABLE [ray].[InvRcptTyp] CHECK CONSTRAINT [FK_InvRcptTyp_AstDocTyp]
--GO
ALTER TABLE [ray].[InvRcptTyp] ADD  DEFAULT ((0)) FOR [GenAutoPlaque]
GO
ALTER TABLE [ray].[InvRcptTyp] ADD  DEFAULT ((0)) FOR [ForcePlaque]
GO
ALTER TABLE [ray].[InvRcptTyp] ADD  DEFAULT (newid()) FOR [RowGuid]