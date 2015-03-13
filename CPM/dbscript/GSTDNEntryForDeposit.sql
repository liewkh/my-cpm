USE [CPM]
GO
/****** Object:  View [dbo].[GSTDNEntryForDeposit]    Script Date: 03/13/2015 14:47:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTDNEntryForDeposit]
as
--SEASON PARKING RECEIVABLE
SELECT Year(dah.InvoiceDate) as "Years",
datepart(m, dah.InvoiceDate) as "Months",
li.LocationInfoId, 
'SEASON PARKING RECEIVABLE' as AccountName,
'1-2302' as AccountCode,
       sum(dad.amount) AS "DebitAmount",0 AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,43 as seq,
       'DAD' as Source
FROM debtoraccountheader dah,debtoraccountdetail dad,debtor d,locationinfo li
where dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.locationinfoid = li.locationinfoid
and dad.xref in ('2')
and dah.status <> 'C'
and dah.txntype = 'DN'
GROUP BY li.LocationInfoId, li.LocationName, Year(dah.InvoiceDate), Month(dah.InvoiceDate)

union

--DEPOSIT RECEIVED
SELECT Year(dah.InvoiceDate) as "Years",
datepart(m, dah.InvoiceDate) as "Months",
li.LocationInfoId, 
'DEPOSIT RECEIVED' as AccountName,
'2-4002' as AccountCode,
       0 AS "DebitAmount",sum(dad.amount) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,44 as seq,
       'DAD' as Source
from debtoraccountheader dah,debtoraccountdetail dad,debtor d,locationinfo li
where dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.locationinfoid = li.locationinfoid
and dad.xref in ('2')
and dah.status <> 'C'
and dah.txntype = 'DN'
GROUP BY li.LocationInfoId, li.LocationName, Year(dah.InvoiceDate), Month(dah.InvoiceDate)
