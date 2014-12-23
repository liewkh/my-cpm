set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fxGetLocationCode] (@LocationInfoId bigint)
RETURNS nvarchar(200)
AS
BEGIN
declare @locationCode nvarchar(200)

select @locationCode = li.LocationCode
from locationinfo li where li.locationinfoid = @LocationInfoId

return isnull(@locationCode,'')

end










