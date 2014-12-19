USE [CPM]
GO
/****** Object:  UserDefinedFunction [dbo].[fxGetAccountCode]    Script Date: 12/19/2014 18:10:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fxGetAccountCode] (@LocationInfoId bigint)
RETURNS nvarchar(200)
AS
BEGIN
declare @accountCode nvarchar(200)

select @accountCode = bm.AccountNo 
from locationinfo li,bankmstr bm
where li.bankcode = bm.bankcode
and li.locationinfoid = @LocationInfoId

return isnull(@accountCode,'')

end