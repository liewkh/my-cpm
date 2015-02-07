USE [CPM]
GO
/****** Object:  View [dbo].[GSTPaymentEntryMiscSR]    Script Date: 02/07/2015 15:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTPaymentEntryMiscSR]
as
--SEASON PARKING RECEIVABLE
select A.Years,A.Months,A.LocationInfoId,A.aAccountName,A.AccountCode,
sum(A.amount) as DebitAmount,0 AS "CreditAmount",A.LocationCode,A.Seq,A.Source from 
(SELECT distinct Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
'SEASON PARKING RECEIVABLE' as AccountName,
'1-2302' as AccountCode,
dp.amount,
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,31 as seq,
       'GSTPaymentEntryMiscSR-31' as Source
from debtorpayment dp, locationinfo li,
debtor d,debtoraccountheader dah,debtoraccountdetail dad
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
and dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.debtorid = dp.debtorid
and convert(varchar(200),dah.debtoraccountheaderid) = dp.debtoraccountheaderid
and dad.xref in ('3','5')
and dah.status <> 'C'
and dad.taxcode in ('SR','NA')
and dp.txntype = 'R'
) as A
GROUP BY A.LocationInfoId, A.Years, A.Months,A.AccountName,A.AccountCode,A.LocationCode,A.Seq,A.Source

union

--OTHER INCOME
select A.Years,A.Months,A.LocationInfoId,A.AccountName,A.AccountCode,
0 AS "DebitAmount",sum(A.amount) as CreditAmount,A.LocationCode,A.Seq,A.Source from 
(SELECT distinct Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
'OTHER INCOME' as AccountName,
'4-2100' as AccountCode,
dp.amount,
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,32 as seq,
       'GSTPaymentEntryMiscSR-32' as Source
from debtorpayment dp, locationinfo li,
debtor d,debtoraccountheader dah,debtoraccountdetail dad
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
and dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.debtorid = dp.debtorid
and convert(varchar(200),dah.debtoraccountheaderid) = dp.debtoraccountheaderid
and dad.xref in ('3','5')
and dah.status <> 'C'
and dad.taxcode in ('SR','NA')
and dp.txntype = 'R'
) as A
GROUP BY A.LocationInfoId, A.Years, A.Months,A.AccountName,A.AccountCode,A.LocationCode,A.Seq,A.Source


/* union

--GST OUTPUT TAX
SELECT Year(dah.InvoiceDate) as "Years",
datepart(m, dah.InvoiceDate) as "Months",
li.LocationInfoId, 
'GST OUTPUT TAX' as AccountName,
'2-9950' as AccountCode,
       0 AS "DebitAmount",sum(dad.amount) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,33 as seq,
       'GSTPaymentEntryMiscSR-33' as Source
from debtorpayment dp, locationinfo li,
debtor d,debtoraccountheader dah,debtoraccountdetail dad
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
and dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.debtorid = dp.debtorid
and convert(varchar(200),dah.debtoraccountheaderid) = dp.debtoraccountheaderid
and dad.xref in ('5')
and dah.status <> 'C'
GROUP BY li.LocationInfoId, li.LocationName, Year(dah.InvoiceDate), Month(dah.InvoiceDate) */
