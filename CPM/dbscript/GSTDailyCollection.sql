USE [CPM]
GO
/****** Object:  View [dbo].[GSTDailyCollection]    Script Date: 01/18/2015 19:42:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[GSTDailyCollection]
as
SELECT Year(dc.TransactionDate) as "Years",datepart(m, transactiondate) as "Months",li.LocationInfoId, 
'BANK - ' + dbo.fxGetBank(li.locationinfoid) as AccountName,dbo.fxGetAccountCode(li.locationinfoid) as AccountCode,
       SUM(dc.Cashier1 + dc.Cashier2 + dc.Cashier3 + 
           dc.valetservice1 + dc.valetservice2 + dc.valetservice3 +
           dc.Motorcycle1 + dc.Motorcycle2 + dc.Motorcycle3 + 
           dc.Aps1 + dc.Aps2 + dc.Aps3 + dc.Aps4 + dc.Aps5 + dc.Aps6)  
       AS "DebitAmount",0 AS "CreditAmount",dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,1 as seq,
       'GSTDailyCollection-1' as Source
FROM   dbo.DailyCollection AS dc INNER JOIN
       dbo.LocationInfo AS li ON dc.LocationInfoId = li.LocationInfoId
GROUP BY li.LocationInfoId, li.LocationName, Year(dc.TransactionDate), Month(dc.TransactionDate)




union

--DAILY COLLETCTION
SELECT Year(dc.TransactionDate) as "Years",datepart(m, transactiondate) as "Months",li.LocationInfoId, 
'DAILY COLLECTION' as AccountName,'4-1200'  as AccountCode,
0 AS "DebitAmount",
       SUM(dc.Cashier1 + dc.Cashier2 + dc.Cashier3 + 
           dc.valetservice1 + dc.valetservice2 + dc.valetservice3 +
           dc.Motorcycle1 + dc.Motorcycle2 + dc.Motorcycle3 + 
           dc.Aps1 + dc.Aps2 + dc.Aps3 + dc.Aps4 + dc.Aps5 + dc.Aps6 - dc.GSTAmount)  
       AS "CreditAmount",dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,2 as seq,
       'GSTDailyCollection-2' as Source
FROM   dbo.DailyCollection AS dc INNER JOIN
       dbo.LocationInfo AS li ON dc.LocationInfoId = li.LocationInfoId

GROUP BY li.LocationInfoId, li.LocationName, Year(dc.TransactionDate), Month(dc.TransactionDate)

union
--DAILY COLLECTION GST
Select Year(dc.TransactionDate) as "Years",datepart(m, transactiondate) as "Months",li.LocationInfoId, 
'GST OUTPUT TAX' as AccountName,'2-9950' as AccountCode,
       0 AS "DebitAmount",SUM(dc.GSTAmount) AS "CreditAmount",
dbo.fxGetLocationCode(li.locationinfoid) as LocationCode,3 as seq,
       'GSTDailyCollection-3' as Source
FROM   dbo.DailyCollection AS dc INNER JOIN
       dbo.LocationInfo AS li ON dc.LocationInfoId = li.LocationInfoId

GROUP BY li.LocationInfoId, li.LocationName, Year(dc.TransactionDate), Month(dc.TransactionDate)
