using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using TokenBasedAuthApi.Dto;
using TokenBasedAuthApi.Services;

namespace TokenBasedAuthApi.EndPoints
{
    public static class AuthEndpoints
    {
        public static RouteGroupBuilder CreateAuthEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("").WithTags("Auth");
            group.MapPost("/api/auth/login",
            [AllowAnonymous]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            //JWT bearer is generated by the server as a response to a login request
            async (UserManager<IdentityUser> userMgr, ITokenService tokenService, ILoggerFactory loggerFactory, [FromBody] LoginDto user) =>
            {
                var logger = loggerFactory.CreateLogger(typeof(AuthEndpoints));
                logger.LogDebug("login requested for {user.UserName}", user.UserName);
                var isValid = MiniValidator.TryValidate(user, out var errors);
                if (!isValid)
                {
                    return Results.ValidationProblem(errors);
                }
                var identityUsr = await userMgr.FindByNameAsync(user.UserName);
                if (identityUsr == null)
                    return Results.Unauthorized();
                if (await userMgr.CheckPasswordAsync(identityUsr, user.Password))
                {
                    List<string> userRoles = new List<string>();
                    var roles = await userMgr.GetRolesAsync(identityUsr);
                    if (roles != null)
                        userRoles.AddRange(roles);
                    // username and password are correct, so using TokenService to generate a JWT bearer
                    var accessToken = tokenService.CreateToken(user.UserName, userRoles);
                    return Results.Ok(accessToken);
                }
                else
                {
                    return Results.Unauthorized();
                }
            });

            return group;
        }
        
    }
}
