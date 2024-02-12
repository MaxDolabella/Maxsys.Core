using Maxsys.Core.Web.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Maxsys.Core.Web.Extensions;

public static class IHttpContextAccessorExtensions
{
    /// <exception cref="NotLoggedUserException"></exception>
    public static Guid GetLoggedUserId(this IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor.HttpContext!.GetLoggedUserId();
    }
}