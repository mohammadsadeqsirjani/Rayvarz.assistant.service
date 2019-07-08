CREATE TABLE [ray].[UserDomain](
	[UserId] [varchar](12) NOT NULL,
	[RaySys] [varchar](12) NOT NULL,
	[Branch] [int] NOT NULL,
	[Func] [varchar](50) NOT NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_USERDOMAIN] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RaySys] ASC,
	[Branch] ASC,
	[Func] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[UserDomain]  WITH CHECK ADD  CONSTRAINT [FK_UserDomain_Branch] FOREIGN KEY([Branch])
REFERENCES [ray].[Branch] ([Branch])
GO

ALTER TABLE [ray].[UserDomain] CHECK CONSTRAINT [FK_UserDomain_Branch]
GO
ALTER TABLE [ray].[UserDomain]  WITH CHECK ADD  CONSTRAINT [FK_UserDomainFunc] FOREIGN KEY([RaySys], [Func])
REFERENCES [ray].[Func] ([RaySys], [Func])
GO

ALTER TABLE [ray].[UserDomain] CHECK CONSTRAINT [FK_UserDomainFunc]
GO
ALTER TABLE [ray].[UserDomain]  WITH CHECK ADD  CONSTRAINT [FK_UserDomainUser] FOREIGN KEY([UserId])
REFERENCES [ray].[UserId] ([UserId])
GO

ALTER TABLE [ray].[UserDomain] CHECK CONSTRAINT [FK_UserDomainUser]
GO
ALTER TABLE [ray].[UserDomain]  WITH CHECK ADD  CONSTRAINT [FKF_UserDomain_RaySys_RaySys] FOREIGN KEY([RaySys])
REFERENCES [ray].[RaySys] ([RaySys])
GO

ALTER TABLE [ray].[UserDomain] CHECK CONSTRAINT [FKF_UserDomain_RaySys_RaySys]
GO
ALTER TABLE [ray].[UserDomain] ADD  CONSTRAINT [DF__UserDomai__Branc__1F5986F6]  DEFAULT ((1)) FOR [Branch]
GO
ALTER TABLE [ray].[UserDomain] ADD  DEFAULT (newid()) FOR [RowGUID]