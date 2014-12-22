USE [CPM]
GO
/****** Object:  View [dbo].[GSTExportVw]    Script Date: 12/19/2014 18:12:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTExportVw]
as
Select '' as "Journal Number",TransactionDate as Date,'Whatever Context' as Memo,
AccountCode as "Account Number",DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
'' as "Job",'' as "Allocation Memo",LocationInfoId
from [GSTDailyCollection]