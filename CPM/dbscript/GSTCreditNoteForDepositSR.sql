USE [CPM]
GO
/****** Object:  View [dbo].[GSTCreditNoteForDepositSR]    Script Date: 03/15/2015 15:49:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTCreditNoteForDepositSR]
as
--SEASON PARKING RECEIVABLE
SELECT Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
'SEASON PARKING RECEIVABLE' as AccountName,
'1-2302' as AccountCode,
       0 AS "DebitAmount",sum(dp.amount) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,21 as seq,
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
and dad.xref in ('3','5')
and dad.TaxCode <> 'ZRL'
)
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.PaymentDate), Month(dp.PaymentDate)



union

--OTHER INCOME
SELECT Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
'OTHER INCOME' as AccountName,
'4-2100' as AccountCode,
       sum(dp.amount) AS "DebitAmount",0 AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,22 as seq,
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
and dad.xref in ('3')
and dad.TaxCode = 'SR'
)
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.PaymentDate), Month(dp.PaymentDate)

union

--GST OUTPUT TAX
SELECT Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
'GST OUTPUT TAX' as AccountName,
'2-9950' as AccountCode,
       sum(dp.amount) AS "DebitAmount",0 AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,23 as seq,
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
and dad.xref in ('5')
)
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.PaymentDate), Month(dp.PaymentDate)



