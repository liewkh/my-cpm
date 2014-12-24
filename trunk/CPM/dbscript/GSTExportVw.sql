USE [CPM]
GO
/****** Object:  View [dbo].[GSTExportVw]    Script Date: 12/23/2014 15:09:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTExportVw]
as
Select '' as "Journal Number",TransactionDate as Date,Memo,
AccountCode as "Account Number",DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
'' as "Job",'' as "Allocation Memo",LocationInfoId,Seq,Source
from [GSTDailyCollection]