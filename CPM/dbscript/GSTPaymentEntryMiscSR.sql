USE [CPM]
GO
/****** Object:  View [dbo].[GSTPaymentEntryMiscSR]    Script Date: 04/04/2015 01:15:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTPaymentEntryMiscSR]
as
--SEASON PARKING RECEIVABLE
SELECT Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
--'SEASON PARKING RECEIVABLE' as AccountName,
'BANK ' + dp.BankCode as AccountName,
dbo.fxGetBankAccountCode(dp.BankCode) as AccountCode,
       sum(dp.amount) AS "DebitAmount",0 AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,31 as seq,
       'GSTPaymentEntryMiscSR-31' as Source
from debtorpayment dp, locationinfo li,
debtor d--,debtoraccountheader dah,debtoraccountdetail dad
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
--and dah.debtoraccountheaderid = dad.debtoraccountheaderid
--and d.debtorid = dah.debtorid
and d.debtorid = dp.debtorid
--and convert(varchar(200),dah.debtoraccountheaderid) = dp.debtoraccountheaderid
--and dad.xref in ('3','5')
--and dah.status <> 'C'
--and dah.txntype = 'R'
--and dad.taxcode in ('SR','NA')
and dp.debtorpaymentid in
(
select distinct dp.debtorpaymentid from debtorpayment dp
left join debtoraccountdetail dad
on convert(varchar(200),dad.debtoraccountheaderid) = dp.debtoraccountheaderid
where dp.status <> 'C'
and dp.txntype = 'R'
and dad.xref in ('3')
and dad.TaxCode in ('SR')
)
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.PaymentDate), Month(dp.PaymentDate),dp.BankCode

union

--OTHER INCOME
SELECT Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
--'OTHER INCOME' as AccountName,
'SEASON PARKING RECEIVABLE' as AccountName,
'1-2302' as AccountCode,
       0 AS "DebitAmount",sum(dp.amount) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,32 as seq,
       'GSTPaymentEntryMiscSR-32' as Source
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
--and dad.taxcode in ('SR','NA')
and dp.debtorpaymentid in
(
select distinct dp.debtorpaymentid from debtorpayment dp
left join debtoraccountdetail dad
on convert(varchar(200),dad.debtoraccountheaderid) = dp.debtoraccountheaderid
where dp.status <> 'C'
and dp.txntype = 'R'
and dad.xref in ('3')
and dad.TaxCode in ('SR')
)
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.PaymentDate), Month(dp.PaymentDate)

/*union

--GST OUTPUT TAX
SELECT Year(dp.PaymentDate) as "Years",
datepart(m, dp.PaymentDate) as "Months",
li.LocationInfoId, 
'GST OUTPUT TAX' as AccountName,
'2-9950' as AccountCode,
       0 AS "DebitAmount",sum(isnull(dp.gstamount,0)) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,33 as seq,
       'GSTPaymentEntryMiscSR-33' as Source
from debtorpayment dp, locationinfo li,
debtor d--,debtoraccountheader dah,debtoraccountdetail dad
where dp.debtorid = d.debtorid
and d.locationinfoid = li.locationinfoid
--and dah.debtoraccountheaderid = dad.debtoraccountheaderid
--and d.debtorid = dah.debtorid
and d.debtorid = dp.debtorid
--and convert(varchar(200),dah.debtoraccountheaderid) = dp.debtoraccountheaderid
--and dad.xref in ('5')
--and dah.txntype = 'R'
--and dah.status <> 'C'
and dp.debtorpaymentid in
(
select distinct dp.debtorpaymentid from debtorpayment dp
left join debtoraccountdetail dad
on convert(varchar(200),dad.debtoraccountheaderid) = dp.debtoraccountheaderid
where dp.status <> 'C'
and dp.txntype = 'R'
and dad.xref in ('5')
)
GROUP BY li.LocationInfoId, li.LocationName, Year(dp.PaymentDate), Month(dp.PaymentDate)
*/

