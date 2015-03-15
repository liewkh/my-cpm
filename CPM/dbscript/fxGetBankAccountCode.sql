set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fxGetBankAccountCode] (@BankCode nvarchar(200))
RETURNS nvarchar(200)
AS
BEGIN
declare @accountCode nvarchar(200)

select @accountCode = bm.AccountCode
from bankmstr bm
where bm.bankCode = @BankCode


return isnull(@accountCode,'')

end


