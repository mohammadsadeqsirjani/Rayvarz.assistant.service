CREATE TABLE [ray].[InvDocTyp](
	[DocType] [tinyint] IDENTITY(1,1) NOT NULL,
	[DocTypeDesc] [varchar](40) NULL,
	[DescShop] [varchar](40) NULL,
	[SeqDocType] [smallint] NOT NULL,
	[CatgType] [tinyint] NULL,
	[IsDocOpn] [tinyint] NULL,
	[OpnRcptPriceTyp] [tinyint] NULL,
	[VbFormName] [varchar](40) NULL,
	[VbDescFrmName] [varchar](40) NULL,
	[AudtDocType] [tinyint] NULL,
	[IsAuditDoc] [tinyint] NULL,
	[TabNo] [smallint] NULL,
	[RefDoc] [tinyint] NULL,
	[RefDocNo] [tinyint] NULL,
	[MoveDoc] [tinyint] NULL,
	[SerailAcc] [tinyint] NULL,
	[SortInEdGrp] [tinyint] NULL,
	[IsShop] [tinyint] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_INVDOCTYP] PRIMARY KEY CLUSTERED 
(
	[DocType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
--ALTER TABLE [ray].[InvDocTyp]  WITH CHECK ADD  CONSTRAINT [FK_InvDocTypCtgDoc] FOREIGN KEY([CatgType])
--REFERENCES [ray].[InvCtgDoc] ([CatgType])
--GO

--ALTER TABLE [ray].[InvDocTyp] CHECK CONSTRAINT [FK_InvDocTypCtgDoc]
--GO
ALTER TABLE [ray].[InvDocTyp] ADD  CONSTRAINT [DF__InvDocTyp__RowGu__0C19F8B9]  DEFAULT (newid()) FOR [RowGuid]