USE [CPM]
GO
/****** Object:  View [dbo].[GSTCreditNoteSeasonParking]    Script Date: 03/03/2015 14:59:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTCreditNoteSeasonParking]
as
--SEASON PARKING RECEIVABLE
SELECT Year(dp.paymentdate) as "Years",
datepart(m, dp.paymentdate) as "Months",
li.LocationInfoId, 
'SEASON PARKING RECEIVABLE' as AccountName,
'1-2302' as AccountCode,
       0 AS "DebitAmount",sum(dp.amount) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,14 as seq,
       'DAD' as Source
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
and dp.txntype = 'CN'
and dad.xref in ('1','4'))
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.paymentdate), Month(dp.paymentdate)

union

--SEASON PARKING CHARGES
SELECT Year(dp.paymentdate) as "Years",
datepart(m, dp.paymentdate) as "Months",
li.LocationInfoId, 
'SEASON PARKING CHARGES' as AccountName,
'4-1100' as AccountCode,
       sum(dp.amount) AS "DebitAmount",0 AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,15 as seq,
       'DAD' as Source
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
and dp.txntype = 'CN'
and dad.xref in ('1'))
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.paymentdate), Month(dp.paymentdate)


union

--GST OUTPUT TAX
SELECT Year(dp.paymentdate) as "Years",
datepart(m, dp.paymentdate) as "Months",
li.LocationInfoId, 
'GST OUTPUT TAX' as AccountName,
'2-9950' as AccountCode,
       sum(dp.amount) AS "DebitAmount",0 AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,16 as seq,
       'DAD' as Source
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
and dp.txntype = 'CN'
and dad.xref in ('4'))
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.paymentdate), Month(dp.paymentdate)
