USE [CPM]
GO
/****** Object:  View [dbo].[GSTPaymentEntryMisc]    Script Date: 02/07/2015 13:32:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTPaymentEntryMisc]
as
SELECT Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
'BANK - ' + dbo.fxGetBank(li.locationinfoid) as AccountName,
dbo.fxGetAccountCode(li.locationinfoid) as AccountCode,
       sum(dad.amount) AS "DebitAmount",0 AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,27 as seq,
       'GSTPaymentEntryMisc-27' as Source
from debtorpayment dp, locationinfo li,
debtor d,debtoraccountheader dah,debtoraccountdetail dad
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
and dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.debtorid = dp.debtorid
and convert(varchar(200),dah.debtoraccountheaderid) = dp.debtoraccountheaderid
and dad.xref in ('2')
and dah.status <> 'C'
and dp.debtoraccountheaderid in
(
select distinct dp.debtoraccountheaderid from debtorpayment dp
left join debtoraccountdetail dad
on convert(varchar(200),dad.debtoraccountheaderid) = dp.debtoraccountheaderid
where dp.status <> 'C'
and dp.txntype = 'R'
and dad.xref in ('2'))
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.PaymentDate), Month(dp.PaymentDate)

union

--SEASON PARKING RECEIVABLE
SELECT Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
'SEASON PARKING RECEIVABLE' as AccountName,
'1-2302' as AccountCode,
       0 AS "DebitAmount",sum(dad.amount) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,28 as seq,
       'GSTPaymentEntryMisc-28' as Source
from debtorpayment dp, locationinfo li,
debtor d,debtoraccountheader dah,debtoraccountdetail dad
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
and dah.debtoraccountheaderid = dad.debtoraccountheaderid
and d.debtorid = dah.debtorid
and d.debtorid = dp.debtorid
and convert(varchar(200),dah.debtoraccountheaderid) = dp.debtoraccountheaderid
and dad.xref in ('2')
and dah.status <> 'C'
and dp.debtoraccountheaderid in
(
select distinct dp.debtoraccountheaderid from debtorpayment dp
left join debtoraccountdetail dad
on convert(varchar(200),dad.debtoraccountheaderid) = dp.debtoraccountheaderid
where dp.status <> 'C'
and dp.txntype = 'R'
and dad.xref in ('2'))
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.PaymentDate), Month(dp.PaymentDate)
