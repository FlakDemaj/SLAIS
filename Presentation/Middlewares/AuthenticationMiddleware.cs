using System.Security.Claims;

using Application.Common;
using Application.Common.Interfaces;

using Domain.Common;
using Domain.Common.Enums;
using Domain.Common.Exceptions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Presentation.Middlewares;

public class AuthenticationMiddleware
{
    private RequestDelegate Next { get; }

    public AuthenticationMiddleware(RequestDelegate next)
    {
        Next = next;
    }

    public async Task Invoke(HttpContext context)
    {

        if (CheckIfAllowAnonymousExists(context))
        {
            await Next(context);
            return;
        }

        var userGuid = CheckUserGuid(context);

        var userRole = CheckUserRole(context);

        var authentication = new Authentication(
            userGuid,
            userRole);

        context.Items.Add(nameof(IAuthentication), authentication);

        await Next(context);
    }

    private static Guid CheckUserGuid(HttpContext context)
    {
        var userGuidString =
            context
                .User
                .Claims
                .FirstOrDefault(c =>
                    c.Type == JwtRegisteredClaimNames.Sub)?
                .Value;

        if (string.IsNullOrWhiteSpace(userGuidString))
        {
            throw new SlaisException(CommonErrorCodes.DefaultErrorCode);
        }

        if (!Guid.TryParse(userGuidString, out var userGuid))
        {
            throw new SlaisException(CommonErrorCodes.DefaultErrorCode);
        }

        return userGuid;
    }

    private static Roles CheckUserRole(HttpContext context)
    {
        var userRoleString =
            context
                .User
                .Claims
                .FirstOrDefault(
                    c =>
                        c.Type == ClaimTypes.Role)?
                .Value;

        if (string.IsNullOrWhiteSpace(userRoleString))
        {
            throw new SlaisException(CommonErrorCodes.DefaultErrorCode);
        }

        if (!Enum.TryParse<Roles>(userRoleString, out var userRole))
        {
            throw new SlaisException(CommonErrorCodes.DefaultErrorCode);
        }

        return userRole;
    }

    private static bool CheckIfAllowAnonymousExists(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint == null)
        {
            return true;
        }

        var allowAnonymous = endpoint.Metadata.Any(f => f is AllowAnonymousAttribute);
        return allowAnonymous;
    }

}
