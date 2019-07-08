CREATE TABLE [ray].[RaySys](
	[RaySys] [varchar](12) NOT NULL,
	[SysDsc] [varchar](45) NOT NULL,
	[SysLtnDsc] [varchar](45) NULL,
	[Domain] [varchar](12) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_RAYSYS] PRIMARY KEY CLUSTERED 
(
	[RaySys] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[RaySys]  WITH CHECK ADD  CONSTRAINT [FK_RaySys_RaySys] FOREIGN KEY([Domain])
REFERENCES [ray].[RaySys] ([RaySys])
GO

ALTER TABLE [ray].[RaySys] CHECK CONSTRAINT [FK_RaySys_RaySys]
GO
ALTER TABLE [ray].[RaySys] ADD  DEFAULT (newid()) FOR [RowGUID]