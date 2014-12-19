USE [CPM]
GO
/****** Object:  View [dbo].[GSTDailyCollection]    Script Date: 12/19/2014 18:11:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTDailyCollection]
as
SELECT CONVERT(varchar, CAST(dc.TransactionDate AS datetime), 103) TransactionDate,li.LocationInfoId, li.LocationName as Memo,dbo.fxGetAccountCode(li.LocationInfoId) as AccountCode,
       SUM(dc.Cashier1 + dc.Cashier2 + dc.Cashier3 + dc.Motorcycle1 + dc.Motorcycle2 + dc.Motorcycle3 + 
       dc.Aps1 + dc.Aps2 + dc.Aps3 + dc.Aps4 + dc.Aps5 + dc.Aps6 + 
       dc.DepositCar + dc.DepositMotorcycle + dc.DepositOther +
       dc.SeasonMotorcycleCash + dc.SeasonMotorcycleCheque + dc.SeasonMotorcycleCreditCard + dc.SeasonCarCash + dc.SeasonCarCheque + dc.SeasonCarCreditCard)
       AS "CreditAmount",'' AS "DebitAmount"
FROM   dbo.DailyCollection AS dc INNER JOIN
       dbo.LocationInfo AS li ON dc.LocationInfoId = li.LocationInfoId
GROUP BY li.LocationInfoId, li.LocationName, dc.TransactionDate
union
SELECT CONVERT(varchar, CAST(dc.TransactionDate AS datetime), 103) TransactionDate,li.LocationInfoId, li.LocationName as Memo,dbo.fxGetAccountCode(li.LocationInfoId) as AccountCode,
       SUM(dc.GSTAmount)
       AS "CreditAmount",'' AS "DebitAmount"
FROM   dbo.DailyCollection AS dc INNER JOIN
       dbo.LocationInfo AS li ON dc.LocationInfoId = li.LocationInfoId
GROUP BY li.LocationInfoId, li.LocationName, dc.TransactionDate