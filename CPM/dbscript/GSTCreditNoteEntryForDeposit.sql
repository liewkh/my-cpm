USE [CPM]
GO
/****** Object:  View [dbo].[GSTCreditNoteEntryForDeposit]    Script Date: 04/17/2015 23:39:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTCreditNoteEntryForDeposit]
as
--SEASON PARKING RECEIVABLE
SELECT Year(dp.paymentdate) as "Years",
datepart(m, dp.paymentdate) as "Months",
li.LocationInfoId, 
'SEASON PARKING RECEIVABLE' as AccountName,
'1-2302' as AccountCode,
       0 AS "DebitAmount",sum(dp.amount) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,17 as seq,
       'GSTCreditNoteEntryForDeposit-17' as Source
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
and dad.xref in ('2','8'))
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.paymentdate), Month(dp.paymentdate)

union

--DEPOSIT RECEIVED
SELECT Year(dp.paymentdate) as "Years",
datepart(m, dp.paymentdate) as "Months",
li.LocationInfoId, 
'DEPOSIT RECEIVED' as AccountName,
'2-4002' as AccountCode,
       sum(dp.amount) AS "DebitAmount",0 AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,18 as seq,
       'GSTCreditNoteEntryForDeposit-18' as Source
from debtor d,locationinfo li,debtorpayment dp
where d.locationinfoid = li.locationinfoid
and dp.debtorid = d.debtorid
and dp.debtorpaymentid in
(
select distinct dp.debtorpaymentid from debtorpayment dp
left join debtoraccountdetail dad
on convert(varchar(200),dad.debtoraccountheaderid) = dp.debtoraccountheaderid
where dp.status <> 'C'
and dp.txntype = 'CN'
and dad.xref in ('2','8'))
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.paymentdate), Month(dp.paymentdate)
