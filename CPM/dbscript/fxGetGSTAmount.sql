USE [CPM]
GO
/****** Object:  UserDefinedFunction [dbo].[fxGetGSTAmount]    Script Date: 01/02/2015 11:34:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fxGetGSTAmount] (@debtorAccountHeaderId bigint)
RETURNS numeric(18,2)
AS
BEGIN

declare @gstAmt numeric(18,2)
SET @gstAmt = 0

SELECT  @gstAmt = COALESCE(SUM(dad.amount) ,0)
FROM debtoraccountdetail dad
where debtorAccountHeaderId = @debtorAccountHeaderId
and dad.details like '%GST%'


IF @@rowcount = 0
    Set @gstAmt = 0

return @gstAmt
end