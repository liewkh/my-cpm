USE [CPM]
GO
/****** Object:  View [dbo].[GSTExportVw]    Script Date: 04/17/2015 21:31:36 ******/
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
'Invoice - ' + dbo.fxGetLocationCode(LocationInfoId) + '_Billing as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from [GSTInvoiceEntryForSeason]

union

--GSTInvoiceEntryForDeposit
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
dbo.fxGetLocationCode(LocationInfoId) + '_Deposit as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from GSTInvoiceEntryForDeposit

union

--GSTInvoiceEntryForDepositZR
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
dbo.fxGetLocationCode(LocationInfoId) + '_OI_ZR as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from GSTInvoiceEntryForDepositZR

union

--GSTInvoiceEntryForDepositSR
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
dbo.fxGetLocationCode(LocationInfoId) + '_OI_SR as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from GSTInvoiceEntryForDepositSR

union

--GSTCreditNoteSeasonParking
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
'CN - ' + dbo.fxGetLocationCode(LocationInfoId) + '_Billing as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from GSTCreditNoteSeasonParking

union

--GSTCreditNoteEntryForDeposit
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
'CN - ' + dbo.fxGetLocationCode(LocationInfoId) + '_Deposit as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from GSTCreditNoteEntryForDeposit

union
--[GSTCreditNoteForDepositZR]
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
'CN - ' + dbo.fxGetLocationCode(LocationInfoId) + '_OI_ZR as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from GSTCreditNoteForDepositZR

union
--[GSTCreditNoteForDepositSR]
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
'CN - ' + dbo.fxGetLocationCode(LocationInfoId) + '_OI_SR as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from GSTCreditNoteForDepositSR

union

--[GSTPaymentEntry]
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
'Payment - ' + dbo.fxGetLocationCode(LocationInfoId) + '_Collection as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from [GSTPaymentEntry]

union

--[GSTPaymentEntryDeposit]
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
'PayDep - ' + dbo.fxGetLocationCode(LocationInfoId) + '_Collection as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from  GSTPaymentEntryDeposit

union

--[GSTPaymentEntryMiscZR]
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
'Payment - ' + dbo.fxGetLocationCode(LocationInfoId) + '_OI_ZR as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from  GSTPaymentEntryMiscZR

union

--[GSTPaymentEntryMiscSR]
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
'Payment - ' + dbo.fxGetLocationCode(LocationInfoId) + '_OI_SR as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from  GSTPaymentEntryMiscSR

union

--GSTInvoiceEntryForDepositOS
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
dbo.fxGetLocationCode(LocationInfoId) + '_OI_OS as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from GSTInvoiceEntryForDepositOS

union

--GSTCreditNoteForDepositOS
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
'CN - ' + dbo.fxGetLocationCode(LocationInfoId) + '_OI_OS as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from GSTCreditNoteForDepositOS

union

--GSTCreditNoteForDepositOS
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
'Payment - ' + dbo.fxGetLocationCode(LocationInfoId) + '_OI_OS as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from GSTPaymentEntryMiscOS

union

--GSTDNEntryForSeason
Select AccountCode as "A/C Code", 
AccountName as "A/C Name",
'DN - ' + dbo.fxGetLocationCode(LocationInfoId) + '_Billing as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', (Months * 4) - 3, 3) + '''' + RIGHT("Years", 2) as Memo,
DebitAmount as "Debit Amount",
CreditAmount as "Credit Amount",
dbo.fxGetLocationCode(LocationInfoId) as "Job",LocationInfoId,Seq,Source,Years,Months
from GSTDNEntryForSeason

UNION

--GSTDNEntryForDeposit
SELECT     AccountCode AS [A/C Code], AccountName AS [A/C Name], 'DN - ' + dbo.fxGetLocationCode(LocationInfoId) 
                      + '_Deposit as ' + SUBSTRING('JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC ', Months * 4 - 3, 3) + '''' + RIGHT(Years, 2) AS Memo, 
                      DebitAmount AS [Debit Amount], CreditAmount AS [Credit Amount], dbo.fxGetLocationCode(LocationInfoId) AS Job, LocationInfoId, seq, Source, Years, 
                      Months
FROM         dbo.GSTDNEntryForDeposit
