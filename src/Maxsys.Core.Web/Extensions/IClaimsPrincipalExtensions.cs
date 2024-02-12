using System.Security.Claims;
using Maxsys.Core.Web.Exceptions;

namespace Maxsys.Core.Web.Extensions;

public static class IClaimsPrincipalExtensions
{
    /// <exception cref="NotLoggedUserException"></exception>
    public static Guid GetLoggedUserId(this ClaimsPrincipal user)
    {
        var userIdValue = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userIdValue, out var guid))
            throw new NotLoggedUserException();

        return guid;
    }
}