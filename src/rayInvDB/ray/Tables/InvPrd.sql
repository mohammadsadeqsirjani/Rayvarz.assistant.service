CREATE TABLE [ray].[InvPrd](
	[FiscalYear] [int] NOT NULL,
	[StartFiscalYear] [char](8) NOT NULL,
	[EndFiscalYear] [char](8) NOT NULL,
	[FiscalStatus] [tinyint] NULL,
	[IsMoveAMT] [tinyint] NULL,
	[IsMoveRemainQty] [tinyint] NULL,
	[IsSixteenMonth] [tinyint] NULL,
	[EndSixteenMonthYear] [char](8) NULL,
	[IsMovePermanently] [tinyint] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_INVPRD] PRIMARY KEY CLUSTERED 
(
	[FiscalYear] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
--ALTER TABLE [ray].[InvPrd]  WITH CHECK ADD  CONSTRAINT [FK_InvPrdFsSt] FOREIGN KEY([FiscalStatus])
--REFERENCES [ray].[InvFsSt] ([FiscalStatus])
--GO

--ALTER TABLE [ray].[InvPrd] CHECK CONSTRAINT [FK_InvPrdFsSt]
--GO
ALTER TABLE [ray].[InvPrd] ADD  CONSTRAINT [DF__InvPrd__IsMoveAM__22A007F5]  DEFAULT ((0)) FOR [IsMoveAMT]
GO
ALTER TABLE [ray].[InvPrd] ADD  CONSTRAINT [DF__InvPrd__IsMoveRe__23942C2E]  DEFAULT ((0)) FOR [IsMoveRemainQty]
GO
ALTER TABLE [ray].[InvPrd] ADD  DEFAULT ((0)) FOR [IsSixteenMonth]
GO
ALTER TABLE [ray].[InvPrd] ADD  DEFAULT ((0)) FOR [IsMovePermanently]
GO
ALTER TABLE [ray].[InvPrd] ADD  DEFAULT (newid()) FOR [RowGuid]