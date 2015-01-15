USE [CPM]
GO
/****** Object:  View [dbo].[GSTInvoiceEntryForDepositZR]    Script Date: 01/15/2015 14:45:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTInvoiceEntryForDepositZR]
as
--SEASON PARKING RECEIVABLE
SELECT Year(dah.InvoiceDate) as "Years",
datepart(m, dah.InvoiceDate) as "Months",
li.LocationInfoId, 
'SEASON PARKING RECEIVABLE' as AccountName,
'1-2303' as AccountCode,
       sum(dad.amount) AS "DebitAmount",0 AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,9 as seq,
       'DAD' as Source
FROM debtoraccountheader dah,debtoraccountdetail dad,debtor d,locationinfo li
where dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.locationinfoid = li.locationinfoid
and dad.xref in ('3')
and dah.status <> 'C'
and dad.TaxCode = 'ZR'
GROUP BY li.LocationInfoId, li.LocationName, Year(dah.InvoiceDate), Month(dah.InvoiceDate)

union

--SEASON PARKING RECEIVABLE
SELECT Year(dah.InvoiceDate) as "Years",
datepart(m, dah.InvoiceDate) as "Months",
li.LocationInfoId, 
'OTHER INCOME' as AccountName,
'4-2100' as AccountCode,
       0 AS "DebitAmount",sum(dad.amount) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,10 as seq,
       'DAD' as Source
FROM debtoraccountheader dah,debtoraccountdetail dad,debtor d,locationinfo li
where dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.locationinfoid = li.locationinfoid
and dad.xref in ('3')
and dah.status <> 'C'
and dad.TaxCode = 'ZR'
GROUP BY li.LocationInfoId, li.LocationName, Year(dah.InvoiceDate), Month(dah.InvoiceDate)

