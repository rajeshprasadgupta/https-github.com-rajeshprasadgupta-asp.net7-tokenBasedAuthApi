using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniValidation;
using TokenBasedAuthApi.Dto;

namespace TokenBasedAuthApi.EndPoints
{
    public static class IdentityEndpoints
    {
        public static RouteGroupBuilder CreateIdentityEndpoints(this IEndpointRouteBuilder routes)
        {

            var group = routes.MapGroup("").WithTags("Identity");

            group.MapGet("/api/identity/users",
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            async (UserManager<IdentityUser> userMgr) => //using inline lambada as route handler
            {
                var users = await userMgr.Users.ToListAsync();
                if (users == null)
                    return Results.BadRequest();
                var names = new List<UserResponseDto>();
                foreach (var user in users)
                {
                    names.Add(new UserResponseDto(user.UserName, user.Email));
                }
                return Results.Ok(names);
            });

            group.MapGet("/api/identity/roles",
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            async (RoleManager<IdentityRole> roleMgr) => //using inline lambada as route handler
            {
                var roles = await roleMgr.Roles.ToListAsync();
                if (roles == null)
                    return Results.BadRequest();

                return Results.Ok(roles.Select(r => r.Name).ToList());
            });

            group.MapGet("/api/identity/role/{username}", 
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            async (UserManager<IdentityUser> userMgr, string userName) =>
            {
                var user = await userMgr.FindByNameAsync(userName);
                if (user == null)
                    return Results.BadRequest("user not found");
                var roles = await userMgr.GetRolesAsync(user);
                if (roles == null)
                    return Results.BadRequest("roles not found");
                return Results.Ok(roles);
            });

            group.MapPost("/api/identity/add-role",
            [Authorize(Roles = "Administrator")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            async (RoleManager<IdentityRole> roleManager, string role) =>
            {
                var identityrole = new IdentityRole()
                {
                    Name = role,
                    NormalizedName = role.ToUpper(),
                };

                var result = await roleManager.CreateAsync(identityrole);
                if (result.Succeeded)
                {

                    return Results.Ok();
                }
                else
                {
                    return Results.BadRequest();
                }
            });


            

            group.MapPost("/api/identity/add-user",
            [Authorize(Roles = "Administrator")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            async (UserManager<IdentityUser> userMgr, UserRequestDto user) =>
            {
                var isValid = MiniValidator.TryValidate(user, out var errors);
                if (!isValid)
                {
                    return Results.ValidationProblem(errors);
                }
                var identityUser = new IdentityUser()
                {
                    UserName = user.UserName,
                    Email = user.Email
                };

                var result = await userMgr.CreateAsync
            (identityUser, user.Password);

                if (result.Succeeded)
                {
                    return Results.Ok();
                }
                else
                {
                    return Results.BadRequest();
                }
            });

            group.MapPut("/api/identity/assign-role/user={username}&role={role}",
            [Authorize(Roles = "Administrator")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            async (UserManager<IdentityUser> userMgr, RoleManager <IdentityRole> roleManager, string userName, string role) =>
            {
                var user = await userMgr.FindByNameAsync(userName);
                if (user == null)
                    return Results.BadRequest("User not found");
                var identityRole = await roleManager.FindByNameAsync(role);
                if (identityRole == null)
                    return Results.BadRequest("Role not found");
                var result = await userMgr.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                    return Results.BadRequest();
                return Results.Ok();
            });



            group.MapDelete("/api/identity/user/{username}",
            [Authorize(Roles = "Administrator")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            async (UserManager<IdentityUser> userMgr, string userName) =>
            {
                var user = await userMgr.FindByNameAsync(userName);
                if (user == null)
                    return Results.BadRequest("user not found");
                var result = await userMgr.DeleteAsync(user);
                if (!result.Succeeded)
                    return Results.BadRequest("failed to delete user");
                return Results.Ok();
            });
            return group;
        }
    }
}
