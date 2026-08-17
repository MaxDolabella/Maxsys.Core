using System.Security.Claims;
using Maxsys.Core.Exceptions;

namespace Maxsys.Core.Extensions;

public static class ClaimsPrincipalExtensions
{
    extension(ClaimsPrincipal user)
    {
        /// <summary>
        /// Atalho para '<see cref="ClaimsPrincipal"/>.FindFirst(<paramref name="identifier"/>)?.Value'
        /// </summary>
        /// <remarks>default <paramref name="identifier"/> = <see cref="ClaimTypes.NameIdentifier"/></remarks>
        /// <exception cref="NotAuthenticatedUserException"></exception>
        public string GetIdentifier(string identifier = ClaimTypes.NameIdentifier)
        {
            return user.FindFirst(identifier)?.Value
                ?? throw new NotAuthenticatedUserException();
        }

        /// <summary>
        /// Obtém '<see cref="ClaimsPrincipal"/>.FindFirst(<paramref name="identifier"/>)?.Value' como <see cref="Guid"/>
        /// </summary>
        /// <remarks>default <paramref name="identifier"/> = <see cref="ClaimTypes.NameIdentifier"/></remarks>
        /// <exception cref="NotAuthenticatedUserException"></exception>
        public Guid GetIdentifierAsGuid(string identifier = ClaimTypes.NameIdentifier)
        {
            var value = user.GetIdentifier(identifier);

            return Guid.TryParse(value, out var guid)
                ? guid
                : throw new NotAuthenticatedUserException($"Invalid cast: {value} is not a Guid.");
        }
    }
}