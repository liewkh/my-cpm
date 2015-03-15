USE [CPM]
GO
/****** Object:  View [dbo].[GSTPaymentEntryMiscOS]    Script Date: 03/15/2015 23:33:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[GSTPaymentEntryMiscOS]
as
--SEASON PARKING RECEIVABLE
SELECT Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
'BANK - ' + dp.BankCode as AccountName,
dbo.fxGetBankAccountCode(dp.BankCode) as AccountCode,
       0 AS "DebitAmount",sum(dp.amount) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,38 as seq,
       'DAD' as Source
from debtorpayment dp, locationinfo li,
debtor d
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
and dp.TxnType = 'R'
and d.debtorid = dp.debtorid
and dp.debtorpaymentid in
(
select distinct dp.debtorpaymentid from debtorpayment dp
left join debtoraccountdetail dad
on convert(varchar(200),dad.debtoraccountheaderid) = dp.debtoraccountheaderid
where dp.status <> 'C'
and dp.txntype = 'R'
and dad.xref in ('3')
and dad.TaxCode = 'OS'
)
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.PaymentDate), Month(dp.PaymentDate),dp.BankCode



union

SELECT Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
'SEASON PARKING RECEIVABLE' as AccountName,
'1-2302' as AccountCode,
       sum(dp.amount) AS "DebitAmount",0 AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,39 as seq,
       'DAD' as Source
from debtorpayment dp, locationinfo li,
debtor d
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
and dp.TxnType = 'R'
and d.debtorid = dp.debtorid
and dp.debtorpaymentid in
(
select distinct dp.debtorpaymentid from debtorpayment dp
left join debtoraccountdetail dad
on convert(varchar(200),dad.debtoraccountheaderid) = dp.debtoraccountheaderid
where dp.status <> 'C'
and dp.txntype = 'R'
and dad.xref in ('3')
and dad.TaxCode = 'OS'
)
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.PaymentDate), Month(dp.PaymentDate)




