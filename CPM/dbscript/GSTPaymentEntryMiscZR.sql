USE [CPM]
GO
/****** Object:  View [dbo].[GSTPaymentEntryMiscZR]    Script Date: 04/04/2015 01:15:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTPaymentEntryMiscZR]
as
SELECT Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
'BANK - ' + dp.BankCode as AccountName,
dbo.fxGetBankAccountCode(dp.BankCode) as AccountCode,
       sum(dp.amount) AS "DebitAmount",0 AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,29 as seq,
       'GSTPaymentEntryMiscZR-29' as Source
from debtorpayment dp, locationinfo li,
debtor d--,debtoraccountheader dah,debtoraccountdetail dad
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
--and dah.debtoraccountheaderid = dad.debtoraccountheaderid
--and d.debtorid = dah.debtorid
and d.debtorid = dp.debtorid
--and convert(varchar(200),dah.debtoraccountheaderid) = dp.debtoraccountheaderid
--and dad.xref in ('3')
--and dah.status <> 'C'
--and dah.txntype = 'R'
--and dad.taxcode = 'ZRL'
and dp.debtorpaymentid in
(
select distinct dp.debtorpaymentid from debtorpayment dp
left join debtoraccountdetail dad
on convert(varchar(200),dad.debtoraccountheaderid) = dp.debtoraccountheaderid
where dp.status <> 'C'
and dp.txntype = 'R'
and dad.xref in ('3')
and dad.TaxCode = 'ZRL'
)
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.PaymentDate), Month(dp.PaymentDate),dp.BankCode

union

--SEASON PARKING RECEIVABLE
SELECT Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
'SEASON PARKING RECEIVABLE' as AccountName,
'1-2302' as AccountCode,
       0 AS "DebitAmount",sum(dp.amount) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,30 as seq,
       'GSTPaymentEntryMiscZR-30' as Source
from debtorpayment dp, locationinfo li,
debtor d--,debtoraccountheader dah,debtoraccountdetail dad
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
--and dah.debtoraccountheaderid = dad.debtoraccountheaderid
--and d.debtorid = dah.debtorid
and d.debtorid = dp.debtorid
--and convert(varchar(200),dah.debtoraccountheaderid) = dp.debtoraccountheaderid
--and dad.xref in ('3')
--and dah.status <> 'C'
--and dah.txntype = 'R'
--and dad.taxcode = 'ZRL'
and dp.debtorpaymentid in
(
select distinct dp.debtorpaymentid from debtorpayment dp
left join debtoraccountdetail dad
on convert(varchar(200),dad.debtoraccountheaderid) = dp.debtoraccountheaderid
where dp.status <> 'C'
and dp.txntype = 'R'
and dad.xref in ('3')
and dad.TaxCode = 'ZRL'
)
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.PaymentDate), Month(dp.PaymentDate)
