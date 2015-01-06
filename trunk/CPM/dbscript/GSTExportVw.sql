USE [CPM]
GO
/****** Object:  View [dbo].[GSTExportVw]    Script Date: 01/06/2015 11:38:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTExportVw]
as


Select '' as "Journal Number", 
SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Date,Memo,
AccountCode as "Account Number",DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",'' as "Allocation Memo",LocationInfoId,Seq,Source,Years,Months
from [GSTDailyCollection]





