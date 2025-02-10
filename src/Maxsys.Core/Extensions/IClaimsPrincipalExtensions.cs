using System.Security.Claims;
using Maxsys.Core.Exceptions;

namespace Maxsys.Core.Extensions;

public static class ClaimsPrincipalExtensions
{
    [Obsolete("Uses GetIdentifierAsGuid()", true)]
    public static Guid GetLoggedUserId(this ClaimsPrincipal user)
    {
        var userIdValue = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userIdValue, out var guid))
            throw new NotLoggedUserException();

        return guid;
    }

    public static string GetIdentifier(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new NotLoggedUserException();
    }

    public static Guid GetIdentifierAsGuid(this ClaimsPrincipal user)
    {
        var identifier = GetIdentifier(user);

        return Guid.TryParse(identifier, out var guid)
            ? guid
            : throw new NotLoggedUserException($"Invalid cast: {ClaimTypes.NameIdentifier} is not a Guid.");
    }
}