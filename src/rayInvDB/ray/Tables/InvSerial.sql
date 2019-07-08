CREATE TABLE [ray].[InvSerial](
	[StoreNo] [varchar](6) NOT NULL,
	[PartNo] [varchar](20) NOT NULL,
	[Serail] [varchar](50) NOT NULL,
	[Soh] [money] NULL,
	[TotRcpt] [money] NULL,
	[TotIssu] [money] NULL,
	[ExpDate] [char](8) NULL,
	[Seri] [varchar](30) NULL,
	[FiscalYear] [int] NOT NULL,
	[Model] [varchar](20) NULL,
	[Potention] [money] NULL,
	[OptmIsuDat] [char](8) NULL,
	[QcNo] [varchar](20) NULL,
	[ConsSt] [tinyint] NULL,
	[ValidExit] [tinyint] NULL,
	[ConfirmDate] [char](8) NULL,
	[TagNo] [int] NULL,
	[Colour] [tinyint] NULL,
	[Cstmr] [varchar](10) NULL,
	[MotorNo] [varchar](20) NULL,
	[ChassisNo] [varchar](20) NULL,
	[Supplier] [varchar](10) NULL,
	[TrnsPkgTyp] [smallint] NULL,
	[SuppSerial] [varchar](50) NULL,
	[PackCount] [smallint] NULL,
	[IssueNo] [int] NULL,
	[SuppExpDate] [char](8) NULL,
	[SuppEntrDate] [char](8) NULL,
	[Binno] [varchar](35) NULL,
	[MultyMediaDoc] [int] NULL,
	[MultyMediaDocX] [char](36) NULL,
	[OrdrNO] [bigint] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[RowGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_INVSERIAL] PRIMARY KEY CLUSTERED 
(
	[StoreNo] ASC,
	[PartNo] ASC,
	[Serail] ASC,
	[FiscalYear] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
--ALTER TABLE [ray].[InvSerial]  WITH CHECK ADD  CONSTRAINT [FK_INVSERIA_INVSERIAL_PKGTYP] FOREIGN KEY([TrnsPkgTyp])
--REFERENCES [ray].[PkgTyp] ([PakgType])
--GO

--ALTER TABLE [ray].[InvSerial] CHECK CONSTRAINT [FK_INVSERIA_INVSERIAL_PKGTYP]
--GO
--ALTER TABLE [ray].[InvSerial]  WITH CHECK ADD  CONSTRAINT [FK_InvSerial_Colour] FOREIGN KEY([Colour])
--REFERENCES [ray].[Colour] ([Colour])
--GO

--ALTER TABLE [ray].[InvSerial] CHECK CONSTRAINT [FK_InvSerial_Colour]
--GO
--ALTER TABLE [ray].[InvSerial]  WITH CHECK ADD  CONSTRAINT [FK_InvSerial_ConsSt] FOREIGN KEY([ConsSt])
--REFERENCES [ray].[InvCnsSt] ([ConsStatus])
--GO

--ALTER TABLE [ray].[InvSerial] CHECK CONSTRAINT [FK_InvSerial_ConsSt]
--GO
--ALTER TABLE [ray].[InvSerial]  WITH CHECK ADD  CONSTRAINT [FK_InvSerial_Cstmr] FOREIGN KEY([Cstmr])
--REFERENCES [ray].[Cstmr] ([Cstmr])
--GO

--ALTER TABLE [ray].[InvSerial] CHECK CONSTRAINT [FK_InvSerial_Cstmr]
--GO
--ALTER TABLE [ray].[InvSerial]  WITH CHECK ADD  CONSTRAINT [FK_InvSerial_InvOrdr] FOREIGN KEY([OrdrNO])
--REFERENCES [ray].[InvOrdr] ([OrdrNO])
--GO

--ALTER TABLE [ray].[InvSerial] CHECK CONSTRAINT [FK_InvSerial_InvOrdr]
--GO
--ALTER TABLE [ray].[InvSerial]  WITH CHECK ADD  CONSTRAINT [FK_InvSerial_Model] FOREIGN KEY([Model])
--REFERENCES [ray].[InvModel] ([Model])
--GO

--ALTER TABLE [ray].[InvSerial] CHECK CONSTRAINT [FK_InvSerial_Model]
--GO
--ALTER TABLE [ray].[InvSerial]  WITH NOCHECK ADD  CONSTRAINT [FK_InvSerial_Part] FOREIGN KEY([PartNo])
--REFERENCES [ray].[ItemData] ([PartNo])
--GO

--ALTER TABLE [ray].[InvSerial] CHECK CONSTRAINT [FK_InvSerial_Part]
--GO
--ALTER TABLE [ray].[InvSerial]  WITH CHECK ADD  CONSTRAINT [FK_InvSerial_Store] FOREIGN KEY([StoreNo])
--REFERENCES [ray].[Store] ([StoreNo])
--GO

--ALTER TABLE [ray].[InvSerial] CHECK CONSTRAINT [FK_InvSerial_Store]
--GO
--ALTER TABLE [ray].[InvSerial]  WITH CHECK ADD  CONSTRAINT [FK_InvSerial_Supplier] FOREIGN KEY([Supplier])
--REFERENCES [ray].[Supplier] ([Supplier])
--GO

--ALTER TABLE [ray].[InvSerial] CHECK CONSTRAINT [FK_InvSerial_Supplier]
--GO
ALTER TABLE [ray].[InvSerial]  WITH CHECK ADD  CONSTRAINT [FK_InvSerial_Year] FOREIGN KEY([FiscalYear])
REFERENCES [ray].[InvPrd] ([FiscalYear])
GO

ALTER TABLE [ray].[InvSerial] CHECK CONSTRAINT [FK_InvSerial_Year]
GO
ALTER TABLE [ray].[InvSerial] ADD  DEFAULT (newid()) FOR [RowGuid]