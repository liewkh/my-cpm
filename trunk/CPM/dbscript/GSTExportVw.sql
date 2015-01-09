USE [CPM]
GO
/****** Object:  View [dbo].[GSTExportVw]    Script Date: 01/09/2015 16:40:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTExportVw]
as
--GSTDailyCollection
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
dbo.fxGetLocationCode(LocationInfoId) + '_DC as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from [GSTDailyCollection]

union

--GSTInvoiceEntryForSeason
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
dbo.fxGetLocationCode(LocationInfoId) + ' Billing as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from [GSTInvoiceEntryForSeason]

union

--GSTInvoiceEntryForDeposit
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
dbo.fxGetLocationCode(LocationInfoId) + ' Deposit as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from GSTInvoiceEntryForDeposit

union


--GSTInvoiceEntryForManualInvoice
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
dbo.fxGetLocationCode(LocationInfoId) + ' Late Payment as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from GSTInvoiceEntryForManualInvoice

union

--GSTCreditNoteSeasonParking
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
dbo.fxGetLocationCode(LocationInfoId) + ' Billing as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from GSTCreditNoteSeasonParking
