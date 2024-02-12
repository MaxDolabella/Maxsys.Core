using Maxsys.Core.Web.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Maxsys.Core.Web.Extensions;

public static class IHttpContextExtensions
{
    /// <exception cref="NotLoggedUserException"></exception>
    public static Guid GetLoggedUserId(this HttpContext httpContext)
    {
        return httpContext.User.GetLoggedUserId();
    }
}