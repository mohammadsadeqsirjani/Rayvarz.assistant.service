CREATE TABLE [ray].[RaySysYr](
	[RaySys] [varchar](12) NOT NULL,
	[Yr] [int] NOT NULL,
	[StartDate] [char](8) NOT NULL,
	[LastDate] [char](8) NOT NULL,
	[IsClosed] [tinyint] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[SupplementTerm] [tinyint] NULL,
	[SuppDate] [char](8) NULL,
	[CurrDate] [char](8) NULL,
 CONSTRAINT [PK_RAYSYSYR] PRIMARY KEY CLUSTERED 
(
	[RaySys] ASC,
	[Yr] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[RaySysYr]  WITH CHECK ADD  CONSTRAINT [FK_RaySysYr] FOREIGN KEY([RaySys])
REFERENCES [ray].[RaySys] ([RaySys])
GO

ALTER TABLE [ray].[RaySysYr] CHECK CONSTRAINT [FK_RaySysYr]
GO
ALTER TABLE [ray].[RaySysYr] ADD  CONSTRAINT [DF__RaySysYr__IsClos__33FF9E21]  DEFAULT ((0)) FOR [IsClosed]
GO
ALTER TABLE [ray].[RaySysYr] ADD  DEFAULT (newid()) FOR [RowGUID]