CREATE TABLE [ray].[Func](
	[RaySys] [varchar](12) NOT NULL,
	[Func] [varchar](50) NOT NULL,
	[FuncDsc] [varchar](45) NOT NULL,
	[FuncLtnDsc] [varchar](45) NULL,
	[MainFunc] [varchar](50) NULL,
	[Srt] [int] NULL,
	[AnyBranch] [tinyint] NULL,
	[ProjectTyp] [varchar](50) NULL,
	[FuncStat] [tinyint] NULL,
	[FuncAct] [varchar](30) NULL,
	[ShortcutKey] [varchar](30) NULL,
	[ToolbarSort] [tinyint] NULL,
	[HasSeparator] [tinyint] NULL,
	[UsrAdd] [tinyint] NULL,
	[FuncIconKey] [varchar](60) NULL,
	[MRSFunc] [varchar](50) NULL,
	[OldFunc] [varchar](50) NULL,
	[ToolBarProjectTyp] [varchar](50) NULL,
	[ToolBarDsc] [varchar](50) NULL,
	[ToolBarLtnDsc] [varchar](50) NULL,
	[WebFuncAct] [varchar](30) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[MRSFuncRaySys] [varchar](12) NULL,
	[WinFunc] [varchar](50) NULL,
 CONSTRAINT [PK_FUNC] PRIMARY KEY CLUSTERED 
(
	[RaySys] ASC,
	[Func] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO
--ALTER TABLE [ray].[Func]  WITH CHECK ADD  CONSTRAINT [FK_Func_FuncAct] FOREIGN KEY([FuncAct])
--REFERENCES [ray].[FuncAct] ([FuncAct])
--GO

--ALTER TABLE [ray].[Func] CHECK CONSTRAINT [FK_Func_FuncAct]
--GO
--ALTER TABLE [ray].[Func]  WITH CHECK ADD  CONSTRAINT [FK_Func_FuncStat] FOREIGN KEY([FuncStat])
--REFERENCES [ray].[FuncStat] ([FuncStat])
--GO

--ALTER TABLE [ray].[Func] CHECK CONSTRAINT [FK_Func_FuncStat]
--GO
--ALTER TABLE [ray].[Func]  WITH CHECK ADD  CONSTRAINT [FK_Func_MRSFuncRaySys] FOREIGN KEY([MRSFuncRaySys])
--REFERENCES [ray].[RaySys] ([RaySys])
--GO

--ALTER TABLE [ray].[Func] CHECK CONSTRAINT [FK_Func_MRSFuncRaySys]
--GO
--ALTER TABLE [ray].[Func]  WITH CHECK ADD  CONSTRAINT [FK_FuncRaySys] FOREIGN KEY([RaySys])
--REFERENCES [ray].[RaySys] ([RaySys])
--GO

--ALTER TABLE [ray].[Func] CHECK CONSTRAINT [FK_FuncRaySys]
--GO
ALTER TABLE [ray].[Func] ADD  CONSTRAINT [DF__Func__AnyBranch__54F67D98]  DEFAULT ((0)) FOR [AnyBranch]
GO
ALTER TABLE [ray].[Func] ADD  CONSTRAINT [DF__Func__HasSeparat__55EAA1D1]  DEFAULT ((0)) FOR [HasSeparator]
GO
ALTER TABLE [ray].[Func] ADD  CONSTRAINT [DF__Func__UsrAdd__56DEC60A]  DEFAULT ((0)) FOR [UsrAdd]
GO
ALTER TABLE [ray].[Func] ADD  DEFAULT (newid()) FOR [RowGUID]