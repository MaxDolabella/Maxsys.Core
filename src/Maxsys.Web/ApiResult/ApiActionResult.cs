using Microsoft.AspNetCore.Mvc;

namespace Maxsys.Web;

public class ApiActionResult : ObjectResult
{
    public ApiActionResult(ApiResultBase apiResult) : base(apiResult)
    {
        StatusCode = apiResult.StatusCode;
    }
}