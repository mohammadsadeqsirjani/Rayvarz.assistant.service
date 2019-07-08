CREATE TYPE [ray].[InvAssistantUdt_uiControlList] AS TABLE
(
	id varchar(50),
	title VARCHAR(128),
	[state] TINYINT,
	defaultValue_id varchar(50),
	defaultValue_dsc varchar(50)
)
