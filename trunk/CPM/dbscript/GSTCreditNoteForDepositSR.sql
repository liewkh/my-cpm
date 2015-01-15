USE [CPM]
GO
/****** Object:  View [dbo].[GSTCreditNoteForDepositSR]    Script Date: 01/15/2015 14:42:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTCreditNoteForDepositSR]
as
--SEASON PARKING RECEIVABLE
SELECT Year(dah.InvoiceDate) as "Years",
datepart(m, dah.InvoiceDate) as "Months",
li.LocationInfoId, 
'SEASON PARKING RECEIVABLE' as AccountName,
'1-2302' as AccountCode,
       0 AS "DebitAmount",sum(dad.amount) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,21 as seq,
       'DAD' as Source
from debtorpayment dp, locationinfo li,
debtor d,debtoraccountheader dah,debtoraccountdetail dad
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
and dp.TxnType = 'CN'
and dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.debtorid = dp.debtorid
and convert(varchar(200),dah.debtoraccountheaderid) = dp.debtoraccountheaderid
and dad.xref in ('3','5')
and dah.status <> 'C'
and (dad.TaxCode <> 'ZR')
GROUP BY li.LocationInfoId, li.LocationName, Year(dah.InvoiceDate), Month(dah.InvoiceDate)



union

--OTHER INCOME
SELECT Year(dah.InvoiceDate) as "Years",
datepart(m, dah.InvoiceDate) as "Months",
li.LocationInfoId, 
'OTHER INCOME' as AccountName,
'4-2100' as AccountCode,
       sum(dad.amount) AS "DebitAmount",0 AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,22 as seq,
       'DAD' as Source
from debtorpayment dp, locationinfo li,
debtor d,debtoraccountheader dah,debtoraccountdetail dad
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
and dp.TxnType = 'CN'
and dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.debtorid = dp.debtorid
and convert(varchar(200),dah.debtoraccountheaderid) = dp.debtoraccountheaderid
and dad.xref in ('3')
and dah.status <> 'C'
and dad.TaxCode = 'SR'
GROUP BY li.LocationInfoId, li.LocationName, Year(dah.InvoiceDate), Month(dah.InvoiceDate)

union

--GST OUTPUT TAX
SELECT Year(dah.InvoiceDate) as "Years",
datepart(m, dah.InvoiceDate) as "Months",
li.LocationInfoId, 
'GST OUTPUT TAX' as AccountName,
'2-9950' as AccountCode,
       sum(dad.amount) AS "DebitAmount",0 AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,23 as seq,
       'DAD' as Source
from debtorpayment dp, locationinfo li,
debtor d,debtoraccountheader dah,debtoraccountdetail dad
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
and dp.TxnType = 'CN'
and dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.debtorid = dp.debtorid
and convert(varchar(200),dah.debtoraccountheaderid) = dp.debtoraccountheaderid
and dad.xref in ('5')
and dah.status <> 'C'
and dad.TaxCode = 'SR'
GROUP BY li.LocationInfoId, li.LocationName, Year(dah.InvoiceDate), Month(dah.InvoiceDate)


