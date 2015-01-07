USE [CPM]
GO
/****** Object:  View [dbo].[GSTExportVw]    Script Date: 01/07/2015 14:13:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTExportVw]
as


Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
dbo.fxGetLocationCode(LocationInfoId) + '_DC as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from [GSTDailyCollection]





