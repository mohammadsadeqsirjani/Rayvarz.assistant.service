CREATE FUNCTION [ray].[InvAssistantTVFUNC_SerialSoh_GetList]
(
	@storeNo varchar(6),
	@partNo as varchar(20),
	@f_serial as varchar(50)
)
RETURNS @returntable TABLE
(
	FiscalYear int,
	StoreNo  varchar(6),
	PartNo varchar(20),
	Serail varchar(50),
	TotRcpt money,
	TotIssu money,
	Supplier varchar(50),
	SupName varchar(150),
	TrnsPkgTyp smallint,
	PakgTypeDesc varchar(50),
	SuppSerial varchar(50),
	SuppEntrDate varchar(8),
	SuppExpDate varchar(8),
	ExpDate  varchar(8),
	ConfirmDate varchar(8),
	Soh money,
	OrdrNO int,
	OrdrDsc varchar(60)
)
AS
BEGIN
	INSERT @returntable
	SELECT 
    [Limit1].[FiscalYear] AS [FiscalYear], 
    [Limit1].[StoreNo] AS [StoreNo], 
    [Limit1].[PartNo] AS [PartNo], 
    [Limit1].[Serail] AS [Serail], 
    [Limit1].[TotRcpt] AS [TotRcpt], 
    [Limit1].[TotIssu] AS [TotIssu], 
    [Limit1].[Supplier] AS [Supplier], 
  [Extent6].[SupName] AS [SupName], 
    [Limit1].[TrnsPkgTyp] AS [TrnsPkgTyp], 
   [Extent7].[PakgTypeDesc] AS [PakgTypeDesc], 
   [Limit1].[SuppSerial] AS [SuppSerial], 
    [Limit1].[SuppEntrDate] AS [SuppEntrDate], 
    [Limit1].[SuppExpDate] AS [SuppExpDate], 
    [Limit1].[ExpDate] AS [ExpDate], 
    [Limit1].[ConfirmDate] AS [ConfirmDate],
    [Limit1].[Soh] AS [Soh],
 --   [Limit1].[Model] AS [Model], 
 --   [Limit1].[ConsSt] AS [ConsSt], 
 --   [Limit1].[Seri] AS [Seri], 
 --   [Limit1].[MultyMediaDoc] AS [MultyMediaDoc], 
 --   [Limit1].[PackCount] AS [PackCount], 
 --   [Limit1].[Binno] AS [Binno], 
    [Limit1].[OrdrNO] AS [OrdrNO], 
	[Extent8].[OrdrDsc] AS [OrdrDsc]
 --   [Limit1].[MultyMediaDocX] AS [MultyMediaDocX], 
 --   [Limit1].[Colour] AS [Colour], 
 --   [Limit1].[cstmr] AS [cstmr], 
 --   [Limit1].[MotorNo] AS [MotorNo], 
 --   [Limit1].[ChassisNo] AS [ChassisNo], 
 --   [Limit1].[OptmIsuDat] AS [OptmIsuDat], 
 --   [Limit1].[Potention] AS [Potention], 
 --   [Limit1].[QcNo] AS [QcNo], 
 --   [Limit1].[IssueNo] AS [IssueNo], 
 --   [Limit1].[ValidExit] AS [ValidExit], 
 --   [Limit1].[RowGUID] AS [RowGUID], 
 --   [Limit1].[RowVersion] AS [RowVersion], 
 --   [Extent2].[StoreDsc] AS [StoreDsc], 
 --   [Extent2].[RowGUID] AS [RowGUID1], 
 --   [Extent2].[RowVersion] AS [RowVersion1], 
 --   [Extent3].[PartNoDsc] AS [PartNoDsc], 
 --   [Extent3].[PartLtnDsc] AS [PartLtnDsc], 
 --   [Extent3].[RowGUID] AS [RowGUID2], 
 --   [Extent3].[RowVersion] AS [RowVersion2], 
 --   [Extent4].[ModelDsc] AS [ModelDsc], 
 --   [Extent4].[RowGUID] AS [RowGUID3], 
 --   [Extent4].[RowVersion] AS [RowVersion3], 
 --   [Extent5].[ConsStatusDsc] AS [ConsStatusDsc], 
 --   [Extent5].[RowGUID] AS [RowGUID4], 
 --   [Extent5].[RowVersion] AS [RowVersion4], 

 --   [Extent6].[RowGUID] AS [RowGUID5], 
 --   [Extent6].[RowVersion] AS [RowVersion5], 

 --   [Extent7].[RowGUID] AS [RowGUID6], 
 --   [Extent7].[RowVersion] AS [RowVersion6], 
 --   [Extent8].[OrdrDsc] AS [OrdrDsc], 
 --   [Extent8].[RowGUID] AS [RowGUID7], 
 --   [Extent8].[RowVersion] AS [RowVersion7], 
 --   [Extent9].[ColourDsc] AS [ColourDsc], 
 --   [Extent9].[RowGUID] AS [RowGUID8], 
 --   [Extent9].[RowVersion] AS [RowVersion8], 
 --   [Extent10].[CSTMRNAME] AS [CSTMRNAME], 
 --   [Extent10].[RowGUID] AS [RowGUID9], 
 --   [Extent10].[RowVersion] AS [RowVersion9]
    FROM           (SELECT TOP (33) [Extent1].[FiscalYear] AS [FiscalYear], [Extent1].[StoreNo] AS [StoreNo], [Extent1].[PartNo] AS [PartNo], [Extent1].[Serail] AS [Serail], [Extent1].[Model] AS [Model], [Extent1].[ConsSt] AS [ConsSt], [Extent1].[Seri] AS [Seri], [Extent1].[MultyMediaDoc] AS [MultyMediaDoc], [Extent1].[Soh] AS [Soh], [Extent1].[TotRcpt] AS [TotRcpt], [Extent1].[TotIssu] AS [TotIssu], [Extent1].[Supplier] AS [Supplier], [Extent1].[TrnsPkgTyp] AS [TrnsPkgTyp], [Extent1].[SuppSerial] AS [SuppSerial], [Extent1].[PackCount] AS [PackCount], [Extent1].[SuppExpDate] AS [SuppExpDate], [Extent1].[SuppEntrDate] AS [SuppEntrDate], [Extent1].[Binno] AS [Binno], [Extent1].[OrdrNO] AS [OrdrNO], [Extent1].[MultyMediaDocX] AS [MultyMediaDocX], [Extent1].[Colour] AS [Colour], [Extent1].[cstmr] AS [cstmr], [Extent1].[MotorNo] AS [MotorNo], [Extent1].[ChassisNo] AS [ChassisNo], [Extent1].[ExpDate] AS [ExpDate], [Extent1].[OptmIsuDat] AS [OptmIsuDat], [Extent1].[ConfirmDate] AS [ConfirmDate], [Extent1].[Potention] AS [Potention], [Extent1].[QcNo] AS [QcNo], [Extent1].[IssueNo] AS [IssueNo], [Extent1].[ValidExit] AS [ValidExit], [Extent1].[RowGUID] AS [RowGUID], [Extent1].[RowVersion] AS [RowVersion]
        FROM [ray].[InvSerial] AS [Extent1]
        WHERE ([ray].[InvAssistantFUNC_getCurrentFiscalYear]() = [Extent1].[FiscalYear])
		AND (@storeNo = [Extent1].[StoreNo]) 
		AND (@partNo = [Extent1].[PartNo]) 
		AND (@f_serial is null or @f_serial = '' or [Extent1].[Serail] = @f_serial)) AS [Limit1]
    --LEFT OUTER JOIN [ray].[InvVw_Store] AS [Extent2] ON [Limit1].[StoreNo] = [Extent2].[StoreNo]
    --LEFT OUTER JOIN [ray].[ItemData] AS [Extent3] ON [Limit1].[PartNo] = [Extent3].[PartNo]
    --LEFT OUTER JOIN [ray].[InvModel] AS [Extent4] ON [Limit1].[Model] = [Extent4].[Model]
    --LEFT OUTER JOIN [ray].[InvCnsSt] AS [Extent5] ON [Limit1].[ConsSt] = [Extent5].[ConsStatus]
    LEFT OUTER JOIN [ray].[Supplier] AS [Extent6] ON [Limit1].[Supplier] = [Extent6].[Supplier]
    LEFT OUTER JOIN [ray].[PkgTyp] AS [Extent7] ON [Limit1].[TrnsPkgTyp] = [Extent7].[PakgType]
    LEFT OUTER JOIN [ray].[invordr] AS [Extent8] ON [Limit1].[OrdrNO] = [Extent8].[OrdrNo]
    --LEFT OUTER JOIN [ray].[Colour] AS [Extent9] ON [Limit1].[Colour] = [Extent9].[Colour]
    --LEFT OUTER JOIN [ray].[CSTMR] AS [Extent10] ON [Limit1].[cstmr] = [Extent10].[CSTMR]



	RETURN
END
