USE [CPM]
GO
/****** Object:  UserDefinedFunction [dbo].[fxGetDepositAmount]    Script Date: 03/13/2015 14:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fxGetDepositAmount] (@debtorAccountHeaderId bigint)
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
