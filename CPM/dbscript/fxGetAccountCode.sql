USE [CPM]
GO
/****** Object:  UserDefinedFunction [dbo].[fxGetAccountCode]    Script Date: 01/07/2015 14:14:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fxGetAccountCode] (@LocationInfoId bigint)
RETURNS nvarchar(200)
AS
BEGIN
declare @accountCode nvarchar(200)

select @accountCode = bm.AccountCode 
from locationinfo li,bankmstr bm
where li.bankcode = bm.bankcode
and li.locationinfoid = @LocationInfoId

return isnull(@accountCode,'')

end









