USE [CPM]
GO
/****** Object:  View [dbo].[GSTPaymentEntry]    Script Date: 03/13/2015 14:44:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTPaymentEntry]
as
SELECT Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
'BANK - ' + dbo.fxGetBank(li.locationinfoid) as AccountName,
dbo.fxGetAccountCode(li.locationinfoid) as AccountCode,
       sum(dp.amount+isnull(dp.gstamount,0) - dbo.fxGetDepositAmount(dp.debtoraccountheaderid)) AS "DebitAmount",0 AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,24 as seq,
       'GSTPaymentEntry-24' as Source
from debtorpayment dp, locationinfo li,debtor d
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
and dp.debtorpaymentid in
(
select distinct dp.debtorpaymentid from debtorpayment dp
left join debtoraccountdetail dad
on convert(varchar(200),dad.debtoraccountheaderid) = dp.debtoraccountheaderid
where dp.status <> 'C'
and dp.txntype = 'R'
and dad.xref in ('1','4'))
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.PaymentDate), Month(dp.PaymentDate)

union

--SEASON PARKING RECEIVABLE
SELECT Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
'SEASON PARKING RECEIVABLE' as AccountName,
'1-2302' as AccountCode,
       0 AS "DebitAmount",sum(dp.amount - dbo.fxGetDepositAmount(dp.debtoraccountheaderid)) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,25 as seq,
       'GSTPaymentEntry-25' as Source
from debtorpayment dp, locationinfo li,
debtor d
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
and d.debtorid = dp.debtorid
and dp.debtorpaymentid in
(
select distinct dp.debtorpaymentid from debtorpayment dp
left join debtoraccountdetail dad
on convert(varchar(200),dad.debtoraccountheaderid) = dp.debtoraccountheaderid
where dp.status <> 'C'
and dp.txntype = 'R'
and dad.xref in ('1','4'))
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.PaymentDate), Month(dp.PaymentDate)

--union

--GST OUTPUT TAX
/*SELECT Year(dah.InvoiceDate) as "Years",
datepart(m, dah.InvoiceDate) as "Months",
li.LocationInfoId, 
'GST OUTPUT TAX' as AccountName,
'2-9950' as AccountCode,
       0 AS "DebitAmount",sum(dad.amount) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,26 as seq,
       'GSTPaymentEntry-26' as Source
from debtorpayment dp, locationinfo li,
debtor d,debtoraccountheader dah,debtoraccountdetail dad
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
and dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.debtorid = dp.debtorid
and convert(varchar(200),dah.debtoraccountheaderid) = dp.debtoraccountheaderid
and dad.xref in ('4')
and dah.status <> 'C'
GROUP BY li.LocationInfoId, li.LocationName, Year(dah.InvoiceDate), Month(dah.InvoiceDate)*/
