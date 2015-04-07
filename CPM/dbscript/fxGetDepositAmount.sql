USE [CPM]
GO
/****** Object:  UserDefinedFunction [dbo].[fxGetDepositAmount]    Script Date: 04/07/2015 17:00:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[fxGetDepositAmount] (@debtorAccountHeaderId nvarchar)
RETURNS numeric(18,2)
AS
BEGIN

declare @depositAmt numeric(18,2)
SET @depositAmt = 0

SELECT  @depositAmt = COALESCE(SUM(dad.amount) ,0)
FROM debtoraccountdetail dad
where debtorAccountHeaderId = @debtorAccountHeaderId
and dad.xref in ('2')
and dad.TaxCode = 'OS'


IF @@rowcount = 0
    Set @depositAmt = 0

return @depositAmt
end