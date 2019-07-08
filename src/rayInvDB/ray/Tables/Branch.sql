CREATE TABLE [ray].[Branch](
	[Branch] [int] NOT NULL,
	[BranchDsc] [varchar](40) NULL,
	[BranchLDsc] [varchar](40) NULL,
	[IsCenter] [tinyint] NULL,
	[DBName] [varchar](100) NULL,
	[PayCrnDat] [char](8) NULL,
	[DBBranch] [int] NULL,
	[AstAccSerial] [tinyint] NULL,
	[AstFrOwner] [numeric](25, 0) NULL,
	[AstToOwner] [numeric](25, 0) NULL,
	[AstFrCenter] [numeric](25, 0) NULL,
	[AstToCenter] [numeric](25, 0) NULL,
	[AstFrSettlement] [int] NULL,
	[AstToSettlement] [int] NULL,
	[AstFrCenter2] [int] NULL,
	[AstToCenter2] [int] NULL,
	[AstFrCenter3] [int] NULL,
	[AstToCenter3] [int] NULL,
	[RowGUID] [uniqueidentifier] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[NationalCode] [varchar](15) NULL,
 CONSTRAINT [PK_BRANCH] PRIMARY KEY CLUSTERED 
(
	[Branch] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[Branch] ADD  CONSTRAINT [DF__Branch__Branch__27F8EE98]  DEFAULT ((1)) FOR [Branch]
GO
ALTER TABLE [ray].[Branch] ADD  CONSTRAINT [DF__Branch__IsCenter__28ED12D1]  DEFAULT ((0)) FOR [IsCenter]
GO
ALTER TABLE [ray].[Branch] ADD  DEFAULT (newid()) FOR [RowGUID]