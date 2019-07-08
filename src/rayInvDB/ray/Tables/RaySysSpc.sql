CREATE TABLE [ray].[RaySysSpc](
	[RaySys] [varchar](12) NOT NULL,
	[InfTit] [varchar](100) NOT NULL,
	[InfDsc] [varchar](100) NULL,
	[InfVal] [varchar](max) NULL,
	[InfImage] [varbinary](max) NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[InfDscLatin] [varchar](100) NULL,
	[ShowAsParameter] [tinyint] NULL,
	[SystemVisibility] [varchar](100) NULL,
 CONSTRAINT [PK_RAYSYSSPC] PRIMARY KEY CLUSTERED 
(
	[RaySys] ASC,
	[InfTit] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [ray].[RaySysSpc]  WITH CHECK ADD  CONSTRAINT [FK_RaySysSpcSys] FOREIGN KEY([RaySys])
REFERENCES [ray].[RaySys] ([RaySys])
GO

ALTER TABLE [ray].[RaySysSpc] CHECK CONSTRAINT [FK_RaySysSpcSys]
GO
ALTER TABLE [ray].[RaySysSpc] ADD  DEFAULT (newid()) FOR [RowGUID]
GO
ALTER TABLE [ray].[RaySysSpc] ADD  CONSTRAINT [DF__RaySysSpc__ShowA__25F175EC]  DEFAULT ((0)) FOR [ShowAsParameter]