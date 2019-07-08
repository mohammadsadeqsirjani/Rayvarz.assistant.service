CREATE TABLE [ray].[InvReqTyp](
	[ReqType] [tinyint] NOT NULL,
	[ReqTypeDesc] [varchar](20) NULL,
	[ReqTypeLtnDesc] [varchar](20) NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_INVREQTYP] PRIMARY KEY CLUSTERED 
(
	[ReqType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [ray].[InvReqTyp] ADD  DEFAULT (newid()) FOR [RowGuid]