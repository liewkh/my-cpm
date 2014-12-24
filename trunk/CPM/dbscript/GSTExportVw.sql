USE [CPM]
GO
/****** Object:  View [dbo].[GSTExportVw]    Script Date: 12/24/2014 17:03:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTExportVw]
as
Select '' as "Journal Number",CONVERT(VARCHAR(3), DATENAME(MONTH, "MONTH"), 7) + '''' + RIGHT("Year", 2) as Date,Memo,
AccountCode as "Account Number",DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",'' as "Allocation Memo",LocationInfoId,Seq,Source,Year,Month
from [GSTDailyCollection]
