
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTInvoiceEntryForSeason]
as
--SEASON PARKING RECEIVABLE
SELECT Year(dah.InvoiceDate) as "Years",
datepart(m, dah.InvoiceDate) as "Months",
li.LocationInfoId, 
'SEASON PARKING RECEIVABLE' as AccountName,
'1-2302' as AccountCode,
       sum(dad.amount) AS "DebitAmount",0 AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,4 as seq,
       'GSTInvoiceEntryForSeason-4' as Source
FROM debtoraccountheader dah,debtoraccountdetail dad,debtor d,locationinfo li
where dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.locationinfoid = li.locationinfoid
and dad.xref in ('1','4')
and dah.status <> 'C'
and dah.txntype = 'I'
GROUP BY li.LocationInfoId, li.LocationName, Year(dah.InvoiceDate), Month(dah.InvoiceDate)

union

--SEASON PARKING CHARGES
SELECT Year(dah.InvoiceDate) as "Years",
datepart(m, dah.InvoiceDate) as "Months",
li.LocationInfoId, 
'SEASON PARKING CHARGES' as AccountName,
'4-1100' as AccountCode,
       0 AS "DebitAmount",sum(dad.amount) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,5 as seq,
       'GSTInvoiceEntryForSeason-5' as Source
from debtoraccountheader dah,debtoraccountdetail dad,debtor d,locationinfo li
where dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.locationinfoid = li.locationinfoid
and dad.xref in ('1')
and dah.status <> 'C'
and dah.txntype = 'I'
GROUP BY li.LocationInfoId, li.LocationName, Year(dah.InvoiceDate), Month(dah.InvoiceDate)

union

--GST OUTPUT TAX
SELECT Year(dah.InvoiceDate) as "Years",
datepart(m, dah.InvoiceDate) as "Months",
li.LocationInfoId, 
'GST OUTPUT TAX' as AccountName,
'2-9950' as AccountCode,
       0 AS "DebitAmount",sum(dad.amount) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,6 as seq,
       'GSTInvoiceEntryForSeason-6' as Source
from debtoraccountheader dah,debtoraccountdetail dad,debtor d,locationinfo li
where dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.locationinfoid = li.locationinfoid
and dad.xref in ('4')
and dah.status <> 'C'
and dah.txntype = 'I'
GROUP BY li.LocationInfoId, li.LocationName, Year(dah.InvoiceDate), Month(dah.InvoiceDate)
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

