set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fxGetBank] (@LocationInfoId bigint)
RETURNS nvarchar(200)
AS
BEGIN
declare @bank nvarchar(200)

select @bank = bm.bankDesc
from locationinfo li, BankMstr bm
where li.locationinfoid = @LocationInfoId
and li.BankCode = bm.BankCode

return isnull(@bank,'')

end










